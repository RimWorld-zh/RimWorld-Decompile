using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A2B RID: 2603
	public class Trigger_NoPawnsVeryTiredAndSleeping : Trigger
	{
		// Token: 0x060039D3 RID: 14803 RVA: 0x001E89CB File Offset: 0x001E6DCB
		public Trigger_NoPawnsVeryTiredAndSleeping(float extraRestThreshOffset = 0f)
		{
			this.extraRestThreshOffset = extraRestThreshOffset;
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x001E89DC File Offset: 0x001E6DDC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Need_Rest rest = lord.ownedPawns[i].needs.rest;
					if (rest != null)
					{
						if (rest.CurLevelPercentage < 0.14f + this.extraRestThreshOffset && !lord.ownedPawns[i].Awake())
						{
							return false;
						}
					}
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x040024B9 RID: 9401
		private float extraRestThreshOffset;
	}
}
