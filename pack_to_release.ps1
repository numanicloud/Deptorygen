function pack([string]$path) {
    Write-Host "==== ${path} をパッケージ化します" -ForegroundColor Cyan;
    dotnet pack $_ -c Release -o nupkgs/release/ --version-suffix $ver -v minimal;
    sleep 2
}

$paths = "Source/Deptorygen/Deptorygen.csproj",`
    "Source/Deptorygen.Annotations/Deptorygen.Annotations.csproj"

$ver = cat nupkgs/release/version
$ver = [System.Int32]::Parse($ver) + 1

rm nupkgs/release/*.nupkg
$paths | foreach { pack $_ }

echo $ver > nupkgs/release/version