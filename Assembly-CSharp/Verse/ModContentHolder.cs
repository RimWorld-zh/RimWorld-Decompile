using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC2 RID: 3266
	public class ModContentHolder<T> where T : class
	{
		// Token: 0x040030CF RID: 12495
		private ModContentPack mod;

		// Token: 0x040030D0 RID: 12496
		public Dictionary<string, T> contentList = new Dictionary<string, T>();

		// Token: 0x06004814 RID: 18452 RVA: 0x0025F092 File Offset: 0x0025D492
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x0025F0B0 File Offset: 0x0025D4B0
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

		// Token: 0x06004816 RID: 18454 RVA: 0x0025F158 File Offset: 0x0025D558
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

		// Token: 0x06004817 RID: 18455 RVA: 0x0025F220 File Offset: 0x0025D620
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

		// Token: 0x06004818 RID: 18456 RVA: 0x0025F258 File Offset: 0x0025D658
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
