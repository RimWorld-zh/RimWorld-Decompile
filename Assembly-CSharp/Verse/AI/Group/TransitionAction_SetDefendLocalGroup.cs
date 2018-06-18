using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A07 RID: 2567
	public class TransitionAction_SetDefendLocalGroup : TransitionAction
	{
		// Token: 0x06003984 RID: 14724 RVA: 0x001E7980 File Offset: 0x001E5D80
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
