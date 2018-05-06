using System.Reflection;
using System.Resources;

namespace VirtualFileSystem
{
    internal static class StringResources
    {
        public static ResourceManager Manager { get; } = new ResourceManager(
            $"{typeof(StringResources).Namespace}.Properties.Resources", typeof(StringResources).Assembly);

    }
}
