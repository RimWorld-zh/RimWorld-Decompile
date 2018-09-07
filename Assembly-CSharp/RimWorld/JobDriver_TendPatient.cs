using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_TendPatient : JobDriver
	{
		private bool usesMedicine;

		private const int BaseTendDuration = 600;

		public JobDriver_TendPatient()
		{
		}

		protected Thing MedicineUsed
		{
			get
			{
				return this.job.targetB.Thing;
			}
		}

		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
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

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.Deliveree;
			Job job = this.job;
			if (!pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (this.usesMedicine)
			{
				int num = this.pawn.Map.reservationManager.CanReserveStack(this.pawn, this.MedicineUsed, 10, null, false);
				if (num > 0)
				{
					pawn = this.pawn;
					target = this.MedicineUsed;
					job = this.job;
					int maxPawns = 10;
					int stackCount = Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(this.Deliveree));
					if (pawn.Reserve(target, job, maxPawns, stackCount, null, errorOnFailed))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(delegate()
			{
				if (!WorkGiver_Tend.GoodLayingStatusForTend(this.Deliveree, this.pawn))
				{
					return true;
				}
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
				return this.pawn == this.Deliveree && this.pawn.Faction == Faction.OfPlayer && !this.pawn.playerSettings.selfTend;
			});
			base.AddEndCondition(delegate
			{
				if (this.pawn.Faction == Faction.OfPlayer && HealthAIUtility.ShouldBeTendedNowByPlayer(this.Deliveree))
				{
					return JobCondition.Ongoing;
				}
				if (this.pawn.Faction != Faction.OfPlayer && this.Deliveree.health.HasHediffsNeedingTend(false))
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
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
			yield return Toils_General.Wait(duration, TargetIndex.None).FailOnCannotTouch(TargetIndex.A, interactionCell).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).PlaySustainerOrSound(SoundDefOf.Interact_Tend);
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

		public override void Notify_DamageTaken(DamageInfo dinfo)
		{
			base.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.Faction != Faction.OfPlayer && this.pawn == this.Deliveree)
			{
				this.pawn.jobs.CheckForJobOverride();
			}
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal PathEndMode <interactionCell>__0;

			internal Toil <gotoToil>__0;

			internal int <duration>__0;

			internal Toil <tryToFindMoreMedicine>__1;

			internal JobDriver_TendPatient $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_TendPatient.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
				{
					this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					this.FailOn(delegate()
					{
						if (!WorkGiver_Tend.GoodLayingStatusForTend(this.Deliveree, this.pawn))
						{
							return true;
						}
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
						return this.pawn == this.Deliveree && this.pawn.Faction == Faction.OfPlayer && !this.pawn.playerSettings.selfTend;
					});
					base.AddEndCondition(delegate
					{
						if (this.pawn.Faction == Faction.OfPlayer && HealthAIUtility.ShouldBeTendedNowByPlayer(this.Deliveree))
						{
							return JobCondition.Ongoing;
						}
						if (this.pawn.Faction != Faction.OfPlayer && this.Deliveree.health.HasHediffsNeedingTend(false))
						{
							return JobCondition.Ongoing;
						}
						return JobCondition.Succeeded;
					});
					this.FailOnAggroMentalState(TargetIndex.A);
					Toil reserveMedicine = null;
					if (this.usesMedicine)
					{
						reserveMedicine = Toils_Tend.ReserveMedicine(TargetIndex.B, base.Deliveree).FailOnDespawnedNullOrForbidden(TargetIndex.B);
						this.$current = reserveMedicine;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				}
				case 1u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_Tend.PickupMedicine(TargetIndex.B, base.Deliveree).FailOnDestroyedOrNull(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_Haul.CheckForGetOpportunityDuplicate(<MakeNewToils>c__AnonStorey.reserveMedicine, TargetIndex.B, TargetIndex.None, true, null);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					break;
				case 5u:
					duration = (int)(1f / this.pawn.GetStatValue(StatDefOf.MedicalTendSpeed, true) * 600f);
					this.$current = Toils_General.Wait(duration, TargetIndex.None).FailOnCannotTouch(TargetIndex.A, interactionCell).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).PlaySustainerOrSound(SoundDefOf.Interact_Tend);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = Toils_Tend.FinalizeTend(base.Deliveree);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					if (this.usesMedicine)
					{
						Toil tryToFindMoreMedicine = new Toil();
						tryToFindMoreMedicine.initAction = delegate()
						{
							if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.MedicineUsed.DestroyedOrNull())
							{
								Thing thing = HealthAIUtility.FindBestMedicine(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Deliveree);
								if (thing != null)
								{
									<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.targetB = thing;
									<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.JumpToToil(<MakeNewToils>c__AnonStorey.reserveMedicine);
								}
							}
						};
						this.$current = tryToFindMoreMedicine;
						if (!this.$disposing)
						{
							this.$PC = 8;
						}
						return true;
					}
					goto IL_2E0;
				case 8u:
					goto IL_2E0;
				case 9u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				interactionCell = ((base.Deliveree != this.pawn) ? PathEndMode.InteractionCell : PathEndMode.OnCell);
				gotoToil = Toils_Goto.GotoThing(TargetIndex.A, interactionCell);
				this.$current = gotoToil;
				if (!this.$disposing)
				{
					this.$PC = 5;
				}
				return true;
				IL_2E0:
				this.$current = Toils_Jump.Jump(gotoToil);
				if (!this.$disposing)
				{
					this.$PC = 9;
				}
				return true;
			}

			Toil IEnumerator<Toil>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.AI.Toil>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JobDriver_TendPatient.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_TendPatient.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil reserveMedicine;

				internal JobDriver_TendPatient.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal bool <>m__0()
				{
					if (!WorkGiver_Tend.GoodLayingStatusForTend(this.<>f__ref$0.$this.Deliveree, this.<>f__ref$0.$this.pawn))
					{
						return true;
					}
					if (this.<>f__ref$0.$this.MedicineUsed != null && this.<>f__ref$0.$this.pawn.Faction == Faction.OfPlayer)
					{
						if (this.<>f__ref$0.$this.Deliveree.playerSettings == null)
						{
							return true;
						}
						if (!this.<>f__ref$0.$this.Deliveree.playerSettings.medCare.AllowsMedicine(this.<>f__ref$0.$this.MedicineUsed.def))
						{
							return true;
						}
					}
					return this.<>f__ref$0.$this.pawn == this.<>f__ref$0.$this.Deliveree && this.<>f__ref$0.$this.pawn.Faction == Faction.OfPlayer && !this.<>f__ref$0.$this.pawn.playerSettings.selfTend;
				}

				internal JobCondition <>m__1()
				{
					if (this.<>f__ref$0.$this.pawn.Faction == Faction.OfPlayer && HealthAIUtility.ShouldBeTendedNowByPlayer(this.<>f__ref$0.$this.Deliveree))
					{
						return JobCondition.Ongoing;
					}
					if (this.<>f__ref$0.$this.pawn.Faction != Faction.OfPlayer && this.<>f__ref$0.$this.Deliveree.health.HasHediffsNeedingTend(false))
					{
						return JobCondition.Ongoing;
					}
					return JobCondition.Succeeded;
				}

				internal void <>m__2()
				{
					if (this.<>f__ref$0.$this.MedicineUsed.DestroyedOrNull())
					{
						Thing thing = HealthAIUtility.FindBestMedicine(this.<>f__ref$0.$this.pawn, this.<>f__ref$0.$this.Deliveree);
						if (thing != null)
						{
							this.<>f__ref$0.$this.job.targetB = thing;
							this.<>f__ref$0.$this.JumpToToil(this.reserveMedicine);
						}
					}
				}
			}
		}
	}
}
