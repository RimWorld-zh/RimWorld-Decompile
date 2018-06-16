using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A77 RID: 2679
	public class MentalState_Slaughterer : MentalState
	{
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06003B85 RID: 15237 RVA: 0x001F7614 File Offset: 0x001F5A14
		public bool SlaughteredRecently
		{
			get
			{
				return this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < 3750;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06003B86 RID: 15238 RVA: 0x001F7650 File Offset: 0x001F5A50
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.lastSlaughterTicks >= 0;
			}
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x001F7671 File Offset: 0x001F5A71
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSlaughterTicks, "lastSlaughterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.animalsSlaughtered, "animalsSlaughtered", 0, false);
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x001F76A0 File Offset: 0x001F5AA0
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(600) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.Slaughter) && SlaughtererMentalStateUtility.FindAnimal(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x001F770B File Offset: 0x001F5B0B
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
