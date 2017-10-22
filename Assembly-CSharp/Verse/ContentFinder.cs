using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class ContentFinder<T> where T : class
	{
		public static T Get(string itemPath, bool reportFailure = true)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a resource \"" + itemPath + "\" from a different thread. All resources must be loaded in the main thread.");
				return (T)null;
			}
			T val = (T)null;
			foreach (ModContentPack runningMod in LoadedModManager.RunningMods)
			{
				val = runningMod.GetContentHolder<T>().Get(itemPath);
				if (val != null)
				{
					return val;
				}
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
				return val;
			}
			if (reportFailure)
			{
				Log.Error("Could not load " + typeof(T) + " at " + itemPath + " in any active mod or in base resources.");
			}
			return (T)null;
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
					foreach (T item2 in runningMod.GetContentHolder<T>().GetAllUnderPath(folderPath))
					{
						yield return item2;
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
					for (int i = 0; i < array.Length; i++)
					{
						T item = array[i];
						yield return item;
					}
				}
			}
		}
	}
}
