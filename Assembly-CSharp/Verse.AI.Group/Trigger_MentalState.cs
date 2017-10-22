namespace Verse.AI.Group
{
	public class Trigger_MentalState : Trigger
	{
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
						goto IL_002d;
				}
			}
			bool result = false;
			goto IL_0052;
			IL_002d:
			result = true;
			goto IL_0052;
			IL_0052:
			return result;
		}
	}
}
