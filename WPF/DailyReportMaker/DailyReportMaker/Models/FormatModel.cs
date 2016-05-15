using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyReportMaker {

	/// <summary>
	///		書式を定義します。
	/// </summary>
	class FormatModel {

		/// <summary>
		///		日報データのプレビュー（ タイトル ）の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 教室番号</para>
		///		<para>{1} : 日付</para>
		///		<para>{2} : TAの氏名</para>
		/// </remarks>
		public static string DailyReportPreviewTitle { get; private set; } = "日報：{0} {1:MM/dd} {2}";

		/// <summary>
		///		項目のタイトルの書式を取得します。
		/// </summary>
		/// <remarks>{0} : サブタイトル</remarks>
		public static string SubTitle { get; private set; } = "◆ {0}";

		/// <summary>
		///		勤怠概要の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 勤務開始時刻</para>
		///		<para>{1} : 勤務終了時刻</para>
		///		<para>{2} : 詳細情報・備考</para>
		/// </remarks>
		public static string AttendanceInfo { get; private set; } = @"　勤務時間 : {0:H:mm} ～ {1:H:mm}
　詳細情報・備考 : {2}";

		/// <summary>
		///		勤怠概要（詳細情報・備考なし）の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 勤務開始時刻</para>
		///		<para>{1} : 勤務終了時刻</para>
		/// </remarks>
		public static string AttendanceInfoWithoutDesc { get; private set; } = @"　勤務時間 : {0:H:mm} ～ {1:H:mm}";

		/// <summary>
		///		業務概要の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 開始時刻</para>
		///		<para>{1} : 終了時刻</para>
		///		<para>{2} : 内容</para>
		///		<para>{3} : 備考</para>
		/// </remarks>
		public static string WorkSummary { get; private set; } = "　{0:H:mm} ～ {1:H:mm} : {2} （ 備考 : {3} ）";

		/// <summary>
		///		業務概要（備考なし）の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 開始時刻</para>
		///		<para>{1} : 終了時刻</para>
		///		<para>{2} : 内容</para>
		/// </remarks>
		public static string WorkSummaryWithoutRemarks { get; private set; } = "　{0:H:mm} ～ {1:H:mm} : {2}";

		/// <summary>
		///		詳細情報の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 詳細情報（利用者の状況や備品状況）</para>
		/// </remarks>
		public static string Description { get; private set; } = "　{0}";

		/// <summary>
		///		詳細情報の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 詳細情報（勤怠の詳細情報・備考）</para>
		/// </remarks>
		public static string Description2 { get; private set; } = "　　{0}";

		/// <summary>
		///		項目の詳細情報の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 項目の詳細情報</para>
		/// </remarks>
		public static string ItemDescription { get; private set; } = "　　　{0}";

		/// <summary>
		///		項目のタイトルと件数の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : サブタイトル</para>
		///		<para>{1} : 件数</para>
		///	</remarks>
		public static string SubTitleWithCount { get; private set; } = "◆ {0}　{1}件";

		/// <summary>
		///		PC・ソフトウェアトラブルの書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 番号</para>
		///		<para>{1} : 備品の種類</para>
		///		<para>{2} : 名前</para>
		///		<para>{3} : 発生時刻</para>
		///		<para>{4} : 詳細情報</para>
		/// </remarks>
		public static string FailureInfo { get; private set; } = @"　[{0}]
　　{1} : {2}
　　発生時刻 : {3:H:mm}
　　詳細情報 : {4}";

		/// <summary>
		///		学生からの質問の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 番号</para>
		///		<para>{1} : 質問時刻</para>
		///		<para>{2} : 質問内容</para>
		///		<para>{3} : 回答</para>
		/// </remarks>
		public static string Questions { get; private set; } = @"　[{0}]
　　質問時刻 : {1:H:mm}
　　Q. {2}
　　A. {3}";

		/// <summary>
		///		不正印刷情報の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 番号</para>
		///		<para>{1} : 発見した時刻</para>
		///		<para>{2} : ユーザー</para>
		///		<para>{3} : ファイル名</para>
		///		<para>{4} : 詳細情報</para>
		/// </remarks>
		public static string InjusticePrints { get; private set; } = @"　[{0}]
　　発見した時刻 : {1:H:mm}
　　ユーザー : {2}
　　ファイル名 : {3}
　　詳細情報 : {4}";

		/// <summary>
		///		2重ログイン情報の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 番号</para>
		///		<para>{1} : 発見した時刻</para>
		///		<para>{2} : ユーザー</para>
		///		<para>{3} : 詳細情報</para>
		/// </remarks>
		public static string DuplicateLogins { get; private set; } = @"　[{0}]
　　発見した時刻 : {1:H:mm}
　　ユーザー : {2}
　　詳細情報 : {3}";

		/// <summary>
		///		遺失物情報の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 番号</para>
		///		<para>{1} : 発見した時刻</para>
		///		<para>{2} : 発見した場所</para>
		///		<para>{3} : 名前</para>
		///		<para>{4} : 備考</para>
		/// </remarks>
		public static string LostSumthings { get; private set; } = @"　[{0}]
　　発見した時刻 : {1:H:mm}
　　発見した場所 : {2}
　　名前 : {3}
　　備考 : {4}";

		/// <summary>
		///		遺失物情報の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 番号</para>
		///		<para>{1} : 発見した時刻</para>
		///		<para>{2} : 発見した場所</para>
		///		<para>{3} : 名前</para>
		/// </remarks>
		public static string LostSumthingsWithoutRemarks { get; private set; } = @"　[{0}]
　　発見した時刻 : {1:H:mm}
　　発見した場所 : {2}
　　名前 : {3}";

		/// <summary>
		///		その他の注意の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 番号</para>
		///		<para>{1} : 詳細情報</para>
		///		<para>{2} : 人数</para>
		/// </remarks>
		public static string OtherMatters { get; private set; } = @"　{0}. {1} （ {2}名 ）";


	}
}
