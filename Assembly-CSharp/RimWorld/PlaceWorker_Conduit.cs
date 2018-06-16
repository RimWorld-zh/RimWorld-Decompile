using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6E RID: 3182
	public class PlaceWorker_Conduit : PlaceWorker
	{
		// Token: 0x060045D1 RID: 17873 RVA: 0x0024C7E4 File Offset: 0x0024ABE4
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
