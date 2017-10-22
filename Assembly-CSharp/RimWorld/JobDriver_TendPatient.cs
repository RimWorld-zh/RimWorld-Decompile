using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_TendPatient : JobDriver
	{
		private const int BaseTendDuration = 600;

		private bool usesMedicine;

		protected Thing MedicineUsed
		{
			get
			{
				return base.CurJob.targetB.Thing;
			}
		}

		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)base.CurJob.targetA.Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usesMedicine, "usesMedicine", false, false);
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usesMedicine = (this.MedicineUsed != null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn((Func<bool>)delegate
			{
				if (!WorkGiver_Tend.GoodLayingStatusForTend(((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0058: stateMachine*/)._003C_003Ef__this.Deliveree, ((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0058: stateMachine*/)._003C_003Ef__this.pawn))
				{
					return true;
				}
				if (((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0058: stateMachine*/)._003C_003Ef__this.MedicineUsed != null)
				{
					if (((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0058: stateMachine*/)._003C_003Ef__this.Deliveree.playerSettings == null)
					{
						return true;
					}
					if (!((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0058: stateMachine*/)._003C_003Ef__this.Deliveree.playerSettings.medCare.AllowsMedicine(((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0058: stateMachine*/)._003C_003Ef__this.MedicineUsed.def))
					{
						return true;
					}
				}
				if (((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0058: stateMachine*/)._003C_003Ef__this.pawn == ((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0058: stateMachine*/)._003C_003Ef__this.Deliveree && (((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0058: stateMachine*/)._003C_003Ef__this.pawn.playerSettings == null || !((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0058: stateMachine*/)._003C_003Ef__this.pawn.playerSettings.selfTend))
				{
					return true;
				}
				return false;
			});
			this.AddEndCondition((Func<JobCondition>)delegate
			{
				if (HealthAIUtility.ShouldBeTendedNow(((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0070: stateMachine*/)._003C_003Ef__this.Deliveree))
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			if (this.usesMedicine)
			{
				Toil reserveMedicine = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null).FailOnDespawnedNullOrForbidden(TargetIndex.B);
				yield return reserveMedicine;
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B);
				yield return Toils_Tend.PickupMedicine(TargetIndex.B, this.Deliveree).FailOnDestroyedOrNull(TargetIndex.B);
				yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveMedicine, TargetIndex.B, TargetIndex.None, true, null);
			}
			PathEndMode interactionCell = (PathEndMode)((this.Deliveree == base.pawn) ? 1 : 4);
			Toil gotoToil = Toils_Goto.GotoThing(TargetIndex.A, interactionCell);
			yield return gotoToil;
			int duration = (int)(1.0 / base.pawn.GetStatValue(StatDefOf.MedicalTendSpeed, true) * 600.0);
			yield return Toils_General.Wait(duration).FailOnCannotTouch(TargetIndex.A, interactionCell).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).PlaySustainerOrSound(SoundDefOf.Interact_Tend);
			yield return Toils_Tend.FinalizeTend(this.Deliveree);
			if (this.usesMedicine)
			{
				yield return new Toil
				{
					initAction = (Action)delegate
					{
						if (((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0253: stateMachine*/)._003C_003Ef__this.MedicineUsed.DestroyedOrNull() && Medicine.GetMedicineCountToFullyHeal(((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0253: stateMachine*/)._003C_003Ef__this.Deliveree) > 0)
						{
							Thing thing = HealthAIUtility.FindBestMedicine(((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0253: stateMachine*/)._003C_003Ef__this.pawn, ((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0253: stateMachine*/)._003C_003Ef__this.Deliveree);
							if (thing != null)
							{
								((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0253: stateMachine*/)._003C_003Ef__this.CurJob.targetB = thing;
								((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0253: stateMachine*/)._003C_003Ef__this.JumpToToil(((_003CMakeNewToils_003Ec__Iterator19)/*Error near IL_0253: stateMachine*/)._003CreserveMedicine_003E__0);
							}
						}
					}
				};
			}
			yield return Toils_Jump.Jump(gotoToil);
		}
	}
}
