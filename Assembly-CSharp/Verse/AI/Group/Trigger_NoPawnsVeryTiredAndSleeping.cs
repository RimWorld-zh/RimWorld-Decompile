using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A27 RID: 2599
	public class Trigger_NoPawnsVeryTiredAndSleeping : Trigger
	{
		// Token: 0x060039CD RID: 14797 RVA: 0x001E8C0B File Offset: 0x001E700B
		public Trigger_NoPawnsVeryTiredAndSleeping(float extraRestThreshOffset = 0f)
		{
			this.extraRestThreshOffset = extraRestThreshOffset;
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x001E8C1C File Offset: 0x001E701C
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

		// Token: 0x040024B4 RID: 9396
		private float extraRestThreshOffset;
	}
}
