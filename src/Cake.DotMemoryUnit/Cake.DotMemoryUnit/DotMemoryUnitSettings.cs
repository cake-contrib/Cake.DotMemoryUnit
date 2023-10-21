using Cake.Core.Tooling;

namespace Cake.DotMemoryUnit
{
    public class DotMemoryUnitSettings : ToolSettings
    {
        public bool PropagateExitCode { get; set; } = true;
        public bool NoUpdates { get; set; } = true;
    }
}
