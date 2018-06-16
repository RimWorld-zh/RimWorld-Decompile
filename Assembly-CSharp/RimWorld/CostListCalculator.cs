using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000904 RID: 2308
	public static class CostListCalculator
	{
		// Token: 0x06003588 RID: 13704 RVA: 0x001CD487 File Offset: 0x001CB887
		public static void Reset()
		{
			CostListCalculator.cachedCosts.Clear();
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x001CD494 File Offset: 0x001CB894
		public static List<ThingDefCountClass> CostListAdjusted(this Thing thing)
		{
			return thing.def.CostListAdjusted(thing.Stuff, true);
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x001CD4BC File Offset: 0x001CB8BC
		public static List<ThingDefCountClass> CostListAdjusted(this BuildableDef entDef, ThingDef stuff, bool errorOnNullStuff = true)
		{
			CostListCalculator.CostListPair key = new CostListCalculator.CostListPair(entDef, stuff);
			List<ThingDefCountClass> list;
			if (!CostListCalculator.cachedCosts.TryGetValue(key, out list))
			{
				list = new List<ThingDefCountClass>();
				int num = 0;
				if (entDef.MadeFromStuff)
				{
					if (errorOnNullStuff && stuff == null)
					{
						Log.Error("Cannot get AdjustedCostList for " + entDef + " with null Stuff.", false);
						ThingDef thingDef = GenStuff.DefaultStuffFor(entDef);
						return (thingDef == null) ? null : entDef.CostListAdjusted(GenStuff.DefaultStuffFor(entDef), true);
					}
					if (stuff != null)
					{
						num = Mathf.RoundToInt((float)entDef.costStuffCount / stuff.VolumePerUnit);
						if (num < 1)
						{
							num = 1;
						}
					}
					else
					{
						num = entDef.costStuffCount;
					}
				}
				else if (stuff != null)
				{
					Log.Error(string.Concat(new object[]
					{
						"Got AdjustedCostList for ",
						entDef,
						" with stuff ",
						stuff,
						" but is not MadeFromStuff."
					}), false);
				}
				bool flag = false;
				if (entDef.costList != null)
				{
					for (int i = 0; i < entDef.costList.Count; i++)
					{
						ThingDefCountClass thingDefCountClass = entDef.costList[i];
						if (thingDefCountClass.thingDef == stuff)
						{
							list.Add(new ThingDefCountClass(thingDefCountClass.thingDef, thingDefCountClass.count + num));
							flag = true;
						}
						else
						{
							list.Add(thingDefCountClass);
						}
					}
				}
				if (!flag && num > 0)
				{
					list.Add(new ThingDefCountClass(stuff, num));
				}
				CostListCalculator.cachedCosts.Add(key, list);
			}
			return list;
		}

		// Token: 0x04001CFF RID: 7423
		private static Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>> cachedCosts = new Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>>(CostListCalculator.FastCostListPairComparer.Instance);

		// Token: 0x02000905 RID: 2309
		private struct CostListPair : IEquatable<CostListCalculator.CostListPair>
		{
			// Token: 0x0600358C RID: 13708 RVA: 0x001CD672 File Offset: 0x001CBA72
			public CostListPair(BuildableDef buildable, ThingDef stuff)
			{
				this.buildable = buildable;
				this.stuff = stuff;
			}

			// Token: 0x0600358D RID: 13709 RVA: 0x001CD684 File Offset: 0x001CBA84
			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine<BuildableDef>(seed, this.buildable);
				return Gen.HashCombine<ThingDef>(seed, this.stuff);
			}

			// Token: 0x0600358E RID: 13710 RVA: 0x001CD6B8 File Offset: 0x001CBAB8
			public override bool Equals(object obj)
			{
				return obj is CostListCalculator.CostListPair && this.Equals((CostListCalculator.CostListPair)obj);
			}

			// Token: 0x0600358F RID: 13711 RVA: 0x001CD6EC File Offset: 0x001CBAEC
			public bool Equals(CostListCalculator.CostListPair other)
			{
				return this == other;
			}

			// Token: 0x06003590 RID: 13712 RVA: 0x001CD710 File Offset: 0x001CBB10
			public static bool operator ==(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return lhs.buildable == rhs.buildable && lhs.stuff == rhs.stuff;
			}

			// Token: 0x06003591 RID: 13713 RVA: 0x001CD74C File Offset: 0x001CBB4C
			public static bool operator !=(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x04001D00 RID: 7424
			public BuildableDef buildable;

			// Token: 0x04001D01 RID: 7425
			public ThingDef stuff;
		}

		// Token: 0x02000906 RID: 2310
		private class FastCostListPairComparer : IEqualityComparer<CostListCalculator.CostListPair>
		{
			// Token: 0x06003593 RID: 13715 RVA: 0x001CD774 File Offset: 0x001CBB74
			public bool Equals(CostListCalculator.CostListPair x, CostListCalculator.CostListPair y)
			{
				return x == y;
			}

			// Token: 0x06003594 RID: 13716 RVA: 0x001CD790 File Offset: 0x001CBB90
			public int GetHashCode(CostListCalculator.CostListPair obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x04001D02 RID: 7426
			public static readonly CostListCalculator.FastCostListPairComparer Instance = new CostListCalculator.FastCostListPairComparer();
		}
	}
}
