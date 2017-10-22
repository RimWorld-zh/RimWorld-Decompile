using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class ContentFinder<T> where T : class
	{
		public static T Get(string itemPath, bool reportFailure = true)
		{
			T result;
			T val;
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a resource \"" + itemPath + "\" from a different thread. All resources must be loaded in the main thread.");
				result = (T)null;
			}
			else
			{
				val = (T)null;
				List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
				for (int num = runningModsListForReading.Count - 1; num >= 0; num--)
				{
					val = runningModsListForReading[num].GetContentHolder<T>().Get(itemPath);
					if (val != null)
						goto IL_0067;
				}
				if (typeof(T) == typeof(Texture2D))
				{
					val = (T)(object)Resources.Load<Texture2D>(GenFilePaths.ContentPath<Texture2D>() + itemPath);
				}
				if (typeof(T) == typeof(AudioClip))
				{
					val = (T)(object)Resources.Load<AudioClip>(GenFilePaths.ContentPath<AudioClip>() + itemPath);
				}
				if (val != null)
				{
					result = val;
				}
				else
				{
					if (reportFailure)
					{
						Log.Error("Could not load " + typeof(T) + " at " + itemPath + " in any active mod or in base resources.");
					}
					result = (T)null;
				}
			}
			goto IL_0136;
			IL_0067:
			result = val;
			goto IL_0136;
			IL_0136:
			return result;
		}

		public static IEnumerable<T> GetAllInFolder(string folderPath)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get all resources in a folder \"" + folderPath + "\" from a different thread. All resources must be loaded in the main thread.");
			}
			else
			{
				foreach (ModContentPack runningMod in LoadedModManager.RunningMods)
				{
					using (IEnumerator<T> enumerator2 = runningMod.GetContentHolder<T>().GetAllUnderPath(folderPath).GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							T item2 = enumerator2.Current;
							yield return item2;
							/*Error: Unable to find new state assignment for yield return*/;
						}
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
					T[] array = items;
					int num = 0;
					if (num < array.Length)
					{
						T item = array[num];
						yield return item;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_0257:
			/*Error near IL_0258: Unexpected return in MoveNext()*/;
		}
	}
}
