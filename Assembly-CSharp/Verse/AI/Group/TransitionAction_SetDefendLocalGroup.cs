using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A07 RID: 2567
	public class TransitionAction_SetDefendLocalGroup : TransitionAction
	{
		// Token: 0x06003982 RID: 14722 RVA: 0x001E78AC File Offset: 0x001E5CAC
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
