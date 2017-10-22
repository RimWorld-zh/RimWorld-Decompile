using RimWorld;

namespace Verse.AI
{
	public class MentalState_Slaughterer : MentalState
	{
		private int lastSlaughterTicks = -1;

		private int animalsSlaughtered;

		private const int NoAnimalToSlaughterCheckInterval = 600;

		private const int MinTicksBetweenSlaughter = 3750;

		private const int MaxAnimalsSlaughtered = 4;

		public bool SlaughteredRecently
		{
			get
			{
				return this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < 3750;
			}
		}

		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.lastSlaughterTicks >= 0;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSlaughterTicks, "lastSlaughterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.animalsSlaughtered, "animalsSlaughtered", 0, false);
		}

		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (base.pawn.IsHashIntervalTick(600))
			{
				if (base.pawn.CurJob != null && base.pawn.CurJob.def == JobDefOf.Slaughter)
					return;
				if (SlaughtererMentalStateUtility.FindAnimal(base.pawn) == null)
				{
					base.RecoverFromState();
				}
			}
		}

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
