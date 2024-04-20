using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cake.Core.IO;

namespace Cake.DotMemoryUnit
{
    internal sealed class DotMemoryUnitProcessRunner : IProcessRunner
    {
        public FilePath? FilePath { get; set; }

        public ProcessSettings ProcessSettings { get; set; } = new ProcessSettings();

        private sealed class InterceptedProcess : IProcess
        {
            public void Dispose() { }

            public void WaitForExit() { }

            public bool WaitForExit(int milliseconds)
            {
                return true;
            }

            public int GetExitCode()
            {
                return 0;
            }

            public IEnumerable<string> GetStandardError()
            {
                return Enumerable.Empty<string>();
            }

            public IEnumerable<string> GetStandardOutput()
            {
                return Enumerable.Empty<string>();
            }

            public void Kill() { }
        }

        [MemberNotNull(nameof(FilePath))]
        [MemberNotNull(nameof(ProcessSettings))]
        public IProcess Start(FilePath filePath, ProcessSettings settings)
        {
            FilePath = filePath;
            ProcessSettings = settings;
            return new InterceptedProcess();
        }
    }
}
