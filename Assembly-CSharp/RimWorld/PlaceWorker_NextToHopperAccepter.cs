using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6B RID: 3179
	public class PlaceWorker_NextToHopperAccepter : PlaceWorker
	{
		// Token: 0x060045D7 RID: 17879 RVA: 0x0024DDE4 File Offset: 0x0024C1E4
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = loc + GenAdj.CardinalDirections[i];
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						ThingDef thingDef = GenConstruct.BuiltDefOf(thing.def) as ThingDef;
						if (thingDef != null && thingDef.building != null)
						{
							if (thingDef.building.wantsHopperAdjacent)
							{
								return true;
							}
						}
					}
				}
			}
			return "MustPlaceNextToHopperAccepter".Translate();
		}
	}
}
