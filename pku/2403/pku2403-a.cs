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

			// 求めるHay Pointの値
			int hayPoints = 0;

			while( true ) {
                
				input = Console.ReadLine().Split( ' ' );
				
				// もし、ピリオドのみ行であれば、次のジョブ記述を読み取ります。
				// ※文字列の比較に、「==」演算子を使用することができます。
				if( input[0] == "." ) {
					break;
				}

				// HayPointListに登録されている単語のみを抽出します。
				hayPoints += input.Where( si => HayPointList.ContainsKey( si ) )
								  // その単語に関連付けられている、ドルの値を取り出します。
								  .Select( si => HayPointList[si] )
								  // ドルの値の合計を求めます。
								  .Sum();

				// ※Where → Select とメソッドをつなげた場合、コンパイル時に最適化されます。
			}

			// Hay Pointの値を出力します。
			Console.WriteLine( hayPoints );
		}

	}

}
