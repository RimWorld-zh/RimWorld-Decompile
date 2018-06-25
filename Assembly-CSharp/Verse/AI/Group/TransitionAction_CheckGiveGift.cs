using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A09 RID: 2569
	public class TransitionAction_CheckGiveGift : TransitionAction
	{
		// Token: 0x0600398A RID: 14730 RVA: 0x001E8190 File Offset: 0x001E6590
		public override void DoAction(Transition trans)
		{
			if (DebugSettings.instantVisitorsGift || (trans.target.lord.numPawnsLostViolently == 0 && Rand.Chance(VisitorGiftForPlayerUtility.ChanceToLeaveGift(trans.target.lord.faction, trans.Map))))
			{
				VisitorGiftForPlayerUtility.CheckGiveGift(trans.target.lord.ownedPawns, trans.target.lord.faction);
			}
		}
	}
}
