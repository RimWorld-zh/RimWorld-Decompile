using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E7 RID: 2535
	public class LordJob_ExitMapNear : LordJob
	{
		// Token: 0x04002460 RID: 9312
		private IntVec3 near;

		// Token: 0x04002461 RID: 9313
		private float radius;

		// Token: 0x04002462 RID: 9314
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04002463 RID: 9315
		private bool canDig;

		// Token: 0x04002464 RID: 9316
		private bool useAvoidGridSmart;

		// Token: 0x04002465 RID: 9317
		public const float DefaultRadius = 12f;

		// Token: 0x06003903 RID: 14595 RVA: 0x001E62F7 File Offset: 0x001E46F7
		public LordJob_ExitMapNear()
		{
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x001E6307 File Offset: 0x001E4707
		public LordJob_ExitMapNear(IntVec3 near, LocomotionUrgency locomotion, float radius = 12f, bool canDig = false, bool useAvoidGridSmart = false)
		{
			this.near = near;
			this.locomotion = locomotion;
			this.radius = radius;
			this.canDig = canDig;
			this.useAvoidGridSmart = useAvoidGridSmart;
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x001E633C File Offset: 0x001E473C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ExitMapNear lordToil_ExitMapNear = new LordToil_ExitMapNear(this.near, this.radius, this.locomotion, this.canDig);
			if (this.useAvoidGridSmart)
			{
				lordToil_ExitMapNear.avoidGridMode = AvoidGridMode.Smart;
			}
			stateGraph.AddToil(lordToil_ExitMapNear);
			return stateGraph;
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x001E6394 File Offset: 0x001E4794
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.near, "near", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
		}
	}
}
