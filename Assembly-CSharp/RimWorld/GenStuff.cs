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
			ThingDef result;
			if (!td.MadeFromStuff)
			{
				result = null;
			}
			else if (ThingDefOf.WoodLog.stuffProps.CanMake(td))
			{
				result = ThingDefOf.WoodLog;
			}
			else if (ThingDefOf.Steel.stuffProps.CanMake(td))
			{
				result = ThingDefOf.Steel;
			}
			else if (ThingDefOf.Cloth.stuffProps.CanMake(td))
			{
				result = ThingDefOf.Cloth;
			}
			else
			{
				ThingDef leatherDef = ThingDefOf.Cow.race.leatherDef;
				result = ((!leatherDef.stuffProps.CanMake(td)) ? ((!ThingDefOf.BlocksGranite.stuffProps.CanMake(td)) ? ((!ThingDefOf.Plasteel.stuffProps.CanMake(td)) ? GenStuff.RandomStuffFor(td) : ThingDefOf.Plasteel) : ThingDefOf.BlocksGranite) : leatherDef);
			}
			return result;
		}

		public static ThingDef RandomStuffFor(ThingDef td)
		{
			return td.MadeFromStuff ? GenStuff.AllowedStuffsFor(td).RandomElement() : null;
		}

		public static ThingDef RandomStuffByCommonalityFor(ThingDef td, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			ThingDef result;
			if (!td.MadeFromStuff)
			{
				result = null;
			}
			else
			{
				ThingDef thingDef = default(ThingDef);
				if (!GenStuff.TryRandomStuffByCommonalityFor(td, out thingDef, maxTechLevel))
				{
					thingDef = GenStuff.DefaultStuffFor(td);
				}
				result = thingDef;
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
			bool result;
			if (!td.MadeFromStuff)
			{
				stuff = null;
				result = true;
			}
			else
			{
				IEnumerable<ThingDef> source = GenStuff.AllowedStuffsFor(td);
				if (maxTechLevel != 0)
				{
					source = from x in source
					where (int)x.techLevel <= (int)maxTechLevel
					select x;
				}
				result = source.TryRandomElementByWeight<ThingDef>((Func<ThingDef, float>)((ThingDef x) => x.stuffProps.commonality), out stuff);
			}
			return result;
		}
	}
}
