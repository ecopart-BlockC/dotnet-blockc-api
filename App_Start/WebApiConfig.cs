using BlockC_Api.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Asp.Versioning.Routing;
using System.Web.Http.Routing;

namespace BlockC_Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();

            var contraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof(ApiVersionRouteConstraint)
                }
            };

            config.MapHttpAttributeRoutes(contraintResolver);

            config.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
            });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{apiVersion}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                , constraints: new { apiVersion = new ApiVersionRouteConstraint() }
            );

        }
    }
}
