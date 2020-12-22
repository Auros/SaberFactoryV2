﻿using System.IO;
using IPA.Utilities;

namespace SaberFactory.Helpers
{
    public static class PathTools
    {
        public static string SaberFactoryUserPath => Path.Combine(UnityGame.UserDataPath, "Saber Factory");

        public static string ToFullPath(string relativePath) => Path.Combine(UnityGame.InstallPath, relativePath);

        public static string ToRelativePath(string path) => path.Substring(UnityGame.InstallPath.Length+1);
    }
}