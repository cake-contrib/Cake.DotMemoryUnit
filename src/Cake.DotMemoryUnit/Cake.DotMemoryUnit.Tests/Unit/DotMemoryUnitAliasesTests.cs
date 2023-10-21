using Cake.Common.Tools.XUnit;
using Cake.DotMemoryUnit.Tests.Fixtures;
using Cake.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;
using Xunit;

namespace Cake.DotMemoryUnit.Tests.Unit
{
    public class DotMemoryUnitAliasesTests
    {
        [Fact]
        public void Should_Run_Tool()
        {
            // Given
            var fixture = new DotMemoryUnitFixture();
            fixture.FileSystem.CreateFile("/Working/tools/xunit.console.exe");

            // When
            fixture.Context.DotMemoryUnit(ctx =>
            {
                ctx.XUnit2(new FilePath[] { "./Test.dll" });
            });

            Assert.Single(fixture.ProcessRunner.Results);
        }
    }
}
