using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000050 RID: 80
	public class JobDriver_TendPatient : JobDriver
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0001A1E8 File Offset: 0x000185E8
		protected Thing MedicineUsed
		{
			get
			{
				return this.job.targetB.Thing;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0001A210 File Offset: 0x00018610
		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0001A23A File Offset: 0x0001863A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usesMedicine, "usesMedicine", false, false);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0001A255 File Offset: 0x00018655
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usesMedicine = (this.MedicineUsed != null);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0001A270 File Offset: 0x00018670
		public override bool TryMakePreToilReservations()
		{
			bool result;
			if (!this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null))
			{
				result = false;
			}
			else
			{
				if (this.usesMedicine)
				{
					int num = this.pawn.Map.reservationManager.CanReserveStack(this.pawn, this.MedicineUsed, 10, null, false);
					if (num <= 0 || !this.pawn.Reserve(this.MedicineUsed, this.job, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(this.Deliveree)), null))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0001A32C File Offset: 0x0001872C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(delegate()
			{
				bool result;
				if (!WorkGiver_Tend.GoodLayingStatusForTend(this.Deliveree, this.pawn))
				{
					result = true;
				}
				else
				{
					if (this.MedicineUsed != null && this.pawn.Faction == Faction.OfPlayer)
					{
						if (this.Deliveree.playerSettings == null)
						{
							return true;
						}
						if (!this.Deliveree.playerSettings.medCare.AllowsMedicine(this.MedicineUsed.def))
						{
							return true;
						}
					}
					result = (this.pawn == this.Deliveree && this.pawn.Faction == Faction.OfPlayer && !this.pawn.playerSettings.selfTend);
				}
				return result;
			});
			base.AddEndCondition(delegate
			{
				JobCondition result;
				if (this.pawn.Faction == Faction.OfPlayer && HealthAIUtility.ShouldBeTendedNowByPlayer(this.Deliveree))
				{
					result = JobCondition.Ongoing;
				}
				else if (this.pawn.Faction != Faction.OfPlayer && this.Deliveree.health.HasHediffsNeedingTend(false))
				{
					result = JobCondition.Ongoing;
				}
				else
				{
					result = JobCondition.Succeeded;
				}
				return result;
			});
			this.FailOnAggroMentalState(TargetIndex.A);
			Toil reserveMedicine = null;
			if (this.usesMedicine)
			{
				reserveMedicine = Toils_Tend.ReserveMedicine(TargetIndex.B, this.Deliveree).FailOnDespawnedNullOrForbidden(TargetIndex.B);
				yield return reserveMedicine;
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B);
				yield return Toils_Tend.PickupMedicine(TargetIndex.B, this.Deliveree).FailOnDestroyedOrNull(TargetIndex.B);
				yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveMedicine, TargetIndex.B, TargetIndex.None, true, null);
			}
			PathEndMode interactionCell = (this.Deliveree != this.pawn) ? PathEndMode.InteractionCell : PathEndMode.OnCell;
			Toil gotoToil = Toils_Goto.GotoThing(TargetIndex.A, interactionCell);
			yield return gotoToil;
			int duration = (int)(1f / this.pawn.GetStatValue(StatDefOf.MedicalTendSpeed, true) * 600f);
			yield return Toils_General.Wait(duration).FailOnCannotTouch(TargetIndex.A, interactionCell).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).PlaySustainerOrSound(SoundDefOf.Interact_Tend);
			yield return Toils_Tend.FinalizeTend(this.Deliveree);
			if (this.usesMedicine)
			{
				yield return new Toil
				{
					initAction = delegate()
					{
						if (this.MedicineUsed.DestroyedOrNull())
						{
							Thing thing = HealthAIUtility.FindBestMedicine(this.pawn, this.Deliveree);
							if (thing != null)
							{
								this.job.targetB = thing;
								this.JumpToToil(reserveMedicine);
							}
						}
					}
				};
			}
			yield return Toils_Jump.Jump(gotoToil);
			yield break;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0001A358 File Offset: 0x00018758
		public override void Notify_DamageTaken(DamageInfo dinfo)
		{
			base.Notify_DamageTaken(dinfo);
			if (dinfo.Def.externalViolence && this.pawn.Faction != Faction.OfPlayer && this.pawn == this.Deliveree)
			{
				this.pawn.jobs.CheckForJobOverride();
			}
		}

		// Token: 0x040001E4 RID: 484
		private bool usesMedicine;

		// Token: 0x040001E5 RID: 485
		private const int BaseTendDuration = 600;
	}
}
