using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;

namespace Phoneword_Droid_Remix {
	/// <summary>
	///		発信履歴のActivityクラスを定義します。
	/// </summary>
	/// <remarks>ListAdapterを使用するため、ListActivityを継承します。</remarks>
	[Activity( Label = "@string/CallHistory" )]
	public class CallHistoryActivity : ListActivity {

		/// <summary>
		///		このアクティビティが作成された時に実行されます。
		/// </summary>
		/// <param name="bundle">バンドル</param>
		protected override void OnCreate( Bundle bundle ) {

			// バンドルオブジェクトをListActivityのOnCreateメソッドに指定し、実行します。
			base.OnCreate( bundle );

			// Intent経由で、MainActivityから発信履歴のデータを取得します。
			var phoneNumbers = Intent.Extras.GetStringArrayList( "phone_numbers" ) ??
								new string[0];

			// ListAdapterを使って、SimpleListItem1に発信履歴のアイテムをセットします。
			ListAdapter = new ArrayAdapter<string>(
				this,
				Android.Resource.Layout.SimpleListItem1,
				phoneNumbers
			);
		}
	}
}