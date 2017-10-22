using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_ClearSnow : JobDriver
	{
		private const float ClearWorkPerSnowDepth = 100f;

		private float workDone;

		private float TotalNeededWork
		{
			get
			{
				return (float)(100.0 * base.Map.snowGrid.GetDepth(base.TargetLocA));
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil clearToil = new Toil
			{
				tickAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator27)/*Error near IL_006e: stateMachine*/)._003CclearToil_003E__0.actor;
					float statValue;
					float num = statValue = actor.GetStatValue(StatDefOf.WorkSpeedGlobal, true);
					((_003CMakeNewToils_003Ec__Iterator27)/*Error near IL_006e: stateMachine*/)._003C_003Ef__this.workDone += statValue;
					if (((_003CMakeNewToils_003Ec__Iterator27)/*Error near IL_006e: stateMachine*/)._003C_003Ef__this.workDone >= ((_003CMakeNewToils_003Ec__Iterator27)/*Error near IL_006e: stateMachine*/)._003C_003Ef__this.TotalNeededWork)
					{
						((_003CMakeNewToils_003Ec__Iterator27)/*Error near IL_006e: stateMachine*/)._003C_003Ef__this.Map.snowGrid.SetDepth(((_003CMakeNewToils_003Ec__Iterator27)/*Error near IL_006e: stateMachine*/)._003C_003Ef__this.TargetLocA, 0f);
						((_003CMakeNewToils_003Ec__Iterator27)/*Error near IL_006e: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			clearToil.WithEffect(EffecterDefOf.ClearSnow, TargetIndex.A);
			clearToil.PlaySustainerOrSound((Func<SoundDef>)(() => SoundDefOf.Interact_ClearSnow));
			clearToil.WithProgressBar(TargetIndex.A, (Func<float>)(() => ((_003CMakeNewToils_003Ec__Iterator27)/*Error near IL_00cd: stateMachine*/)._003C_003Ef__this.workDone / ((_003CMakeNewToils_003Ec__Iterator27)/*Error near IL_00cd: stateMachine*/)._003C_003Ef__this.TotalNeededWork), true, -0.5f);
			clearToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return clearToil;
		}
	}
}
