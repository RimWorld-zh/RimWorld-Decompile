using System;

namespace Verse.AI.Group
{
	// Token: 0x020009F4 RID: 2548
	public class LordToilData_ExitMap : LordToilData
	{
		// Token: 0x06003942 RID: 14658 RVA: 0x001E6DA5 File Offset: 0x001E51A5
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
		}

		// Token: 0x04002475 RID: 9333
		public LocomotionUrgency locomotion = LocomotionUrgency.None;

		// Token: 0x04002476 RID: 9334
		public bool canDig = false;
	}
}
