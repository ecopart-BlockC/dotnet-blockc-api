using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace BlockC_Api.Classes
{
    public class Mail
    {

        public void EnviarEmail(string destinatario, string copia, string mensagem, string assunto)
        {
            try
            {
                Classes.Database database = new Classes.Database();
                DataTable dtParametros = new DataTable();
                dtParametros = database.BuscarParametroEmail();

                string remetente = string.Empty;
                string remetenteDisplayName = string.Empty;
                string servidor = string.Empty;
                string porta = string.Empty;
                string usuario = string.Empty;
                string senha = string.Empty;

                for (int i = 0; i < dtParametros.Rows.Count -1; i++)
                {
                    switch (dtParametros.Rows[i]["Parametro"].ToString().ToUpper())
                    {
                        case "SMTPSERVER":
                            servidor = dtParametros.Rows[i]["Valor"].ToString();
                            break;
                        case "SMTPUSER":
                            usuario = dtParametros.Rows[i]["Valor"].ToString();
                            break;
                        case "SMTPPASS":
                            senha = dtParametros.Rows[i]["Valor"].ToString();
                            break;
                        case "SMTPPORT":
                            porta = dtParametros.Rows[i]["Valor"].ToString();
                            break;
                        case "MAILFROM":
                            remetente = dtParametros.Rows[i]["Valor"].ToString();
                            break;
                        case "MAILFROMDISPLAYNAME":
                            remetenteDisplayName = dtParametros.Rows[i]["Valor"].ToString();
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(senha))
                    senha = Crypto.Decrypt(senha);

                System.Net.Mail.MailAddress emailFrom = new System.Net.Mail.MailAddress(remetente, remetenteDisplayName);
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient SmtpCliente = new System.Net.Mail.SmtpClient(servidor);
                AlternateView altView = AlternateView.CreateAlternateViewFromString(mensagem, null, MediaTypeNames.Text.Html);

                mailMessage.From = emailFrom;
                mailMessage.Priority = MailPriority.Normal;
                mailMessage.Subject = assunto;
                mailMessage.AlternateViews.Add(altView);

                InserirDestinatarios(ref mailMessage, destinatario, "", false);
                InserirDestinatarios(ref mailMessage, copia, "bruno.bela@itfour.com.br;jose.dantas@blockc.com.br", true);

                if ((!string.IsNullOrEmpty(usuario)) && (!string.IsNullOrEmpty(senha)))
                {
                    System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
                    credential.UserName = usuario;
                    credential.Password = senha;

                    SmtpCliente.UseDefaultCredentials = false;
                    SmtpCliente.Credentials = credential;
                    SmtpCliente.EnableSsl = true;
                }

                SmtpCliente.Port = Convert.ToInt32(porta);
                SmtpCliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpCliente.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "EnviarEmail", "Post", ex.Message, string.Empty);
            }
        }

        protected void InserirDestinatarios(ref MailMessage mailMessage, string destinatarios, string bcc, Boolean inserirCopia)
        {
            try
            {
                if (string.IsNullOrEmpty(destinatarios))
                    return;

                if (destinatarios.IndexOf(";") > 0)
                {
                    while (destinatarios.Length > 0)
                    {
                        string Atual = "";
                        if (destinatarios.IndexOf(";") > 0)
                        {
                            Atual = destinatarios.Substring(0, destinatarios.IndexOf(";")).Trim();
                            destinatarios = destinatarios.Substring(destinatarios.IndexOf(";") + 1).Trim();
                        }
                        else
                        {
                            Atual = destinatarios;
                            destinatarios = "";
                        }

                        if (!string.IsNullOrEmpty(Atual))
                        {
                            if (!inserirCopia)
                                mailMessage.To.Add(Atual);
                            else
                                mailMessage.CC.Add(Atual);
                        }
                    }
                }
                else
                {
                    if (!inserirCopia)
                        mailMessage.To.Add(destinatarios);
                    else
                        mailMessage.CC.Add(destinatarios);
                }

                if (string.IsNullOrEmpty(bcc)) return;
                while (bcc.Length > 0)
                {
                    string Atual = "";
                    if (bcc.IndexOf(";") > 0)
                    {
                        Atual = bcc.Substring(0, bcc.IndexOf(";")).Trim();
                        bcc = bcc.Substring(bcc.IndexOf(";") + 1).Trim();
                    }
                    else
                    {
                        Atual = bcc;
                        bcc = "";
                    }

                    if (!string.IsNullOrEmpty(Atual))
                        mailMessage.Bcc.Add(Atual);
                }

            }
            catch (Exception ex)
            {
                Classes.Database.RegistrarErro("Server API", "InserirDestinatarios", "Post", ex.Message, string.Empty);
            }
        }



    }
}