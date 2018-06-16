using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC4 RID: 3268
	public class ModContentHolder<T> where T : class
	{
		// Token: 0x06004802 RID: 18434 RVA: 0x0025DBC6 File Offset: 0x0025BFC6
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x0025DBE4 File Offset: 0x0025BFE4
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

		// Token: 0x06004804 RID: 18436 RVA: 0x0025DC8C File Offset: 0x0025C08C
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

		// Token: 0x06004805 RID: 18437 RVA: 0x0025DD54 File Offset: 0x0025C154
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

		// Token: 0x06004806 RID: 18438 RVA: 0x0025DD8C File Offset: 0x0025C18C
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

		// Token: 0x040030C6 RID: 12486
		private ModContentPack mod;

		// Token: 0x040030C7 RID: 12487
		public Dictionary<string, T> contentList = new Dictionary<string, T>();
	}
}
