using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using System.IO;

using Nia_Tech.ModelExtentions;

namespace DailyReportMaker {

	/// <summary>
	///		DailyReportModelとView間を仲介します。
	/// </summary>
	class MainViewModel : ViewModelBase {

		#region フィールド

		/// <summary>
		///		DailyReportModelクラスを表します。
		/// </summary>
		private DailyReportModel dailyReportModel;

		/// <summary>
		///		アプリのディレクトリを表します。
		/// </summary>
		private string appDirectory = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );

		/// <summary>
		///		アプリの情報を取得します。
		/// </summary>
		public FileVersionInfo AppInfo { get; private set; }

		#endregion

		#region プロパティ

		#region TAInfo

		/// <summary>
		///		勤務日を取得・設定します。
		/// </summary>
		public DateTime Date {
			get { return dailyReportModel.TAInfo.Date; }
			set {
				dailyReportModel.TAInfo.Date = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		教室番号を取得・設定します。
		/// </summary>
		public string RoomName {
			get { return dailyReportModel.TAInfo.RoomName; }
			set {
				dailyReportModel.TAInfo.RoomName = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		TAの氏名を取得・設定します。
		/// </summary>
		public string TAName {
			get { return dailyReportModel.TAInfo.TAName; }
			set {
				dailyReportModel.TAInfo.TAName = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		TAの氏名を取得・設定します。
		/// </summary>
		public DateTime WorkStartTime {
			get { return dailyReportModel.TAInfo.StartTime; }
			set {
				dailyReportModel.TAInfo.StartTime = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		TAの氏名を取得・設定します。
		/// </summary>
		public DateTime WorkEndTime {
			get { return dailyReportModel.TAInfo.EndTime; }
			set {
				dailyReportModel.TAInfo.EndTime = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		勤怠の詳細情報・備考を取得・設定します。
		/// </summary>
		public string Description {
			get { return dailyReportModel.TAInfo.Description; }
			set {
				dailyReportModel.TAInfo.Description = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		教室名のプリセットのリストを取得します。
		/// </summary>
		public static IEnumerable<RoomItem> RoomNameList =>
			PresetModel.RoomNameList;

		#endregion

		#region 業務概要

		/// <summary>
		///		業務概要のリストのインデックスを表します。
		/// </summary>
		private int workingOverviewListIndex;
		/// <summary>
		///		業務概要のリストのインデックスを取得・設定します。
		/// </summary>
		public int WorkingOverviewListIndex {
			get { return workingOverviewListIndex; }
			set {
				workingOverviewListIndex = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		業務概要のリストを取得します。
		/// </summary>
		public ObservableCollection<WorkOverviewItem> WorkingOverviewList =>
			dailyReportModel.WorkSummary.WorkingOverviewList;

		/// <summary>
		///		利用者の状況情報を取得・設定します。
		/// </summary>
		public string AboutUser {
			get { return dailyReportModel.WorkSummary.AboutUser; }
			set { dailyReportModel.WorkSummary.AboutUser = value; }
		}

		#endregion

		#region 質問

		/// <summary>
		///		質問のリストのインデックスを表します。
		/// </summary>
		private int questionListIndex;
		/// <summary>
		///		質問のリストのインデックスを取得・設定します。
		/// </summary>
		public int QuestionListIndex {
			get { return questionListIndex; }
			set {
				questionListIndex = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		質問のリストを取得します。
		/// </summary>
		public ObservableCollection<QuestionItem> QuestionList =>
			dailyReportModel.QuestionList;

		#endregion

		#region 機器・アプリのトラブル情報

		/// <summary>
		///		障害情報のリストのインデックスを表します。
		/// </summary>
		private int failureInfoListIndex;
		/// <summary>
		///		障害情報のリストのインデックスを取得・設定します。
		/// </summary>
		public int FailureInfoListIndex {
			get { return failureInfoListIndex; }
			set {
				failureInfoListIndex = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		障害情報のリストを取得します。
		/// </summary>
		public ObservableCollection<FailureInfoItem> FailureInfoList =>
			dailyReportModel.FailureInfoList;

		/// <summary>
		///		備品のプリセットのリストを取得します。
		/// </summary>
		public IEnumerable<string> FixtureTypeList =>
			PresetModel.FixtureTypeList;

		#endregion

		#region 不正印刷情報

		/// <summary>
		///		不正印刷情報のリストのインデックスを表します。
		/// </summary>
		private int injusticePrintListIndex;
		/// <summary>
		///		不正印刷情報のリストのインデックスを取得・設定します。
		/// </summary>
		public int InjusticePrintListIndex {
			get { return injusticePrintListIndex; }
			set {
				injusticePrintListIndex = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		不正印刷情報のリストを取得します。
		/// </summary>
		public ObservableCollection<InjusticePrintItem> InjusticePrintList =>
			dailyReportModel.InjusticePrintList;

		#endregion

		#region 2重ログイン情報

		/// <summary>
		///		2重ログイン情報のリストのインデックスを表します。
		/// </summary>
		private int duplicateLoginListIndex;
		/// <summary>
		///		2重ログイン情報のリストのインデックスを取得・設定します。
		/// </summary>
		public int DuplicateLoginListIndex {
			get { return duplicateLoginListIndex; }
			set {
				duplicateLoginListIndex = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		2重ログイン情報のリストを取得します。
		/// </summary>
		public ObservableCollection<DuplicateLoginItem> DuplicateLoginList =>
			dailyReportModel.DuplicateLoginList;

		#endregion

		#region その他の注意情報

		/// <summary>
		///		その他の注意情報のリストのインデックスを表します。
		/// </summary>
		private int otherMatterListIndex;
		/// <summary>
		///		その他の注意情報のリストのインデックスを取得・設定します。
		/// </summary>
		public int OtherMatterListIndex {
			get { return otherMatterListIndex; }
			set {
				otherMatterListIndex = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		その他の注意情報のリストを取得します。
		/// </summary>
		public ObservableCollection<OtherMatterItem> OtherMatterList =>
			dailyReportModel.OtherMatterList;

		#endregion

		#region 遺失物の情報

		/// <summary>
		///		遺失物の情報のリストのインデックスを表します。
		/// </summary>
		private int lostSumthingListIndex;
		/// <summary>
		///		遺失物の情報のリストのインデックスを取得・設定します。
		/// </summary>
		public int LostSumthingListIndex {
			get { return lostSumthingListIndex; }
			set {
				lostSumthingListIndex = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		///		遺失物の情報のリストを取得します。
		/// </summary>
		public ObservableCollection<LostSumthingItem> LostSumthingList =>
			dailyReportModel.LostSumthingList;

		#endregion

		#region 備品状況

		/// <summary>
		///		備品状況の情報を取得・設定します。
		/// </summary>
		public string FixtureInfo {
			get { return dailyReportModel.FixtureInfo; }
			set { dailyReportModel.FixtureInfo = value; }
		}

		#endregion

		#region プレビュー

		/// <summary>
		///		日報データのプレビューのタイトルを取得します。
		/// </summary>
		public string DailyReportPreviewTitle =>
			dailyReportModel.DailyReportPreviewTitle;

		/// <summary>
		///		日報データのプレビューの本文を取得します。
		/// </summary>
		public string DailyReportPreviewContent =>
			dailyReportModel.DailyReportPreviewContent;

		#endregion

		#region ステータスバー

		/// <summary>
		///		ステータス情報を取得します。
		/// </summary>
		public ProReactsCore<string> StatusInfo =>
			dailyReportModel.ProReactsCore;

		#endregion

		#endregion

		#region コンストラクター

		/// <summary>
		///		MainViewModelの新しいインスタンスを生成します。
		/// </summary>
		public MainViewModel() {

			dailyReportModel = new DailyReportModel();
			WorkingOverviewList.Add( new WorkOverviewItem() );

			dailyReportModel.PropertyChanged +=
				( sender, e ) =>
					PropertyChangedFromThis?.Invoke( sender, e );

			dailyReportModel.GenerateDailyReportCompleted +=
				( sender, e ) => {
					NotifyPropertyChanged( nameof( DailyReportPreviewTitle ) );
					NotifyPropertyChanged( nameof( DailyReportPreviewContent ) );
				};

			AppInfo = FileVersionInfo.GetVersionInfo( Assembly.GetExecutingAssembly().Location );

			FormatModel.LoadFormatsCompleted +=
				( sender, e ) => {
					if( string.IsNullOrEmpty( e.AdditionalInfo ) ) {
						dailyReportModel.Reporter.Report( e.Message );
					}
					else {
						dailyReportModel.Reporter.Report( $"{e.Message} (追加情報 : {e.AdditionalInfo})" );
					}
				};

			FormatModel.LoadFormatsAsync( $"{appDirectory}/formats.xml", CancellationToken.None );
		}

		#endregion

		#region メソッド

		/// <summary>
		///		フォーム上に入力したデータをすべてクリアします。
		/// </summary>
		private void ClearInputFormData() {
			dailyReportModel.ClearAll();
			dailyReportModel.Reporter.Report( "フォームに入力した日報データをすべてクリアしました。" );
			NotifyInputFormPropertyChanged();
		}

		/// <summary>
		///		ViewModelで定義したプロパティの変更を一括で通知します。
		/// </summary>
		private void NotifyInputFormPropertyChanged() {
			NotifyPropertyChanged( nameof( Date ) );
			NotifyPropertyChanged( nameof( RoomName ) );
			NotifyPropertyChanged( nameof( TAName ) );
			NotifyPropertyChanged( nameof( WorkStartTime) );
			NotifyPropertyChanged( nameof( WorkEndTime ) );
			NotifyPropertyChanged( nameof( AboutUser ) );
			NotifyPropertyChanged( nameof( FixtureInfo ) );
			NotifyPropertyChanged( nameof( Description ) );
			NotifyPropertyChanged( nameof( DailyReportPreviewTitle ) );
			NotifyPropertyChanged( nameof( DailyReportPreviewContent ) );
		}

		#endregion

		#region イベントハンドラー

		/// <summary>
		///		
		/// </summary>
		public event ComfirmEventHandler ComfirmAction;

		public event CallbackEventHandler<string> LoadDRMInputFileAction;

		public event CallbackEventHandler<string> SaveDRMInputFileAction;

		public event InteractiveMassagingEventHandler<string, string> SaveDRAction;

		#endregion

		#region ActionCommand

		#region TAInfo

		/// <summary>
		///		フォームに入力した日報データをすべてクリアするコマンドを表します。
		/// </summary>
		private ICommand setTodayCommand;
		/// <summary>
		///		フォームに入力した日報データをすべてクリアするコマンドを取得します。
		/// </summary>
		public ICommand SetTodayCommand =>
			setTodayCommand ??
			( setTodayCommand =
				new ActionCommand(
					this,
					_ => Date = DateTime.Today,
					_ => true
				)
			);

		/// <summary>
		///		指定した日報データのファイルを読み込むコマンドを表します。
		/// </summary>
		private ICommand loadDRDataFileCommand;
		/// <summary>
		///		指定した日報データのファイルを読み込むコマンドを取得します。
		/// </summary>
		public ICommand LoadDRDataFileCommand =>
			loadDRDataFileCommand ??
			( loadDRDataFileCommand =
				new ActionCommand(
					this,
					_ => {
						LoadDRMInputFileAction?.Invoke( this,
							new CallbackEventArgs<string>(
								fileName => {
									dailyReportModel.LoadDataFileAsync( fileName );
									NotifyInputFormPropertyChanged();
								}
							)
						);
						
					},
					_ => true
				)
			);

		/// <summary>
		///		指定した日報データのファイルに保存するコマンドを表します。
		/// </summary>
		private ICommand saveDRDataFileCommand;
		/// <summary>
		///		フ指定した日報データのファイルに保存するコマンドを取得します。
		/// </summary>
		public ICommand SaveDRDataFileCommand =>
			saveDRDataFileCommand ??
			( saveDRDataFileCommand =
				new ActionCommand(
					this,
					_ => {
						SaveDRMInputFileAction?.Invoke( this,
							new CallbackEventArgs<string>(
								fileName => dailyReportModel.SaveDataFileAsync( fileName )
							)
						);
					},
					_ => true
				)
			);

		/// <summary>
		///		フォームに入力した日報データをすべてクリアするコマンドを表します。
		/// </summary>
		private ICommand clearInputFormDataCommand;
		/// <summary>
		///		フォームに入力した日報データをすべてクリアするコマンドを取得します。
		/// </summary>
		public ICommand ClearInputFormDataCommand =>
			clearInputFormDataCommand ??
			( clearInputFormDataCommand =
				new ActionCommand(
					this,
					_ => ComfirmAction?.Invoke(
						this,
						new ComfirmEventArgs( "フォームに入力した日報データを全てクリアしてよろしいですか？", () => ClearInputFormData() )
					),
					_ => true
				)
			);

		/// <summary>
		///		日報データの一時ファイルを読み込むコマンドを表します。
		/// </summary>
		private ICommand loadDRTempCommand;
		/// <summary>
		///		日報データの一時ファイルを読み込むコマンドを取得します。
		/// </summary>
		public ICommand LoadDRTempCommand =>
			loadDRTempCommand ??
			( loadDRTempCommand =
				new ActionCommand(
					this,
					_ => {
						dailyReportModel.LoadDataFileAsync( $@"{appDirectory}\temp.drmitatmp" );
						NotifyInputFormPropertyChanged();
					},
					_ => true
				)
			);

		/// <summary>
		///		日報データの一時ファイルに保存するコマンドを表します。
		/// </summary>
		private ICommand saveDRTempCommand;
		/// <summary>
		///		日報データの一時ファイルに保存するコマンドを取得します。
		/// </summary>
		public ICommand SaveDRTempCommand =>
			saveDRTempCommand ??
			( saveDRTempCommand =
				new ActionCommand(
					this,
					_ => dailyReportModel.SaveDataFileAsync( $@"{appDirectory}\temp.drmitatmp" ),
					_ => true
				)
			);

		#endregion

		#region 業務概要

		/// <summary>
		///		業務概要の項目を追加するコマンドを表します。
		/// </summary>
		private ICommand addWorkingOverviewItemCommand;
		/// <summary>
		///		業務概要の項目を追加するコマンドを取得します。
		/// </summary>
		public ICommand AddWorkingOverviewItemCommand =>
			addWorkingOverviewItemCommand ??
			( addWorkingOverviewItemCommand =
				new ActionCommand(
					this,
					_ => {
						WorkingOverviewList.Add( new WorkOverviewItem() );
						WorkingOverviewListIndex = WorkingOverviewList.Count - 1;
					},
					_ => true
				)
			);

		/// <summary>
		///		業務概要の項目をすべてクリアするコマンドを表します。
		/// </summary>
		private ICommand clearWorkingOverviewListCommand;
		/// <summary>
		///		業務概要の項目をすべてクリアするコマンドを取得します。
		/// </summary>
		public ICommand ClearWorkingOverviewListCommand =>
			clearWorkingOverviewListCommand ??
			( clearWorkingOverviewListCommand =
				new ActionCommand(
					this,
					_ => ComfirmAction?.Invoke(
						this,
						new ComfirmEventArgs( "業務概要の項目をすべてクリアしてよろしいですか？", dailyReportModel.ClearWorkOverviewList )
					),
					_ => WorkingOverviewList?.Any() ?? false
				)
			);

		/// <summary>
		///		業務概要の項目を開始時間順にソートするコマンドを表します。
		/// </summary>
		private ICommand sortWorkingOverviewItemByStartTimeCommand;

		/// <summary>
		///		業務概要の項目を開始時間順にソートするコマンドを取得します。
		/// </summary>
		public ICommand SortWorkingOverviewItemByStartTimeCommand =>
			sortWorkingOverviewItemByStartTimeCommand ??
			( sortWorkingOverviewItemByStartTimeCommand =
				new ActionCommand(
					this,
					_ => dailyReportModel.SortWorkOverviewList(),
					_ => WorkingOverviewList?.Any() ?? false
				)
			);

		/// <summary>
		///		選択した業務概要の項目を削除するコマンドを表します。
		/// </summary>
		private ICommand deleteWorkingOverviewItemCommand;

		/// <summary>
		///		選択した業務概要の項目を削除するコマンドを取得します。。
		/// </summary>
		public ICommand DeleteWorkingOverviewItemCommand =>
			deleteWorkingOverviewItemCommand ??
			( deleteWorkingOverviewItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( WorkingOverviewListIndex >= 0 && WorkingOverviewListIndex < WorkingOverviewList.Count ) {
							int index = WorkingOverviewListIndex;
							if( WorkingOverviewListIndex >= 0 && WorkingOverviewListIndex < WorkingOverviewList.Count ) {
								WorkingOverviewList.RemoveAt( WorkingOverviewListIndex );
								WorkingOverviewListIndex = index < WorkingOverviewList.Count ? index : index - 1;
							}
						}
					},
					_ => true
				)
			);

		/// <summary>
		///		選択した業務概要の項目を1つ上に移動するコマンドを表します。
		/// </summary>
		private ICommand moveUpWorkingOverviewItemCommand;

		/// <summary>
		///		選択した業務概要の項目を1つ上に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveUpWorkingOverviewItemCommand =>
			moveUpWorkingOverviewItemCommand ??
			( moveUpWorkingOverviewItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( WorkingOverviewListIndex >= 0 && WorkingOverviewListIndex < WorkingOverviewList.Count ) {
							int index = WorkingOverviewListIndex;
							if( index > 0 ) {
								WorkingOverviewList.Move( index, index - 1 );
							}
						}
					},
					_ => WorkingOverviewList.Count > 1
				)
			);

		/// <summary>
		///		選択した業務概要の項目を1つ下に移動するコマンドを表します。
		/// </summary>
		private ICommand moveDownWorkingOverviewItemCommand;

		/// <summary>
		///		選択した業務概要の項目を1つ下に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveDownWorkingOverviewItemCommand =>
			moveDownWorkingOverviewItemCommand ??
			( moveDownWorkingOverviewItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( WorkingOverviewListIndex >= 0 && WorkingOverviewListIndex < WorkingOverviewList.Count ) {
							int index = WorkingOverviewListIndex;
							if( index < WorkingOverviewList.Count - 1 ) {
								WorkingOverviewList.Move( index, index + 1 );
							}
						}
					},
					_ => WorkingOverviewList.Count > 1
				)
			);

		#endregion

		#region 質問

		/// <summary>
		///		質問の項目を追加するコマンドを表します。
		/// </summary>
		private ICommand addQuestionItemCommand;
		/// <summary>
		///		質問の項目を追加するコマンドを取得します。
		/// </summary>
		public ICommand AddQuestionItemCommand =>
			addQuestionItemCommand ??
			( addQuestionItemCommand =
				new ActionCommand(
					this,
					_ => {
						QuestionList.Add( new QuestionItem() );
						QuestionListIndex = QuestionList.Count - 1;
					},
					_ => true
				)
			);

		/// <summary>
		///		学生からの質問の項目をすべてクリアするコマンドを表します。
		/// </summary>
		private ICommand clearQuestionListCommand;
		/// <summary>
		///		学生からの質問の項目をすべてクリアするコマンドを取得します。
		/// </summary>
		public ICommand ClearQuestionListCommand =>
			clearQuestionListCommand ??
			( clearQuestionListCommand =
				new ActionCommand(
					this,
					_ => ComfirmAction?.Invoke(
						this,
						new ComfirmEventArgs( "質問の項目をすべてクリアしてよろしいですか？", dailyReportModel.ClearQuestionList )
					),
					_ => QuestionList.Any()
				)
			);

		/// <summary>
		///		質問の項目を時間順にソートするコマンドを表します。
		/// </summary>
		private ICommand sortQuestionItemByTimeCommand;

		/// <summary>
		///		質問の項目を時間順にソートするコマンドを取得します。
		/// </summary>
		public ICommand SortQuestionItemByTimeCommand =>
			sortQuestionItemByTimeCommand ??
			( sortQuestionItemByTimeCommand =
				new ActionCommand(
					this,
					_ => dailyReportModel.SortQuestionList(),
					_ => QuestionList.Any()
				)
			);

		/// <summary>
		///		選択した質問の項目を削除するコマンドを表します。
		/// </summary>
		private ICommand deleteQuestionItemCommand;

		/// <summary>
		///		選択した質問の項目を削除するコマンドを取得します。。
		/// </summary>
		public ICommand DeleteQuestionItemCommand =>
			deleteQuestionItemCommand ??
			( deleteQuestionItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( QuestionListIndex >= 0 && QuestionListIndex < QuestionList.Count ) {
							int index = QuestionListIndex;
							if( QuestionListIndex >= 0 && QuestionListIndex < QuestionList.Count ) {
								QuestionList.RemoveAt( QuestionListIndex );
								QuestionListIndex = index < QuestionList.Count ? index : index - 1;
							}
						}
					},
					_ => true
				)
			);

		/// <summary>
		///		選択した質問の項目を1つ上に移動するコマンドを表します。
		/// </summary>
		private ICommand moveUpQuestionItemCommand;

		/// <summary>
		///		選択した質問の項目を1つ上に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveUpQuestionItemCommand =>
			moveUpQuestionItemCommand ??
			( moveUpQuestionItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( QuestionListIndex >= 0 && QuestionListIndex < QuestionList.Count ) {
							int index = QuestionListIndex;
							if( index > 0 ) {
								QuestionList.Move( index, index - 1 );
							}
						}
					},
					_ => QuestionList.Count > 1
				)
			);

		/// <summary>
		///		選択した質問の項目を1つ下に移動するコマンドを表します。
		/// </summary>
		private ICommand moveDownQuestionItemCommand;

		/// <summary>
		///		選択した質問の項目を1つ下に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveDownQuestionItemCommand =>
			moveDownQuestionItemCommand ??
			( moveDownQuestionItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( QuestionListIndex >= 0 && QuestionListIndex < QuestionList.Count ) {
							int index = QuestionListIndex;
							if( index < QuestionList.Count - 1 ) {
								QuestionList.Move( index, index + 1 );
							}
						}
					},
					_ => QuestionList.Count > 1
				)
			);

		#endregion

		#region 機器・アプリのトラブル情報

		/// <summary>
		///		機器・アプリのトラブル情報の項目を追加するコマンドを表します。
		/// </summary>
		private ICommand addFailureInfoItemCommand;
		/// <summary>
		///		機器・アプリのトラブル情報の項目を追加するコマンドを取得します。
		/// </summary>
		public ICommand AddFailureInfoItemCommand =>
			addFailureInfoItemCommand ??
			( addFailureInfoItemCommand =
				new ActionCommand(
					this,
					_ => {
						FailureInfoList.Add( new FailureInfoItem() );
						FailureInfoListIndex = FailureInfoList.Count - 1;
					},
					_ => true
				)
			);

		/// <summary>
		///		機器・アプリのトラブル情報の項目をすべてクリアするコマンドを表します。
		/// </summary>
		private ICommand clearFailureInfoListCommand;
		/// <summary>
		///		機器・アプリのトラブル情報の項目をすべてクリアするコマンドを取得します。
		/// </summary>
		public ICommand ClearFailureInfoListCommand =>
			clearFailureInfoListCommand ??
			( clearFailureInfoListCommand =
				new ActionCommand(
					this,
					_ => ComfirmAction?.Invoke(
						this,
						new ComfirmEventArgs( "機器・アプリのトラブル情報の項目をすべてクリアしてよろしいですか？", dailyReportModel.ClearFailureInfoList )
					),
					_ => FailureInfoList.Any()
				)
			);

		/// <summary>
		///		機器・アプリのトラブル情報の項目を時間順にソートするコマンドを表します。
		/// </summary>
		private ICommand sortFailureInfoItemByTimeCommand;

		/// <summary>
		///		機器・アプリのトラブル情報の項目を時間順にソートするコマンドを取得します。
		/// </summary>
		public ICommand SortFailureInfoItemByTimeCommand =>
			sortFailureInfoItemByTimeCommand ??
			( sortFailureInfoItemByTimeCommand =
				new ActionCommand(
					this,
					_ => dailyReportModel.SortFailureInfoList(),
					_ => FailureInfoList.Any()
				)
			);

		/// <summary>
		///		選択した機器・アプリのトラブル情報の項目を削除するコマンドを表します。
		/// </summary>
		private ICommand deleteFailureInfoItemCommand;

		/// <summary>
		///		選択した機器・アプリのトラブル情報の項目を削除するコマンドを取得します。。
		/// </summary>
		public ICommand DeleteFailureInfoItemCommand =>
			deleteFailureInfoItemCommand ??
			( deleteFailureInfoItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( FailureInfoListIndex >= 0 && FailureInfoListIndex < FailureInfoList.Count ) {
							int index = FailureInfoListIndex;
							if( FailureInfoListIndex >= 0 && FailureInfoListIndex < FailureInfoList.Count ) {
								FailureInfoList.RemoveAt( FailureInfoListIndex );
								FailureInfoListIndex = index < FailureInfoList.Count ? index : index - 1;
							}
						}
					},
					_ => true
				)
			);

		/// <summary>
		///		選択した機器・アプリのトラブル情報の項目を1つ上に移動するコマンドを表します。
		/// </summary>
		private ICommand moveUpFailureInfoItemCommand;

		/// <summary>
		///		選択した機器・アプリのトラブル情報の項目を1つ上に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveUpFailureInfoItemCommand =>
			moveUpFailureInfoItemCommand ??
			( moveUpFailureInfoItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( FailureInfoListIndex >= 0 && FailureInfoListIndex < FailureInfoList.Count ) {
							int index = FailureInfoListIndex;
							if( index > 0 ) {
								FailureInfoList.Move( index, index - 1 );
							}
						}
					},
					_ => FailureInfoList.Count > 1
				)
			);

		/// <summary>
		///		選択した機器・アプリのトラブル情報の項目を1つ下に移動するコマンドを表します。
		/// </summary>
		private ICommand moveDownFailureInfoItemCommand;

		/// <summary>
		///		選択した機器・アプリのトラブル情報の項目を1つ下に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveDownFailureInfoItemCommand =>
			moveDownFailureInfoItemCommand ??
			( moveDownFailureInfoItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( FailureInfoListIndex >= 0 && FailureInfoListIndex < FailureInfoList.Count ) {
							int index = FailureInfoListIndex;
							if( index < FailureInfoList.Count - 1 ) {
								FailureInfoList.Move( index, index + 1 );
							}
						}
					},
					_ => FailureInfoList.Count > 1
				)
			);

		#endregion

		#region 不正印刷情報

		/// <summary>
		///		不正印刷情報の項目を追加するコマンドを表します。
		/// </summary>
		private ICommand addInjusticePrintItemCommand;
		/// <summary>
		///		不正印刷情報の項目を追加するコマンドを取得します。
		/// </summary>
		public ICommand AddInjusticePrintItemCommand =>
			addInjusticePrintItemCommand ??
			( addInjusticePrintItemCommand =
				new ActionCommand(
					this,
					_ => {
						InjusticePrintList.Add( new InjusticePrintItem() );
						InjusticePrintListIndex = InjusticePrintList.Count - 1;
					},
					_ => true
				)
			);

		/// <summary>
		///		不正印刷情報の項目をすべてクリアするコマンドを表します。
		/// </summary>
		private ICommand clearInjusticePrintListCommand;
		/// <summary>
		///		不正印刷情報の項目をすべてクリアするコマンドを取得します。
		/// </summary>
		public ICommand ClearInjusticePrintListCommand =>
			clearInjusticePrintListCommand ??
			( clearInjusticePrintListCommand =
				new ActionCommand(
					this,
					_ => ComfirmAction?.Invoke(
						this,
						new ComfirmEventArgs( "不正印刷情報の項目をすべてクリアしてよろしいですか？", () => InjusticePrintList.Clear() )
					),
					_ => InjusticePrintList.Any()
				)
			);

		/// <summary>
		///		不正印刷情報の項目を時間順にソートするコマンドを表します。
		/// </summary>
		private ICommand sortInjusticePrintItemByTimeCommand;

		/// <summary>
		///		不正印刷情報の項目を時間順にソートするコマンドを取得します。
		/// </summary>
		public ICommand SortInjusticePrintItemByTimeCommand =>
			sortInjusticePrintItemByTimeCommand ??
			( sortInjusticePrintItemByTimeCommand =
				new ActionCommand(
					this,
					_ => dailyReportModel.SortInjusticePrintList(),
					_ => InjusticePrintList.Any()
				)
			);

		/// <summary>
		///		選択した不正印刷情報の項目を削除するコマンドを表します。
		/// </summary>
		private ICommand deleteInjusticePrintItemCommand;

		/// <summary>
		///		選択した不正印刷情報の項目を削除するコマンドを取得します。。
		/// </summary>
		public ICommand DeleteInjusticePrintItemCommand =>
			deleteInjusticePrintItemCommand ??
			( deleteInjusticePrintItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( InjusticePrintListIndex >= 0 && InjusticePrintListIndex < InjusticePrintList.Count ) {
							int index = InjusticePrintListIndex;
							if( InjusticePrintListIndex >= 0 && InjusticePrintListIndex < InjusticePrintList.Count ) {
								InjusticePrintList.RemoveAt( InjusticePrintListIndex );
								InjusticePrintListIndex = index < InjusticePrintList.Count ? index : index - 1;
							}
						}
					},
					_ => true
				)
			);

		/// <summary>
		///		選択した不正印刷情報の項目を1つ上に移動するコマンドを表します。
		/// </summary>
		private ICommand moveUpInjusticePrintItemCommand;

		/// <summary>
		///		選択した不正印刷情報の項目を1つ上に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveUpInjusticePrintItemCommand =>
			moveUpInjusticePrintItemCommand ??
			( moveUpInjusticePrintItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( InjusticePrintListIndex >= 0 && InjusticePrintListIndex < InjusticePrintList.Count ) {
							int index = InjusticePrintListIndex;
							if( index > 0 ) {
								InjusticePrintList.Move( index, index - 1 );
							}
						}
					},
					_ => InjusticePrintList.Count > 1
				)
			);

		/// <summary>
		///		選択した不正印刷情報の項目を1つ下に移動するコマンドを表します。
		/// </summary>
		private ICommand moveDownInjusticePrintItemCommand;

		/// <summary>
		///		選択した不正印刷情報の項目を1つ下に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveDownInjusticePrintItemCommand =>
			moveDownInjusticePrintItemCommand ??
			( moveDownInjusticePrintItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( InjusticePrintListIndex >= 0 && InjusticePrintListIndex < InjusticePrintList.Count ) {
							int index = InjusticePrintListIndex;
							if( index < InjusticePrintList.Count - 1 ) {
								InjusticePrintList.Move( index, index + 1 );
							}
						}
					},
					_ => InjusticePrintList.Count > 1
				)
			);

		#endregion

		#region 2重ログイン情報

		/// <summary>
		///		2重ログイン情報の項目を追加するコマンドを表します。
		/// </summary>
		private ICommand addDuplicateLoginItemCommand;
		/// <summary>
		///		2重ログイン情報の項目を追加するコマンドを取得します。
		/// </summary>
		public ICommand AddDuplicateLoginItemCommand =>
			addDuplicateLoginItemCommand ??
			( addDuplicateLoginItemCommand =
				new ActionCommand(
					this,
					_ => {
						DuplicateLoginList.Add( new DuplicateLoginItem() );
						DuplicateLoginListIndex = DuplicateLoginList.Count - 1;
					},
					_ => true
				)
			);

		/// <summary>
		///		2重ログイン情報の項目をすべてクリアするコマンドを表します。
		/// </summary>
		private ICommand clearDuplicateLoginListCommand;
		/// <summary>
		///		2重ログイン情報の項目をすべてクリアするコマンドを取得します。
		/// </summary>
		public ICommand ClearDuplicateLoginListCommand =>
			clearDuplicateLoginListCommand ??
			( clearDuplicateLoginListCommand =
				new ActionCommand(
					this,
					_ => ComfirmAction?.Invoke(
						this,
						new ComfirmEventArgs( "2重ログイン情報の項目をすべてクリアしてよろしいですか？", dailyReportModel.ClearDuplicateLoginList )
					),
					_ => DuplicateLoginList.Any()
				)
			);

		/// <summary>
		///		2重ログイン情報の項目を時間順にソートするコマンドを表します。
		/// </summary>
		private ICommand sortDuplicateLoginItemByTimeCommand;

		/// <summary>
		///		2重ログイン情報の項目を時間順にソートするコマンドを取得します。
		/// </summary>
		public ICommand SortDuplicateLoginItemByTimeCommand =>
			sortDuplicateLoginItemByTimeCommand ??
			( sortDuplicateLoginItemByTimeCommand =
				new ActionCommand(
					this,
					_ => dailyReportModel.SortDuplicateLoginList(),
					_ => DuplicateLoginList.Any()
				)
			);

		/// <summary>
		///		選択した2重ログイン情報の項目を削除するコマンドを表します。
		/// </summary>
		private ICommand deleteDuplicateLoginItemCommand;

		/// <summary>
		///		選択した2重ログイン情報の項目を削除するコマンドを取得します。。
		/// </summary>
		public ICommand DeleteDuplicateLoginItemCommand =>
			deleteDuplicateLoginItemCommand ??
			( deleteDuplicateLoginItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( DuplicateLoginListIndex >= 0 && DuplicateLoginListIndex < DuplicateLoginList.Count ) {
							int index = DuplicateLoginListIndex;
							if( DuplicateLoginListIndex >= 0 && DuplicateLoginListIndex < DuplicateLoginList.Count ) {
								DuplicateLoginList.RemoveAt( DuplicateLoginListIndex );
								DuplicateLoginListIndex = index < DuplicateLoginList.Count ? index : index - 1;
							}
						}
					},
					_ => true
				)
			);

		/// <summary>
		///		選択した2重ログイン情報の項目を1つ上に移動するコマンドを表します。
		/// </summary>
		private ICommand moveUpDuplicateLoginItemCommand;

		/// <summary>
		///		選択した2重ログイン情報の項目を1つ上に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveUpDuplicateLoginItemCommand =>
			moveUpDuplicateLoginItemCommand ??
			( moveUpDuplicateLoginItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( DuplicateLoginListIndex >= 0 && DuplicateLoginListIndex < DuplicateLoginList.Count ) {
							int index = DuplicateLoginListIndex;
							if( index > 0 ) {
								DuplicateLoginList.Move( index, index - 1 );
							}
						}
					},
					_ => DuplicateLoginList.Count > 1
				)
			);

		/// <summary>
		///		選択した2重ログイン情報の項目を1つ下に移動するコマンドを表します。
		/// </summary>
		private ICommand moveDownDuplicateLoginItemCommand;

		/// <summary>
		///		選択した2重ログイン情報の項目を1つ下に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveDownDuplicateLoginItemCommand =>
			moveDownDuplicateLoginItemCommand ??
			( moveDownDuplicateLoginItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( DuplicateLoginListIndex >= 0 && DuplicateLoginListIndex < DuplicateLoginList.Count ) {
							int index = DuplicateLoginListIndex;
							if( index < DuplicateLoginList.Count - 1 ) {
								DuplicateLoginList.Move( index, index + 1 );
							}
						}
					},
					_ => DuplicateLoginList.Count > 1
				)
			);

		#endregion

		#region その他の注意情報

		/// <summary>
		///		その他の注意情報の項目を追加するコマンドを表します。
		/// </summary>
		private ICommand addOtherMatterItemCommand;
		/// <summary>
		///		その他の注意情報の項目を追加するコマンドを取得します。
		/// </summary>
		public ICommand AddOtherMatterItemCommand =>
			addOtherMatterItemCommand ??
			( addOtherMatterItemCommand =
				new ActionCommand(
					this,
					_ => {
						OtherMatterList.Add( new OtherMatterItem() );
						OtherMatterListIndex = OtherMatterList.Count - 1;
					},
					_ => true
				)
			);

		/// <summary>
		///		その他の注意情報の項目をすべてクリアするコマンドを表します。
		/// </summary>
		private ICommand clearOtherMatterListCommand;
		/// <summary>
		///		その他の注意情報の項目をすべてクリアするコマンドを取得します。
		/// </summary>
		public ICommand ClearOtherMatterListCommand =>
			clearOtherMatterListCommand ??
			( clearOtherMatterListCommand =
				new ActionCommand(
					this,
					_ => ComfirmAction?.Invoke(
						this,
						new ComfirmEventArgs( "その他の注意情報の項目をすべてクリアしてよろしいですか？", dailyReportModel.ClearOtherMatterList )
					),
					_ => OtherMatterList.Any()
				)
			);

		/// <summary>
		///		選択したその他の注意情報の項目を削除するコマンドを表します。
		/// </summary>
		private ICommand deleteOtherMatterItemCommand;

		/// <summary>
		///		選択したその他の注意情報の項目を削除するコマンドを取得します。。
		/// </summary>
		public ICommand DeleteOtherMatterItemCommand =>
			deleteOtherMatterItemCommand ??
			( deleteOtherMatterItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( OtherMatterListIndex >= 0 && OtherMatterListIndex < OtherMatterList.Count ) {
							int index = OtherMatterListIndex;
							if( OtherMatterListIndex >= 0 && OtherMatterListIndex < OtherMatterList.Count ) {
								OtherMatterList.RemoveAt( OtherMatterListIndex );
								OtherMatterListIndex = index < OtherMatterList.Count ? index : index - 1;
							}
						}
					},
					_ => true
				)
			);

		/// <summary>
		///		選択したその他の注意情報の項目を1つ上に移動するコマンドを表します。
		/// </summary>
		private ICommand moveUpOtherMatterItemCommand;

		/// <summary>
		///		選択したその他の注意情報の項目を1つ上に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveUpOtherMatterItemCommand =>
			moveUpOtherMatterItemCommand ??
			( moveUpOtherMatterItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( OtherMatterListIndex >= 0 && OtherMatterListIndex < OtherMatterList.Count ) {
							int index = OtherMatterListIndex;
							if( index > 0 ) {
								OtherMatterList.Move( index, index - 1 );
							}
						}
					},
					_ => OtherMatterList.Count > 1
				)
			);

		/// <summary>
		///		選択したその他の注意情報の項目を1つ下に移動するコマンドを表します。
		/// </summary>
		private ICommand moveDownOtherMatterItemCommand;

		/// <summary>
		///		選択したその他の注意情報の項目を1つ下に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveDownOtherMatterItemCommand =>
			moveDownOtherMatterItemCommand ??
			( moveDownOtherMatterItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( OtherMatterListIndex >= 0 && OtherMatterListIndex < OtherMatterList.Count ) {
							int index = OtherMatterListIndex;
							if( index < OtherMatterList.Count - 1 ) {
								OtherMatterList.Move( index, index + 1 );
							}
						}
					},
					_ => OtherMatterList.Count > 1
				)
			);

		#endregion

		#region 遺失物の情報

		/// <summary>
		///		遺失物の情報の項目を追加するコマンドを表します。
		/// </summary>
		private ICommand addLostSumthingItemCommand;
		/// <summary>
		///		遺失物の情報の項目を追加するコマンドを取得します。
		/// </summary>
		public ICommand AddLostSumthingItemCommand =>
			addLostSumthingItemCommand ??
			( addLostSumthingItemCommand =
				new ActionCommand(
					this,
					_ => {
						LostSumthingList.Add( new LostSumthingItem() );
						LostSumthingListIndex = LostSumthingList.Count - 1;
					},
					_ => true
				)
			);

		/// <summary>
		///		遺失物の情報の項目をすべてクリアするコマンドを表します。
		/// </summary>
		private ICommand clearLostSumthingListCommand;
		/// <summary>
		///		遺失物の情報の項目をすべてクリアするコマンドを取得します。
		/// </summary>
		public ICommand ClearLostSumthingListCommand =>
			clearLostSumthingListCommand ??
			( clearLostSumthingListCommand =
				new ActionCommand(
					this,
					_ => ComfirmAction?.Invoke(
						this,
						new ComfirmEventArgs( "遺失物の情報の項目をすべてをクリアしてよろしいですか？", dailyReportModel.ClearLostSumthingList )
					),
					_ => LostSumthingList.Any()
				)
			);

		/// <summary>
		///		遺失物の情報の項目を時間順にソートするコマンドを表します。
		/// </summary>
		private ICommand sortLostSumthingItemByTimeCommand;

		/// <summary>
		///		遺失物の情報の項目を時間順にソートするコマンドを取得します。
		/// </summary>
		public ICommand SortLostSumthingItemByTimeCommand =>
			sortLostSumthingItemByTimeCommand ??
			( sortLostSumthingItemByTimeCommand =
				new ActionCommand(
					this,
					_ => dailyReportModel.SortLostSumthingList(),
					_ => LostSumthingList.Any()
				)
			);

		/// <summary>
		///		選択した遺失物の情報の項目を削除するコマンドを表します。
		/// </summary>
		private ICommand deleteLostSumthingItemCommand;

		/// <summary>
		///		選択した遺失物の情報の項目を削除するコマンドを取得します。。
		/// </summary>
		public ICommand DeleteLostSumthingItemCommand =>
			deleteLostSumthingItemCommand ??
			( deleteLostSumthingItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( LostSumthingListIndex >= 0 && LostSumthingListIndex < LostSumthingList.Count ) {
							int index = LostSumthingListIndex;
							if( LostSumthingListIndex >= 0 && LostSumthingListIndex < LostSumthingList.Count ) {
								LostSumthingList.RemoveAt( LostSumthingListIndex );
								LostSumthingListIndex = index < LostSumthingList.Count ? index : index - 1;
							}
						}
					},
					_ => true
				)
			);

		/// <summary>
		///		選択した遺失物の情報の項目を1つ上に移動するコマンドを表します。
		/// </summary>
		private ICommand moveUpLostSumthingItemCommand;

		/// <summary>
		///		選択した遺失物の情報の項目を1つ上に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveUpLostSumthingItemCommand =>
			moveUpLostSumthingItemCommand ??
			( moveUpLostSumthingItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( LostSumthingListIndex >= 0 && LostSumthingListIndex < LostSumthingList.Count ) {
							int index = LostSumthingListIndex;
							if( index > 0 ) {
								LostSumthingList.Move( index, index - 1 );
							}
						}
					},
					_ => LostSumthingList.Count > 1
				)
			);

		/// <summary>
		///		選択した遺失物の情報の項目を1つ下に移動するコマンドを表します。
		/// </summary>
		private ICommand moveDownLostSumthingItemCommand;

		/// <summary>
		///		選択した遺失物の情報の項目を1つ下に移動するコマンドを取得します。。
		/// </summary>
		public ICommand MoveDownLostSumthingItemCommand =>
			moveDownLostSumthingItemCommand ??
			( moveDownLostSumthingItemCommand =
				new ActionCommand(
					this,
					_ => {
						if( LostSumthingListIndex >= 0 && LostSumthingListIndex < LostSumthingList.Count ) {
							int index = LostSumthingListIndex;
							if( index < LostSumthingList.Count - 1 ) {
								LostSumthingList.Move( index, index + 1 );
							}
						}
					},
					_ => LostSumthingList.Count > 1
				)
			);

		#endregion

		#region プレビュー

		/// <summary>
		///		遺失物の情報の項目を追加するコマンドを表します。
		/// </summary>
		private ICommand generateDailyReportDataCommand;
		/// <summary>
		///		遺失物の情報の項目を追加するコマンドを取得します。
		/// </summary>
		public ICommand GenerateDailyReportDataCommand =>
			generateDailyReportDataCommand ??
			( generateDailyReportDataCommand =
				new ActionCommand(
					this,
					_ => dailyReportModel.GenerateDailyReportDataAsync(),
					_ => true
				)
			);

		/// <summary>
		///		遺失物の情報の項目を追加するコマンドを表します。
		/// </summary>
		private ICommand copyPreviewContentCommand;
		/// <summary>
		///		遺失物の情報の項目を追加するコマンドを取得します。
		/// </summary>
		public ICommand CopyPreviewContentCommand =>
			copyPreviewContentCommand ??
			( copyPreviewContentCommand =
				new ActionCommand(
					this,
					_ => Clipboard.SetText( dailyReportModel.DailyReportPreviewContent ),
					_ => !string.IsNullOrEmpty( DailyReportPreviewTitle )
				)
			);

		/// <summary>
		///		遺失物の情報の項目を追加するコマンドを表します。
		/// </summary>
		private ICommand saveDailyReportDataCommand;
		/// <summary>
		///		遺失物の情報の項目を追加するコマンドを取得します。
		/// </summary>
		public ICommand SaveDailyReportDataCommand =>
			saveDailyReportDataCommand ??
			( saveDailyReportDataCommand =
				new ActionCommand(
					this,
					_ => {
						SaveDRAction?.Invoke( this,
							new InteractiveMassagingEventArgs<string, string>(
								$"日報_{dailyReportModel.TAInfo.RoomName}_{dailyReportModel.TAInfo.Date.ToString( "MMdd" )}_{dailyReportModel.TAInfo.TAName}",
								fileName => dailyReportModel.SaveDailyReportDataAsync( fileName )
							)
						);
					},
					_ => !string.IsNullOrEmpty( DailyReportPreviewTitle )
				)
			);

		#endregion

		#endregion

	}

}
