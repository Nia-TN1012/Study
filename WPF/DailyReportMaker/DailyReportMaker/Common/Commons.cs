using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyReportMaker {

	/// <summary>
	///		結果の情報を表します。
	/// </summary>
	/// <typeparam name="TResult">結果情報の型</typeparam>
	public class ResultInfo<TResult> {
		/// <summary>
		///		結果情報を表します。
		/// </summary>
		public TResult Result;
		/// <summary>
		///		メッセージを表します。（例えば、画面下部のステータスバーやメッセージダイアログでの表示に使用します。）
		/// </summary>
		public string Message;
		/// <summary>
		///		追加情報を表します。（例えば、例外がスローされた時にそのメッセージを格納します。）
		/// </summary>
		public string AdditionalInfo;
	}

	/// <summary>
	///		結果情報を持つイベント引数です。
	/// </summary>
	/// <typeparam name="TResultInfo">結果の情報の型</typeparam>
	public class NotifyResultEventArgs<TResultInfo> : EventArgs {

		/// <summary>
		///		結果情報を取得します。
		/// </summary>
		public TResultInfo Result { get; private set; }

		/// <summary>
		///		結果情報から、NotifyResultEventArgsの新しいインスタンスを生成します。
		/// </summary>
		/// <param name="_r">結果情報</param>
		public NotifyResultEventArgs( TResultInfo _r ) {
			Result = _r;
		}
	}

	/// <summary>
	///		結果を通知する時に発生するイベントを処理します。
	/// </summary>
	public delegate void NotifyResultEventHandler<TResultInfo>( object sender, NotifyResultEventArgs<TResultInfo> e );


	/// <summary>
	///		確認メッセージとコールバックメソッドの情報を持つイベント引数です。
	/// </summary>
	/// <example>
	///		// ViewModel側
	///			// 宣言
	///			public event ComfirmEventHandler Comfirm;
	///			// イベント発生
	///			Confirm?.Invoke( this, new ComfirmEventArgs( "メッセージ", Method1 ) );
	///			
	///		// View側
	///			// イベントを構成
	///			ViewModel.Comfirm +=
	///				( sender, e ) => {
	///					// 何か処理をします。（ e.Massageで確認メッセージを取得できます。 ）
	///					
	///					// コールバックの呼び出します（ 間接的に、ViewModelのMethod1を呼び出します ）。
	///					e.Callback();
	///					
	///					// Callbackにnullが指定される可能性がある場合、Callbackにnull条件演算子とInvokeメソッドを付けます。
	///					e.Callback?.Invoke();
	///				};
	/// </example>
	public class ComfirmEventArgs : EventArgs {

		/// <summary>
		///		確認メッセージを取得・設定します。
		/// </summary>
		public string Message { get; private set; }

		/// <summary>
		///		コールバックのデリゲートに登録されているメソッドを取得・設定します。
		/// </summary>
		/// <remarks>登録されたメソッドは、構成したイベント実装側にて、このデリゲートを使って間接的に呼び出すことができます。</remarks>
		public Action Callback { get; private set; }

		/// <summary>
		///		確認メッセージとコールバックのメソッドから、ComfirmEventArgsの新しいインスタンスを生成します。
		/// </summary>
		/// <param name="mes">確認メッセージ</param>
		/// <param name="cb">コールバックメソッド</param>
		public ComfirmEventArgs( string mes, Action cb ) {
			Message = mes;
			Callback = cb;
		}
	}

	/// <summary>
	///		イベントを処理し、確認メッセージを表示したり、コールバックを呼び出したりします。
	/// </summary>
	public delegate void ComfirmEventHandler( object sender, ComfirmEventArgs e );

	/// <summary>
	///		コールバックメソッドの情報を持つイベント引数です。
	/// </summary>
	/// <typeparam name="T">コールバックのパラメーターの型</typeparam>
	public class CallbackEventArgs<T> : EventArgs {

		/// <summary>
		///		コールバックのデリゲートに登録されているメソッドを取得・設定します。
		/// </summary>
		/// <remarks>登録されたメソッドは、構成したイベント実装側にて、このデリゲートを使って間接的に呼び出すことができます。</remarks>
		public Action<T> Callback { get; private set; }

		/// <summary>
		///		コールバックのメソッドから、ComfirmEventArgsの新しいインスタンスを生成します。
		/// </summary>
		/// <param name="cb">コールバックメソッド</param>
		public CallbackEventArgs( Action<T> cb ) {
			Callback = cb;
		}
	}

	/// <summary>
	///		イベントを処理し、コールバックを呼び出します。
	/// </summary>
	public delegate void CallbackEventHandler<T>( object sender, CallbackEventArgs<T> e );


	/// <summary>
	///		双方向メッセージの情報を持つイベント引数です。
	/// </summary>
	/// <typeparam name="TSend">送るメッセージの型</typeparam>
	/// <typeparam name="TReply">コールバックのパラメーターの型</typeparam>
	public class InteractiveMassagingEventArgs<TSend, TReply> : EventArgs {

		/// <summary>
		///		メッセージを取得します。
		/// </summary>
		public TSend Message { get; private set; }

		/// <summary>
		///		コールバックのデリゲートに登録されているメソッドを取得します。
		/// </summary>
		/// <remarks>登録されたメソッドは、構成したイベント実装側にて、このデリゲートを使って間接的に呼び出すことができます。</remarks>
		public Action<TReply> Callback { get; private set; }

		/// <summary>
		///		メッセージとコールバックのメソッドから、InteractiveMassagingEventArgsの新しいインスタンスを生成します。
		/// </summary>
		/// <param name="mes">メッセージ</param>
		/// <param name="cb">コールバックメソッド</param>
		public InteractiveMassagingEventArgs( TSend mes, Action<TReply> cb ) {
			Message = mes;
			Callback = cb;
		}
	}

	/// <summary>
	///		メッセージを送信し、コールバックを呼び出します。
	/// </summary>
	public delegate void InteractiveMassagingEventHandler<TSend, TReply>( object sender, InteractiveMassagingEventArgs<TSend, TReply> e );
}
