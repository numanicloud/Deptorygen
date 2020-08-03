# Deptorygen マニュアル

## ガイド

実際にDeptorygenを使用する手順の例を紹介します。

これらのガイドは、Deptorygenを使用する際のベストプラクティスの紹介ですが、
Deptorygenの抱える問題の紹介も含みます。
その部分については、そういった問題に対処するためのベストプラクティスの紹介といった趣になります。
問題の一部は、順次修正されていくかもしれません。

* [基本的な使い方](Guides/BasicStyle.md)
* [コンストラクタで意外な引数を要求されたら](Guides/Constructor.md)
* [キャプチャ、ミックスイン、ファクトリーのファクトリー](Guides/FactoryStructure.md)
* [ファクトリーを別のアセンブリに提供する](Guides/ExportType.md)

## 詳細解説

* [ファクトリー自体のアクセシビリティはどう決まるか](Features/Accessibility.md)

## サンプル集

機能ごとのリファレンスとしてサンプルを用意しました。

[サンプルの読み方](Samples/Schema.md)

* [基本の使い方](Samples/Basic.md)
* [生成したいオブジェクトの依存先も生成したい](Samples/BasicDependency.md)
* [キャッシュできていることを確認](Samples/UseCache.md)
* [生成時にキャッシュしないようにしたい](Samples/Transient.md)
* [どのような依存先がコンストラクターで要求されるか](Samples/AutoAndManual.md)
* [解決メソッドに依存先の一部を直接渡す](Samples/Parameterize.md)
* [他のファクトリー定義を取り込む（ミックスイン）](Samples/Mixin.md)
* [依存関係の解決を別のファクトリーに委譲する（キャプチャ）](Samples/Capture.md)
* [ミックスインとキャプチャを組み合わせる](Samples/CaptureMixin.md)
* [インターフェースを戻り値として、実際には実装を返す](Samples/Resolution.md)
* [コレクションに対してインスタンスを注入する](Samples/Collection.md)
* [IDisposableなインスタンスが破棄されることを確認する](Samples/Disposable.md)
* [GenericHostと連携する](Samples/GenericHost.md)