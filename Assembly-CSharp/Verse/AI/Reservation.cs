using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AA6 RID: 2726
	internal class Reservation : IExposable
	{
		// Token: 0x06003CDB RID: 15579 RVA: 0x0020330D File Offset: 0x0020170D
		public Reservation()
		{
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x0020331D File Offset: 0x0020171D
		public Reservation(Pawn claimant, Job job, int maxPawns, int stackCount, LocalTargetInfo target, ReservationLayerDef layer)
		{
			this.claimant = claimant;
			this.job = job;
			this.maxPawns = maxPawns;
			this.stackCount = stackCount;
			this.target = target;
			this.layer = layer;
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003CDD RID: 15581 RVA: 0x0020335C File Offset: 0x0020175C
		public Pawn Claimant
		{
			get
			{
				return this.claimant;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06003CDE RID: 15582 RVA: 0x00203378 File Offset: 0x00201778
		public Job Job
		{
			get
			{
				return this.job;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003CDF RID: 15583 RVA: 0x00203394 File Offset: 0x00201794
		public LocalTargetInfo Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003CE0 RID: 15584 RVA: 0x002033B0 File Offset: 0x002017B0
		public ReservationLayerDef Layer
		{
			get
			{
				return this.layer;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003CE1 RID: 15585 RVA: 0x002033CC File Offset: 0x002017CC
		public int MaxPawns
		{
			get
			{
				return this.maxPawns;
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06003CE2 RID: 15586 RVA: 0x002033E8 File Offset: 0x002017E8
		public int StackCount
		{
			get
			{
				return this.stackCount;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003CE3 RID: 15587 RVA: 0x00203404 File Offset: 0x00201804
		public Faction Faction
		{
			get
			{
				return this.claimant.Faction;
			}
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x00203424 File Offset: 0x00201824
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
			Scribe_References.Look<Job>(ref this.job, "job", false);
			Scribe_TargetInfo.Look(ref this.target, "target");
			Scribe_Values.Look<int>(ref this.maxPawns, "maxPawns", 0, false);
			Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
			Scribe_Defs.Look<ReservationLayerDef>(ref this.layer, "layer");
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x00203498 File Offset: 0x00201898
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				(this.claimant == null) ? "null" : this.claimant.LabelShort,
				":",
				(this.job == null) ? "null" : this.job.ToString(),
				", ",
				this.target.ToString(),
				", ",
				(this.layer == null) ? "null" : this.layer.ToString(),
				", ",
				this.maxPawns,
				", ",
				this.stackCount
			});
		}

		// Token: 0x0400267E RID: 9854
		private Pawn claimant;

		// Token: 0x0400267F RID: 9855
		private Job job;

		// Token: 0x04002680 RID: 9856
		private LocalTargetInfo target;

		// Token: 0x04002681 RID: 9857
		private ReservationLayerDef layer;

		// Token: 0x04002682 RID: 9858
		private int maxPawns;

		// Token: 0x04002683 RID: 9859
		private int stackCount = -1;
	}
}
