using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace DailyReportMaker {
	/// <summary>
	///		MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {

		private OpenFileDialog ofdDRMData;

		private SaveFileDialog sfdDRMData;

		private SaveFileDialog sfdDailyReport;

		public MainWindow() {
			InitializeComponent();

			ofdDRMData = new OpenFileDialog();
			ofdDRMData.Title = "日報データファイルを開く";
			ofdDRMData.Filter = "日報データファイル(*.drmita)|*.drmita|すべてのファイル(*.*)|*.*";
			ofdDRMData.FilterIndex = 1;

			sfdDRMData = new SaveFileDialog();
			sfdDRMData.Title = "日報データファイルを保存";
			sfdDRMData.Filter = "日報データファイル(*.drmita)|*.drmita|すべてのファイル(*.*)|*.*";
			sfdDRMData.FilterIndex = 1;

			sfdDailyReport = new SaveFileDialog();
			sfdDailyReport.Title = "日報データを保存";
			sfdDailyReport.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
			sfdDailyReport.FilterIndex = 1;

		}

		#region キャプションボタンのイベント

		/// <summary>
		///		閉じる及び、終了ボタンを押した時のイベントです。
		/// </summary>
		private void CloseWindow( object sender, ExecutedRoutedEventArgs e ) {
			// アプリを終了します。
			SystemCommands.CloseWindow( this );
		}

		/// <summary>
		///		最小化ボタンを押した時のイベントです。
		/// </summary>
		private void MinimizeWindow( object sender, ExecutedRoutedEventArgs e ) {
			// ウインドウを最小化します。
			SystemCommands.MinimizeWindow( this );
		}

		/// <summary>
		///		最大化ボタンを押した時のイベントです。
		/// </summary>
		private void MaximizeOrRestoreWindow( object sender, ExecutedRoutedEventArgs e ) {
			// ウィンドウを最大化（ もしくは元のサイズに ）します。
			if( WindowState != WindowState.Maximized )
				SystemCommands.MaximizeWindow( this );
			else
				SystemCommands.RestoreWindow( this );
		}

		#endregion

		/// <summary>
		///		コンテキストメニュー付きのボタンをクリックした時に実行します。
		/// </summary>
		/// <param name="sender">イベントを発生させたコントロール</param>
		/// <param name="e">イベント情報</param>
		private void ButtonWithContextMenu_Click( object sender, RoutedEventArgs e ) {
			// ボタンコントロールにキャストします。
			var button = sender as Button;

			// コンテキストメニューが含まれているか判別します。
			if( button?.ContextMenu != null ) {
				button.ContextMenu.PlacementTarget = button;
				button.ContextMenu.Placement = PlacementMode.Bottom;
				// コンテキストメニューを表示します。
				button.ContextMenu.IsOpen = true;
			}

		}

		private void MainViewModel_ComfirmAction( object sender, ComfirmEventArgs e ) {

			if( MessageBox.Show( e.Message, "確認", MessageBoxButton.YesNo, MessageBoxImage.Question ) == MessageBoxResult.Yes ) {
				e.Callback?.Invoke();
			}

		}

		private void MainViewModel_LoadDRMInputFileAction( object sender, CallbackEventArgs<string> e ) {
			ofdDRMData.FileName = "";
			if( ofdDRMData.ShowDialog() ?? false ) {
				e.Callback?.Invoke( ofdDRMData.FileName );
			}
		}

		private void MainViewModel_SaveDRMInputFileAction( object sender, CallbackEventArgs<string> e ) {
			sfdDRMData.FileName = "";
			if( sfdDRMData.ShowDialog() ?? false ) {
				e.Callback?.Invoke( sfdDRMData.FileName );
			}
		}

		private void MainViewModel_SaveDRAction( object sender, InteractiveMassagingEventArgs<string, string> e ) {
			sfdDailyReport.FileName = $"{e.Message}.txt";
			if( sfdDailyReport.ShowDialog() ?? false ) {
				e.Callback?.Invoke( sfdDailyReport.FileName );
			}
		}

		/// <summary>
		///		ハイパーリンクをクリックした時に発生するイベント処理です。
		/// </summary>
		private void Hyperlink_RequestNavigate( object sender, RequestNavigateEventArgs e ) {
			Process.Start( e.Uri.AbsoluteUri );
		}
	}
}
