using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009EE RID: 2542
	public static class LordMaker
	{
		// Token: 0x06003913 RID: 14611 RVA: 0x001E62A0 File Offset: 0x001E46A0
		public static Lord MakeNewLord(Faction faction, LordJob lordJob, Map map, IEnumerable<Pawn> startingPawns = null)
		{
			Lord result;
			if (map == null)
			{
				Log.Warning("Tried to create a lord with null map.", false);
				result = null;
			}
			else
			{
				Lord lord = new Lord();
				lord.loadID = Find.UniqueIDsManager.GetNextLordID();
				lord.faction = faction;
				map.lordManager.AddLord(lord);
				lord.SetJob(lordJob);
				lord.GotoToil(lord.Graph.StartingToil);
				if (startingPawns != null)
				{
					foreach (Pawn p in startingPawns)
					{
						lord.AddPawn(p);
					}
				}
				result = lord;
			}
			return result;
		}
	}
}
