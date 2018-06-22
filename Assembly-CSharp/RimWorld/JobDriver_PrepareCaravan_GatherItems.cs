using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200003A RID: 58
	public class JobDriver_PrepareCaravan_GatherItems : JobDriver
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0001508C File Offset: 0x0001348C
		public Thing ToHaul
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000150B8 File Offset: 0x000134B8
		public Pawn Carrier
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x000150E8 File Offset: 0x000134E8
		private List<TransferableOneWay> Transferables
		{
			get
			{
				return ((LordJob_FormAndSendCaravan)this.job.lord.LordJob).transferables;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00015118 File Offset: 0x00013518
		private TransferableOneWay Transferable
		{
			get
			{
				TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(this.ToHaul, this.Transferables, TransferAsOneMode.PodsOrCaravanPacking);
				if (transferableOneWay != null)
				{
					return transferableOneWay;
				}
				throw new InvalidOperationException("Could not find any matching transferable.");
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00015154 File Offset: 0x00013554
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.ToHaul, this.job, 1, -1, null);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00015188 File Offset: 0x00013588
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.lordManager.lords.Contains(this.job.lord));
			Toil reserve = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null).FailOnDespawnedOrNull(TargetIndex.A);
			yield return reserve;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return this.DetermineNumToHaul();
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
			yield return this.AddCarriedThingToTransferables();
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserve, TargetIndex.A, TargetIndex.None, true, (Thing x) => this.Transferable.things.Contains(x));
			Toil findCarrier = this.FindCarrier();
			yield return findCarrier;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).JumpIf(() => !JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(this.Carrier, this.pawn, true), findCarrier);
			yield return Toils_General.Wait(25).JumpIf(() => !JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(this.Carrier, this.pawn, true), findCarrier).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return this.PlaceTargetInCarrierInventory();
			yield break;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000151B4 File Offset: 0x000135B4
		private Toil DetermineNumToHaul()
		{
			return new Toil
			{
				initAction = delegate()
				{
					int num = GatherItemsForCaravanUtility.CountLeftToTransfer(this.pawn, this.Transferable, this.job.lord);
					if (this.pawn.carryTracker.CarriedThing != null)
					{
						num -= this.pawn.carryTracker.CarriedThing.stackCount;
					}
					if (num <= 0)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
					}
					else
					{
						this.job.count = num;
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant,
				atomicWithPrevious = true
			};
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000151F0 File Offset: 0x000135F0
		private Toil AddCarriedThingToTransferables()
		{
			return new Toil
			{
				initAction = delegate()
				{
					TransferableOneWay transferable = this.Transferable;
					if (!transferable.things.Contains(this.pawn.carryTracker.CarriedThing))
					{
						transferable.things.Add(this.pawn.carryTracker.CarriedThing);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant,
				atomicWithPrevious = true
			};
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0001522C File Offset: 0x0001362C
		private Toil FindCarrier()
		{
			return new Toil
			{
				initAction = delegate()
				{
					Pawn pawn = this.FindBestCarrier(true);
					if (pawn == null)
					{
						bool flag = this.pawn.GetLord() == this.job.lord;
						if (flag && !MassUtility.IsOverEncumbered(this.pawn))
						{
							pawn = this.pawn;
						}
						else
						{
							pawn = this.FindBestCarrier(false);
							if (pawn == null)
							{
								if (flag)
								{
									pawn = this.pawn;
								}
								else
								{
									IEnumerable<Pawn> source = from x in this.job.lord.ownedPawns
									where JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(x, this.pawn, true)
									select x;
									if (!source.Any<Pawn>())
									{
										base.EndJobWith(JobCondition.Incompletable);
										return;
									}
									pawn = source.RandomElement<Pawn>();
								}
							}
						}
					}
					this.job.SetTarget(TargetIndex.B, pawn);
				}
			};
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0001525C File Offset: 0x0001365C
		private Toil PlaceTargetInCarrierInventory()
		{
			return new Toil
			{
				initAction = delegate()
				{
					Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
					Thing carriedThing = carryTracker.CarriedThing;
					this.Transferable.AdjustTo(Mathf.Max(this.Transferable.CountToTransfer - carriedThing.stackCount, 0));
					carryTracker.innerContainer.TryTransferToContainer(carriedThing, this.Carrier.inventory.innerContainer, carriedThing.stackCount, true);
				}
			};
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0001528C File Offset: 0x0001368C
		public static bool IsUsableCarrier(Pawn p, Pawn forPawn, bool allowColonists)
		{
			return p.IsFormingCaravan() && (p == forPawn || (!p.DestroyedOrNull() && p.Spawned && !p.inventory.UnloadEverything && forPawn.CanReach(p, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) && ((allowColonists && p.IsColonist) || ((p.RaceProps.packAnimal || p.HostFaction == Faction.OfPlayer) && !p.IsBurning() && !p.Downed && !MassUtility.IsOverEncumbered(p)))));
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00015360 File Offset: 0x00013760
		private float GetCarrierScore(Pawn p)
		{
			float lengthHorizontal = (p.Position - this.pawn.Position).LengthHorizontal;
			float num = MassUtility.EncumbrancePercent(p);
			float num2 = 1f - num;
			return num2 - lengthHorizontal / 10f * 0.2f;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x000153B4 File Offset: 0x000137B4
		private Pawn FindBestCarrier(bool onlyAnimals)
		{
			Lord lord = this.job.lord;
			Pawn pawn = null;
			float num = 0f;
			if (lord != null)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = lord.ownedPawns[i];
					if (pawn2 != this.pawn)
					{
						if (!onlyAnimals || pawn2.RaceProps.Animal)
						{
							if (JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(pawn2, this.pawn, false))
							{
								float carrierScore = this.GetCarrierScore(pawn2);
								if (pawn == null || carrierScore > num)
								{
									pawn = pawn2;
									num = carrierScore;
								}
							}
						}
					}
				}
			}
			return pawn;
		}

		// Token: 0x040001C6 RID: 454
		private const TargetIndex ToHaulInd = TargetIndex.A;

		// Token: 0x040001C7 RID: 455
		private const TargetIndex CarrierInd = TargetIndex.B;

		// Token: 0x040001C8 RID: 456
		private const int PlaceInInventoryDuration = 25;
	}
}
