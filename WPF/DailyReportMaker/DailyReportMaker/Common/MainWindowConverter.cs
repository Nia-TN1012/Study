using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DailyReportMaker {

	/// <summary>
	///		ウィンドウのアクティブ状態に対応する Border.BorderBrush の値を求めます。
	/// </summary>
	public sealed class WindowBorderBrushConverter : IValueConverter {

		/// <summary>
		///		ウィンドウのアクティブ状態に対応する、Border.BorderBrush の値を求めます。
		/// </summary>
		/// <param name="value">ウィンドウのアクティブ状態</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>ウィンドウのアクティブ状態に対応する、Border.BorderBrush の値</returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) =>
			( value is bool && ( bool )value ) ? parameter as SolidColorBrush : Brushes.LightGray;

		/// <summary>
		///		このメソッドは使用しません。常に null を返します。
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns>null</returns>
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) =>
			null;
	}

	/// <summary>
	///		ウィンドウサイズに対応する Border.Thickness の値を求めます。
	/// </summary>
	public sealed class BorderThicknessByWindowStateConverter : IValueConverter {

		/// <summary>
		///		ウィンドウサイズに対応する、Border.Thickness の値を求めます。
		/// </summary>
		/// <param name="value">ウィンドウサイズ</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>ウィンドウサイズに対応する、Border.Thickness の値</returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) =>
			// 最大化時にBorder.Thicknessの値を8.0に設定することで、コントロールが画面の外にはみ出ないようにします。
			value is WindowState && ( WindowState )value == WindowState.Maximized ? 8.0 : 1.0;

		/// <summary>
		///		このメソッドは使用しません。常に null を返します。
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns>null</returns>
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) =>
			null;
	}

	/// <summary>
	///		ウィンドウのアクティブ状態に対応する、タイトルバーの文字色を求めます。
	/// </summary>
	public sealed class CaptionBarForegroundConverter : IValueConverter {

		/// <summary>
		///		ウィンドウのアクティブ状態に対応する、タイトルバーの文字色を求めます。
		/// </summary>
		/// <param name="value">ウィンドウのアクティブ状態</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>ウィンドウのアクティブ状態に対応する、タイトルバーの文字色</returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) =>
			( value is bool && ( bool )value ) ? parameter as SolidColorBrush : Brushes.Gray;

		/// <summary>
		///		このメソッドは使用しません。常に null を返します。
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns>null</returns>
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) =>
			null;
	}

	/// <summary>
	///		ウィンドウのアクティブ状態にに対応する、タイトルバーの背景色を求めます。。
	/// </summary>
	public sealed class CaptionBarBackgroundConverter : IValueConverter {

		/// <summary>
		///		ウィンドウのアクティブ状態に対応する、タイトルバーの文字色を求めます。
		/// </summary>
		/// <param name="value">ウィンドウのアクティブ状態</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>ウィンドウのアクティブ状態に対応する、タイトルバーの背景色</returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) =>
			value is bool && ( bool )value ? parameter as SolidColorBrush : Brushes.LightGray;

		/// <summary>
		///		このメソッドは使用しません。常に null を返します。
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns>null</returns>
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => null;
	}


}
