using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000902 RID: 2306
	public static class CostListCalculator
	{
		// Token: 0x04001CFD RID: 7421
		private static Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>> cachedCosts = new Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>>(CostListCalculator.FastCostListPairComparer.Instance);

		// Token: 0x06003587 RID: 13703 RVA: 0x001CD877 File Offset: 0x001CBC77
		public static void Reset()
		{
			CostListCalculator.cachedCosts.Clear();
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x001CD884 File Offset: 0x001CBC84
		public static List<ThingDefCountClass> CostListAdjusted(this Thing thing)
		{
			return thing.def.CostListAdjusted(thing.Stuff, true);
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x001CD8AC File Offset: 0x001CBCAC
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

		// Token: 0x02000903 RID: 2307
		private struct CostListPair : IEquatable<CostListCalculator.CostListPair>
		{
			// Token: 0x04001CFE RID: 7422
			public BuildableDef buildable;

			// Token: 0x04001CFF RID: 7423
			public ThingDef stuff;

			// Token: 0x0600358B RID: 13707 RVA: 0x001CDA62 File Offset: 0x001CBE62
			public CostListPair(BuildableDef buildable, ThingDef stuff)
			{
				this.buildable = buildable;
				this.stuff = stuff;
			}

			// Token: 0x0600358C RID: 13708 RVA: 0x001CDA74 File Offset: 0x001CBE74
			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine<BuildableDef>(seed, this.buildable);
				return Gen.HashCombine<ThingDef>(seed, this.stuff);
			}

			// Token: 0x0600358D RID: 13709 RVA: 0x001CDAA8 File Offset: 0x001CBEA8
			public override bool Equals(object obj)
			{
				return obj is CostListCalculator.CostListPair && this.Equals((CostListCalculator.CostListPair)obj);
			}

			// Token: 0x0600358E RID: 13710 RVA: 0x001CDADC File Offset: 0x001CBEDC
			public bool Equals(CostListCalculator.CostListPair other)
			{
				return this == other;
			}

			// Token: 0x0600358F RID: 13711 RVA: 0x001CDB00 File Offset: 0x001CBF00
			public static bool operator ==(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return lhs.buildable == rhs.buildable && lhs.stuff == rhs.stuff;
			}

			// Token: 0x06003590 RID: 13712 RVA: 0x001CDB3C File Offset: 0x001CBF3C
			public static bool operator !=(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return !(lhs == rhs);
			}
		}

		// Token: 0x02000904 RID: 2308
		private class FastCostListPairComparer : IEqualityComparer<CostListCalculator.CostListPair>
		{
			// Token: 0x04001D00 RID: 7424
			public static readonly CostListCalculator.FastCostListPairComparer Instance = new CostListCalculator.FastCostListPairComparer();

			// Token: 0x06003592 RID: 13714 RVA: 0x001CDB64 File Offset: 0x001CBF64
			public bool Equals(CostListCalculator.CostListPair x, CostListCalculator.CostListPair y)
			{
				return x == y;
			}

			// Token: 0x06003593 RID: 13715 RVA: 0x001CDB80 File Offset: 0x001CBF80
			public int GetHashCode(CostListCalculator.CostListPair obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
