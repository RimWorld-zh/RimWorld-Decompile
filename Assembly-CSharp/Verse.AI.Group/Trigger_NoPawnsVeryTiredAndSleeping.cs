using RimWorld;

namespace Verse.AI.Group
{
	public class Trigger_NoPawnsVeryTiredAndSleeping : Trigger
	{
		private float extraRestThreshOffset;

		public Trigger_NoPawnsVeryTiredAndSleeping(float extraRestThreshOffset = 0f)
		{
			this.extraRestThreshOffset = extraRestThreshOffset;
		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Need_Rest rest = lord.ownedPawns[i].needs.rest;
					if (rest != null && rest.CurLevelPercentage < 0.14000000059604645 + this.extraRestThreshOffset && !lord.ownedPawns[i].Awake())
						goto IL_0066;
				}
				result = true;
			}
			else
			{
				result = false;
			}
			goto IL_0091;
			IL_0091:
			return result;
			IL_0066:
			result = false;
			goto IL_0091;
		}
	}
}
