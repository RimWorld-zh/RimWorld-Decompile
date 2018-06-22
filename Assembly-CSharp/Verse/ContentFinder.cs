using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CBB RID: 3259
	public static class ContentFinder<T> where T : class
	{
		// Token: 0x060047EE RID: 18414 RVA: 0x0025DB38 File Offset: 0x0025BF38
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

		// Token: 0x060047EF RID: 18415 RVA: 0x0025DC80 File Offset: 0x0025C080
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
	}
}
