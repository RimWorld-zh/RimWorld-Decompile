using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class CostListCalculator
	{
		private struct CostListPair : IEquatable<CostListPair>
		{
			public BuildableDef buildable;

			public ThingDef stuff;

			public CostListPair(BuildableDef buildable, ThingDef stuff)
			{
				this.buildable = buildable;
				this.stuff = stuff;
			}

			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine(seed, this.buildable);
				return Gen.HashCombine(seed, this.stuff);
			}

			public override bool Equals(object obj)
			{
				return obj is CostListPair && this.Equals((CostListPair)obj);
			}

			public bool Equals(CostListPair other)
			{
				return this == other;
			}

			public static bool operator ==(CostListPair lhs, CostListPair rhs)
			{
				return lhs.buildable == rhs.buildable && lhs.stuff == rhs.stuff;
			}

			public static bool operator !=(CostListPair lhs, CostListPair rhs)
			{
				return !(lhs == rhs);
			}
		}

		private class FastCostListPairComparer : IEqualityComparer<CostListPair>
		{
			public static readonly FastCostListPairComparer Instance = new FastCostListPairComparer();

			public bool Equals(CostListPair x, CostListPair y)
			{
				return x == y;
			}

			public int GetHashCode(CostListPair obj)
			{
				return obj.GetHashCode();
			}
		}

		private static Dictionary<CostListPair, List<ThingCountClass>> cachedCosts = new Dictionary<CostListPair, List<ThingCountClass>>(FastCostListPairComparer.Instance);

		public static void Reset()
		{
			CostListCalculator.cachedCosts.Clear();
		}

		public static List<ThingCountClass> CostListAdjusted(this Thing thing)
		{
			return thing.def.CostListAdjusted(thing.Stuff, true);
		}

		public static List<ThingCountClass> CostListAdjusted(this BuildableDef entDef, ThingDef stuff, bool errorOnNullStuff = true)
		{
			CostListPair key = new CostListPair(entDef, stuff);
			List<ThingCountClass> list = default(List<ThingCountClass>);
			List<ThingCountClass> result;
			if (!CostListCalculator.cachedCosts.TryGetValue(key, out list))
			{
				list = new List<ThingCountClass>();
				int num = 0;
				if (entDef.MadeFromStuff)
				{
					if (errorOnNullStuff && stuff == null)
					{
						Log.Error("Cannot get AdjustedCostList for " + entDef + " with null Stuff.");
						result = null;
						goto IL_0173;
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
					Log.Error("Got AdjustedCostList for " + entDef + " with stuff " + stuff + " but is not MadeFromStuff.");
				}
				bool flag = false;
				if (entDef.costList != null)
				{
					for (int i = 0; i < entDef.costList.Count; i++)
					{
						ThingCountClass thingCountClass = entDef.costList[i];
						if (thingCountClass.thingDef == stuff)
						{
							list.Add(new ThingCountClass(thingCountClass.thingDef, thingCountClass.count + num));
							flag = true;
						}
						else
						{
							list.Add(thingCountClass);
						}
					}
				}
				if (!flag && num > 0)
				{
					list.Add(new ThingCountClass(stuff, num));
				}
				CostListCalculator.cachedCosts.Add(key, list);
			}
			result = list;
			goto IL_0173;
			IL_0173:
			return result;
		}
	}
}
