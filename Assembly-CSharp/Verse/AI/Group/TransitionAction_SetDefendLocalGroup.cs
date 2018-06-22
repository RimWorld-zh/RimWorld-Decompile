using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A03 RID: 2563
	public class TransitionAction_SetDefendLocalGroup : TransitionAction
	{
		// Token: 0x0600397E RID: 14718 RVA: 0x001E7BC0 File Offset: 0x001E5FC0
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
