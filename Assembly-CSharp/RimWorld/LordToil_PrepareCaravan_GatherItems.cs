using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_PrepareCaravan_GatherItems : LordToil
	{
		private IntVec3 meetingPoint;

		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		public LordToil_PrepareCaravan_GatherItems(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		public override void UpdateAllDuties()
		{
			for (int i = 0; i < base.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = base.lord.ownedPawns[i];
				if (pawn.IsColonist)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherItems);
				}
				else if (pawn.RaceProps.Animal)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait);
				}
			}
		}

		public override void LordToilTick()
		{
			base.LordToilTick();
			if (Find.TickManager.TicksGame % 120 == 0)
			{
				bool flag = true;
				for (int i = 0; i < base.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = base.lord.ownedPawns[i];
					if (pawn.IsColonist && pawn.mindState.lastJobTag != JobTag.WaitingForOthersToFinishGatheringItems)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					List<Pawn> allPawnsSpawned = base.Map.mapPawns.AllPawnsSpawned;
					int num = 0;
					while (num < allPawnsSpawned.Count)
					{
						if (allPawnsSpawned[num].CurJob == null || !(allPawnsSpawned[num].jobs.curDriver is JobDriver_PrepareCaravan_GatherItems) || allPawnsSpawned[num].CurJob.lord != base.lord)
						{
							num++;
							continue;
						}
						flag = false;
						break;
					}
				}
				if (flag)
				{
					base.lord.ReceiveMemo("AllItemsGathered");
				}
			}
		}
	}
}
