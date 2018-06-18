using System;

namespace Verse.AI
{
	// Token: 0x02000AAE RID: 2734
	public class ThinkNode_ChancePerHour_Constant : ThinkNode_ChancePerHour
	{
		// Token: 0x06003D0E RID: 15630 RVA: 0x002047DC File Offset: 0x00202BDC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ChancePerHour_Constant thinkNode_ChancePerHour_Constant = (ThinkNode_ChancePerHour_Constant)base.DeepCopy(resolve);
			thinkNode_ChancePerHour_Constant.mtbHours = this.mtbHours;
			return thinkNode_ChancePerHour_Constant;
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x0020480C File Offset: 0x00202C0C
		protected override float MtbHours(Pawn Pawn)
		{
			return this.mtbHours;
		}

		// Token: 0x0400268E RID: 9870
		private float mtbHours = 1f;
	}
}
