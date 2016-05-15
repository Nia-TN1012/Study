using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DailyReportMaker {

	/// <summary>
	///		ウィンドウのリサイズモードに対応する、最大化・最小化ボタンの表示・非表示の状態を求めます。
	/// </summary>
	/// <remarks>参考記事：http://qiita.com/nia_tn1012/items/7671dc15496244dcdf75</remarks>
	public sealed class ResizeCaptionButtonVisibilityConverter : IValueConverter {

		/// <summary>
		///		ウィンドウのリサイズモードに対応する、最大化・最小化ボタンの表示・非表示の状態を求めます。
		/// </summary>
		/// <param name="value">ウィンドウのリサイズモード</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>ウィンドウのリサイズモードがNoResize以外ならVisible、NoResizeならCollapsed</returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) =>
			// ResizeModeがNoResizeの時はVisibility.Collapsedに、それ以外の時はVisibility.Visibleにします。
			value is ResizeMode && ( ResizeMode )value != ResizeMode.NoResize ? Visibility.Visible : Visibility.Collapsed;

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

	/// <summary>
	///		ウィンドウのリサイズモードに対応する、最大化ボタンの有効・無効の状態を求めます。
	/// </summary>
	/// <remarks>参考記事：http://qiita.com/nia_tn1012/items/7671dc15496244dcdf75</remarks>
	public sealed class MaximizeCaptionButtonEnableConverter : IValueConverter {

		/// <summary>
		///		ウィンドウのリサイズモードに対応する、最大化ボタンの有効・無効の状態を求めます。
		/// </summary>
		/// <param name="value">ウィンドウのリサイズモード</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>ウィンドウのリサイズモードがCanMinimize以外なら有効（ true ）、CanMinimizeなら無効（ false ）</returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) =>
			value is ResizeMode && ( ResizeMode )value != ResizeMode.CanMinimize;

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

	/// <summary>
	///		ウィンドウのサイズに対応する、最大化ボタンの表示文字列を求めます。
	/// </summary>
	public sealed class MaximizeCaptionButtonContentConverter : IValueConverter {

		/// <summary>
		///		ウィンドウのサイズに対応する、最大化ボタンの表示文字列を求めます。
		/// </summary>
		/// <param name="value">ウィンドウのサイズ</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>ウィンドウのサイズがMaximizedなら 2（ 元に戻す ）、そうでなければ 1（ 最大化 ）</returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) =>
			// Malettフォントでは「1」が「最大化」、「2」が「元に戻す」です。
			value is WindowState && ( WindowState )value == WindowState.Maximized ? "2" : "1";

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

	/// <summary>
	///		ウィンドウのサイズに対応する、最大化ボタンのツールチップの文字列を求めます。
	/// </summary>
	public sealed class MaximizeCaptionButtonTooltipConverter : IValueConverter {

		/// <summary>
		///		ウィンドウのサイズに対応する、最大化ボタンのツールチップの文字列を求めます。
		/// </summary>
		/// <param name="value">ウィンドウのサイズ</param>
		/// <param name="targetType">ターゲットの型（使用しません）</param>
		/// <param name="parameter">パラメーター（使用しません）</param>
		/// <param name="culture">カルチャ情報（使用しません）</param>
		/// <returns>ウィンドウのサイズに対応する、最大化ボタンのツールチップの文字列</returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) =>
			value is WindowState && ( WindowState )value == WindowState.Maximized ? "元に戻す" : "最大化";

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