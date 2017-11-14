using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class GenStuff
	{
		public static ThingDef DefaultStuffFor(ThingDef td)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			if (ThingDefOf.WoodLog.stuffProps.CanMake(td))
			{
				return ThingDefOf.WoodLog;
			}
			if (ThingDefOf.Steel.stuffProps.CanMake(td))
			{
				return ThingDefOf.Steel;
			}
			if (ThingDefOf.Cloth.stuffProps.CanMake(td))
			{
				return ThingDefOf.Cloth;
			}
			ThingDef leatherDef = ThingDefOf.Cow.race.leatherDef;
			if (leatherDef.stuffProps.CanMake(td))
			{
				return leatherDef;
			}
			if (ThingDefOf.BlocksGranite.stuffProps.CanMake(td))
			{
				return ThingDefOf.BlocksGranite;
			}
			if (ThingDefOf.Plasteel.stuffProps.CanMake(td))
			{
				return ThingDefOf.Plasteel;
			}
			return GenStuff.RandomStuffFor(td);
		}

		public static ThingDef RandomStuffFor(ThingDef td)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			return GenStuff.AllowedStuffsFor(td).RandomElement();
		}

		public static ThingDef RandomStuffByCommonalityFor(ThingDef td, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			ThingDef result = default(ThingDef);
			if (!GenStuff.TryRandomStuffByCommonalityFor(td, out result, maxTechLevel))
			{
				result = GenStuff.DefaultStuffFor(td);
				return result;
			}
			return result;
		}

		public static IEnumerable<ThingDef> AllowedStuffsFor(ThingDef td)
		{
			if (td.MadeFromStuff)
			{
				List<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefsListForReading;
				int i = 0;
				ThingDef d;
				while (true)
				{
					if (i < allDefs.Count)
					{
						d = allDefs[i];
						if (d.IsStuff && d.stuffProps.CanMake(td))
							break;
						i++;
						continue;
					}
					yield break;
				}
				yield return d;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public static bool TryRandomStuffByCommonalityFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				stuff = null;
				return true;
			}
			IEnumerable<ThingDef> source = GenStuff.AllowedStuffsFor(td);
			if (maxTechLevel != 0)
			{
				source = from x in source
				where (int)x.techLevel <= (int)maxTechLevel
				select x;
			}
			return source.TryRandomElementByWeight<ThingDef>((Func<ThingDef, float>)((ThingDef x) => x.stuffProps.commonality), out stuff);
		}
	}
}
