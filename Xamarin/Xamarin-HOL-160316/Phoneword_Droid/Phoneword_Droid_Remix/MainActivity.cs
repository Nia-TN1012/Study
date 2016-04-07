using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Phoneword_Droid_Remix {
	[Activity( Label = "@string/appname", MainLauncher = true, Icon = "@drawable/icon" )]
	public class MainActivity : Activity {

		static readonly List<string> phoneNumbers = new List<string>();

		/// <summary>
		///		このアクティビティが作成された時に実行されます。
		/// </summary>
		/// <param name="bundle">バンドル</param>
		protected override void OnCreate( Bundle bundle ) {

			// バンドルオブジェクトをActivityのOnCreateメソッドに指定し、実行します。
			base.OnCreate( bundle );

			// リソースにある、Layout -> Main.axmlをContentViewにセットします。
			SetContentView( Resource.Layout.Main );

			// ロードされたレイアウトから UI コントロールを取得します。
			EditText phoneNumberText = FindViewById<EditText>( Resource.Id.PhoneNumberText );
			Button translateButton = FindViewById<Button>( Resource.Id.TranslateButton );
			Button callButton = FindViewById<Button>( Resource.Id.CallButton );
			Button callHistoryButton = FindViewById<Button>( Resource.Id.CallHistoryButton );

			// 発信ボタンを Disable にします
			callButton.Enabled = false;
			// 番号を変換するコードを追加します。
			string translatedNumber = string.Empty;

			// 電話番号の変換ボタンをクリックした時のイベントを実装します。
			translateButton.Click +=
				( object sender, EventArgs e ) => {
					// ユーザーのアルファベットの電話番号を電話番号に変換します。
					translatedNumber = Core.PhonewordTranslator.ToNumber( phoneNumberText.Text );
					// 変換後の電話番号が null or 空文字の時
					if( string.IsNullOrWhiteSpace( translatedNumber ) ) {
						// 発信ボタンのテキストを「Call」にし、ボタンを無効にします。
						callButton.Text = GetString( Resource.String.Call );
						callButton.Enabled = false;
					}
					// 変換後の電話番号が null or 空文字以外の時
					else {
						// 発信ボタンのテキストを「Call [電話番号]」にし、ボタンを有効にします。
						callButton.Text = string.Format( GetString( Resource.String.CallTo ), translatedNumber );
						callButton.Enabled = true;
					}
				};

			// 発信ボタンをクリックした時のイベントを構成します。
			callButton.Click +=
				( object sender, EventArgs e ) => {
					// "Call" ボタンがクリックされたら電話番号へのダイヤルを試みます。
					var callDialog = new AlertDialog.Builder( this );
					// ダイアログのメッセージを構成します。
					callDialog.SetMessage( string.Format( GetString( Resource.String.ComfirmCallTo ), translatedNumber ) );
					// ニュートラルボタンとそれをクリックした時のイベントを構成します。
					callDialog.SetNeutralButton( GetString( Resource.String.Call ), delegate {
						// 掛けた番号のリストに番号を追加します。
						phoneNumbers.Add( translatedNumber );
						// Call History ボタンを有効にします。
						callHistoryButton.Enabled = true;
						// 電話を発信するIntentオブジェクトを作成します。
						var callIntent = new Intent( Intent.ActionCall );
						// データ形式は「tell:[電話番号]」です。
						callIntent.SetData( Android.Net.Uri.Parse( "tel:" + translatedNumber ) );
						//StartActivity( callIntent );
					} );
					// キャンセルボタンとそれをクリックした時のイベントを構成します。
					callDialog.SetNegativeButton( GetString( Resource.String.Cancel ), delegate { } );
					// アラートダイアログを表示し、ユーザーのレスポンスを待ちます。
					callDialog.Show();
				};

			// 発信履歴のボタンをクリックした時のイベントを構成します。
			callHistoryButton.Click +=
				( sender, e ) => {
					// CallHistoryActivityに送るIntentを作成します。
					var intent = new Intent( this, typeof( CallHistoryActivity ) );
					// 発信履歴のデータをIntentにセットします。
					intent.PutStringArrayListExtra( "phone_numbers", phoneNumbers );
					StartActivity( intent );
				};

		}
	}
}

