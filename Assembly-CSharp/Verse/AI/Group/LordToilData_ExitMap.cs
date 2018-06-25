using System;

namespace Verse.AI.Group
{
	// Token: 0x020009F7 RID: 2551
	public class LordToilData_ExitMap : LordToilData
	{
		// Token: 0x04002486 RID: 9350
		public LocomotionUrgency locomotion = LocomotionUrgency.None;

		// Token: 0x04002487 RID: 9351
		public bool canDig = false;

		// Token: 0x06003947 RID: 14663 RVA: 0x001E71FD File Offset: 0x001E55FD
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
		}
	}
}
