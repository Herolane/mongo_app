﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using MatrixWeb.Controllers;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using System.Reflection;
using MatrixCore.DataAccess;
using EnterpriseCore.DataAccessObjects;
using MatrixWeb.Areas.Sales.Controllers;
using MatrixCore.Framework;
using System.Configuration;


namespace MatrixWeb
{
    public static class AutofacBootstrapper
    {
        public static void Initialise()
        {
            var builder = new ContainerBuilder();

            BuildContainer(builder);
        }

        static void BuildContainer(ContainerBuilder builder)
        {
            //Register MVC controllers first
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            //then the types
            builder.RegisterType<MXMongoRepository>().As<IRepository>();

            //Named types
            builder.RegisterType<ClientRepository>().Named<IRepository>("ClientRepository");


            //inject specific implementation of IRepository Interface
            builder.Register(c => new ClientController(c.ResolveNamed<IRepository>("ClientRepository")));

            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
        
    }//End of class
}