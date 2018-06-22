using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF1 RID: 2801
	public static class DefDatabase<T> where T : Def, new()
	{
		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06003E08 RID: 15880 RVA: 0x0020B5FC File Offset: 0x002099FC
		public static IEnumerable<T> AllDefs
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06003E09 RID: 15881 RVA: 0x0020B618 File Offset: 0x00209A18
		public static List<T> AllDefsListForReading
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06003E0A RID: 15882 RVA: 0x0020B634 File Offset: 0x00209A34
		public static int DefCount
		{
			get
			{
				return DefDatabase<T>.defsList.Count;
			}
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0020B654 File Offset: 0x00209A54
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

		// Token: 0x06003E0C RID: 15884 RVA: 0x0020B8AC File Offset: 0x00209CAC
		public static void Add(IEnumerable<T> defs)
		{
			foreach (T def in defs)
			{
				DefDatabase<T>.Add(def);
			}
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x0020B904 File Offset: 0x00209D04
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

		// Token: 0x06003E0E RID: 15886 RVA: 0x0020BA22 File Offset: 0x00209E22
		private static void Remove(T def)
		{
			DefDatabase<T>.defsByName.Remove(def.defName);
			DefDatabase<T>.defsList.Remove(def);
			DefDatabase<T>.SetIndices();
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x0020BA4C File Offset: 0x00209E4C
		public static void Clear()
		{
			DefDatabase<T>.defsList.Clear();
			DefDatabase<T>.defsByName.Clear();
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x0020BA64 File Offset: 0x00209E64
		public static void ClearCachedData()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				T t = DefDatabase<T>.defsList[i];
				t.ClearCachedData();
			}
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x0020BAA8 File Offset: 0x00209EA8
		public static void ResolveAllReferences(bool onlyExactlyMyType = true)
		{
			DefDatabase<T>.SetIndices();
			int i = 0;
			while (i < DefDatabase<T>.defsList.Count)
			{
				try
				{
					if (onlyExactlyMyType)
					{
						T t = DefDatabase<T>.defsList[i];
						if (t.GetType() != typeof(T))
						{
							goto IL_A2;
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
				IL_A2:
				i++;
				continue;
				goto IL_A2;
			}
			DefDatabase<T>.SetIndices();
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x0020BB80 File Offset: 0x00209F80
		private static void SetIndices()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].index = (ushort)i;
			}
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x0020BBC4 File Offset: 0x00209FC4
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

		// Token: 0x06003E14 RID: 15892 RVA: 0x0020BCF4 File Offset: 0x0020A0F4
		public static T GetNamed(string defName, bool errorOnFail = true)
		{
			T result;
			T t2;
			if (errorOnFail)
			{
				T t;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out t))
				{
					result = t;
				}
				else
				{
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
					result = (T)((object)null);
				}
			}
			else if (DefDatabase<T>.defsByName.TryGetValue(defName, out t2))
			{
				result = t2;
			}
			else
			{
				result = (T)((object)null);
			}
			return result;
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x0020BDAC File Offset: 0x0020A1AC
		public static T GetNamedSilentFail(string defName)
		{
			return DefDatabase<T>.GetNamed(defName, false);
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x0020BDC8 File Offset: 0x0020A1C8
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

		// Token: 0x06003E17 RID: 15895 RVA: 0x0020BE2C File Offset: 0x0020A22C
		public static T GetRandom()
		{
			return DefDatabase<T>.defsList.RandomElement<T>();
		}

		// Token: 0x0400273F RID: 10047
		private static List<T> defsList = new List<T>();

		// Token: 0x04002740 RID: 10048
		private static Dictionary<string, T> defsByName = new Dictionary<string, T>();
	}
}
