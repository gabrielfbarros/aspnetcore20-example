﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace AspNetCore20Example.Services.API.Configurations
{
    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centralPrefix;

        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                foreach (var selectorModel in matchedSelectors)
                    selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix, selectorModel.AttributeRouteModel);

                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                foreach (var selectorModel in unmatchedSelectors)
                    selectorModel.AttributeRouteModel = _centralPrefix;
            }
        }
    }

    public static class MvcOptionsExtensions
    {
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }
    }
}