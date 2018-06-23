using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200004D RID: 77
	public class JobDriver_MarryAdjacentPawn : JobDriver
	{
		// Token: 0x040001DE RID: 478
		private int ticksLeftToMarry = 2500;

		// Token: 0x040001DF RID: 479
		private const TargetIndex OtherFianceInd = TargetIndex.A;

		// Token: 0x040001E0 RID: 480
		private const int Duration = 2500;

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600026A RID: 618 RVA: 0x000198E4 File Offset: 0x00017CE4
		private Pawn OtherFiance
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00019914 File Offset: 0x00017D14
		public int TicksLeftToMarry
		{
			get
			{
				return this.ticksLeftToMarry;
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00019930 File Offset: 0x00017D30
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00019948 File Offset: 0x00017D48
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => this.OtherFiance.Drafted || !this.pawn.Position.AdjacentTo8WayOrInside(this.OtherFiance));
			Toil marry = new Toil();
			marry.initAction = delegate()
			{
				this.ticksLeftToMarry = 2500;
			};
			marry.tickAction = delegate()
			{
				this.ticksLeftToMarry--;
				if (this.ticksLeftToMarry <= 0)
				{
					this.ticksLeftToMarry = 0;
					base.ReadyForNextToil();
				}
			};
			marry.defaultCompleteMode = ToilCompleteMode.Never;
			marry.FailOn(() => !this.pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.OtherFiance));
			yield return marry;
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate()
				{
					if (this.pawn.thingIDNumber < this.OtherFiance.thingIDNumber)
					{
						MarriageCeremonyUtility.Married(this.pawn, this.OtherFiance);
					}
				}
			};
			yield break;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00019972 File Offset: 0x00017D72
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToMarry, "ticksLeftToMarry", 0, false);
		}
	}
}
