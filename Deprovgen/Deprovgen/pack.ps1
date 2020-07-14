$ver = cat nupkgs/version
$ver = [System.Int32]::Parse($ver) + 1

rm nupkgs/*.nupkg
dotnet pack Deprovgen.csproj -c Debug -o nupkgs\ --version-suffix $ver --include-source -v minimal

echo $ver > nupkgs/version