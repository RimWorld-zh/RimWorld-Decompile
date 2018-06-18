using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC3 RID: 3267
	public class ModContentHolder<T> where T : class
	{
		// Token: 0x06004800 RID: 18432 RVA: 0x0025DB9E File Offset: 0x0025BF9E
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x0025DBBC File Offset: 0x0025BFBC
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

		// Token: 0x06004802 RID: 18434 RVA: 0x0025DC64 File Offset: 0x0025C064
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

		// Token: 0x06004803 RID: 18435 RVA: 0x0025DD2C File Offset: 0x0025C12C
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

		// Token: 0x06004804 RID: 18436 RVA: 0x0025DD64 File Offset: 0x0025C164
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

		// Token: 0x040030C4 RID: 12484
		private ModContentPack mod;

		// Token: 0x040030C5 RID: 12485
		public Dictionary<string, T> contentList = new Dictionary<string, T>();
	}
}
