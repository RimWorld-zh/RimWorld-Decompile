using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000321 RID: 801
	internal class IncidentWorker_Alphabeavers : IncidentWorker
	{
		// Token: 0x040008BC RID: 2236
		private static readonly FloatRange CountPerColonistRange = new FloatRange(1.7f, 3f);

		// Token: 0x040008BD RID: 2237
		private const int MinCount = 6;

		// Token: 0x040008BE RID: 2238
		private const int MaxCount = 25;

		// Token: 0x06000DAF RID: 3503 RVA: 0x000752A4 File Offset: 0x000736A4
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

		// Token: 0x06000DB0 RID: 3504 RVA: 0x000752E8 File Offset: 0x000736E8
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
