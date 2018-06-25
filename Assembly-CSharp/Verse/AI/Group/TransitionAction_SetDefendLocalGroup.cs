using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A06 RID: 2566
	public class TransitionAction_SetDefendLocalGroup : TransitionAction
	{
		// Token: 0x06003983 RID: 14723 RVA: 0x001E8018 File Offset: 0x001E6418
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
