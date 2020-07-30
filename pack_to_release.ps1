function pack([string]$path) {
    Write-Host "==== ${path} をパッケージ化します" -ForegroundColor Cyan;
    echo $suffix
    dotnet pack $_ -c Release -o nupkgs/release/ --version-suffix $suffix -v minimal;
    sleep 2
}

$paths = "Source/Deptorygen/Deptorygen.csproj",`
    "Source/Deptorygen.Annotations/Deptorygen.Annotations.csproj",`
    "Source/Deptorygen.GenericHost/Deptorygen.GenericHost.csproj"

$ver = cat nupkgs/release/version
$ver = [System.Int32]::Parse($ver) + 1
$suffix = "beta" + $ver

rm nupkgs/release/*.nupkg
$paths | foreach { pack $_ }

echo $ver > nupkgs/release/version