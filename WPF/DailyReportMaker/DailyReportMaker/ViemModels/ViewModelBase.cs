using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Nia_Tech.ModelExtentions;
using System.Windows.Input;

namespace DailyReportMaker {

	/// <summary>
	///		ViewModelにプロパティ変更通知機能と、データ検証機能を追加します。
	/// </summary>
	public abstract class ViewModelBase : NotifyPropertyChangedHelper, INotifyDataErrorInfo {

		/// <summary>
		///		プロパティ名とデータの検証エラー情報のペアを管理するコレクションを表します。
		/// </summary>
		protected Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

		/// <summary>
		///		データの検証エラーがあるかどうかの値を取得します。
		/// </summary>
		public bool HasErrors =>
			errors.Any();

		/// <summary>
		///		データの検証エラー情報が変更された時に発生します。
		/// </summary>
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		/// <summary>
		///		ErrorsChangedのイベントハンドラーを取得します。
		/// </summary>
		public EventHandler<DataErrorsChangedEventArgs> ErrorsChangedFromThis =>
			ErrorsChanged;

		/// <summary>
		///		指定したプロパティのデータの検証エラー情報を取得します。
		/// </summary>
		/// <param name="propertyName">プロパティ名</param>
		/// <returns>指定したプロパティのデータの検証エラー情報</returns>
		public IEnumerable GetErrors( [CallerMemberName]string propertyName = null ) =>
			errors.ContainsKey( propertyName ) ? errors[propertyName] : null;

		/// <summary>
		///		指定したプロパティのデータの検証エラー情報の変更を通知します。
		/// </summary>
		/// <param name="propertyName">プロパティ名</param>
		protected void NotifyErrorsChanged( [CallerMemberName]string propertyName = null ) {
			ErrorsChanged?.Invoke( this, new DataErrorsChangedEventArgs( propertyName ) );
		}
	}

	#region ActionCommandの定義

	/// <summary>
	///		Executeメソッド、CanExecuteメソッドで実行するデリゲートを登録することができるコマンドを定義します。
	/// </summary>
	/// <remarks>UI上でボタンを押した時の動作を定義します。</remarks>
	public class ActionCommand : ICommand {
		/// <summary>
		///		ViewModelBaseの参照を表します。
		/// </summary>
		private ViewModelBase viewModelBase;

		/// <summary>
		///		Executeメソッドで、実行する処理のデリゲートを表します。
		/// </summary>
		private Action<object> action;

		/// <summary>
		///		CanExecuteメソッドで、実行可能かどうか判定するための式のデリゲートを表します。
		/// </summary>
		private Func<object, bool> predicate;

		/// <summary>
		///		ReadBTCInputDataCommandクラスの新しいインスタンスを生成します。
		/// </summary>
		/// <param name="vm">ViewModelBaseの参照</param>
		/// <param name="act">Executeメソッドで、実行する処理</param>
		/// <param name="prdct">CanExecuteメソッドで、このコマンドが実行可能かどうか判定するための式</param>
		public ActionCommand( ViewModelBase vm, Action<object> act = null, Func<object, bool> prdct = null ) {
			viewModelBase = vm;
			action = act;
			predicate = prdct;
			// ViewModelのプロパティ変更通知と連動させます。
			viewModelBase.PropertyChanged +=
				( sender, e ) =>
					CanExecuteChanged?.Invoke( sender, e );
		}

		/// <summary>
		///		実行可能かどうか判定します。
		/// </summary>
		/// <param name="parameter">パラメーター</param>
		/// <returns>true：実行できます。 / false：実行できません。</returns>
		/// <remarks>バインド先のコントロールのIsEnabledに対応しています。predicateがnullの場合、常にtrueが返ります。</remarks>
		public bool CanExecute( object parameter = null ) =>
			predicate?.Invoke( parameter ) ?? true;

		/// <summary>
		///		CanExecuteが変更されたことを通知するイベントハンドラーです。
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary>
		///		コマンドを実行します。
		/// </summary>
		/// <param name="parameter">コマンドのパラメーター</param>
		/// <remarks>actionがnullの場合、何もしません。</remarks>
		public void Execute( object parameter = null ) =>
			action?.Invoke( parameter );
	}

	#endregion
}
