using System;
using System.Globalization;
using System.Windows.Data;

namespace DailyReportMaker {

	/// <summary>
	///		boolの値に対応する、bool?の値を求めます。
	/// </summary>
	public sealed class BoolToNullableBoolConverter : IValueConverter {

		/// <summary>
		///		boolの値に対応する、bool?の値を求めます。
		/// </summary>
		/// <param name="value">boolの値</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>boolの値</returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) =>
			value is bool && ( bool )value;

		/// <summary>
		///		Bool?の値に対応する、boolの値を求めます。
		/// </summary>
		/// <param name="value">Bool?の値</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>bool?の値がtrueならtrue、falseもしくはnullならfalse</returns>
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) =>
			value is bool? && ( ( bool? )value ?? false );
	}
}
