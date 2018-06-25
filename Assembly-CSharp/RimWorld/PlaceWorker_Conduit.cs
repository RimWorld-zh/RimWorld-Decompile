using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_Conduit : PlaceWorker
	{
		public PlaceWorker_Conduit()
		{
		}

		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			List<Thing> thingList = loc.GetThingList(map);
			int i = 0;
			while (i < thingList.Count)
			{
				if (!thingList[i].def.EverTransmitsPower)
				{
					if (thingList[i].def.entityDefToBuild != null)
					{
						ThingDef thingDef = thingList[i].def.entityDefToBuild as ThingDef;
						if (thingDef != null && thingDef.EverTransmitsPower)
						{
							return false;
						}
					}
					i++;
					continue;
				}
				return false;
			}
			return true;
		}
	}
}
