using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A73 RID: 2675
	public class MentalState_Slaughterer : MentalState
	{
		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06003B82 RID: 15234 RVA: 0x001F79FC File Offset: 0x001F5DFC
		public bool SlaughteredRecently
		{
			get
			{
				return this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < 3750;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06003B83 RID: 15235 RVA: 0x001F7A38 File Offset: 0x001F5E38
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.lastSlaughterTicks >= 0;
			}
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x001F7A59 File Offset: 0x001F5E59
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSlaughterTicks, "lastSlaughterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.animalsSlaughtered, "animalsSlaughtered", 0, false);
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x001F7A88 File Offset: 0x001F5E88
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(600) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.Slaughter) && SlaughtererMentalStateUtility.FindAnimal(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x001F7AF3 File Offset: 0x001F5EF3
		public override void Notify_SlaughteredAnimal()
		{
			this.lastSlaughterTicks = Find.TickManager.TicksGame;
			this.animalsSlaughtered++;
			if (this.animalsSlaughtered >= 4)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x0400256F RID: 9583
		private int lastSlaughterTicks = -1;

		// Token: 0x04002570 RID: 9584
		private int animalsSlaughtered;

		// Token: 0x04002571 RID: 9585
		private const int NoAnimalToSlaughterCheckInterval = 600;

		// Token: 0x04002572 RID: 9586
		private const int MinTicksBetweenSlaughter = 3750;

		// Token: 0x04002573 RID: 9587
		private const int MaxAnimalsSlaughtered = 4;
	}
}
