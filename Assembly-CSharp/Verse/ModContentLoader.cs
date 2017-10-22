using RuntimeAudioClipLoader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	public static class ModContentLoader<T> where T : class
	{
		private static string[] AcceptableExtensionsAudio = new string[7]
		{
			".wav",
			".mp3",
			".ogg",
			".xm",
			".it",
			".mod",
			".s3m"
		};

		private static string[] AcceptableExtensionsTexture = new string[2]
		{
			".png",
			".jpg"
		};

		private static string[] AcceptableExtensionsString = new string[1]
		{
			".txt"
		};

		private static bool IsAcceptableExtension(string extension)
		{
			string[] array;
			if (typeof(T) == typeof(AudioClip))
			{
				array = ModContentLoader<T>.AcceptableExtensionsAudio;
				goto IL_008e;
			}
			if (typeof(T) == typeof(Texture2D))
			{
				array = ModContentLoader<T>.AcceptableExtensionsTexture;
				goto IL_008e;
			}
			if (typeof(T) == typeof(string))
			{
				array = ModContentLoader<T>.AcceptableExtensionsString;
				goto IL_008e;
			}
			Log.Error("Unknown content type " + typeof(T));
			bool result = false;
			goto IL_00cf;
			IL_00cf:
			return result;
			IL_008e:
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string b = array2[i];
				if (extension.ToLower() == b)
					goto IL_00b0;
			}
			result = false;
			goto IL_00cf;
			IL_00b0:
			result = true;
			goto IL_00cf;
		}

		public static IEnumerable<LoadedContentItem<T>> LoadAllForMod(ModContentPack mod)
		{
			string contentDirPath = Path.Combine(mod.RootDir, GenFilePaths.ContentPath<T>());
			DirectoryInfo contentDir = new DirectoryInfo(contentDirPath);
			if (contentDir.Exists)
			{
				DeepProfiler.Start("Loading assets of type " + typeof(T) + " for mod " + mod);
				FileInfo[] files = contentDir.GetFiles("*.*", SearchOption.AllDirectories);
				for (int i = 0; i < files.Length; i++)
				{
					FileInfo file = files[i];
					if (ModContentLoader<T>.IsAcceptableExtension(file.Extension))
					{
						LoadedContentItem<T> loadedItem = ModContentLoader<T>.LoadItem(file.FullName, contentDirPath);
						if (loadedItem != null)
						{
							yield return loadedItem;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				DeepProfiler.End();
			}
		}

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
					return new LoadedContentItem<T>(text, (T)(object)GenFile.TextFromRawFile(absFilePath));
				}
				if (typeof(T) == typeof(Texture2D))
				{
					return new LoadedContentItem<T>(text, (T)(object)ModContentLoader<T>.LoadPNG(absFilePath));
				}
				if (typeof(T) == typeof(AudioClip))
				{
					if (Prefs.LogVerbose)
					{
						DeepProfiler.Start("Loading file " + text);
					}
					T val = default(T);
					try
					{
						bool doStream = ModContentLoader<T>.ShouldStreamAudioClipFromPath(absFilePath);
						val = (T)(object)Manager.Load(absFilePath, doStream, true, true);
					}
					finally
					{
						if (Prefs.LogVerbose)
						{
							DeepProfiler.End();
						}
					}
					UnityEngine.Object @object = ((object)val) as UnityEngine.Object;
					if (@object != (UnityEngine.Object)null)
					{
						@object.name = Path.GetFileNameWithoutExtension(new FileInfo(absFilePath).Name);
					}
					return new LoadedContentItem<T>(text, val);
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading " + typeof(T) + " from file.\nabsFilePath: " + absFilePath + "\ncontentDirPath: " + contentDirPath + "\nException: " + ex.ToString());
			}
			return (typeof(T) != typeof(Texture2D)) ? null : ((LoadedContentItem<T>)new LoadedContentItem<Texture2D>(absFilePath, BaseContent.BadTex));
		}

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
				result = (fileInfo.Length > 307200);
			}
			return result;
		}

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
