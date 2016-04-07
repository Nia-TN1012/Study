using System.Text;

namespace Core {
	/// <summary>
	///		�d�b�ԍ����e���L�[�̔z�u�œ����Ȑ����ɕϊ�����@�\��񋟂��܂��B
	/// </summary>
	public static class PhonewordTranslator {

		/// <summary>
		///		�w�肵���d�b�ԍ����e���L�[�̔z�u�œ����Ȑ����ɕϊ����܂��B
		/// </summary>
		/// <param name="raw">�d�b�ԍ�</param>
		/// <returns>�e���L�[�̔z�u�œ����Ȑ����ɕϊ������d�b�ԍ�</returns>
		public static string ToNumber( string raw ) {

			// �d�b�ԍ��������͎��́A�󕶎���Ԃ��܂��B
			if( string.IsNullOrWhiteSpace( raw ) ) {
				return "";
			}

			// ��������啶���ɕϊ����܂��B
			raw = raw.ToUpperInvariant();
			// �ϊ���̓d�b�ԍ����i�[����A�r���_�[
			var newNumber = new StringBuilder();

			foreach( var c in raw ) {
				// �󔒃X�y�[�X�A�n�C�t���A���l�͂��̂܂ܒǉ����܂��B
				if( " -0123456789".Contains( c ) ) {
					newNumber.Append( c );
				}
				// �A���t�@�x�b�g�̓e���L�[�̔z�u�œ����Ȑ����ɕϊ����܂��B
				else {
					var result = TranslateToNumber( c );
					if( result != null ) {
						newNumber.Append( result );
					}
				}
				// �����ȊO�̕����̓X�L�b�v���܂��B
			}
			return newNumber.ToString();
		}

		/// <summary>
		///		�w�肵���������A�w�肵��������̒��Ɋ܂܂�Ă��邩�ǂ������ʂ��܂��B
		/// </summary>
		/// <param name="keyString">�T�����̕�����</param>
		/// <param name="c">�T�����镶��</param>
		/// <returns>keyString����c���܂܂�鎞 : true / �����łȂ��� : false</returns>
		/// <remarks>string�N���X�ŕW����`����Ă���Contains���\�b�h�́A�p�����[�^�[�̌^��string�^�ł��B</remarks>
		static bool Contains( this string keyString, char c ) =>
			keyString.IndexOf( c ) >= 0;

		/// <summary>
		///		�A���t�@�x�b�g���e���L�[�̔z�u�œ����Ȑ����ɕϊ����܂��B
		/// </summary>
		/// <param name="c">�A���t�@�x�b�g</param>
		/// <returns>�e���L�[�̔z�u�œ����Ȑ���</returns>
		static int? TranslateToNumber( char c ) {
			if( "ABC".Contains( c ) ) {
				return 2;
			}
			else if( "DEF".Contains( c ) ) {
				return 3;
			}
			else if( "GHI".Contains( c ) ) {
				return 4;
			}
			else if( "JKL".Contains( c ) ) {
				return 5;
			}
			else if( "MNO".Contains( c ) ) {
				return 6;
			}
			else if( "PQRS".Contains( c ) ) {
				return 7;
			}
			else if( "TUV".Contains( c ) ) {
				return 8;
			}
			else if( "WXYZ".Contains( c ) ) {
				return 9;
			}
			return null;
		}
	}
}