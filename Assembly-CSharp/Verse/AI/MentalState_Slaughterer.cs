using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A75 RID: 2677
	public class MentalState_Slaughterer : MentalState
	{
		// Token: 0x04002570 RID: 9584
		private int lastSlaughterTicks = -1;

		// Token: 0x04002571 RID: 9585
		private int animalsSlaughtered;

		// Token: 0x04002572 RID: 9586
		private const int NoAnimalToSlaughterCheckInterval = 600;

		// Token: 0x04002573 RID: 9587
		private const int MinTicksBetweenSlaughter = 3750;

		// Token: 0x04002574 RID: 9588
		private const int MaxAnimalsSlaughtered = 4;

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06003B86 RID: 15238 RVA: 0x001F7B28 File Offset: 0x001F5F28
		public bool SlaughteredRecently
		{
			get
			{
				return this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < 3750;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06003B87 RID: 15239 RVA: 0x001F7B64 File Offset: 0x001F5F64
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.lastSlaughterTicks >= 0;
			}
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x001F7B85 File Offset: 0x001F5F85
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSlaughterTicks, "lastSlaughterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.animalsSlaughtered, "animalsSlaughtered", 0, false);
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x001F7BB4 File Offset: 0x001F5FB4
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(600) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.Slaughter) && SlaughtererMentalStateUtility.FindAnimal(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x001F7C1F File Offset: 0x001F601F
		public override void Notify_SlaughteredAnimal()
		{
			this.lastSlaughterTicks = Find.TickManager.TicksGame;
			this.animalsSlaughtered++;
			if (this.animalsSlaughtered >= 4)
			{
				base.RecoverFromState();
			}
		}
	}
}
