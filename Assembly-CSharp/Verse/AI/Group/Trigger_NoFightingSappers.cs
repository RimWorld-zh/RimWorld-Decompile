using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A1C RID: 2588
	public class Trigger_NoFightingSappers : Trigger
	{
		// Token: 0x060039B5 RID: 14773 RVA: 0x001E8914 File Offset: 0x001E6D14
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.PawnLost)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn p = lord.ownedPawns[i];
					if (this.IsFightingSapper(p))
					{
						return false;
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

		// Token: 0x060039B6 RID: 14774 RVA: 0x001E8980 File Offset: 0x001E6D80
		private bool IsFightingSapper(Pawn p)
		{
			return !p.Downed && !p.InMentalState && (SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p));
		}
	}
}
