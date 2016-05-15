using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace DailyReportMaker {

	public class XMLIOModel {

		public static Task<ResultInfo<bool>> Load( string url, DailyReportModel drm, CancellationToken cancellationToken ) {

			string fileName = Path.GetFileName( url );

			var result = new ResultInfo<bool> {
				Result =true,
				Message = $"日報データファイル({fileName})を読み込みました",
			};

			try {

				var root = XElement.Load( url );
				cancellationToken.ThrowIfCancellationRequested();

				#region TA情報

				var meta = root.Element( "metadata" );
				drm.TAInfo.Date = DateTime.Parse( meta.Attribute( "date" ).Value );
				drm.TAInfo.RoomName = meta.Attribute( "room_name" ).Value;
				drm.TAInfo.TAName = meta.Attribute( "ta_name" ).Value;
				drm.TAInfo.StartTime = DateTime.Parse( meta.Element( "worktime" ).Attribute( "start" ).Value );
				drm.TAInfo.EndTime = DateTime.Parse( meta.Element( "worktime" ).Attribute( "end" ).Value );
				drm.TAInfo.Description = meta.Element( "description" ).Value;

				#endregion

				#region 業務概要

				var worksum = root.Element( "worksummary" );
				var work = worksum.Element( "workoverviewlist" );

				foreach( var item in work.Elements( "workoverview" ) ) {
					WorkTimeItem wti = new WorkTimeItem();

					wti.StartTime = DateTime.Parse( item.Attribute( "starttime" ).Value );
					wti.EndTime = DateTime.Parse( item.Attribute( "endtime" ).Value );
					wti.Description = item.Element( "description" ).Value;
					wti.Remarks = item.Element( "remarks" ).Value;

					drm.WorkSummary.WorkingOverviewList.Add( wti );
				}

				drm.WorkSummary.AboutUser = worksum.Element( "aboutuser" ).Value;

				#endregion

				#region 機器トラブル

				var failinfo = root.Element( "failureinfolist" );

				foreach( var item in failinfo.Elements( "failureinfo" ) ) {
					FailureInfoItem fii = new FailureInfoItem();

					fii.OccuredTime = DateTime.Parse( item.Attribute( "time" ).Value );
					fii.TypeName = item.Element( "fixture" ).Attribute( "type" ).Value;
					fii.Name = item.Element( "fixture" ).Attribute( "name" ).Value;
					fii.Description = item.Element( "description" ).Value;

					drm.FailureInfoList.Add( fii );
				}

				#endregion

				#region 質問

				var question = root.Element( "questionlist" );

				foreach( var item in question.Elements( "question" ) ) {
					QuestionItem qi = new QuestionItem();

					qi.QuestionTime = DateTime.Parse( item.Attribute( "time" ).Value );
					qi.Question = item.Element( "question" ).Value;
					qi.Answer = item.Element( "answer" ).Value;

					drm.QuestionList.Add( qi );
				}

				#endregion

				#region 不正印刷

				var ijprint = root.Element( "injusticeprintlist" );

				foreach( var item in ijprint.Elements( "injusticeprint" ) ) {
					InjusticePrintItem ipi = new InjusticePrintItem();

					ipi.FoundTime = DateTime.Parse( item.Attribute( "time" ).Value );
					ipi.User = item.Element( "user" ).Attribute( "name" ).Value;
					ipi.FileName = item.Element( "file" ).Attribute( "name" ).Value;
					ipi.Description = item.Element( "description" ).Value;

					drm.InjusticePrintList.Add( ipi );
				}

				#endregion

				#region 2重ログイン

				var duplicate = root.Element( "duplicateloginlist" );

				foreach( var item in duplicate.Elements( "duplicatelogin" ) ) {
					DuplicateLoginItem dli = new DuplicateLoginItem();

					dli.FoundTime = DateTime.Parse( item.Attribute( "time" ).Value );
					dli.User = item.Element( "user" ).Value;
					dli.Description = item.Element( "description" ).Value;

					drm.DuplicateLoginList.Add( dli );
				}

				#endregion

				#region その他の注意

				var others = root.Element( "othermatterlist" );

				foreach( var item in others.Elements( "othermatter" ) ) {
					OtherMatterItem omi = new OtherMatterItem();

					omi.Description = item.Element( "description" ).Value;
					omi.Num = int.Parse( item.Attribute( "num" ).Value );

					drm.OtherMatterList.Add( omi );
				}

				#endregion

				#region 遺失物

				var lostThing = root.Element( "lostsumthinglist" );

				foreach( var item in lostThing.Elements( "lostsumthing" ) ) {
					LostSumthingItem lsi = new LostSumthingItem();

					lsi.FoundTime = DateTime.Parse( item.Attribute( "time" ).Value );
					lsi.FoundPlace = item.Attribute( "place" ).Value;
					lsi.Name = item.Element( "name" ).Value;
					lsi.Remarks = item.Element( "remarks" ).Value;

					drm.LostSumthingList.Add( lsi );
				}

				#endregion

				#region 備品状況

				drm.FixtureInfo = root.Element( "fixtureinfo" ).Value;

				#endregion

			}
			catch( Exception e ) {
				result.Result = false;
				result.Message = $"日報データファイル({fileName})の読み込みに失敗しました。";
				result.AdditionalInfo = e.Message;
			}
			finally {
				if( result.AdditionalInfo != null ) {
					drm.Reporter.Report( $"{result.Message} (追加情報 : {result.AdditionalInfo})" );
				}
				else {
					drm.Reporter.Report( result.Message );
				}
			}


			return Task.FromResult( result );
		}

		public static Task<ResultInfo<bool>> Save( string url, DailyReportModel drm, CancellationToken cancellationToken ) {

			string fileName = Path.GetFileName( url );

			var result = new ResultInfo<bool> {
				Result = true,
				Message = $"日報データファイル({fileName})を保存しました。",
			};

			try {

				#region TA情報

				var meta = new XElement( "metadata",
					new XAttribute( "date", drm.TAInfo.Date.ToString( "yyyy/MM/dd" ) ),
					new XAttribute( "room_name", drm.TAInfo.RoomName ),
					new XAttribute( "ta_name", drm.TAInfo.TAName ),
					new XElement( "worktime",
						new XAttribute( "start", drm.TAInfo.StartTime.ToString( "H:mm" ) ),
						new XAttribute( "end", drm.TAInfo.EndTime.ToString( "H:mm" ) )
					),
					new XElement( "description", drm.TAInfo.Description )
				);

				#endregion

				#region 業務概要

				var work = new XElement( "workoverviewlist",
					drm.WorkSummary.WorkingOverviewList.Select(
						item =>
							new XElement( "workoverview",
								new XAttribute( "starttime", item.StartTime.ToString( "H:mm" ) ),
								new XAttribute( "endtime", item.EndTime.ToString( "H:mm" ) ),
								new XElement( "description", item.Description ),
								new XElement( "remarks", item.Remarks )
						)
					)
				);

				var worksum = new XElement( "worksummary",
					work,
					new XElement( "aboutuser", drm.WorkSummary.AboutUser )
				);

				#endregion

				#region 機器トラブル

				var failinfo = new XElement( "failureinfolist",
					drm.FailureInfoList.Select(
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

				var question = new XElement( "questionlist",
					drm.QuestionList.Select(
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

				var ijprint = new XElement( "injusticeprintlist",
					drm.InjusticePrintList.Select(
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

				var duplicate = new XElement( "duplicateloginlist",
					drm.DuplicateLoginList.Select(
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

				var othters = new XElement( "othermatterlist",
					drm.OtherMatterList.Select(
						item =>
							new XElement( "othermatter",
								new XAttribute( "num", item.Num ),
								new XElement( "description", item.Description )
							)
					)
				);

				#endregion

				#region 遺失物

				var lostThing = new XElement( "lostsumthinglist",
					drm.LostSumthingList.Select(
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

				var fixture = new XElement( "fixtureinfo", drm.FixtureInfo );

				#endregion

				var root = new XElement( "dailyreport",
					meta, worksum, failinfo, question, ijprint, duplicate, othters, lostThing, fixture
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
					drm.Reporter.Report( $"{result.Message} (追加情報 : {result.AdditionalInfo})" );
				}
				else {
					drm.Reporter.Report( result.Message );
				}
			}

			return Task.FromResult( result );
		}

	}

	public class DRIOModel {


		public static Task<ResultInfo<bool>> Save( string url, DailyReportModel drm, CancellationToken cancellationToken ) {

			string fileName = Path.GetFileName( url );

			var result = new ResultInfo<bool> {
				Result = true,
				Message = $"日報データをテキストファイル({fileName})を保存しました。",
			};

			try {

				using( StreamWriter sw = new StreamWriter( url ) ) {
					sw.WriteLine( $"作成日 : {DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" )}" );
					sw.WriteLine( "--- タイトル ---" );
					sw.WriteLine( drm.DailyReportPreviewTitle );
					sw.WriteLine( "------" );
					sw.WriteLine( "--- 本文 ---" );
					sw.WriteLine( drm.DailyReportPreviewContent );
					sw.WriteLine( "------" );
				}

			}
			catch( Exception e ) {
				result.Result = false;
				result.Message = $"日報データをテキストファイル({fileName})に保存できませんでした。";
				result.AdditionalInfo = e.Message;
			}
			finally {
				if( result.AdditionalInfo != null ) {
					drm.Reporter.Report( $"{result.Message} (追加情報 : {result.AdditionalInfo})" );
				}
				else {
					drm.Reporter.Report( result.Message );
				}
			}

			return Task.FromResult( result );
		}

	}
}
