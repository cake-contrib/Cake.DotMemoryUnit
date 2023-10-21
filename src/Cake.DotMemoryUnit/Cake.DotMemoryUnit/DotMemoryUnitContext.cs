using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.DotMemoryUnit
{
    internal sealed class DotMemoryUnitContext : CakeContextAdapter
    {
        private readonly DotMemoryUnitProcessRunner _runner;

        public override ICakeLog Log { get; }

        public override IProcessRunner ProcessRunner => _runner;

        public FilePath? FilePath => _runner.FilePath;

        public ProcessSettings Settings => _runner.ProcessSettings;

        public DotMemoryUnitContext(ICakeContext context)
            : base(context)
        {
            Log = new NullLog();
            _runner = new DotMemoryUnitProcessRunner();
        }
    }
}
