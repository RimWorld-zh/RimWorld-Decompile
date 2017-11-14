using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public static class DefDatabase<T> where T : Def, new()
	{
		private static List<T> defsList = new List<T>();

		private static Dictionary<string, T> defsByName = new Dictionary<string, T>();

		public static IEnumerable<T> AllDefs
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		public static List<T> AllDefsListForReading
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		public static int DefCount
		{
			get
			{
				return DefDatabase<T>.defsList.Count;
			}
		}

		public static void AddAllInMods()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ModContentPack item in Enumerable.ThenBy<ModContentPack, int>(Enumerable.OrderBy<ModContentPack, int>(LoadedModManager.RunningMods, (Func<ModContentPack, int>)((ModContentPack m) => m.OverwritePriority)), (Func<ModContentPack, int>)((ModContentPack x) => LoadedModManager.RunningModsListForReading.IndexOf(x))))
			{
				hashSet.Clear();
				foreach (T item2 in GenDefDatabase.DefsToGoInDatabase<T>(item))
				{
					if (hashSet.Contains(((Def)(object)item2).defName))
					{
						Log.Error("Mod " + item + " has multiple " + typeof(T) + "s named " + ((Def)(object)item2).defName + ". Skipping.");
					}
					else
					{
						if (((Def)(object)item2).defName == "UnnamedDef")
						{
							string text = "Unnamed" + typeof(T).Name + Rand.Range(1, 100000).ToString() + "A";
							Log.Error(typeof(T).Name + " in " + item.ToString() + " with label " + ((Def)(object)item2).label + " lacks a defName. Giving name " + text);
							((Def)(object)item2).defName = text;
						}
						T def = default(T);
						if (DefDatabase<T>.defsByName.TryGetValue(((Def)(object)item2).defName, out def))
						{
							DefDatabase<T>.Remove(def);
						}
						DefDatabase<T>.Add(item2);
					}
				}
			}
		}

		public static void Add(IEnumerable<T> defs)
		{
			foreach (T def in defs)
			{
				DefDatabase<T>.Add(def);
			}
		}

		public static void Add(T def)
		{
			while (DefDatabase<T>.defsByName.ContainsKey(((Def)(object)def).defName))
			{
				Log.Error("Adding duplicate " + typeof(T) + " name: " + ((Def)(object)def).defName);
				object obj = def;
				((Def)obj).defName = ((Def)obj).defName + Mathf.RoundToInt((float)(Rand.Value * 1000.0));
			}
			DefDatabase<T>.defsList.Add(def);
			DefDatabase<T>.defsByName.Add(((Def)(object)def).defName, def);
			if (DefDatabase<T>.defsList.Count > 65535)
			{
				Log.Error("Too many " + typeof(T) + "; over " + (ushort)65535);
			}
			((Def)(object)def).index = (ushort)(DefDatabase<T>.defsList.Count - 1);
		}

		private static void Remove(T def)
		{
			DefDatabase<T>.defsByName.Remove(((Def)(object)def).defName);
			DefDatabase<T>.defsList.Remove(def);
			DefDatabase<T>.SetIndices();
		}

		public static void Clear()
		{
			DefDatabase<T>.defsList.Clear();
			DefDatabase<T>.defsByName.Clear();
		}

		public static void ClearCachedData()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				T val = DefDatabase<T>.defsList[i];
				val.ClearCachedData();
			}
		}

		public static void ResolveAllReferences(bool onlyExactlyMyType = true)
		{
			DefDatabase<T>.SetIndices();
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				try
				{
					if (onlyExactlyMyType)
					{
						T val = DefDatabase<T>.defsList[i];
						if (val.GetType() == typeof(T))
							goto IL_003f;
						goto end_IL_000c;
					}
					goto IL_003f;
					IL_003f:
					T val2 = DefDatabase<T>.defsList[i];
					val2.ResolveReferences();
					end_IL_000c:;
				}
				catch (Exception ex)
				{
					Log.Error("Error while resolving references for def " + DefDatabase<T>.defsList[i] + ": " + ex);
				}
			}
			DefDatabase<T>.SetIndices();
		}

		private static void SetIndices()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				((Def)(object)DefDatabase<T>.defsList[i]).index = (ushort)i;
			}
		}

		public static void ErrorCheckAllDefs()
		{
			foreach (T allDef in DefDatabase<T>.AllDefs)
			{
				T current = allDef;
				foreach (string item in current.ConfigErrors())
				{
					Log.Warning("Config error in " + current + ": " + item);
				}
			}
		}

		public static T GetNamed(string defName, bool errorOnFail = true)
		{
			if (errorOnFail)
			{
				T result = default(T);
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result))
				{
					return result;
				}
				Log.Error("Failed to find " + typeof(T) + " named " + defName + ". There are " + DefDatabase<T>.defsList.Count + " defs of this type loaded.");
				return (T)null;
			}
			T result2 = default(T);
			if (DefDatabase<T>.defsByName.TryGetValue(defName, out result2))
			{
				return result2;
			}
			return (T)null;
		}

		public static T GetNamedSilentFail(string defName)
		{
			return DefDatabase<T>.GetNamed(defName, false);
		}

		public static T GetByShortHash(ushort shortHash)
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				if (((Def)(object)DefDatabase<T>.defsList[i]).shortHash == shortHash)
				{
					return DefDatabase<T>.defsList[i];
				}
			}
			return (T)null;
		}

		public static T GetRandom()
		{
			return GenCollection.RandomElement<T>((IEnumerable<T>)DefDatabase<T>.defsList);
		}
	}
}
