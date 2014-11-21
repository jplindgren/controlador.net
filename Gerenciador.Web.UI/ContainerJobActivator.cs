using Autofac;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI {
    public class ContainerJobActivator : JobActivator {
        private IContainer _container;

        public ContainerJobActivator(IContainer container) {
            _container = container;
        }

        public override object ActivateJob(Type type) {
            return _container.Resolve(type);
        }
    } //class
}