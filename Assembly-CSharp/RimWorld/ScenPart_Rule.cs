using System;

namespace RimWorld
{
	public abstract class ScenPart_Rule : ScenPart
	{
		protected ScenPart_Rule()
		{
		}

		public override void PostGameStart()
		{
			this.ApplyRule();
		}

		protected abstract void ApplyRule();
	}
}
