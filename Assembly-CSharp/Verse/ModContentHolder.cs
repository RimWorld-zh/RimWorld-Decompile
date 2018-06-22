using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC0 RID: 3264
	public class ModContentHolder<T> where T : class
	{
		// Token: 0x06004811 RID: 18449 RVA: 0x0025EFB6 File Offset: 0x0025D3B6
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x0025EFD4 File Offset: 0x0025D3D4
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

		// Token: 0x06004813 RID: 18451 RVA: 0x0025F07C File Offset: 0x0025D47C
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

		// Token: 0x06004814 RID: 18452 RVA: 0x0025F144 File Offset: 0x0025D544
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

		// Token: 0x06004815 RID: 18453 RVA: 0x0025F17C File Offset: 0x0025D57C
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

		// Token: 0x040030CF RID: 12495
		private ModContentPack mod;

		// Token: 0x040030D0 RID: 12496
		public Dictionary<string, T> contentList = new Dictionary<string, T>();
	}
}
