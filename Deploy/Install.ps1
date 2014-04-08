param($installPath, $toolsPath, $package, $project)

function MarkDirectoryAsCopyToOutputRecursive($item)
{
    $item.ProjectItems | ForEach-Object { MarkFileASCopyToOutputDirectory($_) }
}

function MarkFileASCopyToOutputDirectory($item)
{
    Try
    {
        $item.Properties.Item("CopyToOutputDirectory").Value = 1
		Write-Host Attached native binary $item.Name as "Copy always" item
    }
    Catch
    {
        MarkDirectoryAsCopyToOutputRecursive($item)
    }
}

# Now mark everything in the a directory as "Copy to newer"
# http://stackoverflow.com/questions/8474253/nuget-how-can-i-change-property-of-files-with-install-ps1-file
MarkDirectoryAsCopyToOutputRecursive($project.ProjectItems.Item("x64"))
MarkDirectoryAsCopyToOutputRecursive($project.ProjectItems.Item("x86"))