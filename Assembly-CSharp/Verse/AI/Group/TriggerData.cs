using System;

namespace Verse.AI.Group
{
	public abstract class TriggerData : IExposable
	{
		protected TriggerData()
		{
		}

		public abstract void ExposeData();
	}
}
