using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A3F RID: 2623
	public class JobDriver_HaulToContainer : JobDriver
	{
		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06003A3B RID: 14907 RVA: 0x001EE37C File Offset: 0x001EC77C
		private Thing ThingToCarry
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.A);
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06003A3C RID: 14908 RVA: 0x001EE3A4 File Offset: 0x001EC7A4
		private Thing Container
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.B);
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06003A3D RID: 14909 RVA: 0x001EE3CC File Offset: 0x001EC7CC
		private int Duration
		{
			get
			{
				return (this.Container == null || !(this.Container is Building)) ? 0 : this.Container.def.building.haulToContainerDuration;
			}
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x001EE418 File Offset: 0x001EC818
		public override string GetReport()
		{
			Thing thing;
			if (this.pawn.CurJob == this.job && this.pawn.carryTracker.CarriedThing != null)
			{
				thing = this.pawn.carryTracker.CarriedThing;
			}
			else
			{
				thing = base.TargetThingA;
			}
			string result;
			if (thing == null || !this.job.targetB.HasThing)
			{
				result = "ReportHaulingUnknown".Translate();
			}
			else
			{
				result = "ReportHaulingTo".Translate(new object[]
				{
					thing.Label,
					this.job.targetB.Thing.LabelShort
				});
			}
			return result;
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x001EE4D4 File Offset: 0x001EC8D4
		public override bool TryMakePreToilReservations()
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null);
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x001EE574 File Offset: 0x001EC974
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => TransporterUtility.WasLoadingCanceled(this.Container));
			this.FailOn(delegate()
			{
				ThingOwner thingOwner = this.Container.TryGetInnerInteractableThingOwner();
				bool result;
				if (thingOwner != null && !thingOwner.CanAcceptAnyOf(this.ThingToCarry, true))
				{
					result = true;
				}
				else
				{
					IHaulDestination haulDestination = this.Container as IHaulDestination;
					result = (haulDestination != null && !haulDestination.Accepts(this.ThingToCarry));
				}
				return result;
			});
			Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return getToHaulTarget;
			yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
			yield return Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(getToHaulTarget, TargetIndex.A);
			Toil carryToContainer = Toils_Haul.CarryHauledThingToContainer();
			yield return carryToContainer;
			yield return Toils_Goto.MoveOffTargetBlueprint(TargetIndex.B);
			Toil prepare = Toils_General.Wait(this.Duration);
			prepare.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return prepare;
			yield return Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.JumpToCarryToNextContainerIfPossible(carryToContainer, TargetIndex.C);
			yield break;
		}

		// Token: 0x04002511 RID: 9489
		private const TargetIndex CarryThingIndex = TargetIndex.A;

		// Token: 0x04002512 RID: 9490
		private const TargetIndex DestIndex = TargetIndex.B;

		// Token: 0x04002513 RID: 9491
		private const TargetIndex PrimaryDestIndex = TargetIndex.C;
	}
}
