using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A06 RID: 2566
	public class TransitionAction_CheckGiveGift : TransitionAction
	{
		// Token: 0x06003985 RID: 14725 RVA: 0x001E7D38 File Offset: 0x001E6138
		public override void DoAction(Transition trans)
		{
			if (DebugSettings.instantVisitorsGift || (trans.target.lord.numPawnsLostViolently == 0 && Rand.Chance(VisitorGiftForPlayerUtility.ChanceToLeaveGift(trans.target.lord.faction, trans.Map))))
			{
				VisitorGiftForPlayerUtility.CheckGiveGift(trans.target.lord.ownedPawns, trans.target.lord.faction);
			}
		}
	}
}
