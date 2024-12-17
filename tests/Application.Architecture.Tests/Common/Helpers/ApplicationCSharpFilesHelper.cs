namespace Application.Architecture.Tests.Common.Helpers;

using System;
using System.Collections.Generic;

internal static class ApplicationCSharpFilesHelper
{
    private static readonly Lazy<IEnumerable<FileInfo>> _cache = new(LoadFiles);

    internal static IEnumerable<FileInfo> Files => _cache.Value;

    private static IEnumerable<FileInfo> LoadFiles()
    {
        var directoryInfo = DirectoryHelper.GetDirectoryInSolution(Paths.Application);

        // TODO: Find a more performatic way to do this
        return directoryInfo
            .EnumerateFiles("*.cs", SearchOption.AllDirectories)
            .Where(file =>
                !file.FullName.Contains(
                    $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}",
                    StringComparison.OrdinalIgnoreCase
                )
                && !file.FullName.Contains(
                    $"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}",
                    StringComparison.OrdinalIgnoreCase
                )
            );
    }
}
