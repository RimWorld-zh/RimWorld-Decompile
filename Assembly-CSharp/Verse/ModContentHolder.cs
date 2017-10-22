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
				foreach (T value in this.contentList.Values)
				{
					T localObj = value;
					LongEventHandler.ExecuteWhenFinished((Action)delegate
					{
						UnityEngine.Object.Destroy((UnityEngine.Object)(object)localObj);
					});
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
			T val = default(T);
			return (!this.contentList.TryGetValue(path, out val)) ? ((T)null) : val;
		}

		public IEnumerable<T> GetAllUnderPath(string pathRoot)
		{
			using (Dictionary<string, T>.Enumerator enumerator = this.contentList.GetEnumerator())
			{
				KeyValuePair<string, T> kvp;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						kvp = enumerator.Current;
						if (kvp.Key.StartsWith(pathRoot))
							break;
						continue;
					}
					yield break;
				}
				yield return kvp.Value;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_00da:
			/*Error near IL_00db: Unexpected return in MoveNext()*/;
		}
	}
}
