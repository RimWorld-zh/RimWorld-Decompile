using System;

namespace Verse.AI.Group
{
	// Token: 0x020009EA RID: 2538
	public class LordJob_ExitMapNear : LordJob
	{
		// Token: 0x04002471 RID: 9329
		private IntVec3 near;

		// Token: 0x04002472 RID: 9330
		private float radius;

		// Token: 0x04002473 RID: 9331
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04002474 RID: 9332
		private bool canDig;

		// Token: 0x04002475 RID: 9333
		private bool useAvoidGridSmart;

		// Token: 0x04002476 RID: 9334
		public const float DefaultRadius = 12f;

		// Token: 0x06003908 RID: 14600 RVA: 0x001E674F File Offset: 0x001E4B4F
		public LordJob_ExitMapNear()
		{
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x001E675F File Offset: 0x001E4B5F
		public LordJob_ExitMapNear(IntVec3 near, LocomotionUrgency locomotion, float radius = 12f, bool canDig = false, bool useAvoidGridSmart = false)
		{
			this.near = near;
			this.locomotion = locomotion;
			this.radius = radius;
			this.canDig = canDig;
			this.useAvoidGridSmart = useAvoidGridSmart;
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x001E6794 File Offset: 0x001E4B94
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

		// Token: 0x0600390B RID: 14603 RVA: 0x001E67EC File Offset: 0x001E4BEC
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
