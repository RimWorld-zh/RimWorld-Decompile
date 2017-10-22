namespace Verse.AI.Group
{
	public class Trigger_UrgentlyHungry : Trigger
	{
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if ((int)lord.ownedPawns[i].needs.food.CurCategory >= 2)
						goto IL_0038;
				}
			}
			bool result = false;
			goto IL_005d;
			IL_0038:
			result = true;
			goto IL_005d;
			IL_005d:
			return result;
		}
	}
}
