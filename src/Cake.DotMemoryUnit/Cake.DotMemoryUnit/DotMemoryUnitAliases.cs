using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.DotMemoryUnit
{
    [CakeAliasCategory("DotMemoryUnit")]
    public static class DotMemoryUnitAliases
    {
        [CakeMethodAlias]
        [CakeNamespaceImport("Cake.DotMemoryUnit")]
        public static void DotMemoryUnit(
            this ICakeContext context,
            Action<ICakeContext> action,
            DotMemoryUnitSettings? settings = null)
        {
            settings ??= new DotMemoryUnitSettings();

            var tool = new DotMemoryUnitTool(
                context.FileSystem, context.Environment,
                context.ProcessRunner, context.Tools);

            tool.Run(context, action, settings);
        }
    }
}
