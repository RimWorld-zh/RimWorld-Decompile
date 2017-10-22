namespace Verse.AI.Group
{
	public class Trigger_NoMentalState : Trigger
	{
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
						goto IL_002d;
				}
				result = true;
			}
			else
			{
				result = false;
			}
			goto IL_0058;
			IL_0058:
			return result;
			IL_002d:
			result = false;
			goto IL_0058;
		}
	}
}
