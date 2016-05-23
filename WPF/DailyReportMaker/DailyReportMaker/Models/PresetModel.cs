using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyReportMaker {
	/// <summary>
	///		教室番号と名前を定義します。
	/// </summary>
	public class RoomItem {

		/// <summary>
		///		教室名を取得・設定します。
		/// </summary>
		public string Name { get; set; } = "";

		/// <summary>
		///		教室番号を取得・設定します。
		/// </summary>
		public string Num { get; set; } = "";

		/// <summary>
		///		教室名と番号を取得します。
		/// </summary>
		public string FullName =>
			ToString( "{0} ( {1} )" );

		/// <summary>
		///		既定の書式で、現在のインスタンスを文字列形式で取得します。
		/// </summary>
		/// <returns></returns>
		public override string ToString() =>
			ToString( "{1}" );

		/// <summary>
		///		書式を指定して、現在のインスタンスを文字列形式で取得します。
		/// </summary>
		/// <param name="format">書式</param>
		/// <returns></returns>
		/// <remarks>
		///		<para>{0} : Name</para>
		///		<para>{1} : Num</para>
		/// </remarks>
		public string ToString( string format ) =>
			string.Format( format, Name, Num );
	}

	/// <summary>
	///		開始時刻と終了時刻を定義します。
	/// </summary>
	public class TimePeriod {
		/// <summary>
		///		開始時刻を取得・設定します。
		/// </summary>
		public DateTime Start { get; set; } = DateTime.Now;

		/// <summary>
		///		終了時刻を取得・設定します。
		/// </summary>
		public DateTime End { get; set; } = DateTime.Now;

		/// <summary>
		///		既定の書式で、現在のインスタンスを文字列形式で取得します。
		/// </summary>
		/// <returns></returns>
		public override string ToString() =>
			ToString( "{0:H:mm} ～ {1:H:mm}" );

		/// <summary>
		///		書式を指定して、現在のインスタンスを文字列形式で取得します。
		/// </summary>
		/// <param name="format">書式</param>
		/// <returns></returns>
		/// <remarks>
		///		<para>{0} : Start</para>
		///		<para>{1} : End</para>
		/// </remarks>
		public string ToString( string format ) =>
			string.Format( format, Start, End );
	}

	/// <summary>
	///		時間の範囲とその名前を定義します。
	/// </summary>
	public class TimePeriodWithName {

		/// <summary>
		///		時間の名前を取得・設定します。
		/// </summary>
		public string Name { get; set; } = "";

		/// <summary>
		///		時間を取得・設定します。
		/// </summary>
		public TimePeriod Time { get; set; } = new TimePeriod();

		/// <summary>
		///		既定の書式で、現在のインスタンスを文字列形式で取得します。
		/// </summary>
		/// <returns></returns>
		public override string ToString() =>
			ToString( "{0} ( {1} )" );

		/// <summary>
		///		書式を指定して、現在のインスタンスを文字列形式で取得します。
		/// </summary>
		/// <param name="format">書式</param>
		/// <returns></returns>
		/// <remarks>
		///		<para>{0} : Name</para>
		///		<para>{1} : Time</para>
		/// </remarks>
		public string ToString( string format ) =>
			string.Format( format, Name, Time );
	}

	/// <summary>
	///		プリセットの項目を定義します。
	/// </summary>
	/// <remarks>主にコンボボックスで選択するアイテムで使用します。</remarks>
	public class PresetModel {

		#region 
		/*
				現在は、デフォルト値を使用しています。
				将来は外部ファイルから読み込み、プリセットの値や書式をカスタマイズできるようにします。
		*/
		#endregion

		/// <summary>
		///		項目がない時に出力する文字列を取得します。
		/// </summary>
		public static string None { get; private set; } = "特にありませんでした。";

		/// <summary>
		///		プリセットの教室リストを表します。
		/// </summary>
		private static List<RoomItem> roomNameList = new List<RoomItem> {
			new RoomItem { Name = "情報処理教室1", Num = "ICR101" },
			new RoomItem { Name = "情報処理教室2", Num = "ICR102" },
			new RoomItem { Name = "情報処理教室3", Num = "ICR103" },
			new RoomItem { Name = "情報処理教室4", Num = "ICR104" },
			new RoomItem { Name = "情報処理教室5", Num = "ICR201" },
			new RoomItem { Name = "情報処理教室6", Num = "ICR202" },
			new RoomItem { Name = "情報処理教室7", Num = "ICR203" },
			new RoomItem { Name = "情報処理教室8", Num = "ICR301" },
			new RoomItem { Name = "情報処理教室9", Num = "ICR302" }
		};

		/// <summary>
		///		プリセットの教室リストを取得します。
		/// </summary>
		public static IEnumerable<RoomItem> RoomNameList =>
			roomNameList;

		/// <summary>
		///		プリセットの勤務時間のリストを表します。
		/// </summary>
		private static List<TimePeriodWithName> taWorkTimeList = new List<TimePeriodWithName> {};

		/// <summary>
		///		プリセットの勤務時間のリストを取得します。
		/// </summary>
		public static IEnumerable<TimePeriodWithName> TAWorkTimeList =>
			taWorkTimeList;

		/// <summary>
		///		プリセットの授業時間のリストを表します。
		/// </summary>
		private static List<TimePeriodWithName> lessonTimeList = new List<TimePeriodWithName> {};

		/// <summary>
		///		プリセットの授業時間のリストを取得します。
		/// </summary>
		public static IEnumerable<TimePeriodWithName> LessonTimeList =>
			lessonTimeList;

		/// <summary>
		///		プリセットの備品の種別名のリストを表します。
		/// </summary>
		private static List<string> fixtureTypeList = new List<string> {
			"PC名", "プリンター名", "ソフトウェア名", "その他の備品名"
		};

		/// <summary>
		///		プリセットの備品の種別名のリストを取得します。
		/// </summary>
		public static IEnumerable<string> FixtureTypeList =>
			fixtureTypeList;
	}

}
