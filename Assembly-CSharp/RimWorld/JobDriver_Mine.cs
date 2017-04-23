using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Mine : JobDriver
	{
		public const int BaseTicksBetweenPickHits = 120;

		private const int BaseDamagePerPickHit = 80;

		private const float MinMiningSpeedForNPCs = 0.5f;

		private int ticksToPickHit = -1000;

		private Effecter effecter;

		private Thing MineTarget
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Mine.<MakeNewToils>c__Iterator35 <MakeNewToils>c__Iterator = new JobDriver_Mine.<MakeNewToils>c__Iterator35();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_Mine.<MakeNewToils>c__Iterator35 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private void ResetTicksToPickHit()
		{
			float num = this.pawn.GetStatValue(StatDefOf.MiningSpeed, true);
			if (num < 0.5f && this.pawn.Faction != Faction.OfPlayer)
			{
				num = 0.5f;
			}
			this.ticksToPickHit = (int)Math.Round((double)(120f / num));
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToPickHit, "ticksToPickHit", 0, false);
		}
	}
}
