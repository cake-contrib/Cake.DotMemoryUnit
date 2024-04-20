using Cake.Core.IO;

namespace Cake.DotMemoryUnit
{
    internal class DotMemoryUnitTestRunnerSettings
    {
        internal FilePath ExecutablePath { get; }
        internal ProcessArgumentBuilder ArgumentsBuilder { get; }

        internal DotMemoryUnitTestRunnerSettings(FilePath executablePath, ProcessArgumentBuilder argumentBuilder)
        {
            ExecutablePath = executablePath;
            ArgumentsBuilder = argumentBuilder;
        }
    }
}
