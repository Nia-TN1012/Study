using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Linq;

using Nia_Tech.ModelExtentions;

namespace DailyReportMaker {

	/// <summary>
	///		日報データのモデルを定義します。
	/// </summary>
	public class DailyReportModel : ProReactsRev<string> {

		#region フィールド

		/// <summary>
		///		改行文字列（ char型配列 ）を表します。
		/// </summary>
		private static char[] newLine = Environment.NewLine.ToCharArray();
		
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
		///		質問リストを取得します。
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

		#region リストのクリアとソート

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

		#region XMLファイルの読み書き

		/// <summary>
		///		指定したXMLファイルから日報データを読み込みます。
		/// </summary>
		/// <param name="url">XMLファイルのパス</param>
		/// <param name="cancellationToken">処理のキャンセルを要求するトークン</param>
		/// <returns>読み込み結果を格納するResultInfoオブジェクト</returns>
		private Task<ResultInfo<bool>> LoadDataFile( string url, CancellationToken cancellationToken ) {

			string fileName = Path.GetFileName( url );

			var result = new ResultInfo<bool> {
				Result = true,
				Message = $"日報データファイル({fileName})を読み込みました",
			};

			try {
				// LINQ to XMLを利用して、XMLファイルを読み込みます。
				var root = XElement.Load( url );
				cancellationToken.ThrowIfCancellationRequested();

				#region TA情報

				var meta = root.Element( "metadata" );
				TAInfo.Date = DateTime.Parse( meta.Attribute( "date" ).Value );
				TAInfo.RoomName = meta.Attribute( "room_name" ).Value;
				TAInfo.TAName = meta.Attribute( "ta_name" ).Value;
				TAInfo.StartTime = DateTime.Parse( meta.Element( "worktime" ).Attribute( "start" ).Value );
				TAInfo.EndTime = DateTime.Parse( meta.Element( "worktime" ).Attribute( "end" ).Value );
				TAInfo.Description = meta.Element( "description" ).Value;

				cancellationToken.ThrowIfCancellationRequested();

				#endregion

				#region 業務概要

				var workSummary = root.Element( "worksummary" );
				var workOverviewList = workSummary.Element( "workoverviewlist" );

				foreach( var item in workOverviewList.Elements( "workoverview" )
					// SelectメソッドでWorkOverviewItemm型のシーケンスに射影します。
					.Select(
						// ※遅延実行のメソッドなので、ここに指定した処理はforeach文による列挙時に実行されます。
						item => new WorkOverviewItem {
							StartTime = DateTime.Parse( item.Attribute( "starttime" ).Value ),
							EndTime = DateTime.Parse( item.Attribute( "endtime" ).Value ),
							Description = item.Element( "description" ).Value,
							Remarks = item.Element( "remarks" ).Value
						}
					) ) {
					WorkSummary.WorkingOverviewList.Add( item );

					cancellationToken.ThrowIfCancellationRequested();
				}

				#region 上の処理の流れ
				/*
					foreach文
					↓
					work.Elementsメソッド、Selectメソッド
					↓
					1つ目の要素を列挙開始
					↓
					Selectメソッドの中に指定した処理を実行
					↓
					1つ目の要素をObservableCollection<WorkOverviewItem>に追加
					↓
					2つ目の要素を列挙開始
					↓
					Selectメソッドの中に指定した処理を実行
					↓
					2つ目の要素をObservableCollection<WorkOverviewItem>に追加
					↓
					・・・
					↓
					n個目の要素を列挙開始
					↓
					Selectメソッドの中に指定した処理を実行
					↓
					n個目の要素をObservableCollection<WorkOverviewItem>に追加
					↓
					終了

					このように、Selectメソッドの中に指定した処理は、メソッドを呼び出した時は実行せず、
					foreach文などで実際に列挙した時に実行されます。

				*/
				#endregion

				WorkSummary.AboutUser = workSummary.Element( "aboutuser" ).Value;

				#endregion

				#region 機器トラブル

				var failinfo = root.Element( "failureinfolist" );

				foreach( var item in failinfo.Elements( "failureinfo" )
					.Select(
						item => new FailureInfoItem {
							OccuredTime = DateTime.Parse( item.Attribute( "time" ).Value ),
							TypeName = item.Element( "fixture" ).Attribute( "type" ).Value,
							Name = item.Element( "fixture" ).Attribute( "name" ).Value,
							Description = item.Element( "description" ).Value
						}
					) ) {
					FailureInfoList.Add( item );

					cancellationToken.ThrowIfCancellationRequested();
				}

				#endregion

				#region 質問

				var questionList = root.Element( "questionlist" );

				foreach( var item in questionList.Elements( "question" )
					.Select(
						item => new QuestionItem {
							QuestionTime = DateTime.Parse( item.Attribute( "time" ).Value ),
							Question = item.Element( "question" ).Value,
							Answer = item.Element( "answer" ).Value
						}
					) ) {
					QuestionList.Add( item );

					cancellationToken.ThrowIfCancellationRequested();
				}

				#endregion

				#region 不正印刷

				var injusticePrintList = root.Element( "injusticeprintlist" );

				foreach( var item in injusticePrintList.Elements( "injusticeprint" )
					.Select(
						item => new InjusticePrintItem {
							FoundTime = DateTime.Parse( item.Attribute( "time" ).Value ),
							User = item.Element( "user" ).Attribute( "name" ).Value,
							FileName = item.Element( "file" ).Attribute( "name" ).Value,
							Description = item.Element( "description" ).Value
						}
					) ) {
					InjusticePrintList.Add( item );

					cancellationToken.ThrowIfCancellationRequested();
				}

				#endregion

				#region 2重ログイン

				var duplicateLoginList = root.Element( "duplicateloginlist" );

				foreach( var item in duplicateLoginList.Elements( "duplicatelogin" )
					.Select(
						item => new DuplicateLoginItem {
							FoundTime = DateTime.Parse( item.Attribute( "time" ).Value ),
							User = item.Element( "user" ).Value,
							Description = item.Element( "description" ).Value
						}
					) ) {
					DuplicateLoginList.Add( item );

					cancellationToken.ThrowIfCancellationRequested();
				}

				#endregion

				#region その他の注意

				var otherMatterList = root.Element( "othermatterlist" );

				foreach( var item in otherMatterList.Elements( "othermatter" )
					.Select(
						item => new OtherMatterItem {
							Description = item.Element( "description" ).Value,
							Num = int.Parse( item.Attribute( "num" ).Value )
						}
					) ) {
					OtherMatterList.Add( item );

					cancellationToken.ThrowIfCancellationRequested();
				}

				#endregion

				#region 遺失物

				var lostSumthingList = root.Element( "lostsumthinglist" );

				foreach( var item in lostSumthingList.Elements( "lostsumthing" )
					.Select(
						item => new LostSumthingItem {
							FoundTime = DateTime.Parse( item.Attribute( "time" ).Value ),
							FoundPlace = item.Attribute( "place" ).Value,
							Name = item.Element( "name" ).Value,
							Remarks = item.Element( "remarks" ).Value
						}
					) ) {
					LostSumthingList.Add( item );

					cancellationToken.ThrowIfCancellationRequested();
				}

				#endregion

				#region 備品状況

				FixtureInfo = root.Element( "fixtureinfo" ).Value;

				#endregion

			}
			catch( Exception e ) {
				result.Result = false;
				result.Message = $"日報データファイル({fileName})の読み込みに失敗しました。";
				result.AdditionalInfo = e.Message;
			}
			finally {
				if( result.AdditionalInfo != null ) {
					Reporter.Report( $"{result.Message} (追加情報 : {result.AdditionalInfo})" );
				}
				else {
					Reporter.Report( result.Message );
				}
			}


			return Task.FromResult( result );
		}

		/// <summary>
		///		指定したXMLファイルから日報データを読み込みます。
		/// </summary>
		/// <param name="url">XMLファイルのパス</param>
		public async Task LoadDataFileAsync( string url ) {
			ClearAll();
			var result = await LoadDataFile( url, CancellationToken.None );
		}

		/// <summary>
		///		指定したXMLファイルに日報データを保存します。
		/// </summary>
		/// <param name="url">XMLファイルのパス</param>
		/// <param name="cancellationToken">処理のキャンセルを要求するトークン</param>
		/// <returns>保存結果を格納するResultInfoオブジェクト</returns>
		private Task<ResultInfo<bool>> SaveDataFile( string url, CancellationToken cancellationToken ) {

			string fileName = Path.GetFileName( url );

			var result = new ResultInfo<bool> {
				Result = true,
				Message = $"日報データファイル({fileName})を保存しました。",
			};

			try {

				#region TA情報

				var metadata = new XElement( "metadata",
					new XAttribute( "date", TAInfo.Date.ToString( "yyyy/MM/dd" ) ),
					new XAttribute( "room_name", TAInfo.RoomName ),
					new XAttribute( "ta_name", TAInfo.TAName ),
					new XElement( "worktime",
						new XAttribute( "start", TAInfo.StartTime.ToString( "H:mm" ) ),
						new XAttribute( "end", TAInfo.EndTime.ToString( "H:mm" ) )
					),
					new XElement( "description", TAInfo.Description )
				);

				#endregion

				#region 業務概要

				var workOverviewList = new XElement( "workoverviewlist",
					WorkSummary.WorkingOverviewList.Select(
						item =>
							new XElement( "workoverview",
								new XAttribute( "starttime", item.StartTime.ToString( "H:mm" ) ),
								new XAttribute( "endtime", item.EndTime.ToString( "H:mm" ) ),
								new XElement( "description", item.Description ),
								new XElement( "remarks", item.Remarks )
						)
					)
				);

				var workSummary = new XElement( "worksummary",
					workOverviewList,
					new XElement( "aboutuser", WorkSummary.AboutUser )
				);

				#endregion

				#region 機器トラブル

				var failureInfoList = new XElement( "failureinfolist",
					FailureInfoList.Select(
						item =>
							new XElement( "failureinfo",
								new XAttribute( "time", item.OccuredTime.ToString( "H:mm" ) ),
								new XElement( "fixture",
									new XAttribute( "type", item.TypeName ),
									new XAttribute( "name", item.Name )
								),
								new XElement( "description", item.Description )
						)
					)
				);

				#endregion

				#region 質問

				var questionList = new XElement( "questionlist",
					QuestionList.Select(
						item =>
							new XElement( "question",
								new XAttribute( "time", item.QuestionTime.ToString( "H:mm" ) ),
								new XElement( "question", item.Question ),
								new XElement( "answer", item.Answer )
						)
					)
				);

				#endregion

				#region 不正印刷

				var injusticePrintList = new XElement( "injusticeprintlist",
					InjusticePrintList.Select(
						item =>
							new XElement( "injusticeprint",
								new XAttribute( "time", item.FoundTime.ToString( "H:mm" ) ),
								new XElement( "user", new XAttribute( "name", item.User ) ),
								new XElement( "file", new XAttribute( "name", item.FileName ) ),
								new XElement( "description", item.Description )
							)
					)
				);

				#endregion

				#region 2重ログイン

				var duplicateLoginList = new XElement( "duplicateloginlist",
					DuplicateLoginList.Select(
						item =>
							new XElement( "duplicatelogin",
								new XAttribute( "time", item.FoundTime.ToString( "H:mm" ) ),
								new XElement( "user", item.User ),
								new XElement( "description", item.Description )
							)
					)
				);

				#endregion

				#region その他の注意

				var othterMatterList = new XElement( "othermatterlist",
					OtherMatterList.Select(
						item =>
							new XElement( "othermatter",
								new XAttribute( "num", item.Num ),
								new XElement( "description", item.Description )
							)
					)
				);

				#endregion

				#region 遺失物

				var lostSumthingList = new XElement( "lostsumthinglist",
					LostSumthingList.Select(
						item =>
							new XElement( "lostsumthing",
								new XAttribute( "time", item.FoundTime.ToString( "H:mm" ) ),
								new XAttribute( "place", item.FoundPlace ),
								new XElement( "name", item.Name ),
								new XElement( "remarks", item.Remarks )
							)
					)
				);

				#endregion

				#region 備品状況

				var fixtureInfo = new XElement( "fixtureinfo", FixtureInfo );

				#endregion

				var root = new XElement( "dailyreport",
					metadata, workSummary, failureInfoList, questionList, injusticePrintList, duplicateLoginList, othterMatterList, lostSumthingList, fixtureInfo
				);
				root.Save( url );

			}
			catch( Exception e ) {
				result.Result = false;
				result.Message = $"日報データファイル({fileName})の保存に失敗しました。";
				result.AdditionalInfo = e.Message;
			}
			finally {
				if( result.AdditionalInfo != null ) {
					Reporter.Report( $"{result.Message} (追加情報 : {result.AdditionalInfo})" );
				}
				else {
					Reporter.Report( result.Message );
				}
			}

			return Task.FromResult( result );
		}

		/// <summary>
		///		指定したXMLファイルに日報データを保存します。
		/// </summary>
		/// <param name="url">XMLファイルのパス</param>
		public async Task SaveDataFileAsync( string url ) {
			var result = await SaveDataFile( url, CancellationToken.None );
		}

		#endregion

		#region 日報データの作成

		/// <summary>
		///		指定した文字列を1行単位で指定した書式を適用します。
		/// </summary>
		/// <param name="format">1行単位で適用する書式</param>
		/// <param name="value">出力したい文字列</param>
		/// <returns>1行単位で書式を適用した文字列</returns>
		private string StringFormatForEachLine( string format, string value ) {
			// 改行記号がない時はそのまま返します。
			if( !value.Contains( "\n" ) ) {
				return value;
			}

			StringBuilder sb = new StringBuilder();

			sb.AppendLine();
			sb.Append(
				string.Join( "\n",
					value.Split( newLine, StringSplitOptions.RemoveEmptyEntries )
						 .Select( _ => string.Format( format, _ ) )
				)
			);

			return sb.ToString();
		}

		/// <summary>
		///		入力した項目から日報データを生成します。
		/// </summary>
		/// <param name="cancellationToken">処理のキャンセルを要求するトークン</param>
		/// <returns>生成結果を表す、ResultInfoオブジェクト</returns>
		private Task<ResultInfo<bool>> GenerateDailyReportData( CancellationToken cancellationToken ) {

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

				// 詳細情報が入力されている時
				if( string.IsNullOrEmpty( TAInfo.Description ) ) {
					content.AppendLine( string.Format( FormatModel.AttendanceInfoWithoutDesc, TAInfo.StartTime, TAInfo.EndTime ) );
				}
				// 詳細情報がない時
				else {
					content.AppendLine(
						string.Format(
							FormatModel.AttendanceInfo,
							TAInfo.StartTime, TAInfo.EndTime,
							StringFormatForEachLine( FormatModel.DescriptionForAttendance, TAInfo.Description )
						)
					);
				}
				content.AppendLine();

				#endregion

				#region 業務概要

				content.AppendLine( string.Format( FormatModel.SubTitle, "業務概要" ) );
				
				foreach( var item in WorkSummary.WorkingOverviewList ) {
					if( string.IsNullOrEmpty( item.Remarks ) ) {
						content.AppendLine( string.Format( FormatModel.WorkOverviewWithoutRemarks, item.StartTime, item.EndTime, item.Description ) );
					}
					else {
						content.AppendLine( string.Format( FormatModel.WorkOverview, item.StartTime, item.EndTime, item.Description, item.Remarks ) );
					}
				}
				content.AppendLine();

				content.AppendLine( string.Format( FormatModel.SubTitle, "利用者の状況" ) );
				foreach( var item in WorkSummary.AboutUser.Split( newLine, StringSplitOptions.RemoveEmptyEntries ) ) {
					content.AppendLine( string.Format( FormatModel.Description, item ) );
				}
				content.AppendLine();

				#endregion

				#region PC・ソフトウェアのトラブル

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "PC、ソフトウェアなどのトラブル", FailureInfoList.Count ) );
				// リストに項目がある時
				if( FailureInfoList.Any() ) {
					foreach( var item in FailureInfoList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						content.AppendLine(
							string.Format(
								FormatModel.FailureInfo,
								item.Index,
								item.Value.TypeName, item.Value.Name,
								item.Value.OccuredTime,
								StringFormatForEachLine( FormatModel.DescriptionForItem, item.Value.Description )
							)
						);
					}
				}
				// リストが空の時
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region 質問

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "質問", QuestionList.Count ) );
				// リストに項目がある時
				if( QuestionList.Any() ) {
					foreach( var item in QuestionList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						content.AppendLine(
							string.Format(
								FormatModel.Question,
								item.Index,
								item.Value.QuestionTime,
								StringFormatForEachLine( FormatModel.DescriptionForItem, item.Value.Question ),
								StringFormatForEachLine( FormatModel.DescriptionForItem, item.Value.Answer )
							)
						);
					}
				}
				// リストが空の時
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region 不正印刷

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "不正印刷", InjusticePrintList.Count ) );
				// リストに項目がある時
				if( InjusticePrintList.Any() ) {
					foreach( var item in InjusticePrintList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						content.AppendLine(
							string.Format(
								FormatModel.InjusticePrint,
								item.Index,
								item.Value.FoundTime,
								item.Value.User,
								string.IsNullOrEmpty( item.Value.FileName ) ? "N/A" : item.Value.FileName,
								StringFormatForEachLine( FormatModel.DescriptionForItem, item.Value.Description )
							)
						);
					} 
				}
				// リストが空の時
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region 2重ログイン

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "2重ログイン", DuplicateLoginList.Count ) );
				// リストに項目がある時
				if( DuplicateLoginList.Any() ) {
					foreach( var item in DuplicateLoginList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						content.AppendLine(
							string.Format(
								FormatModel.DuplicateLogin,
								item.Index,
								item.Value.FoundTime,
								item.Value.User,
								StringFormatForEachLine( FormatModel.DescriptionForItem, item.Value.Description )
							)
						);
					} 
				}
				// リストが空の時
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region その他の注意

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "その他の注意", OtherMatterList.Count ) );
				// リストに項目がある時
				if( OtherMatterList.Any() ) {
					foreach( var item in OtherMatterList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						content.AppendLine(
							string.Format(
								FormatModel.OtherMatter,
								item.Index,
								item.Value.Description,
								item.Value.Num
							)
						);
					} 
				}
				// リストが空の時
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region 遺失物

				content.AppendLine( string.Format( FormatModel.SubTitleWithCount, "遺失物", LostSumthingList.Count ) );
				// リストに項目がある時
				if( LostSumthingList.Any() ) {
					foreach( var item in LostSumthingList.Select( ( v, i ) => new { Index = i + 1, Value = v } ) ) {
						if( string.IsNullOrEmpty( item.Value.Remarks ) ) {
							content.AppendLine(
								string.Format( FormatModel.LostSumthingsWithoutRemarks, item.Index, item.Value.FoundTime, item.Value.FoundPlace, item.Value.Name )
							);
						}
						else {
							content.AppendLine(
								string.Format( FormatModel.LostSumthing, item.Index, item.Value.FoundTime, item.Value.FoundPlace, item.Value.Name, item.Value.Remarks )
							);
						}
					}
				}
				// リストが空の時
				else {
					content.AppendLine( string.Format( FormatModel.Description, PresetModel.None ) );
				}
				content.AppendLine();

				#endregion

				#region 備品状況

				content.AppendLine( string.Format( FormatModel.SubTitle, "備品状況" ) );
				foreach( var item in FixtureInfo.Split( newLine, StringSplitOptions.RemoveEmptyEntries ) ) {
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
			var result = await GenerateDailyReportData( CancellationToken.None );
			GenerateDailyReportCompleted?.Invoke( this, new NotifyResultEventArgs<ResultInfo<bool>>( result ) );
		}

		/// <summary>
		///		作成した日報データをテキストファイルに保存します。
		/// </summary>
		/// <param name="url">テキストファイルのパス</param>
		/// <param name="cancellationToken">処理のキャンセルを要求するトークン</param>
		/// <returns>保存結果を表す、ResultInfoオブジェクト</returns>
		public Task<ResultInfo<bool>> SaveDailyReportData( string url, CancellationToken cancellationToken ) {

			string fileName = Path.GetFileName( url );

			var result = new ResultInfo<bool> {
				Result = true,
				Message = $"作成済みの日報データをテキストファイル({fileName})を保存しました。",
			};

			try {

				using( StreamWriter sw = new StreamWriter( url ) ) {
					sw.WriteLine( $"作成日 : {DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" )}" );
					sw.WriteLine( "--- タイトル ---" );
					sw.WriteLine( DailyReportPreviewTitle );
					sw.WriteLine( "------" );
					sw.WriteLine( "--- 本文 ---" );
					sw.WriteLine( DailyReportPreviewContent );
					sw.WriteLine( "------" );
				}

			}
			catch( Exception e ) {
				result.Result = false;
				result.Message = $"作成済みの日報データをテキストファイル({fileName})に保存できませんでした。";
				result.AdditionalInfo = e.Message;
			}
			finally {
				if( string.IsNullOrEmpty( result.AdditionalInfo ) ) {
					Reporter.Report( result.Message );
				}
				else {		
					Reporter.Report( $"{result.Message} (追加情報 : {result.AdditionalInfo})" );
				}
			}

			return Task.FromResult( result );
		}

		/// <summary>
		///		作成した日報データをテキストファイルに保存します。
		/// </summary>
		/// <param name="url">テキストファイルのパス</param>
		public async Task SaveDailyReportDataAsync( string url ) {
			var result = await SaveDailyReportData( url, CancellationToken.None );
		}

		#endregion

		#endregion

		#region イベントハンドラー

		public event NotifyResultEventHandler<ResultInfo<bool>> GenerateDailyReportCompleted;

		#endregion

	}
}
