using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC3 RID: 3267
	public class ModContentHolder<T> where T : class
	{
		// Token: 0x040030D6 RID: 12502
		private ModContentPack mod;

		// Token: 0x040030D7 RID: 12503
		public Dictionary<string, T> contentList = new Dictionary<string, T>();

		// Token: 0x06004814 RID: 18452 RVA: 0x0025F372 File Offset: 0x0025D772
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x0025F390 File Offset: 0x0025D790
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

		// Token: 0x06004816 RID: 18454 RVA: 0x0025F438 File Offset: 0x0025D838
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

		// Token: 0x06004817 RID: 18455 RVA: 0x0025F500 File Offset: 0x0025D900
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

		// Token: 0x06004818 RID: 18456 RVA: 0x0025F538 File Offset: 0x0025D938
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
	}
}
