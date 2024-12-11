namespace Application.Architecture.Tests.Common.Helpers;

internal static class DirectoryHelper
{
    private static readonly Dictionary<string, DirectoryInfo> _cache = [];

    public static DirectoryInfo GetDirectoryInSolution(string subDirectory)
    {
        if (_cache.TryGetValue(subDirectory, out var cacheDirectoryInfo))
            return cacheDirectoryInfo;

        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory is not null)
        {
            var directoryInfo = new DirectoryInfo(Path.Combine(directory.FullName, subDirectory));
            if (directoryInfo.Exists)
            {
                _cache.Add(subDirectory, directoryInfo);
                return directoryInfo;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException($"Directory '{subDirectory}' not found");
    }
}
