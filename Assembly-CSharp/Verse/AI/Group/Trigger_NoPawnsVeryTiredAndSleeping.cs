using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A2A RID: 2602
	public class Trigger_NoPawnsVeryTiredAndSleeping : Trigger
	{
		// Token: 0x040024C5 RID: 9413
		private float extraRestThreshOffset;

		// Token: 0x060039D2 RID: 14802 RVA: 0x001E9063 File Offset: 0x001E7463
		public Trigger_NoPawnsVeryTiredAndSleeping(float extraRestThreshOffset = 0f)
		{
			this.extraRestThreshOffset = extraRestThreshOffset;
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x001E9074 File Offset: 0x001E7474
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
	}
}
