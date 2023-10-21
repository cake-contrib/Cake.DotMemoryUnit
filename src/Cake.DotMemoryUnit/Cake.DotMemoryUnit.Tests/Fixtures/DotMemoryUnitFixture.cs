using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing.Fixtures;
using NSubstitute;
using System;

namespace Cake.DotMemoryUnit.Tests.Fixtures
{

    internal abstract class DotMemoryUnitFixture<TSettings> : DotMemoryUnitFixture<TSettings, ToolFixtureResult>
        where TSettings : ToolSettings, new()
    {

        protected override ToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new ToolFixtureResult(path, process);
        }
    }

    internal abstract class DotMemoryUnitFixture<TSettings, TFixtureResult> : ToolFixture<TSettings, TFixtureResult>
        where TSettings : ToolSettings, new()
        where TFixtureResult : ToolFixtureResult
    {
        protected DotMemoryUnitFixture()
            : base("dotMemoryUnit.exe")
        {
            ProcessRunner.Process.SetStandardOutput(new string[] { });
        }
    }

    internal sealed class DotMemoryUnitFixture : DotMemoryUnitFixture<DotMemoryUnitSettings>
    {
        public ICakeContext Context { get; set; }
        public Action<ICakeContext> Action { get; set; }

        public DotMemoryUnitFixture()
        {
            // Setup the Cake Context.
            Context = Substitute.For<ICakeContext>();
            Context.FileSystem.Returns(FileSystem);
            Context.Arguments.Returns(Substitute.For<ICakeArguments>());
            Context.Environment.Returns(Environment);
            Context.Globber.Returns(Globber);
            Context.Log.Returns(Substitute.For<ICakeLog>());
            Context.Registry.Returns(Substitute.For<IRegistry>());
            Context.ProcessRunner.Returns(ProcessRunner);
            Context.Tools.Returns(Tools);

            // Set up the default action that intercepts.
            Action = context =>
            {
                context.ProcessRunner.Start(
                    new FilePath("/Working/tools/test.exe"),
                    new ProcessSettings()
                    {
                        Arguments = "-argument"
                    });
            };
        }

        protected override void RunTool()
        {
            var tool = new DotMemoryUnitTool(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Context, Action, Settings);
        }
    }
}
