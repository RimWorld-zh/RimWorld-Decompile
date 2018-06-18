using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AAA RID: 2730
	internal class Reservation : IExposable
	{
		// Token: 0x06003CE0 RID: 15584 RVA: 0x00202FE9 File Offset: 0x002013E9
		public Reservation()
		{
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x00202FF9 File Offset: 0x002013F9
		public Reservation(Pawn claimant, Job job, int maxPawns, int stackCount, LocalTargetInfo target, ReservationLayerDef layer)
		{
			this.claimant = claimant;
			this.job = job;
			this.maxPawns = maxPawns;
			this.stackCount = stackCount;
			this.target = target;
			this.layer = layer;
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003CE2 RID: 15586 RVA: 0x00203038 File Offset: 0x00201438
		public Pawn Claimant
		{
			get
			{
				return this.claimant;
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003CE3 RID: 15587 RVA: 0x00203054 File Offset: 0x00201454
		public Job Job
		{
			get
			{
				return this.job;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06003CE4 RID: 15588 RVA: 0x00203070 File Offset: 0x00201470
		public LocalTargetInfo Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x0020308C File Offset: 0x0020148C
		public ReservationLayerDef Layer
		{
			get
			{
				return this.layer;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003CE6 RID: 15590 RVA: 0x002030A8 File Offset: 0x002014A8
		public int MaxPawns
		{
			get
			{
				return this.maxPawns;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003CE7 RID: 15591 RVA: 0x002030C4 File Offset: 0x002014C4
		public int StackCount
		{
			get
			{
				return this.stackCount;
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06003CE8 RID: 15592 RVA: 0x002030E0 File Offset: 0x002014E0
		public Faction Faction
		{
			get
			{
				return this.claimant.Faction;
			}
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x00203100 File Offset: 0x00201500
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
			Scribe_References.Look<Job>(ref this.job, "job", false);
			Scribe_TargetInfo.Look(ref this.target, "target");
			Scribe_Values.Look<int>(ref this.maxPawns, "maxPawns", 0, false);
			Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
			Scribe_Defs.Look<ReservationLayerDef>(ref this.layer, "layer");
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x00203174 File Offset: 0x00201574
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

		// Token: 0x04002683 RID: 9859
		private Pawn claimant;

		// Token: 0x04002684 RID: 9860
		private Job job;

		// Token: 0x04002685 RID: 9861
		private LocalTargetInfo target;

		// Token: 0x04002686 RID: 9862
		private ReservationLayerDef layer;

		// Token: 0x04002687 RID: 9863
		private int maxPawns;

		// Token: 0x04002688 RID: 9864
		private int stackCount = -1;
	}
}
