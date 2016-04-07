using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;

namespace Phoneword_Droid_Remix {
	/// <summary>
	///		���M������Activity�N���X���`���܂��B
	/// </summary>
	/// <remarks>ListAdapter���g�p���邽�߁AListActivity���p�����܂��B</remarks>
	[Activity( Label = "@string/CallHistory" )]
	public class CallHistoryActivity : ListActivity {

		/// <summary>
		///		���̃A�N�e�B�r�e�B���쐬���ꂽ���Ɏ��s����܂��B
		/// </summary>
		/// <param name="bundle">�o���h��</param>
		protected override void OnCreate( Bundle bundle ) {

			// �o���h���I�u�W�F�N�g��ListActivity��OnCreate���\�b�h�Ɏw�肵�A���s���܂��B
			base.OnCreate( bundle );

			// Intent�o�R�ŁAMainActivity���甭�M�����̃f�[�^���擾���܂��B
			var phoneNumbers = Intent.Extras.GetStringArrayList( "phone_numbers" ) ??
								new string[0];

			// ListAdapter���g���āASimpleListItem1�ɔ��M�����̃A�C�e�����Z�b�g���܂��B
			ListAdapter = new ArrayAdapter<string>(
				this,
				Android.Resource.Layout.SimpleListItem1,
				phoneNumbers
			);
		}
	}
}