using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using Nia_Tech.ModelExtentions;

namespace DailyReportMaker {

	/// <summary>
	///		日報データのモデルを定義します。
	/// </summary>
	public class DailyReportModel : ProReactsRev<string> {

		#region フィールド

		/// <summary>
		/// 
		/// </summary>
		private static char[] NewLine = Environment.NewLine.ToCharArray();

		#endregion

		#region プロパティ

		/// <summary>
		///		日報のTA情報を取得します。
		/// </summary>
		public TAInfo TAInfo { get; private set; }

		/// <summary>
		///		業務の概要を取得します。
		/// </summary>
		public WorkSummary WorkSummary { get; private set; }

		/// <summary>
		///		障害情報のリストを取得します。
		/// </summary>
		public ObservableCollection<FailureInfoItem> FailureInfoList { get; private set; }

		/// <summary>
		///		学生からの質問リストを取得します。
		/// </summary>
		public ObservableCollection<QuestionItem> QuestionList { get; private set; }

		/// <summary>
		///		不正印刷のリストを取得します。
		/// </summary>
		public ObservableCollection<InjusticePrintItem> InjusticePrintList { get; private set; }

		/// <summary>
		///		2重ログインのリストを取得します。
		/// </summary>
		public ObservableCollection<DuplicateLoginItem> DuplicateLoginList { get; private set; }

		/// <summary>
		///		その他の注意のリストを取得します。
		/// </summary>
		public ObservableCollection<OtherMatterItem> OtherMatterList { get; private set; }

		/// <summary>
		///		遺失物のリストを取得します。
		/// </summary>
		public ObservableCollection<LostSumthingItem> LostSumthingList { get; private set; }

		/// <summary>
		///		備品の状況を取得・設定します。
		/// </summary>
		public string FixtureInfo { get; set; }

		/// <summary>
		///		日報データのプレビュー（ タイトル ）を取得します。
		/// </summary>
		public string DailyReportPreviewTitle { get; private set; }

		/// <summary>
		///		日報データのプレビュー（ 本文 ）を取得します。
		/// </summary>
		public string DailyReportPreviewContent { get; private set; }

		#endregion

		#region コンストラクター

		/// <summary>
		///		DailyReportModelクラスの新しいインスタンスを生成します。
		/// </summary>
		public DailyReportModel() {
			// 項目を初期化します。
			TAInfo = new TAInfo();
			WorkSummary = new WorkSummary();
			FailureInfoList = new ObservableCollection<FailureInfoItem>();
			QuestionList = new ObservableCollection<QuestionItem>();
			InjusticePrintList = new ObservableCollection<InjusticePrintItem>();
			DuplicateLoginList = new ObservableCollection<DuplicateLoginItem>();
			OtherMatterList = new ObservableCollection<OtherMatterItem>();
			LostSumthingList = new ObservableCollection<LostSumthingItem>();
			FixtureInfo = "";
			DailyReportPreviewTitle = "";
			DailyReportPreviewContent = "";

			// バッググラウンドスレッドから同期的にアクセスできるように設定します。（準備工事）
			//BindingOperations.EnableCollectionSynchronization( WorkSummary.WorkingOverviewList, new object() );
			//BindingOperations.EnableCollectionSynchronization( FailureInfoList, new object() );
			//BindingOperations.EnableCollectionSynchronization( QuestionList, new object() );
			//BindingOperations.EnableCollectionSynchronization( InjusticePrintList, new object() );
			//BindingOperations.EnableCollectionSynchronization( DuplicateLoginList, new object() );
			//BindingOperations.EnableCollectionSynchronization( OtherMatterList, new object() );
			//BindingOperations.EnableCollectionSynchronization( LostSumthingList, new object() );
		}

		#endregion

		#region メソッド

		/// <summary>
		///		入力した項目をクリアします。
		/// </summary>
		public void ClearAll() {
			TAInfo = new TAInfo();
			WorkSummary.WorkingOverviewList.Clear();
			WorkSummary.AboutUser = "";
			FailureInfoList.Clear();
			QuestionList.Clear();
			InjusticePrintList.Clear();
			DuplicateLoginList.Clear();
			OtherMatterList.Clear();
			LostSumthingList.Clear();
			FixtureInfo = "";
			DailyReportPreviewTitle = "";
			DailyReportPreviewContent = "";

			Reporter.Report( "すべての入力フォームを初期化しました。" );
		}

		#region リストのクリアとソート

		/// <summary>
		///		勤務概要の項目リストをクリアします。
		/// </summary>
		public void ClearWorkOverviewList() {
			WorkSummary.WorkingOverviewList.Clear();
			// プロパティの変更をボタンに通知します。
			NotifyPropertyChanged( "" );
		}

		/// <summary>
		///		業務概要の項目を開始時間順にソートします。
		/// </summary>
		public void SortWorkOverviewList() {

			// 業務概要の項目を開始時間順にソートします。
			// もし、開始時間が同じであれば、その中では終了時間順にソートします。
			var result = WorkSummary.WorkingOverviewList.OrderBy( item => item.StartTime )
														.ThenBy( item => item.EndTime )
														// 
														.ToList();

			// 業務概要の項目リストをクリアします。
			WorkSummary.WorkingOverviewList.Clear();
			// ソート後のリストの各項目を業務概要の項目リストに追加します。
			foreach( var item in result ) {
				WorkSummary.WorkingOverviewList.Add( item );
			}
			Reporter.Report( "業務概要の項目を開始時間順にソートしました。" );
			NotifyPropertyChanged( "" );
		}

		/// <summary>
		///		質問の項目リストをクリアします。
		/// </summary>
		public void ClearQuestionList() {
			QuestionList.Clear();
			NotifyPropertyChanged( "" );
		}

		/// <summary>
		///		質問の項目を開始時間順にソートします。
		/// </summary>
		public void SortQuestionList() {
			var result = QuestionList.OrderBy( item => item.QuestionTime ).ToList();

			QuestionList.Clear();
			foreach( var item in result ) {
				QuestionList.Add( item );
			}
			Reporter.Report( "質問の項目を時間順にソートしました。" );
			NotifyPropertyChanged( "" );
		}

		public void ClearFailureInfoList() {
			FailureInfoList.Clear();
			NotifyPropertyChanged( "" );
		}

		public void SortFailureInfoList() {
			var result = FailureInfoList.OrderBy( item => item.OccuredTime ).ToList();

			FailureInfoList.Clear();
			foreach( var item in result ) {
				FailureInfoList.Add( item );
			}
			Reporter.Report( "機器・アプリのトラブル情報の項目を時間順にソートしました。" );
			NotifyPropertyChanged( "" );
		}

		public void ClearInjusticePrintList() {
			InjusticePrintList.Clear();
			NotifyPropertyChanged( "" );
		}

		public void SortInjusticePrintList() {
			var result = InjusticePrintList.OrderBy( item => item.FoundTime ).ToList();

			InjusticePrintList.Clear();
			foreach( var item in result ) {
				InjusticePrintList.Add( item );
			}
			Reporter.Report( "不正印刷情報の項目を時間順にソートしました。" );
			NotifyPropertyChanged( "" );
		}

		public void ClearDuplicateLoginList() {
			DuplicateLoginList.Clear();
			NotifyPropertyChanged( "" );
		}

		public void SortDuplicateLoginList() {
			var result = DuplicateLoginList.OrderBy( item => item.FoundTime ).ToList();

			DuplicateLoginList.Clear();
			foreach( var item in result ) {
				DuplicateLoginList.Add( item );
			}
			Reporter.Report( "2重ログイン情報の項目を時間順にソートしました。" );
			NotifyPropertyChanged( "" );
		}

		public void ClearOtherMatterList() {
			OtherMatterList.Clear();
			NotifyPropertyChanged( "" );
		}

		public void ClearLostSumthingList() {
			LostSumthingList.Clear();
			NotifyPropertyChanged( "" );
		}

		public void SortLostSumthingList() {
			var result = LostSumthingList.OrderBy( item => item.FoundTime ).ToList();

			LostSumthingList.Clear();
			foreach( var item in result ) {
				LostSumthingList.Add( item );
			}
			Reporter.Report( "遺失物の情報の項目を時間順にソートしました。" );
			NotifyPropertyChanged( "" );
		}

		#endregion

		private string FormatDescription( string format, string value ) {
			if( !value.Contains( "\n" ) ) {
				return value;
			}

			StringBuilder sb = new StringBuilder();

			sb.AppendLine();
			sb.Append(
				string.Join( "\n",
					value.Split( NewLine, StringSplitOptions.RemoveEmptyEntries )
						 .Select( _ => string.Format( format, _ ) )
				)
			);

			return sb.ToString();
		}

		/// <summary>
		///		入力した項目から日報データを生成します。
		/// </summary>
		/// <returns></returns>
		private Task<ResultInfo<bool>> GenerateDailyReportData() {

			var result = new ResultInfo<bool> {
				Result = true, Message = "日報データを作成しました。"
			};

			Reporter.Report( "日報データを作成しています。" );

			try {
				StringBuilder content = new StringBuilder();

				#region TA情報

				DailyReportPreviewTitle = string.Format( FormatModel.DailyReportPreviewTitle, TAInfo.RoomName, TAInfo.Date, TAInfo.TAName );

				#endregion

				#region 勤怠概要

				content.AppendLine( string.Format( FormatModel.SubTitle, "勤怠概要" ) );

				if( string.IsNullOrEmpty( TAInfo.Description ) ) {
					content.AppendLine( string.Format( FormatModel.AttendanceInfoWithoutDesc, TAInfo.StartTime, TAInfo.EndTime ) );
				}
				else {
					content.AppendLine(
						string.Format(
							FormatModel.AttendanceInfo,
							TAInfo.StartTime, TAInfo.EndTime,
							FormatDescription( FormatModel.Description2, TAInfo.Description )
						)
					);
				}
				content.AppendLine();

				#endregion

				#region 業務概要

				content.AppendLine( string.Format( FormatModel.SubTitle, "業務概要" ) );
				
				foreach( var item in WorkSummary.WorkingOverviewList ) {
					if( string.IsNullOrEmpty( item.Remarks ) ) {
						content.AppendLine( string.Format( FormatModel.WorkSummaryWithoutRemarks, item.StartTime, item.EndTime, item.Description ) );
					}
					else {
						content.AppendLine( string.Format( FormatModel.WorkSummary, item.StartTime, item.EndTime, item.Description, item.Remarks ) );
					}
				}
				content.AppendLine();

				content.AppendLine( string.Format( FormatModel.SubTitle, "利用者の状況" ) );
				foreach( var item in WorkSummary.AboutUser.Split( NewLine, StringSplitOptions.RemoveEmptyEntries ) ) {
					content.AppendLine( string.Format( FormatModel.Description, item ) );
				}
				content.AppendLine();

				#endregion

				#region PC・ソフトウェアのトラブル

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "PC、ソフトウェアなどのトラブル", FailureInfoList.Count ) );
				if( FailureInfoList.Any() ) {
					foreach( var item in FailureInfoList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						content.AppendLine(
							string.Format(
								FormatModel.FailureInfo,
								item.Index,
								item.Value.TypeName, item.Value.Name,
								item.Value.OccuredTime,
								FormatDescription( FormatModel.ItemDescription, item.Value.Description )
							)
						);
					}
				}
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region 学生からの質問

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "質問", QuestionList.Count ) );
				if( QuestionList.Any() ) {
					foreach( var item in QuestionList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						content.AppendLine(
							string.Format(
								FormatModel.Questions,
								item.Index,
								item.Value.QuestionTime,
								FormatDescription( FormatModel.ItemDescription, item.Value.Question ),
								FormatDescription( FormatModel.ItemDescription, item.Value.Answer )
							)
						);
					}
				}
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region 不正印刷

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "不正印刷", InjusticePrintList.Count ) );
				if( InjusticePrintList.Any() ) {
					foreach( var item in InjusticePrintList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						content.AppendLine(
							string.Format(
								FormatModel.InjusticePrints,
								item.Index,
								item.Value.FoundTime,
								item.Value.User,
								string.IsNullOrEmpty( item.Value.FileName ) ? "N/A" : item.Value.FileName,
								FormatDescription( FormatModel.ItemDescription, item.Value.Description )
							)
						);
					} 
				}
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region 2重ログイン

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "2重ログイン", DuplicateLoginList.Count ) );
				if( DuplicateLoginList.Any() ) {
					foreach( var item in DuplicateLoginList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						content.AppendLine(
							string.Format(
								FormatModel.DuplicateLogins,
								item.Index,
								item.Value.FoundTime,
								item.Value.User,
								FormatDescription( FormatModel.ItemDescription, item.Value.Description )
							)
						);
					} 
				}
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region その他の注意

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "その他の注意", OtherMatterList.Count ) );
				if( OtherMatterList.Any() ) {
					foreach( var item in OtherMatterList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						content.AppendLine(
							string.Format(
								FormatModel.OtherMatters,
								item.Index,
								item.Value.Description,
								item.Value.Num
							)
						);
					} 
				}
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region 遺失物

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "遺失物", LostSumthingList.Count ) );
				if( LostSumthingList.Any() ) {
					foreach( var item in LostSumthingList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						if( string.IsNullOrEmpty( item.Value.Remarks ) ) {
							content.AppendLine(
								string.Format( FormatModel.LostSumthingsWithoutRemarks, item.Index, item.Value.FoundTime, item.Value.FoundPlace, item.Value.Name )
							);
						}
						else {
							content.AppendLine(
								string.Format( FormatModel.LostSumthings, item.Index, item.Value.FoundTime, item.Value.FoundPlace, item.Value.Name, item.Value.Remarks )
							);
						}
					}
				}
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region 備品状況

				content.AppendLine( string.Format( FormatModel.SubTitle, "備品状況" ) );
				foreach( var item in FixtureInfo.Split( NewLine, StringSplitOptions.RemoveEmptyEntries ) ) {
					content.AppendLine( string.Format( FormatModel.Description, item ) );
				}
				content.AppendLine();

				#endregion

				content.AppendLine( "以上" );

				DailyReportPreviewContent = content.ToString();

			}
			catch( Exception e ) {
				result.Result = false;
				result.Message = "日報データの作成に失敗しました。";
				result.AdditionalInfo = e.Message;
				DailyReportPreviewContent = "";
			}
			finally {
				Reporter.Report( result.Message );
			}

			return Task.FromResult( result );
		}

		/// <summary>
		///		入力した項目から日報データを生成します。
		/// </summary>
		public async Task GenerateDailyReportDataAsync() {
			var result = await GenerateDailyReportData();
			GenerateDailyReportCompleted?.Invoke( this, new NotifyResultEventArgs<ResultInfo<bool>>( result ) );
		}

		#endregion

		#region イベントハンドラー

		public event NotifyResultEventHandler<ResultInfo<bool>> GenerateDailyReportCompleted;

		#endregion

	}
}
