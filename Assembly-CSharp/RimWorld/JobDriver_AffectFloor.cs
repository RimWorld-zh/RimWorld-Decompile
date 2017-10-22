using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_AffectFloor : JobDriver
	{
		private float workLeft = -1000f;

		protected bool clearSnow;

		protected abstract int BaseWorkAmount
		{
			get;
		}

		protected abstract DesignationDef DesDef
		{
			get;
		}

		protected virtual StatDef SpeedStat
		{
			get
			{
				return null;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn((Func<bool>)delegate
			{
				if (!((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_002f: stateMachine*/)._003C_003Ef__this.CurJob.ignoreDesignations && ((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_002f: stateMachine*/)._003C_003Ef__this.Map.designationManager.DesignationAt(((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_002f: stateMachine*/)._003C_003Ef__this.TargetLocA, ((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_002f: stateMachine*/)._003C_003Ef__this.DesDef) == null)
				{
					return true;
				}
				return false;
			});
			ReservationLayerDef floor = ReservationLayerDefOf.Floor;
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, floor);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_008c: stateMachine*/)._003C_003Ef__this.workLeft = (float)((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_008c: stateMachine*/)._003C_003Ef__this.BaseWorkAmount;
				},
				tickAction = (Action)delegate
				{
					float num = (float)((((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.SpeedStat == null) ? 1.0 : ((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003CdoWork_003E__0.actor.GetStatValue(((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.SpeedStat, true));
					((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.workLeft -= num;
					if (((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003CdoWork_003E__0.actor.skills != null)
					{
						((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003CdoWork_003E__0.actor.skills.Learn(SkillDefOf.Construction, 0.22f, false);
					}
					if (((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.clearSnow)
					{
						((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.Map.snowGrid.SetDepth(((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.TargetLocA, 0f);
					}
					if (((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.workLeft <= 0.0)
					{
						((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.DoEffect(((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.TargetLocA);
						Designation designation = ((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.Map.designationManager.DesignationAt(((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.TargetLocA, ((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.DesDef);
						if (designation != null)
						{
							designation.Delete();
						}
						((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00a3: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
					}
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.WithProgressBar(TargetIndex.A, (Func<float>)(() => (float)(1.0 - ((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00c9: stateMachine*/)._003C_003Ef__this.workLeft / (float)((_003CMakeNewToils_003Ec__IteratorB)/*Error near IL_00c9: stateMachine*/)._003C_003Ef__this.BaseWorkAmount)), false, -0.5f);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			yield return doWork;
		}

		protected abstract void DoEffect(IntVec3 c);

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}
	}
}
