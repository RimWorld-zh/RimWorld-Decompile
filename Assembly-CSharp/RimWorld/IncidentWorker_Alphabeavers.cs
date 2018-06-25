using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000323 RID: 803
	internal class IncidentWorker_Alphabeavers : IncidentWorker
	{
		// Token: 0x040008BF RID: 2239
		private static readonly FloatRange CountPerColonistRange = new FloatRange(1.7f, 3f);

		// Token: 0x040008C0 RID: 2240
		private const int MinCount = 6;

		// Token: 0x040008C1 RID: 2241
		private const int MaxCount = 25;

		// Token: 0x06000DB2 RID: 3506 RVA: 0x000753FC File Offset: 0x000737FC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				IntVec3 intVec;
				result = RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, null);
			}
			return result;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00075440 File Offset: 0x00073840
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef alphabeaver = PawnKindDefOf.Alphabeaver;
			IntVec3 intVec;
			bool result;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, null))
			{
				result = false;
			}
			else
			{
				int freeColonistsCount = map.mapPawns.FreeColonistsCount;
				float randomInRange = IncidentWorker_Alphabeavers.CountPerColonistRange.RandomInRange;
				int num = Mathf.Clamp(GenMath.RoundRandom((float)freeColonistsCount * randomInRange), 6, 25);
				for (int i = 0; i < num; i++)
				{
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
					Pawn newThing = PawnGenerator.GeneratePawn(alphabeaver, null);
					GenSpawn.Spawn(newThing, loc, map, WipeMode.Vanish);
				}
				Find.LetterStack.ReceiveLetter("LetterLabelBeaversArrived".Translate(), "BeaversArrived".Translate(), LetterDefOf.ThreatSmall, new TargetInfo(intVec, map, false), null, null);
				result = true;
			}
			return result;
		}
	}
}
