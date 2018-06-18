using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A1D RID: 2589
	public class Trigger_NoFightingSappers : Trigger
	{
		// Token: 0x060039B6 RID: 14774 RVA: 0x001E827C File Offset: 0x001E667C
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

		// Token: 0x060039B7 RID: 14775 RVA: 0x001E82E8 File Offset: 0x001E66E8
		private bool IsFightingSapper(Pawn p)
		{
			return !p.Downed && !p.InMentalState && (SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p));
		}
	}
}
