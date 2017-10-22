using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_BuildSnowman : JobDriver
	{
		protected const int BaseWorkAmount = 2300;

		private float workLeft = -1000f;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator1A)/*Error near IL_006e: stateMachine*/)._003C_003Ef__this.workLeft = 2300f;
				},
				tickAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator1A)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.workLeft -= ((_003CMakeNewToils_003Ec__Iterator1A)/*Error near IL_0085: stateMachine*/)._003CdoWork_003E__0.actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
					if (((_003CMakeNewToils_003Ec__Iterator1A)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.workLeft <= 0.0)
					{
						Thing thing = ThingMaker.MakeThing(ThingDefOf.Snowman, null);
						thing.SetFaction(((_003CMakeNewToils_003Ec__Iterator1A)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.pawn.Faction, null);
						GenSpawn.Spawn(thing, ((_003CMakeNewToils_003Ec__Iterator1A)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.TargetLocA, ((_003CMakeNewToils_003Ec__Iterator1A)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.Map);
						((_003CMakeNewToils_003Ec__Iterator1A)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
					}
					else
					{
						JoyUtility.JoyTickCheckEnd(((_003CMakeNewToils_003Ec__Iterator1A)/*Error near IL_0085: stateMachine*/)._003C_003Ef__this.pawn, JoyTickFullJoyAction.EndJob, 1f);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			doWork.FailOn((Func<bool>)(() => !JoyUtility.EnjoyableOutsideNow(((_003CMakeNewToils_003Ec__Iterator1A)/*Error near IL_00a8: stateMachine*/)._003C_003Ef__this.pawn, null)));
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return doWork;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}
	}
}
