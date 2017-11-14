using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobDriver_PrepareCaravan_GatherItems : JobDriver
	{
		private const TargetIndex ToHaulInd = TargetIndex.A;

		private const TargetIndex CarrierInd = TargetIndex.B;

		private const int PlaceInInventoryDuration = 25;

		public Thing ToHaul
		{
			get
			{
				return base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Pawn Carrier
		{
			get
			{
				return (Pawn)base.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		private List<TransferableOneWay> Transferables
		{
			get
			{
				return ((LordJob_FormAndSendCaravan)base.job.lord.LordJob).transferables;
			}
		}

		private TransferableOneWay Transferable
		{
			get
			{
				TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(this.ToHaul, this.Transferables);
				if (transferableOneWay != null)
				{
					return transferableOneWay;
				}
				throw new InvalidOperationException("Could not find any matching transferable.");
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(this.ToHaul, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004b: stateMachine*/)._0024this.Map.lordManager.lords.Contains(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004b: stateMachine*/)._0024this.job.lord));
			Toil reserve = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null).FailOnDespawnedOrNull(TargetIndex.A);
			yield return reserve;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private Toil DetermineNumToHaul()
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				int num = GatherItemsForCaravanUtility.CountLeftToTransfer(base.pawn, this.Transferable, base.job.lord);
				if (base.pawn.carryTracker.CarriedThing != null)
				{
					num -= base.pawn.carryTracker.CarriedThing.stackCount;
				}
				if (num <= 0)
				{
					base.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
				}
				else
				{
					base.job.count = num;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		private Toil AddCarriedThingToTransferables()
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				TransferableOneWay transferable = this.Transferable;
				if (!transferable.things.Contains(base.pawn.carryTracker.CarriedThing))
				{
					transferable.things.Add(base.pawn.carryTracker.CarriedThing);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		private Toil FindCarrier()
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn pawn = this.FindBestCarrier(true);
				if (pawn == null)
				{
					bool flag = base.pawn.GetLord() == base.job.lord;
					if (flag && !MassUtility.IsOverEncumbered(base.pawn))
					{
						pawn = base.pawn;
					}
					else
					{
						pawn = this.FindBestCarrier(false);
						if (pawn == null)
						{
							if (!flag)
							{
								IEnumerable<Pawn> source = from x in base.job.lord.ownedPawns
								where JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(x, base.pawn, true)
								select x;
								if (source.Any())
								{
									pawn = source.RandomElement();
									goto IL_00aa;
								}
								base.EndJobWith(JobCondition.Incompletable);
								return;
							}
							pawn = base.pawn;
						}
					}
				}
				goto IL_00aa;
				IL_00aa:
				base.job.SetTarget(TargetIndex.B, pawn);
			};
			return toil;
		}

		private Toil PlaceTargetInCarrierInventory()
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn_CarryTracker carryTracker = base.pawn.carryTracker;
				Thing carriedThing = carryTracker.CarriedThing;
				this.Transferable.AdjustTo(Mathf.Max(this.Transferable.CountToTransfer - carriedThing.stackCount, 0));
				carryTracker.innerContainer.TryTransferToContainer(carriedThing, this.Carrier.inventory.innerContainer, carriedThing.stackCount, true);
			};
			return toil;
		}

		public static bool IsUsableCarrier(Pawn p, Pawn forPawn, bool allowColonists)
		{
			if (p == forPawn)
			{
				return true;
			}
			int result;
			if (!p.DestroyedOrNull() && p.Spawned && !p.inventory.UnloadEverything && forPawn.CanReach(p, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				if (allowColonists && p.IsColonist)
				{
					return true;
				}
				if ((p.RaceProps.packAnimal || p.HostFaction == Faction.OfPlayer) && !p.IsBurning() && !p.Downed)
				{
					result = ((!MassUtility.IsOverEncumbered(p)) ? 1 : 0);
					goto IL_009b;
				}
				result = 0;
				goto IL_009b;
			}
			return false;
			IL_009b:
			return (byte)result != 0;
		}

		private float GetCarrierScore(Pawn p)
		{
			float lengthHorizontal = (p.Position - base.pawn.Position).LengthHorizontal;
			float num = MassUtility.EncumbrancePercent(p);
			float num2 = (float)(1.0 - num);
			return (float)(num2 - lengthHorizontal / 10.0 * 0.20000000298023224);
		}

		private Pawn FindBestCarrier(bool onlyAnimals)
		{
			Lord lord = base.job.lord;
			Pawn pawn = null;
			float num = 0f;
			if (lord != null)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = lord.ownedPawns[i];
					if (pawn2 != base.pawn && (!onlyAnimals || pawn2.RaceProps.Animal) && JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(pawn2, base.pawn, false))
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
			return pawn;
		}
	}
}
