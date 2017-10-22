using System;

namespace Verse.AI.Group
{
	public class Trigger_TickCondition : Trigger
	{
		private Func<bool> condition;

		public Trigger_TickCondition(Func<bool> condition)
		{
			this.condition = condition;
		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				return this.condition();
			}
			return false;
		}
	}
}
