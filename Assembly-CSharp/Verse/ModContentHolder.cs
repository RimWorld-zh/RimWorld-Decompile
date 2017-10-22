using System;
using System.Collections.Generic;
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
				Dictionary<string, T>.ValueCollection.Enumerator enumerator = this.contentList.Values.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						T current = enumerator.Current;
						T localObj = current;
						LongEventHandler.ExecuteWhenFinished((Action)delegate
						{
							UnityEngine.Object.Destroy((UnityEngine.Object)(object)localObj);
						});
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			}
			this.contentList.Clear();
		}

		public void ReloadAll()
		{
			foreach (LoadedContentItem<T> item in ModContentLoader<T>.LoadAllForMod(this.mod))
			{
				if (this.contentList.ContainsKey(item.internalPath))
				{
					Log.Warning("Tried to load duplicate " + typeof(T) + " with path: " + item.internalPath);
				}
				else
				{
					this.contentList.Add(item.internalPath, item.contentItem);
				}
			}
		}

		public T Get(string path)
		{
			T result = default(T);
			if (this.contentList.TryGetValue(path, out result))
			{
				return result;
			}
			return (T)null;
		}

		public IEnumerable<T> GetAllUnderPath(string pathRoot)
		{
			Dictionary<string, T>.Enumerator enumerator = this.contentList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, T> kvp = enumerator.Current;
					if (kvp.Key.StartsWith(pathRoot))
					{
						yield return kvp.Value;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
