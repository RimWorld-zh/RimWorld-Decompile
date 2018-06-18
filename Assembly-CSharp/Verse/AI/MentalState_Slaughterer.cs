using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A77 RID: 2679
	public class MentalState_Slaughterer : MentalState
	{
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06003B87 RID: 15239 RVA: 0x001F76E8 File Offset: 0x001F5AE8
		public bool SlaughteredRecently
		{
			get
			{
				return this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < 3750;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06003B88 RID: 15240 RVA: 0x001F7724 File Offset: 0x001F5B24
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.lastSlaughterTicks >= 0;
			}
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x001F7745 File Offset: 0x001F5B45
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSlaughterTicks, "lastSlaughterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.animalsSlaughtered, "animalsSlaughtered", 0, false);
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x001F7774 File Offset: 0x001F5B74
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(600) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.Slaughter) && SlaughtererMentalStateUtility.FindAnimal(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x001F77DF File Offset: 0x001F5BDF
		public override void Notify_SlaughteredAnimal()
		{
			this.lastSlaughterTicks = Find.TickManager.TicksGame;
			this.animalsSlaughtered++;
			if (this.animalsSlaughtered >= 4)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x04002574 RID: 9588
		private int lastSlaughterTicks = -1;

		// Token: 0x04002575 RID: 9589
		private int animalsSlaughtered;

		// Token: 0x04002576 RID: 9590
		private const int NoAnimalToSlaughterCheckInterval = 600;

		// Token: 0x04002577 RID: 9591
		private const int MinTicksBetweenSlaughter = 3750;

		// Token: 0x04002578 RID: 9592
		private const int MaxAnimalsSlaughtered = 4;
	}
}
