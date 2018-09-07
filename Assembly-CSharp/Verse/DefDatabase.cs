using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public static class DefDatabase<T> where T : Def, new()
	{
		private static List<T> defsList = new List<T>();

		private static Dictionary<string, T> defsByName = new Dictionary<string, T>();

		[CompilerGenerated]
		private static Func<ModContentPack, int> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ModContentPack, int> <>f__am$cache1;

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
			foreach (ModContentPack modContentPack in (from m in LoadedModManager.RunningMods
			orderby m.OverwritePriority
			select m).ThenBy((ModContentPack x) => LoadedModManager.RunningModsListForReading.IndexOf(x)))
			{
				hashSet.Clear();
				foreach (T t in GenDefDatabase.DefsToGoInDatabase<T>(modContentPack))
				{
					if (hashSet.Contains(t.defName))
					{
						Log.Error(string.Concat(new object[]
						{
							"Mod ",
							modContentPack,
							" has multiple ",
							typeof(T),
							"s named ",
							t.defName,
							". Skipping."
						}), false);
					}
					else
					{
						if (t.defName == "UnnamedDef")
						{
							string text = "Unnamed" + typeof(T).Name + Rand.Range(1, 100000).ToString() + "A";
							Log.Error(string.Concat(new string[]
							{
								typeof(T).Name,
								" in ",
								modContentPack.ToString(),
								" with label ",
								t.label,
								" lacks a defName. Giving name ",
								text
							}), false);
							t.defName = text;
						}
						T def;
						if (DefDatabase<T>.defsByName.TryGetValue(t.defName, out def))
						{
							DefDatabase<T>.Remove(def);
						}
						DefDatabase<T>.Add(t);
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
			while (DefDatabase<T>.defsByName.ContainsKey(def.defName))
			{
				Log.Error(string.Concat(new object[]
				{
					"Adding duplicate ",
					typeof(T),
					" name: ",
					def.defName
				}), false);
				T t = def;
				t.defName += Mathf.RoundToInt(Rand.Value * 1000f);
			}
			DefDatabase<T>.defsList.Add(def);
			DefDatabase<T>.defsByName.Add(def.defName, def);
			if (DefDatabase<T>.defsList.Count > 65535)
			{
				Log.Error(string.Concat(new object[]
				{
					"Too many ",
					typeof(T),
					"; over ",
					ushort.MaxValue
				}), false);
			}
			def.index = (ushort)(DefDatabase<T>.defsList.Count - 1);
		}

		private static void Remove(T def)
		{
			DefDatabase<T>.defsByName.Remove(def.defName);
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
				T t = DefDatabase<T>.defsList[i];
				t.ClearCachedData();
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
						T t = DefDatabase<T>.defsList[i];
						if (t.GetType() != typeof(T))
						{
							goto IL_9B;
						}
					}
					T t2 = DefDatabase<T>.defsList[i];
					t2.ResolveReferences();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error while resolving references for def ",
						DefDatabase<T>.defsList[i],
						": ",
						ex
					}), false);
				}
				IL_9B:;
			}
			DefDatabase<T>.SetIndices();
		}

		private static void SetIndices()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].index = (ushort)i;
			}
		}

		public static void ErrorCheckAllDefs()
		{
			foreach (T t in DefDatabase<T>.AllDefs)
			{
				try
				{
					if (!t.ignoreConfigErrors)
					{
						foreach (string text in t.ConfigErrors())
						{
							Log.Error(string.Concat(new object[]
							{
								"Config error in ",
								t,
								": ",
								text
							}), false);
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception in ConfigErrors() of ",
						t.defName,
						": ",
						ex
					}), false);
				}
			}
		}

		public static T GetNamed(string defName, bool errorOnFail = true)
		{
			if (errorOnFail)
			{
				T result;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result))
				{
					return result;
				}
				Log.Error(string.Concat(new object[]
				{
					"Failed to find ",
					typeof(T),
					" named ",
					defName,
					". There are ",
					DefDatabase<T>.defsList.Count,
					" defs of this type loaded."
				}), false);
				return (T)((object)null);
			}
			else
			{
				T result2;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result2))
				{
					return result2;
				}
				return (T)((object)null);
			}
		}

		public static T GetNamedSilentFail(string defName)
		{
			return DefDatabase<T>.GetNamed(defName, false);
		}

		public static T GetByShortHash(ushort shortHash)
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				if (DefDatabase<T>.defsList[i].shortHash == shortHash)
				{
					return DefDatabase<T>.defsList[i];
				}
			}
			return (T)((object)null);
		}

		public static T GetRandom()
		{
			return DefDatabase<T>.defsList.RandomElement<T>();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static DefDatabase()
		{
		}

		[CompilerGenerated]
		private static int <AddAllInMods>m__0(ModContentPack m)
		{
			return m.OverwritePriority;
		}

		[CompilerGenerated]
		private static int <AddAllInMods>m__1(ModContentPack x)
		{
			return LoadedModManager.RunningModsListForReading.IndexOf(x);
		}
	}
}
