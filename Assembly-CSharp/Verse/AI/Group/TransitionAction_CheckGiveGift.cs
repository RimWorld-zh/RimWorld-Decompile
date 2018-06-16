using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A0A RID: 2570
	public class TransitionAction_CheckGiveGift : TransitionAction
	{
		// Token: 0x06003989 RID: 14729 RVA: 0x001E7A24 File Offset: 0x001E5E24
		public override void DoAction(Transition trans)
		{
			if (DebugSettings.instantVisitorsGift || (trans.target.lord.numPawnsLostViolently == 0 && Rand.Chance(VisitorGiftForPlayerUtility.ChanceToLeaveGift(trans.target.lord.faction, trans.Map))))
			{
				VisitorGiftForPlayerUtility.CheckGiveGift(trans.target.lord.ownedPawns, trans.target.lord.faction);
			}
		}
	}
}
