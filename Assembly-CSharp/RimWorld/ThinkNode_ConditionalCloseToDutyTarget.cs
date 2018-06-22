using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DD RID: 477
	public class ThinkNode_ConditionalCloseToDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x06000972 RID: 2418 RVA: 0x00056790 File Offset: 0x00054B90
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCloseToDutyTarget thinkNode_ConditionalCloseToDutyTarget = (ThinkNode_ConditionalCloseToDutyTarget)base.DeepCopy(resolve);
			thinkNode_ConditionalCloseToDutyTarget.maxDistToDutyTarget = this.maxDistToDutyTarget;
			return thinkNode_ConditionalCloseToDutyTarget;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x000567C0 File Offset: 0x00054BC0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty.focus.IsValid && pawn.Position.InHorDistOf(pawn.mindState.duty.focus.Cell, this.maxDistToDutyTarget);
		}

		// Token: 0x040003E0 RID: 992
		private float maxDistToDutyTarget = 10f;
	}
}
