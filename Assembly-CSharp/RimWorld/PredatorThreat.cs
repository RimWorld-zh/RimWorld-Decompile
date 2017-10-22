using Verse;

namespace RimWorld
{
	public class PredatorThreat : IExposable
	{
		private const int ExpireAfterTicks = 600;

		public Pawn predator;

		public int lastAttackTicks;

		public bool Expired
		{
			get
			{
				if (!this.predator.Spawned)
				{
					return true;
				}
				return Find.TickManager.TicksGame >= this.lastAttackTicks + 600;
			}
		}

		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.predator, "predator", false);
			Scribe_Values.Look<int>(ref this.lastAttackTicks, "lastAttackTicks", 0, false);
		}
	}
}
