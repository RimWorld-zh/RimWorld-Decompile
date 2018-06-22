using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A19 RID: 2585
	public class Trigger_NoFightingSappers : Trigger
	{
		// Token: 0x060039B0 RID: 14768 RVA: 0x001E84BC File Offset: 0x001E68BC
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

		// Token: 0x060039B1 RID: 14769 RVA: 0x001E8528 File Offset: 0x001E6928
		private bool IsFightingSapper(Pawn p)
		{
			return !p.Downed && !p.InMentalState && (SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p));
		}
	}
}
