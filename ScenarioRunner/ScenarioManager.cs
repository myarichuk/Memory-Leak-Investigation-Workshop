using Autofac;
using ScenarioRunner.IssueScenarios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScenarioRunner
{
    public class ScenarioManager : IDisposable
    {
        private readonly IContainer _container;
        private readonly Lazy<IReadOnlyList<IScenario>> _cachedScenarios;

        public ScenarioManager()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => typeof(IScenario).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .SingleInstance();            

            _container = builder.Build();
            _cachedScenarios = new Lazy<IReadOnlyList<IScenario>>(() => _container.Resolve<IEnumerable<IScenario>>().ToList());
        }

        public IReadOnlyList<IScenario> Scenarios => _cachedScenarios.Value;

        public void Dispose() => _container.Dispose();
    }
}