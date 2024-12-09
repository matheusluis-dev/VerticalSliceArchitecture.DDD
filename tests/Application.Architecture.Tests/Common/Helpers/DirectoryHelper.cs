namespace Application.Architecture.Tests.Common.Helpers;

public static class DirectoryHelper
{
    public static bool TryGetDirectoryInSolution(
        string subDirectory,
        out DirectoryInfo? directoryInfo
    )
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null)
        {
            directoryInfo = new DirectoryInfo(Path.Combine(directory.FullName, subDirectory));
            if (directoryInfo.Exists)
                return true;

            directory = directory.Parent;
        }

        directoryInfo = null;
        return false;
    }
}
