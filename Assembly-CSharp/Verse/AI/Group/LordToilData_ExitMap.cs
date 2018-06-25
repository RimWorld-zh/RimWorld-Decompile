using System;

namespace Verse.AI.Group
{
	// Token: 0x020009F6 RID: 2550
	public class LordToilData_ExitMap : LordToilData
	{
		// Token: 0x04002476 RID: 9334
		public LocomotionUrgency locomotion = LocomotionUrgency.None;

		// Token: 0x04002477 RID: 9335
		public bool canDig = false;

		// Token: 0x06003946 RID: 14662 RVA: 0x001E6ED1 File Offset: 0x001E52D1
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
		}
	}
}
