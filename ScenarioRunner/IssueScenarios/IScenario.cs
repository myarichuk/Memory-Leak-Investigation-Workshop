using System.Threading.Tasks;

namespace ScenarioRunner.IssueScenarios
{
    public interface IScenario
    {
        string Name { get; }

        Task StartAsync();
        Task StopAsync();
    }
}
