using System;

namespace Verse.AI
{
	// Token: 0x02000AAA RID: 2730
	public class ThinkNode_ChancePerHour_Constant : ThinkNode_ChancePerHour
	{
		// Token: 0x04002689 RID: 9865
		private float mtbHours = 1f;

		// Token: 0x06003D09 RID: 15625 RVA: 0x00204B00 File Offset: 0x00202F00
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ChancePerHour_Constant thinkNode_ChancePerHour_Constant = (ThinkNode_ChancePerHour_Constant)base.DeepCopy(resolve);
			thinkNode_ChancePerHour_Constant.mtbHours = this.mtbHours;
			return thinkNode_ChancePerHour_Constant;
		}

		// Token: 0x06003D0A RID: 15626 RVA: 0x00204B30 File Offset: 0x00202F30
		protected override float MtbHours(Pawn Pawn)
		{
			return this.mtbHours;
		}
	}
}
