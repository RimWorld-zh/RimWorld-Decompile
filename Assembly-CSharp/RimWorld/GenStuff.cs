using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097F RID: 2431
	public static class GenStuff
	{
		// Token: 0x060036C7 RID: 14023 RVA: 0x001D4284 File Offset: 0x001D2684
		public static ThingDef DefaultStuffFor(BuildableDef bd)
		{
			ThingDef result;
			if (!bd.MadeFromStuff)
			{
				result = null;
			}
			else
			{
				ThingDef thingDef = bd as ThingDef;
				if (thingDef != null)
				{
					if (thingDef.IsMeleeWeapon)
					{
						if (ThingDefOf.Steel.stuffProps.CanMake(bd))
						{
							return ThingDefOf.Steel;
						}
						if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
						{
							return ThingDefOf.Plasteel;
						}
					}
					if (thingDef.IsApparel)
					{
						if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
						{
							return ThingDefOf.Cloth;
						}
						if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
						{
							return ThingDefOf.Leather_Plain;
						}
						if (ThingDefOf.Steel.stuffProps.CanMake(bd))
						{
							return ThingDefOf.Steel;
						}
					}
				}
				if (ThingDefOf.WoodLog.stuffProps.CanMake(bd))
				{
					result = ThingDefOf.WoodLog;
				}
				else if (ThingDefOf.Steel.stuffProps.CanMake(bd))
				{
					result = ThingDefOf.Steel;
				}
				else if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
				{
					result = ThingDefOf.Plasteel;
				}
				else if (ThingDefOf.BlocksGranite.stuffProps.CanMake(bd))
				{
					result = ThingDefOf.BlocksGranite;
				}
				else if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
				{
					result = ThingDefOf.Cloth;
				}
				else if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
				{
					result = ThingDefOf.Leather_Plain;
				}
				else
				{
					result = GenStuff.AllowedStuffsFor(bd, TechLevel.Undefined).First<ThingDef>();
				}
			}
			return result;
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x001D4440 File Offset: 0x001D2840
		public static ThingDef RandomStuffFor(ThingDef td)
		{
			ThingDef result;
			if (!td.MadeFromStuff)
			{
				result = null;
			}
			else
			{
				result = GenStuff.AllowedStuffsFor(td, TechLevel.Undefined).RandomElement<ThingDef>();
			}
			return result;
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x001D4474 File Offset: 0x001D2874
		public static ThingDef RandomStuffByCommonalityFor(ThingDef td, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			ThingDef result;
			if (!td.MadeFromStuff)
			{
				result = null;
			}
			else
			{
				ThingDef thingDef;
				if (!GenStuff.TryRandomStuffByCommonalityFor(td, out thingDef, maxTechLevel))
				{
					thingDef = GenStuff.DefaultStuffFor(td);
				}
				result = thingDef;
			}
			return result;
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x001D44B4 File Offset: 0x001D28B4
		public static IEnumerable<ThingDef> AllowedStuffsFor(BuildableDef td, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				yield break;
			}
			List<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefs.Count; i++)
			{
				ThingDef d = allDefs[i];
				if (d.IsStuff && (maxTechLevel == TechLevel.Undefined || d.techLevel <= maxTechLevel) && d.stuffProps.CanMake(td))
				{
					yield return d;
				}
			}
			yield break;
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x001D44E8 File Offset: 0x001D28E8
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
				IEnumerable<ThingDef> source = GenStuff.AllowedStuffsFor(td, maxTechLevel);
				result = source.TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff);
			}
			return result;
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x001D4540 File Offset: 0x001D2940
		public static bool TryRandomStuffFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			bool result;
			if (!td.MadeFromStuff)
			{
				stuff = null;
				result = true;
			}
			else
			{
				IEnumerable<ThingDef> source = GenStuff.AllowedStuffsFor(td, maxTechLevel);
				result = source.TryRandomElement(out stuff);
			}
			return result;
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x001D457C File Offset: 0x001D297C
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, Faction faction)
		{
			return GenStuff.RandomStuffInexpensiveFor(thingDef, (faction == null) ? TechLevel.Undefined : faction.def.techLevel);
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x001D45B0 File Offset: 0x001D29B0
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, TechLevel maxTechLevel)
		{
			ThingDef result;
			if (!thingDef.MadeFromStuff)
			{
				result = null;
			}
			else
			{
				IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor(thingDef, maxTechLevel);
				float cheapestPrice = -1f;
				foreach (ThingDef thingDef2 in enumerable)
				{
					float num = thingDef2.BaseMarketValue / thingDef2.VolumePerUnit;
					if (cheapestPrice == -1f || num < cheapestPrice)
					{
						cheapestPrice = num;
					}
				}
				enumerable = from x in enumerable
				where x.BaseMarketValue / x.VolumePerUnit <= cheapestPrice * 4f
				select x;
				ThingDef thingDef3;
				if (enumerable.TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out thingDef3))
				{
					result = thingDef3;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
	}
}
