using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Mine : JobDriver
	{
		private int ticksToPickHit = -1000;

		private Effecter effecter = null;

		public const int BaseTicksBetweenPickHits = 120;

		private const int BaseDamagePerPickHit = 80;

		private const float MinMiningSpeedForNPCs = 0.5f;

		private Thing MineTarget
		{
			get
			{
				return base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(this.MineTarget, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0036: stateMachine*/;
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnCellMissingDesignation(TargetIndex.A, DesignationDefOf.Mine);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private void ResetTicksToPickHit()
		{
			float num = base.pawn.GetStatValue(StatDefOf.MiningSpeed, true);
			if (num < 0.5 && base.pawn.Faction != Faction.OfPlayer)
			{
				num = 0.5f;
			}
			this.ticksToPickHit = (int)Math.Round(120.0 / num);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToPickHit, "ticksToPickHit", 0, false);
		}
	}
}
