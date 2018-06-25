using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public class ModContentHolder<T> where T : class
	{
		private ModContentPack mod;

		public Dictionary<string, T> contentList = new Dictionary<string, T>();

		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		public void ClearDestroy()
		{
			if (typeof(UnityEngine.Object).IsAssignableFrom(typeof(T)))
			{
				foreach (T localObj2 in this.contentList.Values)
				{
					T localObj = localObj2;
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						UnityEngine.Object.Destroy((UnityEngine.Object)((object)localObj));
					});
				}
			}
			this.contentList.Clear();
		}

		public void ReloadAll()
		{
			foreach (LoadedContentItem<T> loadedContentItem in ModContentLoader<T>.LoadAllForMod(this.mod))
			{
				if (this.contentList.ContainsKey(loadedContentItem.internalPath))
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to load duplicate ",
						typeof(T),
						" with path: ",
						loadedContentItem.internalPath
					}), false);
				}
				else
				{
					this.contentList.Add(loadedContentItem.internalPath, loadedContentItem.contentItem);
				}
			}
		}

		public T Get(string path)
		{
			T t;
			T result;
			if (this.contentList.TryGetValue(path, out t))
			{
				result = t;
			}
			else
			{
				result = (T)((object)null);
			}
			return result;
		}

		public IEnumerable<T> GetAllUnderPath(string pathRoot)
		{
			foreach (KeyValuePair<string, T> kvp in this.contentList)
			{
				if (kvp.Key.StartsWith(pathRoot))
				{
					yield return kvp.Value;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <ClearDestroy>c__AnonStorey1
		{
			internal T localObj;

			public <ClearDestroy>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				UnityEngine.Object.Destroy((UnityEngine.Object)((object)this.localObj));
			}
		}

		[CompilerGenerated]
		private sealed class <GetAllUnderPath>c__Iterator0 : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T>
		{
			internal Dictionary<string, T>.Enumerator $locvar0;

			internal KeyValuePair<string, T> <kvp>__1;

			internal string pathRoot;

			internal ModContentHolder<T> $this;

			internal T $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetAllUnderPath>c__Iterator0()
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
					enumerator = this.contentList.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					IL_A5:
					if (enumerator.MoveNext())
					{
						kvp = enumerator.Current;
						if (kvp.Key.StartsWith(pathRoot))
						{
							this.$current = kvp.Value;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_A5;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
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
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
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
				ModContentHolder<T>.<GetAllUnderPath>c__Iterator0 <GetAllUnderPath>c__Iterator = new ModContentHolder<T>.<GetAllUnderPath>c__Iterator0();
				<GetAllUnderPath>c__Iterator.$this = this;
				<GetAllUnderPath>c__Iterator.pathRoot = pathRoot;
				return <GetAllUnderPath>c__Iterator;
			}
		}
	}
}
