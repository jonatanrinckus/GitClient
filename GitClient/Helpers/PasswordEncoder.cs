using System;
using System.IO;
using System.Security.Cryptography;

namespace GitClient.Helpers
{
	public class PasswordEncoder
	{
		// Change the two values below to be something other than the example.
		// Once changed and in use, do not change the value below again or you
		// won't be able to decrypt previously stored passwords.
		private readonly byte[] _mInitializationVector = { 0x01, 0x24, 0x34, 0x58, 0x80, 0xBC, 0xd3, 0xFF };

		public PasswordEncoder()
		{
		}

		public PasswordEncoder(string inPassword)
		{
			EncryptedPassword = EncryptWithByteArray(inPassword, ByteArray);
		}

		public string EncryptWithByteArray(string inPassword)
		{
			EncryptedPassword = EncryptWithByteArray(inPassword, ByteArray);
			return EncryptedPassword;
		}

		private string EncryptWithByteArray(string inPassword, string inByteArray)
		{
			try
			{
				var tmpKey = new byte[20];
				tmpKey = System.Text.Encoding.UTF8.GetBytes(inByteArray.Substring(0, 8));
				var des = new DESCryptoServiceProvider();
				var inputArray = System.Text.Encoding.UTF8.GetBytes(inPassword);
				var ms = new MemoryStream();
				var cs = new CryptoStream(ms, des.CreateEncryptor(tmpKey, _mInitializationVector), CryptoStreamMode.Write);
				cs.Write(inputArray, 0, inputArray.Length);
				cs.FlushFinalBlock();
				return Convert.ToBase64String(ms.ToArray());
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string DecryptWithByteArray()
		{
			return DecryptWithByteArray(EncryptedPassword, ByteArray);
		}

		private string DecryptWithByteArray(string strText, string strEncrypt)
		{
			try
			{
				var tmpKey = new byte[20];
				tmpKey = System.Text.Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));
				var des = new DESCryptoServiceProvider();
				byte[] inputByteArray = inputByteArray = Convert.FromBase64String(strText);
				var ms = new MemoryStream();
				var cs = new CryptoStream(ms, des.CreateDecryptor(tmpKey, _mInitializationVector), CryptoStreamMode.Write);
				cs.Write(inputByteArray, 0, inputByteArray.Length);
				cs.FlushFinalBlock();
				var encoding = System.Text.Encoding.UTF8;
				return encoding.GetString(ms.ToArray());
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string EncryptedPassword { get; set; }

		public string ByteArray { get; set; } = "3as#2s+2!@$$#)U0l%Ra>#%";
	}
}
