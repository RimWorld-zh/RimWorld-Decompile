using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using RuntimeAudioClipLoader;
using UnityEngine;

namespace Verse
{
	public static class ModContentLoader<T> where T : class
	{
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

		private static string[] AcceptableExtensionsTexture = new string[]
		{
			".png",
			".jpg"
		};

		private static string[] AcceptableExtensionsString = new string[]
		{
			".txt"
		};

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
			if (typeof(T) == typeof(Texture2D))
			{
				return (LoadedContentItem<T>)new LoadedContentItem<Texture2D>(absFilePath, BaseContent.BadTex);
			}
			return null;
		}

		private static bool ShouldStreamAudioClipFromPath(string absPath)
		{
			if (!File.Exists(absPath))
			{
				return false;
			}
			FileInfo fileInfo = new FileInfo(absPath);
			return fileInfo.Length > 307200L;
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

		// Note: this type is marked as 'beforefieldinit'.
		static ModContentLoader()
		{
		}

		[CompilerGenerated]
		private sealed class <LoadAllForMod>c__Iterator0 : IEnumerable, IEnumerable<LoadedContentItem<T>>, IEnumerator, IDisposable, IEnumerator<LoadedContentItem<T>>
		{
			internal ModContentPack mod;

			internal string <contentDirPath>__0;

			internal DirectoryInfo <contentDir>__0;

			internal FileInfo[] $locvar0;

			internal int $locvar1;

			internal FileInfo <file>__1;

			internal LoadedContentItem<T> <loadedItem>__2;

			internal LoadedContentItem<T> $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <LoadAllForMod>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					contentDirPath = Path.Combine(mod.RootDir, GenFilePaths.ContentPath<T>());
					contentDir = new DirectoryInfo(contentDirPath);
					if (!contentDir.Exists)
					{
						return false;
					}
					DeepProfiler.Start(string.Concat(new object[]
					{
						"Loading assets of type ",
						typeof(T),
						" for mod ",
						mod
					}));
					files = contentDir.GetFiles("*.*", SearchOption.AllDirectories);
					i = 0;
					break;
				case 1u:
					IL_12F:
					i++;
					break;
				default:
					return false;
				}
				if (i >= files.Length)
				{
					DeepProfiler.End();
					this.$PC = -1;
				}
				else
				{
					file = files[i];
					if (!ModContentLoader<T>.IsAcceptableExtension(file.Extension))
					{
						goto IL_12F;
					}
					loadedItem = ModContentLoader<T>.LoadItem(file.FullName, contentDirPath);
					if (loadedItem != null)
					{
						this.$current = loadedItem;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_12F;
				}
				return false;
			}

			LoadedContentItem<T> IEnumerator<LoadedContentItem<T>>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.LoadedContentItem<T>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<LoadedContentItem<T>> IEnumerable<LoadedContentItem<T>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ModContentLoader<T>.<LoadAllForMod>c__Iterator0 <LoadAllForMod>c__Iterator = new ModContentLoader<T>.<LoadAllForMod>c__Iterator0();
				<LoadAllForMod>c__Iterator.mod = mod;
				return <LoadAllForMod>c__Iterator;
			}
		}
	}
}
