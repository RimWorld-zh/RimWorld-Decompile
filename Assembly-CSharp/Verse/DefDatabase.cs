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
			foreach (ModContentPack item in (IEnumerable<ModContentPack>)(from m in LoadedModManager.RunningMods
			orderby m.OverwritePriority
			select m).ThenBy<ModContentPack, int>((Func<ModContentPack, int>)((ModContentPack x) => LoadedModManager.RunningModsListForReading.IndexOf(x))))
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
			foreach (T item in defs)
			{
				DefDatabase<T>.Add(item);
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
							goto IL_0042;
						goto end_IL_000e;
					}
					goto IL_0042;
					IL_0042:
					T val2 = DefDatabase<T>.defsList[i];
					val2.ResolveReferences();
					end_IL_000e:;
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
			T result;
			if (errorOnFail)
			{
				T val = default(T);
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out val))
				{
					result = val;
				}
				else
				{
					Log.Error("Failed to find " + typeof(T) + " named " + defName + ". There are " + DefDatabase<T>.defsList.Count + " defs of this type loaded.");
					result = (T)null;
				}
			}
			else
			{
				T val2 = default(T);
				result = ((!DefDatabase<T>.defsByName.TryGetValue(defName, out val2)) ? ((T)null) : val2);
			}
			return result;
		}

		public static T GetNamedSilentFail(string defName)
		{
			return DefDatabase<T>.GetNamed(defName, false);
		}

		public static T GetByShortHash(ushort shortHash)
		{
			int num = 0;
			T result;
			while (true)
			{
				if (num < DefDatabase<T>.defsList.Count)
				{
					if (((Def)(object)DefDatabase<T>.defsList[num]).shortHash == shortHash)
					{
						result = DefDatabase<T>.defsList[num];
						break;
					}
					num++;
					continue;
				}
				result = (T)null;
				break;
			}
			return result;
		}

		public static T GetRandom()
		{
			return ((IEnumerable<T>)DefDatabase<T>.defsList).RandomElement<T>();
		}
	}
}
