using System;
using System.Collections.Generic;
using System.IO;
using RuntimeAudioClipLoader;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC2 RID: 3266
	public static class ModContentLoader<T> where T : class
	{
		// Token: 0x040030D3 RID: 12499
		private static string[] AcceptableExtensionsAudio = new string[]
		{
			".wav",
			".mp3",
			".ogg",
			".xm",
			".it",
			".mod",
			".s3m"
		};

		// Token: 0x040030D4 RID: 12500
		private static string[] AcceptableExtensionsTexture = new string[]
		{
			".png",
			".jpg"
		};

		// Token: 0x040030D5 RID: 12501
		private static string[] AcceptableExtensionsString = new string[]
		{
			".txt"
		};

		// Token: 0x06004817 RID: 18455 RVA: 0x0025F3E0 File Offset: 0x0025D7E0
		private static bool IsAcceptableExtension(string extension)
		{
			string[] array;
			if (typeof(T) == typeof(AudioClip))
			{
				array = ModContentLoader<T>.AcceptableExtensionsAudio;
			}
			else if (typeof(T) == typeof(Texture2D))
			{
				array = ModContentLoader<T>.AcceptableExtensionsTexture;
			}
			else
			{
				if (typeof(T) != typeof(string))
				{
					Log.Error("Unknown content type " + typeof(T), false);
					return false;
				}
				array = ModContentLoader<T>.AcceptableExtensionsString;
			}
			foreach (string b in array)
			{
				if (extension.ToLower() == b)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x0025F4C0 File Offset: 0x0025D8C0
		public static IEnumerable<LoadedContentItem<T>> LoadAllForMod(ModContentPack mod)
		{
			string contentDirPath = Path.Combine(mod.RootDir, GenFilePaths.ContentPath<T>());
			DirectoryInfo contentDir = new DirectoryInfo(contentDirPath);
			if (!contentDir.Exists)
			{
				yield break;
			}
			DeepProfiler.Start(string.Concat(new object[]
			{
				"Loading assets of type ",
				typeof(T),
				" for mod ",
				mod
			}));
			foreach (FileInfo file in contentDir.GetFiles("*.*", SearchOption.AllDirectories))
			{
				if (ModContentLoader<T>.IsAcceptableExtension(file.Extension))
				{
					LoadedContentItem<T> loadedItem = ModContentLoader<T>.LoadItem(file.FullName, contentDirPath);
					if (loadedItem != null)
					{
						yield return loadedItem;
					}
				}
			}
			DeepProfiler.End();
			yield break;
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x0025F4EC File Offset: 0x0025D8EC
		public static LoadedContentItem<T> LoadItem(string absFilePath, string contentDirPath = null)
		{
			string text = absFilePath;
			if (contentDirPath != null)
			{
				text = text.Substring(contentDirPath.ToString().Length);
			}
			text = text.Substring(0, text.Length - Path.GetExtension(text).Length);
			text = text.Replace('\\', '/');
			try
			{
				if (typeof(T) == typeof(string))
				{
					return new LoadedContentItem<T>(text, (T)((object)GenFile.TextFromRawFile(absFilePath)));
				}
				if (typeof(T) == typeof(Texture2D))
				{
					return new LoadedContentItem<T>(text, (T)((object)ModContentLoader<T>.LoadPNG(absFilePath)));
				}
				if (typeof(T) == typeof(AudioClip))
				{
					if (Prefs.LogVerbose)
					{
						DeepProfiler.Start("Loading file " + text);
					}
					T t;
					try
					{
						bool doStream = ModContentLoader<T>.ShouldStreamAudioClipFromPath(absFilePath);
						t = (T)((object)Manager.Load(absFilePath, doStream, true, true));
					}
					finally
					{
						if (Prefs.LogVerbose)
						{
							DeepProfiler.End();
						}
					}
					UnityEngine.Object @object = t as UnityEngine.Object;
					if (@object != null)
					{
						@object.name = Path.GetFileNameWithoutExtension(new FileInfo(absFilePath).Name);
					}
					return new LoadedContentItem<T>(text, t);
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception loading ",
					typeof(T),
					" from file.\nabsFilePath: ",
					absFilePath,
					"\ncontentDirPath: ",
					contentDirPath,
					"\nException: ",
					ex.ToString()
				}), false);
			}
			LoadedContentItem<T> result;
			if (typeof(T) == typeof(Texture2D))
			{
				result = (LoadedContentItem<T>)new LoadedContentItem<Texture2D>(absFilePath, BaseContent.BadTex);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x0025F70C File Offset: 0x0025DB0C
		private static bool ShouldStreamAudioClipFromPath(string absPath)
		{
			bool result;
			if (!File.Exists(absPath))
			{
				result = false;
			}
			else
			{
				FileInfo fileInfo = new FileInfo(absPath);
				result = (fileInfo.Length > 307200L);
			}
			return result;
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x0025F748 File Offset: 0x0025DB48
		private static Texture2D LoadPNG(string filePath)
		{
			Texture2D texture2D = null;
			if (File.Exists(filePath))
			{
				byte[] data = File.ReadAllBytes(filePath);
				texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, true);
				texture2D.LoadImage(data);
				texture2D.Compress(true);
				texture2D.name = Path.GetFileNameWithoutExtension(filePath);
				texture2D.filterMode = FilterMode.Bilinear;
				texture2D.anisoLevel = 2;
				texture2D.Apply(true, true);
			}
			return texture2D;
		}
	}
}
