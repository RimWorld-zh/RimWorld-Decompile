using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Hunt : JobDriver
	{
		private int jobStartTick = -1;

		private const TargetIndex VictimInd = TargetIndex.A;

		private const TargetIndex CorpseInd = TargetIndex.A;

		private const TargetIndex StoreCellInd = TargetIndex.B;

		private const int MaxHuntTicks = 5000;

		public Pawn Victim
		{
			get
			{
				Corpse corpse = this.Corpse;
				return (corpse == null) ? ((Pawn)base.job.GetTarget(TargetIndex.A).Thing) : corpse.InnerPawn;
			}
		}

		private Corpse Corpse
		{
			get
			{
				return base.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.jobStartTick, "jobStartTick", 0, false);
		}

		public override string GetReport()
		{
			return (this.Victim == null) ? base.GetReport() : base.job.def.reportString.Replace("TargetA", this.Victim.LabelShort);
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve((Thing)this.Victim, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn((Func<bool>)delegate
			{
				bool result;
				if (!((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0058: stateMachine*/)._0024this.job.ignoreDesignations)
				{
					Pawn victim = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0058: stateMachine*/)._0024this.Victim;
					if (victim != null && !victim.Dead && ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0058: stateMachine*/)._0024this.Map.designationManager.DesignationOn(victim, DesignationDefOf.Hunt) == null)
					{
						result = true;
						goto IL_0064;
					}
				}
				result = false;
				goto IL_0064;
				IL_0064:
				return result;
			});
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_007b: stateMachine*/)._0024this.jobStartTick = Find.TickManager.TicksGame;
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private Toil StartCollectCorpseToil()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate
			{
				if (this.Victim == null)
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					TaleRecorder.RecordTale(TaleDefOf.Hunted, base.pawn, this.Victim);
					Corpse corpse = this.Victim.Corpse;
					if (corpse == null || !base.pawn.CanReserveAndReach((Thing)corpse, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false))
					{
						base.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					else
					{
						corpse.SetForbidden(false, true);
						IntVec3 c = default(IntVec3);
						if (StoreUtility.TryFindBestBetterStoreCellFor((Thing)corpse, base.pawn, base.Map, StoragePriority.Unstored, base.pawn.Faction, out c, true))
						{
							base.pawn.Reserve((Thing)corpse, base.job, 1, -1, null);
							base.pawn.Reserve(c, base.job, 1, -1, null);
							base.job.SetTarget(TargetIndex.B, c);
							base.job.SetTarget(TargetIndex.A, (Thing)corpse);
							base.job.count = 1;
							base.job.haulMode = HaulMode.ToCellStorage;
						}
						else
						{
							base.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
						}
					}
				}
			};
			return toil;
		}
	}
}
