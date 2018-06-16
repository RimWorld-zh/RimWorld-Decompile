using System;

namespace Verse.AI
{
	// Token: 0x02000AAE RID: 2734
	public class ThinkNode_ChancePerHour_Constant : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D0C RID: 15628 RVA: 0x00204708 File Offset: 0x00202B08
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ChancePerHour_Constant thinkNode_ChancePerHour_Constant = (ThinkNode_ChancePerHour_Constant)base.DeepCopy(resolve);
			thinkNode_ChancePerHour_Constant.mtbHours = this.mtbHours;
			return thinkNode_ChancePerHour_Constant;
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x00204738 File Offset: 0x00202B38
		protected override float MtbHours(Pawn Pawn)
		{
			return this.mtbHours;
		}

		// Token: 0x0400268E RID: 9870
		private float mtbHours = 1f;
	}
}
