using System;
using System.IO;

namespace gr
{
    internal static class AssetHelper
    {
        private static readonly string s_assetRoot = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        internal static string GetPath(string assetPath)
        {
            return Path.Combine(s_assetRoot, assetPath);
        }
    }
}