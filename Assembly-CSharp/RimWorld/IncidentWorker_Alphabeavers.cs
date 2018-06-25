using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	internal class IncidentWorker_Alphabeavers : IncidentWorker
	{
		private static readonly FloatRange CountPerColonistRange = new FloatRange(1.7f, 3f);

		private const int MinCount = 6;

		private const int MaxCount = 25;

		public IncidentWorker_Alphabeavers()
		{
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static IncidentWorker_Alphabeavers()
		{
		}
	}
}
