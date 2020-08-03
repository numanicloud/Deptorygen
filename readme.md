# Deptorygen

DeptorygenはC#コードに対してアナライザーとCodeFixを提供することで、
クラス間の依存関係を解決しながらインスタンスを生成するDIコンテナの役割を果たします。

プロジェクト名は「Dependency Factory Generator」の略です。

## 入手

Nuget Galleryにて、以下のパッケージをインストールしてください。

* Deptorygen
* Deptorygen.Annotations

GenericHostとの連携機能が必要であれば、以下のパッケージもインストールしてください。

* Deptorygen.GenericHost

## 実行

Visual Studio 2019で動作確認しています。

動作させるために、`%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\Common7\IDE` に .NET Framework向けの `netstandard.dll` を配置する必要があります。
このDLLは恐らく、 `%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Microsoft\Microsoft.NET.Build.Extensions\net461\lib\nestandard.dll` にあります。

Deptorygenパッケージをプロジェクトにインストールしたら、まずはVisual Studioを再起動する必要があります。
再起動後、`[Factory]`属性がついているインターフェースに対して出るクイックヒント(電球マーク💡)を通じてコード生成を実行することができます。

## 例

```csharp
using System;
using Deptorygen.Annotations;
using UseDeptorygen.Infra;

namespace UseDeptorygen.Samples.BasicDependency
{
	class Service
	{
		public void Show()
		{
			Console.WriteLine("This is Service!");
		}
	}

	class Client
	{
		private readonly Service _service;

		public Client(Service service)
		{
			_service = service;
		}

		public void Execute()
		{
			Console.WriteLine("# Client");
			_service.Show();
		}
	}

	[Factory]
	interface IFactory
	{
        // Serviceクラスを解決してもらいたい
		Service ResolveService();
        // ClientクラスはServiceクラスに依存している。
        // これも解決してもらいたい
		Client ResolveClient();
	}

	class BasicDependencySample : ISample
	{
		public void Run()
		{
			var factory = new Factory();
			factory.ResolveClient().Execute();
			factory.Dispose();
		}
	}
}
```

生成されるコードは以下の通りです。

```csharp
// <autogenerated />
using System;
using System.Collections.Generic;

namespace UseDeptorygen.Samples.BasicDependency
{
    internal partial class Factory : IFactory
        , IDisposable
    {
        private Service? _ResolveServiceCache;
        private Client? _ResolveClientCache;

        public Factory()
        {
        }

        public Service ResolveService()
        {
            return _ResolveServiceCache ??= new Service();
        }

        public Client ResolveClient()
        {
            return _ResolveClientCache ??= new Client(ResolveService());
        }

        public void Dispose()
        {
        }
    }
}
```

実行結果は以下の通りです。

```
# Client
This is Service!
```

## 詳しい使い方

[リポジトリ内のマニュアル](https://github.com/NumAniCloud/Deptorygen/blob/master/Documents/Index.md)をご覧ください。
