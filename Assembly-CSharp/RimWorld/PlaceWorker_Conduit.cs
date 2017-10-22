using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_Conduit : PlaceWorker
	{
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			List<Thing> thingList = loc.GetThingList(map);
			int num = 0;
			AcceptanceReport result;
			while (true)
			{
				if (num < thingList.Count)
				{
					if (thingList[num].def.EverTransmitsPower)
					{
						result = false;
						break;
					}
					if (thingList[num].def.entityDefToBuild != null)
					{
						ThingDef thingDef = thingList[num].def.entityDefToBuild as ThingDef;
						if (thingDef != null && thingDef.EverTransmitsPower)
						{
							result = false;
							break;
						}
					}
					num++;
					continue;
				}
				result = true;
				break;
			}
			return result;
		}
	}
}
