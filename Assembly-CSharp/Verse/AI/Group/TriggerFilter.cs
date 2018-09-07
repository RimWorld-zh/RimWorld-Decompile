using System;

namespace Verse.AI.Group
{
	public abstract class TriggerFilter
	{
		protected TriggerFilter()
		{
		}

		public abstract bool AllowActivation(Lord lord, TriggerSignal signal);
	}
}
