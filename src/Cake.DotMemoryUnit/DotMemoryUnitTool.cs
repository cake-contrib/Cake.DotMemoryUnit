using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.DotMemoryUnit
{
    public class DotMemoryUnitTool : Tool<DotMemoryUnitSettings>
    {
        public DotMemoryUnitTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools
        )
            : base(fileSystem, environment, processRunner, tools) { }

        public void Run(
            ICakeContext context,
            Action<ICakeContext> action,
            DotMemoryUnitSettings settings
        )
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            // Run the tool.
            Run(settings, GetArguments(context, action, settings));
        }

        private ProcessArgumentBuilder GetArguments(
            ICakeContext context,
            Action<ICakeContext> action,
            DotMemoryUnitSettings settings
        )
        {
            var testRunnerSettings = GetTestRunnerSettings(context, action);

            var builder = new ProcessArgumentBuilder();

            builder.AppendQuoted(testRunnerSettings.ExecutablePath.FullPath);

            if (settings.PropagateExitCode)
            {
                builder.Append("--propagate-exit-code");
            }

            if (settings.NoUpdates)
            {
                builder.Append("--no-updates");
            }

            if (!testRunnerSettings.ArgumentsBuilder.IsNullOrEmpty())
            {
                builder.Append("--");
                testRunnerSettings.ArgumentsBuilder.CopyTo(builder);
            }

            return builder;
        }

        /// <summary>
        /// Get the target executable and arguments.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action to run DotCover for.</param>
        /// <returns>The target settings.</returns>
        private DotMemoryUnitTestRunnerSettings GetTestRunnerSettings(
            ICakeContext context,
            Action<ICakeContext> action
        )
        {
            // Run the tool using the interceptor.
            var targetContext = InterceptAction(context, action);

            // Validate arguments.
            if (targetContext.FilePath == null)
            {
                throw new CakeException("No tool was started in the specified action.");
            }

            return new DotMemoryUnitTestRunnerSettings(targetContext.FilePath, targetContext.Settings.Arguments);
        }

        private static DotMemoryUnitContext InterceptAction(
            ICakeContext context,
            Action<ICakeContext> action
        )
        {
            var interceptor = new DotMemoryUnitContext(context);
            action(interceptor);
            return interceptor;
        }

        protected override string GetToolName() => "DotMemoryUnit";

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "dotMemoryUnit.exe" };
        }
    }
}
