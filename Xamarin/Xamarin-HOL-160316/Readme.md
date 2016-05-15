#Xamarin-HOL-160316

2016/03/16にエクセルソフト株式会社様 主催の[ハンズオンセミナー](https://github.com/ytabuchi/XamarinHOL)で勉強した時のソースコードです。

##Phoneword_Droid  
勉強会で作成した時のAndroidネイティブのプロジェクトです。
  
##Phoneword_Droid_Remix  
Phoneword_Droidから、**UIを日本語及び英語で表示できる**ように改造してみました。  
Androidネイティブアプリにて、国際化対応にする時は、**言語ごとにvaluesフォルダー（例 : 日本語の場合、「values-ja」）を作成し、
その中のStrings.xmlにそれぞれの言語に対応した文字列リソースを追加**します。

>参考 : Androidアプリを国際化する - 週末プログラマーのチラシの裏
http://epiphaneia.hatenablog.com/entry/2015/05/05/014930

```xml:values/String.xml
<?xml version="1.0" encoding="utf-8"?>
<resources>
	<!-- デフォルト（ 英語 ）の文字列リソースです。 -->
	<string name="appname">Phoneword_Remix</string>

	<string name="Input">Enter a Phoneword</string>
	<string name="Translate">Translate</string>
	<string name="Call">Call</string>
	<string name="CallTo">Call {0}</string>
	<string name="ComfirmCallTo">Call {0} ?</string>
	<string name="Cancel">Cancel</string>
	<string name="CallHistory">Call History</string>
</resources>
```

```xml:values-ja/String.xml
<?xml version="1.0" encoding="utf-8"?>
<resources>
	<!-- 日本語の文字列リソースです。 -->
	<string name="appname">Phoneword_Remix</string>

	<string name="Input">電話番号を入力</string>
	<string name="Translate">変換</string>
	<string name="Call">発信</string>
	<string name="CallTo">{0} に発信</string>
	<string name="ComfirmCallTo">{0} に発信しますか?</string>
	<string name="Cancel">キャンセル</string>
	<string name="CallHistory">発信履歴</string>
</resources>
```

プログラムからは、```Content.GetString```メソッドに```Resource.String.リソース名（string要素のname属性に指定した文字列）```を指定し、
デバイスの言語に対応した文字列リソースから文字列を取得します。

ちょっとしたテクニックとして、String.xmlのCallToのように**文字列リソースを書式指定の形**にし、
プログラム側では```string.Format```メソッドの書式指定にそれを指定することで、国際化に対応した書式指定文字列を作成することができます。

```c-sharp:文字列リソースを書式指定に利用
string callTo = string.Format( GetString( Resource.String.CallTo ), phoneNumber );

// 上のコードは、以下のコードと等価です。
// デフォルト（英語）の場合
string callTo = string.Format( "Call {0}", phoneNumber );
// 日本語の場合
string callTo = string.Format( "{0} に発信", phoneNumber );
```
