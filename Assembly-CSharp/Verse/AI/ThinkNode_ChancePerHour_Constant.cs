using System;

namespace Verse.AI
{
	// Token: 0x02000AAD RID: 2733
	public class ThinkNode_ChancePerHour_Constant : ThinkNode_ChancePerHour
	{
		// Token: 0x04002691 RID: 9873
		private float mtbHours = 1f;

		// Token: 0x06003D0D RID: 15629 RVA: 0x00204F0C File Offset: 0x0020330C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ChancePerHour_Constant thinkNode_ChancePerHour_Constant = (ThinkNode_ChancePerHour_Constant)base.DeepCopy(resolve);
			thinkNode_ChancePerHour_Constant.mtbHours = this.mtbHours;
			return thinkNode_ChancePerHour_Constant;
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x00204F3C File Offset: 0x0020333C
		protected override float MtbHours(Pawn Pawn)
		{
			return this.mtbHours;
		}
	}
}
