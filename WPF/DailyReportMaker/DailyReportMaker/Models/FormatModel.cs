using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DailyReportMaker {

	/// <summary>
	///		書式を定義します。
	/// </summary>
	class FormatModel {

		#region プロパティ

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
		///		項目のタイトルと件数の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : サブタイトル</para>
		///		<para>{1} : 件数</para>
		///	</remarks>
		public static string SubTitleWithCount { get; private set; } = "◆ {0}　{1}件";

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
		public static string DescriptionForAttendance { get; private set; } = "　　{0}";

		/// <summary>
		///		項目の詳細情報の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 項目の詳細情報</para>
		/// </remarks>
		public static string DescriptionForItem { get; private set; } = "　　　{0}";

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
		public static string WorkOverview { get; private set; } = "　{0:H:mm} ～ {1:H:mm} : {2} （ 備考 : {3} ）";

		/// <summary>
		///		業務概要（備考なし）の書式を取得します。
		/// </summary>
		/// <remarks>
		///		<para>{0} : 開始時刻</para>
		///		<para>{1} : 終了時刻</para>
		///		<para>{2} : 内容</para>
		/// </remarks>
		public static string WorkOverviewWithoutRemarks { get; private set; } = "　{0:H:mm} ～ {1:H:mm} : {2}";

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
		public static string Question { get; private set; } = @"　[{0}]
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
		public static string InjusticePrint { get; private set; } = @"　[{0}]
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
		public static string DuplicateLogin { get; private set; } = @"　[{0}]
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
		public static string LostSumthing { get; private set; } = @"　[{0}]
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
		public static string OtherMatter { get; private set; } = @"　{0}. {1} （ {2}名 ）";

		#endregion

		private static void SetDefaultFormat() {
			DailyReportPreviewTitle = "日報：{0} {1:MM/dd} {2}";

			SubTitle = "◆ {0}";
			SubTitleWithCount = "◆ {0}　{1}件";

			Description = "　{0}";
			DescriptionForAttendance = "　　{0}";
			DescriptionForItem = "　　　{0}";

			AttendanceInfo = @"　勤務時間 : {0:H:mm} ～ {1:H:mm}
　詳細情報・備考 : {2}";
			AttendanceInfoWithoutDesc = @"　勤務時間 : {0:H:mm} ～ {1:H:mm}";

			WorkOverview = "　{0:H:mm} ～ {1:H:mm} : {2} （ 備考 : {3} ）";
			WorkOverviewWithoutRemarks = "　{0:H:mm} ～ {1:H:mm} : {2}";

			FailureInfo = @"　[{0}]
　　{1} : {2}
　　発生時刻 : {3:H:mm}
　　詳細情報 : {4}";
			Question = @"　[{0}]
　　質問時刻 : {1:H:mm}
　　Q. {2}
　　A. {3}";
			InjusticePrint = @"　[{0}]
　　発見した時刻 : {1:H:mm}
　　ユーザー : {2}
　　ファイル名 : {3}
　　詳細情報 : {4}";
			DuplicateLogin = @"　[{0}]
　　発見した時刻 : {1:H:mm}
　　ユーザー : {2}
　　詳細情報 : {3}";
			LostSumthing = @"　[{0}]
　　発見した時刻 : {1:H:mm}
　　発見した場所 : {2}
　　名前 : {3}
　　備考 : {4}";
			LostSumthingsWithoutRemarks = @"　[{0}]
　　発見した時刻 : {1:H:mm}
　　発見した場所 : {2}
　　名前 : {3}";
			OtherMatter = @"　{0}. {1} （ {2}名 ）";
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="url"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private static Task<ResultInfo<bool>> LoadFormats( string url, CancellationToken cancellationToken ) {

			var result = new ResultInfo<bool> {
				Result = true,
				Message = "準備完了"
			};

			try {

				XElement root = XElement.Load( url );

				DailyReportPreviewTitle = root.Element( "head" ).Element( "title" ).Value;

				var body = root.Element( "body" );

				SubTitle = body.Element( "itemname" ).Value;
				SubTitleWithCount = body.Element( "itemnamewithcount" ).Value;

				Description = body.Element( "description" ).Value;
				DescriptionForAttendance = body.Element( "descriptionforattendance" ).Value;
				DescriptionForItem = body.Element( "descriptionforitem" ).Value;

				AttendanceInfo = body.Element( "attendanceinfo" ).Value;
				AttendanceInfoWithoutDesc = body.Element( "attendanceinfonodesc" ).Value;

				WorkOverview = body.Element( "workoverview" ).Value;
				WorkOverviewWithoutRemarks = body.Element( "workoverviewnoremarks" ).Value;

				FailureInfo = body.Element( "failureinfo" ).Value;
				Question = body.Element( "question" ).Value;
				InjusticePrint = body.Element( "injusticeprint" ).Value;
				DuplicateLogin = body.Element( "duplicatelogin" ).Value;
				LostSumthing = body.Element( "lostsumthing" ).Value;
				LostSumthingsWithoutRemarks = body.Element( "lostsumthingnoremarks" ).Value;
				OtherMatter = body.Element( "othermatter" ).Value;

			}
			catch( Exception e ) {
				result.Result = false;
				result.Message = "書式指定ファイル（formats.xml）のロードに失敗しました。既定の書式を使用します。";
				result.AdditionalInfo = e.Message;

				SetDefaultFormat();

			}

			return Task.FromResult( result );
		}

		public static event EventHandler<ResultInfo<bool>> LoadFormatsCompleted;

		/// <summary>
		///		
		/// </summary>
		/// <param name="url"></param>
		/// <param name="cancellationToken"></param>
		public static async Task LoadFormatsAsync( string url, CancellationToken cancellationToken ) {

			var result = await LoadFormats( url, cancellationToken );
			LoadFormatsCompleted?.Invoke( null, result );

		}


	}
}
