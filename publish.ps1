$key = Read-Host "Nuget APIキーを入力"
$version = Read-Host "パッケージ バージョンを入力"

cd nupkgs/release

$names = "Deptorygen.${version}.nupkg",`
    "Deptorygen.Annotations.${version}.nupkg",`
    "Deptorygen.GenericHost.${version}.nupkg"

$names | foreach { dotnet nuget push $_ -k $key -s https://api.nuget.org/v3/index.json }

cd ../../