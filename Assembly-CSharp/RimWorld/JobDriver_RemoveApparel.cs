using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_RemoveApparel : JobDriver
	{
		private const int DurationTicks = 60;

		private Apparel TargetApparel
		{
			get
			{
				return (Apparel)base.TargetA.Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_004a: stateMachine*/)._003C_003Ef__this.pawn.pather.StopDead();
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 60
			};
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.pawn.apparel.WornApparel.Contains(((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.TargetApparel))
					{
						Apparel apparel = default(Apparel);
						if (((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.pawn.apparel.TryDrop(((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.TargetApparel, out apparel))
						{
							((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.CurJob.targetA = (Thing)apparel;
							if (((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.CurJob.haulDroppedApparel)
							{
								apparel.SetForbidden(false, false);
								StoragePriority currentPriority = HaulAIUtility.StoragePriorityAtFor(apparel.Position, apparel);
								IntVec3 c = default(IntVec3);
								if (StoreUtility.TryFindBestBetterStoreCellFor((Thing)apparel, ((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.pawn, ((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.Map, currentPriority, ((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.pawn.Faction, out c, true))
								{
									((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.CurJob.count = apparel.stackCount;
									((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.CurJob.targetB = c;
								}
								else
								{
									((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Incompletable);
								}
							}
							else
							{
								((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Succeeded);
							}
						}
						else
						{
							((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Incompletable);
						}
					}
					else
					{
						((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_009d: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Incompletable);
					}
				}
			};
			if (base.CurJob.haulDroppedApparel)
			{
				yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
				yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
				yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false).FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator53)/*Error near IL_011a: stateMachine*/)._003C_003Ef__this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)));
				Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				yield return carryToCell;
				yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
			}
		}
	}
}
