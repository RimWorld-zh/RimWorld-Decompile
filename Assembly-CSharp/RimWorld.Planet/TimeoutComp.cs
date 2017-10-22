using Verse;

namespace RimWorld.Planet
{
	public class TimeoutComp : WorldObjectComp
	{
		private int timeoutEndTick = -1;

		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		public int TicksLeft
		{
			get
			{
				return this.Active ? (this.timeoutEndTick - Find.TickManager.TicksGame) : 0;
			}
		}

		public void StartTimeout(int ticks)
		{
			this.timeoutEndTick = Find.TickManager.TicksGame + ticks;
		}

		public override void CompTick()
		{
			base.CompTick();
			if (this.ShouldRemoveWorldObjectNow)
			{
				Find.WorldObjects.Remove(base.parent);
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timeoutEndTick, "timeoutEndTick", 0, false);
		}

		public override string CompInspectStringExtra()
		{
			return (!this.Active || base.ParentHasMap) ? null : "WorldObjectTimeout".Translate(this.TicksLeft.ToStringTicksToPeriod(false, false, false));
		}
	}
}
