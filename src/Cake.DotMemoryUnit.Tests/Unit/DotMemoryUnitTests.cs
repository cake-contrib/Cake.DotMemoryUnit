using Cake.Common.Tools.NUnit;
using Cake.Common.Tools.XUnit;
using Cake.Core;
using Cake.Core.IO;
using Cake.DotMemoryUnit;
using Cake.DotMemoryUnit.Tests.Fixtures;
using Cake.Testing;
using Xunit;

namespace Cake.DotMemoryUnit.Tests.Unit
{
    public class DotMemoryUnitTests
    {
        [Fact]
        public void Should_Throw_If_Context_Is_Null()
        {
            // Given
            var fixture = new DotMemoryUnitFixture
            {
                Context = null
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "context");
        }

        [Fact]
        public void Should_Throw_If_Action_Is_Null()
        {
            // Given
            var fixture = new DotMemoryUnitFixture
            {
                Action = null
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "action");
        }

        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new DotMemoryUnitFixture
            {
                Settings = null
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_No_Tool_Was_Intercepted()
        {
            // Given
            var fixture = new DotMemoryUnitFixture
            {
                Action = context => { }
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsCakeException(result, "No tool was started in the specified action.");
        }

        [Fact]
        public void Should_Capture_Tool_And_Arguments_From_Action()
        {
            // Given
            var fixture = new DotMemoryUnitFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/tools/test.exe\" --propagate-exit-code --no-updates -- -argument", result.Args);
        }

        [Fact]
        public void Should_Not_Append_PropagateExitCode()
        {
            // Given
            var fixture = new DotMemoryUnitFixture()
            {
                Settings =
                {
                    PropagateExitCode = false
                }
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/tools/test.exe\" --no-updates -- -argument", result.Args);
        }

        [Fact]
        public void Should_Not_Append_NoUpdates()
        {
            // Given
            var fixture = new DotMemoryUnitFixture
            {
                Settings =
                {
                    NoUpdates = false
                }
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/tools/test.exe\" --propagate-exit-code -- -argument", result.Args);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Not_Capture_Arguments_From_Action_If_Excluded(string arguments)
        {
            // Given
            var fixture = new DotMemoryUnitFixture();
            fixture.Action = context =>
            {
                context.ProcessRunner.Start(
                    new FilePath("/Working/tools/test.exe"),
                    new ProcessSettings()
                    {
                        Arguments = arguments
                    });
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/tools/test.exe\" --propagate-exit-code --no-updates", result.Args);
        }

        [Fact]
        public void Should_Capture_XUnit()
        {
            // Given
            var fixture = new DotMemoryUnitFixture();
            fixture.FileSystem.CreateFile("/Working/tools/xunit.console.exe");
            fixture.Action = context =>
            {
                context.XUnit2(
                    new FilePath[] { "./Test.dll" },
                    new XUnit2Settings { ShadowCopy = false });
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/tools/xunit.console.exe\" --propagate-exit-code --no-updates -- \"/Working/Test.dll\" -noshadow", result.Args);
        }

        [Fact]
        public void Should_Capture_NUnit()
        {
            // Given
            var fixture = new DotMemoryUnitFixture();
            fixture.FileSystem.CreateFile("/Working/tools/nunit-console.exe");
            fixture.Action = context =>
            {
                context.NUnit(
                    new FilePath[] { "./Test.dll" },
                    new NUnitSettings { ShadowCopy = false });
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/tools/nunit-console.exe\" --propagate-exit-code --no-updates -- \"/Working/Test.dll\" -noshadow", result.Args);
        }
    }
}