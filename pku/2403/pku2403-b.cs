// http://poj.org/problem?id=2403 「Hay Point」
using System;
using System.Collections.Generic;
using System.Linq;

public class Program {

	public static void Main() {

		// 標準入力から1行分読み込み、半角スペースで区切ります。
		string[] input = Console.ReadLine().Split( ' ' );
		// Hay Pointのリストに登録する単語数（The number of words in the Hay Point dictionary）
		int numOfRegistedWords = int.Parse( input[0] ),
			// ジョブ記述の数（The number of job descriptions）
			numOfJobs = int.Parse( input[1] );

		// Hay Pointのリスト（単語をキーに、ドルの値とのペアを構成します）
		Dictionary<string, int> HayPointList = new Dictionary<string, int>();

		// HayPointListに単語とドルの値とのペアを登録します。
		for( int i = 0; i < numOfRegistedWords; i++ ) {
			input = Console.ReadLine().Split( ' ' );
			// ※Dictionaryクラスのインデクサーのセット側で、未登録のキーが指定されていた場合、新規のキー・値のペアを追加します。
			HayPointList[input[0]] = int.Parse( input[1] );
		}
		
		// ジョブ記述ごとにHay Pointの値を求め、出力します。
		for( int i = 0; i < numOfJobs; i++ ) {

			// ジョブ記述に含まれる単語のリスト
			List<string> jobWords = new List<string>();

			// ピリオドの直前までの単語をリストに纏めます。
			while( true ) {
				input = Console.ReadLine().Split( ' ' );
				// もし、ピリオドのみ行であれば、jobWordsへの追加を終了します。。
				if( input[0] == "." ) {
					break;
				}
				// 単語を追加します。
				jobWords.AddRange( input );
			}

			// Hay Pointの値を求めます。

			// 同じ単語でグループ化しします。
			int hayPoints = jobWords.GroupBy( _ => _ )
				// 単語ごとにjobWordsにその単語が含まれている数を格納したリストを生成します。
				.Select( _ => new { Word = _.Key, Count = _.Count() } )
				// jobWords に含まれている単語とHay Pointのリストに登録した単語を、Joinメソッドでリレーションシップを結びます。
				// ⇒ jobWordsに含まれている単語の中で、Hay Pointのリストに登録した単語のみを抽出することができます。
				.Join(
					// innerには、Hay Pointのリストを指定します。
					HayPointList,
					// 1つ目のキーには、jobWordsに含まれている単語を指定します。
					keyOuter => keyOuter.Word,
					// 2つ目のキーには、Hay Pointのリストに登録されている単語を指定します。
					keyInner => keyInner.Key,
					// Hay Pointのリストに登録した単語に関連付けられているドルの値と
					// その単語がjobWordsに含まれている数のリストを生成します。
					( outer, inner ) => new { Value = inner.Value, Count = outer.Count }
				)
				// ドルの値とそれに関連付けられたカウント数の積を合計します。
				.Sum( _ => _.Value * _.Count );

			// Hay Pointの値を出力します。
			Console.WriteLine( hayPoints );
		}

	}

}
