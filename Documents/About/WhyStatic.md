# 静的コード生成の利点

C#向けの多くのDIコンテナは実行時にリフレクションを利用してインスタンスを生成します。
これに筆者は不満点がいくつかありました。

* インスタンスを生成する手順がどうなっているかを知ることが難しい
* 型を登録し忘れている場合、コンパイル時にエラーが出ず実行時に判明する
* インスタンスを生成する際に追加の引数を渡せないDIコンテナが多い
* インスタンスの寿命をより複雑に管理したい

Deptorygenでは、依存関係の解決を静的コード生成を通じて行うことで問題を解消します。

* インスタンスを生成するコードが生成されるので、それを見ればどのような手順で生成されているか理解できます。([基本的な使い方](../Guides/BasicStyle.md))
* 型を登録し忘れている場合エラーが出るか、あるいは不足している変数を生成されたクラスのコンストラクタを見て知ることができます。([コンストラクタで意外な引数を要求されたら](../Guides/Constructor.md))
* インスタンスを生成する際には追加の引数を渡すことができます。([解決メソッドに直接オブジェクトを渡す](../Samples/Parameterize.md))
* サービスクラスのインスタンスの寿命はファクトリークラスのインスタンスの寿命を基準に決定されますので、継承や委譲を駆使してインスタンスの寿命を複雑に管理することができます。([ファクトリーを別のアセンブリに提供する](../Guides/ExportType.md),[依存関係の解決に別のファクトリーも利用する(キャプチャ)](../Samples/Capture.md)など)