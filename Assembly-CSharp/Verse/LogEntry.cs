using RimWorld;
using UnityEngine;

namespace Verse
{
	public abstract class LogEntry : IExposable
	{
		protected int ticksAbs = -1;

		protected int randSeed = 0;

		public int Age
		{
			get
			{
				return Find.TickManager.TicksAbs - this.ticksAbs;
			}
		}

		public virtual Texture2D Icon
		{
			get
			{
				return null;
			}
		}

		public LogEntry()
		{
			this.ticksAbs = Find.TickManager.TicksAbs;
			this.randSeed = Rand.Int;
		}

		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksAbs, "ticksAbs", 0, false);
			Scribe_Values.Look<int>(ref this.randSeed, "randSeed", 0, false);
		}

		public abstract string ToGameStringFromPOV(Thing pov);

		public abstract bool Concerns(Thing t);

		public virtual void ClickedFromPOV(Thing pov)
		{
		}

		public virtual string GetTipString()
		{
			return "OccurredTimeAgo".Translate(this.Age.ToStringTicksToPeriod(true, false, true)).CapitalizeFirst() + ".";
		}

		public void Debug_OverrideTicks(int newTicks)
		{
			this.ticksAbs = newTicks;
		}
	}
}
