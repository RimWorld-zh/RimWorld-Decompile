using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A05 RID: 2565
	public class TransitionAction_SetDefendLocalGroup : TransitionAction
	{
		// Token: 0x06003982 RID: 14722 RVA: 0x001E7CEC File Offset: 0x001E60EC
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
