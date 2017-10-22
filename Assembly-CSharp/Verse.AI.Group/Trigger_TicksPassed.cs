using UnityEngine;

namespace Verse.AI.Group
{
	public class Trigger_TicksPassed : Trigger
	{
		private int duration = 100;

		protected TriggerData_TicksPassed Data
		{
			get
			{
				return (TriggerData_TicksPassed)base.data;
			}
		}

		public int TicksLeft
		{
			get
			{
				return Mathf.Max(this.duration - this.Data.ticksPassed, 0);
			}
		}

		public Trigger_TicksPassed(int tickLimit)
		{
			base.data = new TriggerData_TicksPassed();
			this.duration = tickLimit;
		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.Tick)
			{
				TriggerData_TicksPassed data = this.Data;
				data.ticksPassed++;
				result = (data.ticksPassed > this.duration);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0;
			}
		}
	}
}
