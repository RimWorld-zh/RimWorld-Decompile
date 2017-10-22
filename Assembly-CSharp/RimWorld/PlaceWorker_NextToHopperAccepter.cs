using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_NextToHopperAccepter : PlaceWorker
	{
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			int num = 0;
			AcceptanceReport result;
			while (true)
			{
				if (num < 4)
				{
					IntVec3 c = loc + GenAdj.CardinalDirections[num];
					if (c.InBounds(map))
					{
						List<Thing> thingList = c.GetThingList(map);
						for (int i = 0; i < thingList.Count; i++)
						{
							Thing thing = thingList[i];
							ThingDef thingDef = GenConstruct.BuiltDefOf(thing.def) as ThingDef;
							if (thingDef != null && thingDef.building != null && thingDef.building.wantsHopperAdjacent)
								goto IL_0088;
						}
					}
					num++;
					continue;
				}
				result = "MustPlaceNextToHopperAccepter".Translate();
				break;
				IL_0088:
				result = true;
				break;
			}
			return result;
		}
	}
}
