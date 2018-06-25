using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AA9 RID: 2729
	internal class Reservation : IExposable
	{
		// Token: 0x04002686 RID: 9862
		private Pawn claimant;

		// Token: 0x04002687 RID: 9863
		private Job job;

		// Token: 0x04002688 RID: 9864
		private LocalTargetInfo target;

		// Token: 0x04002689 RID: 9865
		private ReservationLayerDef layer;

		// Token: 0x0400268A RID: 9866
		private int maxPawns;

		// Token: 0x0400268B RID: 9867
		private int stackCount = -1;

		// Token: 0x06003CDF RID: 15583 RVA: 0x00203719 File Offset: 0x00201B19
		public Reservation()
		{
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x00203729 File Offset: 0x00201B29
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
		// (get) Token: 0x06003CE1 RID: 15585 RVA: 0x00203768 File Offset: 0x00201B68
		public Pawn Claimant
		{
			get
			{
				return this.claimant;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06003CE2 RID: 15586 RVA: 0x00203784 File Offset: 0x00201B84
		public Job Job
		{
			get
			{
				return this.job;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003CE3 RID: 15587 RVA: 0x002037A0 File Offset: 0x00201BA0
		public LocalTargetInfo Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003CE4 RID: 15588 RVA: 0x002037BC File Offset: 0x00201BBC
		public ReservationLayerDef Layer
		{
			get
			{
				return this.layer;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x002037D8 File Offset: 0x00201BD8
		public int MaxPawns
		{
			get
			{
				return this.maxPawns;
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06003CE6 RID: 15590 RVA: 0x002037F4 File Offset: 0x00201BF4
		public int StackCount
		{
			get
			{
				return this.stackCount;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003CE7 RID: 15591 RVA: 0x00203810 File Offset: 0x00201C10
		public Faction Faction
		{
			get
			{
				return this.claimant.Faction;
			}
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x00203830 File Offset: 0x00201C30
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
			Scribe_References.Look<Job>(ref this.job, "job", false);
			Scribe_TargetInfo.Look(ref this.target, "target");
			Scribe_Values.Look<int>(ref this.maxPawns, "maxPawns", 0, false);
			Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
			Scribe_Defs.Look<ReservationLayerDef>(ref this.layer, "layer");
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x002038A4 File Offset: 0x00201CA4
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
	}
}
