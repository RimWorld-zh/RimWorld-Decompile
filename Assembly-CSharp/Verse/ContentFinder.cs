using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public static class ContentFinder<T> where T : class
	{
		public static T Get(string itemPath, bool reportFailure = true)
		{
			T result;
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a resource \"" + itemPath + "\" from a different thread. All resources must be loaded in the main thread.", false);
				result = (T)((object)null);
			}
			else
			{
				T t = (T)((object)null);
				List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
				for (int i = runningModsListForReading.Count - 1; i >= 0; i--)
				{
					t = runningModsListForReading[i].GetContentHolder<T>().Get(itemPath);
					if (t != null)
					{
						return t;
					}
				}
				if (typeof(T) == typeof(Texture2D))
				{
					t = (T)((object)Resources.Load<Texture2D>(GenFilePaths.ContentPath<Texture2D>() + itemPath));
				}
				if (typeof(T) == typeof(AudioClip))
				{
					t = (T)((object)Resources.Load<AudioClip>(GenFilePaths.ContentPath<AudioClip>() + itemPath));
				}
				if (t != null)
				{
					result = t;
				}
				else
				{
					if (reportFailure)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not load ",
							typeof(T),
							" at ",
							itemPath,
							" in any active mod or in base resources."
						}), false);
					}
					result = (T)((object)null);
				}
			}
			return result;
		}

		public static IEnumerable<T> GetAllInFolder(string folderPath)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get all resources in a folder \"" + folderPath + "\" from a different thread. All resources must be loaded in the main thread.", false);
				yield break;
			}
			foreach (ModContentPack mod in LoadedModManager.RunningMods)
			{
				foreach (T item in mod.GetContentHolder<T>().GetAllUnderPath(folderPath))
				{
					yield return item;
				}
			}
			T[] items = null;
			if (typeof(T) == typeof(Texture2D))
			{
				items = (T[])Resources.LoadAll<Texture2D>(GenFilePaths.ContentPath<Texture2D>() + folderPath);
			}
			if (typeof(T) == typeof(AudioClip))
			{
				items = (T[])Resources.LoadAll<AudioClip>(GenFilePaths.ContentPath<AudioClip>() + folderPath);
			}
			if (items != null)
			{
				foreach (T item2 in items)
				{
					yield return item2;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GetAllInFolder>c__Iterator0 : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T>
		{
			internal string folderPath;

			internal IEnumerator<ModContentPack> $locvar0;

			internal ModContentPack <mod>__1;

			internal IEnumerator<T> $locvar1;

			internal T <item>__2;

			internal T[] <items>__0;

			internal T[] $locvar2;

			internal int $locvar3;

			internal T <item>__3;

			internal T $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetAllInFolder>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (!UnityData.IsInMainThread)
					{
						Log.Error("Tried to get all resources in a folder \"" + folderPath + "\" from a different thread. All resources must be loaded in the main thread.", false);
						return false;
					}
					enumerator = LoadedModManager.RunningMods.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					i++;
					goto IL_23B;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						Block_9:
						try
						{
							switch (num)
							{
							}
							if (enumerator2.MoveNext())
							{
								item = enumerator2.Current;
								this.$current = item;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								flag = true;
								return true;
							}
						}
						finally
						{
							if (!flag)
							{
								if (enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						mod = enumerator.Current;
						enumerator2 = mod.GetContentHolder<T>().GetAllUnderPath(folderPath).GetEnumerator();
						num = 4294967293u;
						goto Block_9;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				items = null;
				if (typeof(T) == typeof(Texture2D))
				{
					items = (T[])Resources.LoadAll<Texture2D>(GenFilePaths.ContentPath<Texture2D>() + folderPath);
				}
				if (typeof(T) == typeof(AudioClip))
				{
					items = (T[])Resources.LoadAll<AudioClip>(GenFilePaths.ContentPath<AudioClip>() + folderPath);
				}
				if (items == null)
				{
					goto IL_24F;
				}
				array = items;
				i = 0;
				IL_23B:
				if (i < array.Length)
				{
					item2 = array[i];
					this.$current = item2;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_24F:
				this.$PC = -1;
				return false;
			}

			T IEnumerator<T>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
						try
						{
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ContentFinder<T>.<GetAllInFolder>c__Iterator0 <GetAllInFolder>c__Iterator = new ContentFinder<T>.<GetAllInFolder>c__Iterator0();
				<GetAllInFolder>c__Iterator.folderPath = folderPath;
				return <GetAllInFolder>c__Iterator;
			}
		}
	}
}
