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
				return base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Pawn Carrier
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		private List<TransferableOneWay> Transferables
		{
			get
			{
				return ((LordJob_FormAndSendCaravan)base.CurJob.lord.LordJob).transferables;
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

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator9)/*Error near IL_004b: stateMachine*/)._003C_003Ef__this.Map.lordManager.lords.Contains(((_003CMakeNewToils_003Ec__Iterator9)/*Error near IL_004b: stateMachine*/)._003C_003Ef__this.CurJob.lord)));
			Toil reserve = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return reserve;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return this.DetermineNumToHaul();
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true);
			yield return this.AddCarriedThingToTransferables();
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserve, TargetIndex.A, TargetIndex.None, true, (Predicate<Thing>)((Thing x) => ((_003CMakeNewToils_003Ec__Iterator9)/*Error near IL_00fb: stateMachine*/)._003C_003Ef__this.Transferable.things.Contains(x)));
			Toil findCarrier = this.FindCarrier();
			yield return findCarrier;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).JumpIf((Func<bool>)(() => !JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(((_003CMakeNewToils_003Ec__Iterator9)/*Error near IL_014e: stateMachine*/)._003C_003Ef__this.Carrier, ((_003CMakeNewToils_003Ec__Iterator9)/*Error near IL_014e: stateMachine*/)._003C_003Ef__this.pawn, true)), findCarrier);
			yield return Toils_General.Wait(25).JumpIf((Func<bool>)(() => !JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(((_003CMakeNewToils_003Ec__Iterator9)/*Error near IL_017e: stateMachine*/)._003C_003Ef__this.Carrier, ((_003CMakeNewToils_003Ec__Iterator9)/*Error near IL_017e: stateMachine*/)._003C_003Ef__this.pawn, true)), findCarrier).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return this.PlaceTargetInCarrierInventory();
		}

		private Toil DetermineNumToHaul()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate
			{
				int num = GatherItemsForCaravanUtility.CountLeftToTransfer(base.pawn, this.Transferable, base.CurJob.lord);
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
					base.CurJob.count = num;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		private Toil AddCarriedThingToTransferables()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate
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
			toil.initAction = (Action)delegate()
			{
				Pawn pawn = this.FindBestCarrier(true);
				if (pawn == null)
				{
					bool flag = base.pawn.GetLord() == base.CurJob.lord;
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
								IEnumerable<Pawn> source = from x in base.CurJob.lord.ownedPawns
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
				base.CurJob.SetTarget(TargetIndex.B, (Thing)pawn);
			};
			return toil;
		}

		private Toil PlaceTargetInCarrierInventory()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate
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
			if (!p.DestroyedOrNull() && p.Spawned && !p.inventory.UnloadEverything && forPawn.CanReach((Thing)p, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
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
			Lord lord = base.CurJob.lord;
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
