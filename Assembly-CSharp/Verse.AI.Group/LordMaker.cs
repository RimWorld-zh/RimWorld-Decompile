using RimWorld;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	public static class LordMaker
	{
		public static Lord MakeNewLord(Faction faction, LordJob lordJob, Map map, IEnumerable<Pawn> startingPawns = null)
		{
			Lord result;
			if (map == null)
			{
				Log.Warning("Tried to create a lord with null map.");
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
					foreach (Pawn item in startingPawns)
					{
						lord.AddPawn(item);
					}
				}
				result = lord;
			}
			return result;
		}
	}
}
