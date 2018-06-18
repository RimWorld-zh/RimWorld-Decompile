using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6D RID: 3181
	public class PlaceWorker_Conduit : PlaceWorker
	{
		// Token: 0x060045CF RID: 17871 RVA: 0x0024C7BC File Offset: 0x0024ABBC
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
