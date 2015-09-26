using System;
using System.IO;
using System.Security.Cryptography;

public static class ExtensionsMethods
{
	/// <summary>
	/// Получить SHA1 хеш потока файла.
	/// </summary>
	/// <param name="stream">Поток файла.</param>
	/// <returns>Хеш.</returns>
	public static string GetSHA1Hash(this FileStream stream)
	{
		var data = new byte[stream.Length];
		stream.Read(data, 0, (int)stream.Length);

		SHA1Managed SHhash = new SHA1Managed();
		var hash = SHhash.ComputeHash(data);

		var res = BitConverter.ToString(hash);
		return res.Replace("-", String.Empty);
	}
}