using System.Text;

namespace Core {
	/// <summary>
	///		電話番号をテンキーの配置で等価な数字に変換する機能を提供します。
	/// </summary>
	public static class PhonewordTranslator {

		/// <summary>
		///		指定した電話番号をテンキーの配置で等価な数字に変換します。
		/// </summary>
		/// <param name="raw">電話番号</param>
		/// <returns>テンキーの配置で等価な数字に変換した電話番号</returns>
		public static string ToNumber( string raw ) {

			// 電話番号が未入力時は、空文字を返します。
			if( string.IsNullOrWhiteSpace( raw ) ) {
				return "";
			}

			// 小文字を大文字に変換します。
			raw = raw.ToUpperInvariant();
			// 変換後の電話番号を格納する、ビルダー
			var newNumber = new StringBuilder();

			foreach( var c in raw ) {
				// 空白スペース、ハイフン、数値はそのまま追加します。
				if( " -0123456789".Contains( c ) ) {
					newNumber.Append( c );
				}
				// アルファベットはテンキーの配置で等価な数字に変換します。
				else {
					var result = TranslateToNumber( c );
					if( result != null ) {
						newNumber.Append( result );
					}
				}
				// 数字以外の文字はスキップします。
			}
			return newNumber.ToString();
		}

		/// <summary>
		///		指定した文字が、指定した文字列の中に含まれているかどうか判別します。
		/// </summary>
		/// <param name="keyString">探索元の文字列</param>
		/// <param name="c">探索する文字</param>
		/// <returns>keyString内にcが含まれる時 : true / そうでない時 : false</returns>
		/// <remarks>stringクラスで標準定義されているContainsメソッドは、パラメーターの型がstring型です。</remarks>
		static bool Contains( this string keyString, char c ) =>
			keyString.IndexOf( c ) >= 0;

		/// <summary>
		///		アルファベットをテンキーの配置で等価な数字に変換します。
		/// </summary>
		/// <param name="c">アルファベット</param>
		/// <returns>テンキーの配置で等価な数字</returns>
		static int? TranslateToNumber( char c ) {
			if( "ABC".Contains( c ) ) {
				return 2;
			}
			else if( "DEF".Contains( c ) ) {
				return 3;
			}
			else if( "GHI".Contains( c ) ) {
				return 4;
			}
			else if( "JKL".Contains( c ) ) {
				return 5;
			}
			else if( "MNO".Contains( c ) ) {
				return 6;
			}
			else if( "PQRS".Contains( c ) ) {
				return 7;
			}
			else if( "TUV".Contains( c ) ) {
				return 8;
			}
			else if( "WXYZ".Contains( c ) ) {
				return 9;
			}
			return null;
		}
	}
}