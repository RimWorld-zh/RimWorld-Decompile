using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_FillWithBeds : SymbolResolver
	{
		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache0;

		public SymbolResolver_FillWithBeds()
		{
		}

		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef;
			if (rp.singleThingDef != null)
			{
				thingDef = rp.singleThingDef;
			}
			else if (rp.faction != null && rp.faction.def.techLevel >= TechLevel.Medieval)
			{
				thingDef = ThingDefOf.Bed;
			}
			else
			{
				thingDef = Rand.Element<ThingDef>(ThingDefOf.Bed, ThingDefOf.Bedroll, ThingDefOf.SleepingSpot);
			}
			ThingDef singleThingStuff;
			if (rp.singleThingStuff != null && rp.singleThingStuff.stuffProps.CanMake(thingDef))
			{
				singleThingStuff = rp.singleThingStuff;
			}
			else
			{
				singleThingStuff = GenStuff.RandomStuffInexpensiveFor(thingDef, rp.faction);
			}
			bool @bool = Rand.Bool;
			foreach (IntVec3 intVec in rp.rect)
			{
				if (@bool)
				{
					if (intVec.x % 3 != 0 || intVec.z % 2 != 0)
					{
						continue;
					}
				}
				else if (intVec.x % 2 != 0 || intVec.z % 3 != 0)
				{
					continue;
				}
				Rot4 rot = (!@bool) ? Rot4.North : Rot4.West;
				if (!GenSpawn.WouldWipeAnythingWith(intVec, rot, thingDef, map, (Thing x) => x.def.category == ThingCategory.Building))
				{
					if (!BaseGenUtility.AnyDoorAdjacentCardinalTo(GenAdj.OccupiedRect(intVec, rot, thingDef.Size), map))
					{
						ResolveParams resolveParams = rp;
						resolveParams.rect = GenAdj.OccupiedRect(intVec, rot, thingDef.size);
						resolveParams.singleThingDef = thingDef;
						resolveParams.singleThingStuff = singleThingStuff;
						resolveParams.thingRot = new Rot4?(rot);
						BaseGen.symbolStack.Push("bed", resolveParams);
					}
				}
			}
		}

		[CompilerGenerated]
		private static bool <Resolve>m__0(Thing x)
		{
			return x.def.category == ThingCategory.Building;
		}
	}
}
