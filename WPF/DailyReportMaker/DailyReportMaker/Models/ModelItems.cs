using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DailyReportMaker {
	
	/// <summary>
	///		日報のTA情報を表します。
	/// </summary>
	public class TAInfo {
		/// <summary>
		///		日報の日付を取得・設定します。
		/// </summary>
		public DateTime Date { get; set; } = DateTime.Today;

		/// <summary>
		///		教室番号を取得・設定します。
		/// </summary>
		public string RoomName { get; set; } = "";

		/// <summary>
		///		TAの氏名を取得・設定します。
		/// </summary>
		public string TAName { get; set; } = "";

		/// <summary>
		///		勤務開始時間を取得・設定します。
		/// </summary>
		public DateTime StartTime { get; set; } = DateTime.Today;

		/// <summary>
		///		勤務終了時間を取得・設定します。
		/// </summary>
		public DateTime EndTime { get; set; } = DateTime.Today;

		/// <summary>
		///		勤怠の詳細情報・備考を取得・設定します。
		/// </summary>
		public string Description { get; set; } = "";

	}

	/// <summary>
	///		業務時間の項目を表します。
	/// </summary>
	public class WorkTimeItem {
		/// <summary>
		///		開始時間を取得・設定します。
		/// </summary>
		public DateTime StartTime { get; set; } = DateTime.Now;

		/// <summary>
		///		終了時間を取得・設定します。
		/// </summary>
		public DateTime EndTime { get; set; } = DateTime.Now.AddHours( 1 );

		/// <summary>
		///		業務内容を取得・設定します。
		/// </summary>
		public string Description { get; set; } = "";

		/// <summary>
		///		備考を表します。
		/// </summary>
		public string Remarks { get; set; } = "";
	}

	/// <summary>
	///		業務の概要を表します。
	/// </summary>
	public class WorkSummary {
		/// <summary>
		///		業務時間の項目リストを取得・設定します。
		/// </summary>
		public ObservableCollection<WorkTimeItem> WorkingOverviewList { get; private set; } = new ObservableCollection<WorkTimeItem>();

		/// <summary>
		///		利用者の状況を取得・設定します。
		/// </summary>
		public string AboutUser { get; set; } = "";
	}

	/// <summary>
	///		PCやプリンター、アプリなどのトラブル情報を表します。
	/// </summary>
	public class FailureInfoItem {

		/// <summary>
		///		種別名を取得・設定します。
		/// </summary>
		public string TypeName { get; set; } = "";

		/// <summary>
		///		PC名、プリンター名、アプリ名を取得・設定します。
		/// </summary>
		public string Name { get; set; } = "";

		/// <summary>
		///		発生日時を取得・設定します。
		/// </summary>
		public DateTime OccuredTime { get; set; } = DateTime.Now;

		/// <summary>
		///		詳細情報を取得・設定します。
		/// </summary>
		public string Description { get; set; } = "";

	}

	/// <summary>
	///		ユーザーからの質問を表します。
	/// </summary>
	public class QuestionItem {
		/// <summary>
		///		質問日時を取得・設定します。
		/// </summary>
		public DateTime QuestionTime { get; set; } = DateTime.Now;

		/// <summary>
		///		質問事項を取得・設定します。
		/// </summary>
		public string Question { get; set; } = "";

		/// <summary>
		///		質問に対する回答を取得・設定します。
		/// </summary>
		public string Answer { get; set; } = "";
	}

	/// <summary>
	///		不正印刷の項目を表します。
	/// </summary>
	public class InjusticePrintItem {
		/// <summary>
		///		発見した時刻を取得・設定します。
		/// </summary>
		public DateTime FoundTime { get; set; } = DateTime.Now;

		/// <summary>
		///		不正印刷をしたユーザーを取得・設定します。
		/// </summary>
		public string User { get; set; } = "";

		/// <summary>
		///		不正印刷したファイル名を取得・設定します。
		/// </summary>
		public string FileName { get; set; } = "";

		/// <summary>
		///		詳細情報を取得・設定します。
		/// </summary>
		public string Description { get; set; } = "";
	}

	/// <summary>
	///		2重ログインの項目を表します。
	/// </summary>
	public class DuplicateLoginItem {
		/// <summary>
		///		発見した時刻を取得・設定します。
		/// </summary>
		public DateTime FoundTime { get; set; } = DateTime.Now;

		/// <summary>
		///		ユーザー名を取得・設定します。
		/// </summary>
		public string User { get; set; } = "";

		/// <summary>
		///		詳細情報を取得・設定します。
		/// </summary>
		public string Description { get; set; } = "";
	}

	/// <summary>
	///		遺失物を表します。
	/// </summary>
	public class LostSumthingItem {
		/// <summary>
		///		発見した時刻を取得・設定します。
		/// </summary>
		public DateTime FoundTime { get; set; } = DateTime.Now;

		/// <summary>
		///		発見した場所を取得・設定します。
		/// </summary>
		public string FoundPlace { get; set; } = "";

		/// <summary>
		///		遺失物の名前を取得・設定します。
		/// </summary>
		public string Name { get; set; } = "";

		/// <summary>
		///		備考を取得・設定します。
		/// </summary>
		public string Remarks { get; set; } = "";
	}

	/// <summary>
	///		その他の注意を表します。
	/// </summary>
	public class OtherMatterItem {

		/// <summary>
		///		内容を取得・設定します。
		/// </summary>
		public string Description { get; set; } = "";

		/// <summary>
		///		対象人数を取得・設定します。
		/// </summary>
		public int Num { get; set; } = 0;
	}

}
