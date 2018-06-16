using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DD RID: 477
	public class ThinkNode_ConditionalCloseToDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x06000974 RID: 2420 RVA: 0x0005677C File Offset: 0x00054B7C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCloseToDutyTarget thinkNode_ConditionalCloseToDutyTarget = (ThinkNode_ConditionalCloseToDutyTarget)base.DeepCopy(resolve);
			thinkNode_ConditionalCloseToDutyTarget.maxDistToDutyTarget = this.maxDistToDutyTarget;
			return thinkNode_ConditionalCloseToDutyTarget;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x000567AC File Offset: 0x00054BAC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty.focus.IsValid && pawn.Position.InHorDistOf(pawn.mindState.duty.focus.Cell, this.maxDistToDutyTarget);
		}

		// Token: 0x040003E2 RID: 994
		private float maxDistToDutyTarget = 10f;
	}
}
