using System;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_FillWithBeds : SymbolResolver
	{
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			bool @bool = Rand.Bool;
			ThingDef thingDef = rp.singleThingDef ?? Rand.Element(ThingDefOf.Bed, ThingDefOf.Bedroll, ThingDefOf.SleepingSpot);
			ThingDef singleThingStuff = rp.singleThingStuff ?? GenStuff.RandomStuffByCommonalityFor(thingDef, (rp.faction != null) ? rp.faction.def.techLevel : TechLevel.Undefined);
			foreach (IntVec3 item in rp.rect)
			{
				IntVec3 current = item;
				if (@bool)
				{
					if (current.x % 3 == 0 && current.z % 2 == 0)
						goto IL_00dc;
				}
				else if (current.x % 2 == 0 && current.z % 3 == 0)
					goto IL_00dc;
				continue;
				IL_00dc:
				Rot4 rot = (!@bool) ? Rot4.North : Rot4.West;
				if (!GenSpawn.WouldWipeAnythingWith(current, rot, thingDef, map, (Predicate<Thing>)((Thing x) => x.def.category == ThingCategory.Building)) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(GenAdj.OccupiedRect(current, rot, thingDef.Size), map))
				{
					ResolveParams resolveParams = rp;
					resolveParams.rect = GenAdj.OccupiedRect(current, rot, thingDef.size);
					resolveParams.singleThingDef = thingDef;
					resolveParams.singleThingStuff = singleThingStuff;
					resolveParams.thingRot = new Rot4?(rot);
					BaseGen.symbolStack.Push("bed", resolveParams);
				}
			}
		}
	}
}
