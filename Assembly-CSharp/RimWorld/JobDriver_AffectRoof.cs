using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_AffectRoof : JobDriver
	{
		private const TargetIndex CellInd = TargetIndex.A;

		private const TargetIndex GotoTargetInd = TargetIndex.B;

		private const float BaseWorkAmount = 65f;

		private float workLeft;

		protected IntVec3 Cell
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.A).Cell;
			}
		}

		protected abstract PathEndMode PathEndMode
		{
			get;
		}

		protected abstract void DoEffect();

		protected abstract bool DoWorkFailOn();

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.B);
			ReservationLayerDef ceiling = ReservationLayerDefOf.Ceiling;
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, ceiling);
			yield return Toils_Goto.Goto(TargetIndex.B, this.PathEndMode);
			Toil doWork = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__IteratorC)/*Error near IL_008b: stateMachine*/)._003C_003Ef__this.workLeft = 65f;
				},
				tickAction = (Action)delegate
				{
					float statValue = ((_003CMakeNewToils_003Ec__IteratorC)/*Error near IL_00a2: stateMachine*/)._003CdoWork_003E__0.actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
					((_003CMakeNewToils_003Ec__IteratorC)/*Error near IL_00a2: stateMachine*/)._003C_003Ef__this.workLeft -= statValue;
					if (((_003CMakeNewToils_003Ec__IteratorC)/*Error near IL_00a2: stateMachine*/)._003C_003Ef__this.workLeft <= 0.0)
					{
						((_003CMakeNewToils_003Ec__IteratorC)/*Error near IL_00a2: stateMachine*/)._003C_003Ef__this.DoEffect();
						((_003CMakeNewToils_003Ec__IteratorC)/*Error near IL_00a2: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
					}
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.B, this.PathEndMode);
			doWork.PlaySoundAtStart(SoundDefOf.RoofStart);
			doWork.PlaySoundAtEnd(SoundDefOf.RoofFinish);
			doWork.WithEffect(EffecterDefOf.RoofWork, TargetIndex.A);
			doWork.FailOn(new Func<bool>(this.DoWorkFailOn));
			doWork.WithProgressBar(TargetIndex.A, (Func<float>)(() => (float)(1.0 - ((_003CMakeNewToils_003Ec__IteratorC)/*Error near IL_0124: stateMachine*/)._003C_003Ef__this.workLeft / 65.0)), false, -0.5f);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			yield return doWork;
		}
	}
}
