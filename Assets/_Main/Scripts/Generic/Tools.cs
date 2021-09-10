using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace DreamHouseSpectra.Util
{
	public class Tools
	{
		public static List<string> StringToList(string stringToConvert)
		{
			List<string> stringArray = new List<string>();
			string[] splited = stringToConvert.Split(',');
			string current = string.Empty;

			foreach (string s in splited)
			{
				current = s.Replace("{", string.Empty).Replace("}", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace('"', ' ').Trim();
				if (!string.IsNullOrEmpty(current.Trim()))
					stringArray.Add(current);
			}
			return stringArray;
		}

		public static Texture2D DecodeImage(string base64Image, int width, int height)
		{
			byte[] bytes = Convert.FromBase64String(base64Image);
			Texture2D tx = new Texture2D(width, height);
			tx.LoadImage(bytes);

			return tx;
		}

		public static string CreateSHA256(string textBase)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(textBase));

				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
					builder.Append(bytes[i].ToString("x2"));

				return builder.ToString();
			}
		}

		public static void ClearParent(Transform parent)
		{
			foreach (Transform son in parent)
				UnityEngine.Object.Destroy(son.gameObject);
		}

		public static void DisablePoolObjects(Transform parent)
		{
			foreach (Transform son in parent)
				son.gameObject.SetActive(false);
		}

		public static string EncodeImage(Texture2D img)
		{
			return Convert.ToBase64String(img.EncodeToPNG());
		}

		public static Sprite CreateSprite(Texture2D tex)
		{
			return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f); ;
		}

		public static string NameOf(Action method)
		{
			return method.Method.Name;
		}

		public static bool FileExistInPath(string folder, string fileName)
		{
			string path = Path.Combine(folder, fileName);
			return File.Exists(path);
		}

		public static void OverwriteFileJSON(string filePath, object obj)
		{
			File.WriteAllText(filePath, JsonUtility.ToJson(obj));
		}

		public static string ToMoneyFormat(float value)
		{
			return "$ " + value.ToString("N0");
		}
	}
}