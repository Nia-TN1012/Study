using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DailyReportMaker {

	/// <summary>
	///		boolの値に対応する、Visibilityの値を求めます。
	/// </summary>
	public sealed class BoolToVisibilityConverter : IValueConverter {

		/// <summary>
		///		boolの値に対応する、Visibilityの値を求めます。
		/// </summary>
		/// <param name="value">boolの値</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>boolの値がtrueならVisible、falseならCollapsed</returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) =>
			( value is bool && ( bool )value ) ? Visibility.Visible : Visibility.Collapsed;

		/// <summary>
		///		Visibilityの値に対応する、boolの値を求めます。
		/// </summary>
		/// <param name="value">Visibilityの値</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>Visibilityの値がVisibleならtrue、そうでなければfalse</returns>
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) =>
			value is Visibility && ( Visibility )value == Visibility.Visible;
	}
}
