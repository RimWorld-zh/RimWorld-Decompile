using System;

namespace Verse.AI.Group
{
	// Token: 0x020009EB RID: 2539
	public class LordJob_ExitMapNear : LordJob
	{
		// Token: 0x06003907 RID: 14599 RVA: 0x001E5FE3 File Offset: 0x001E43E3
		public LordJob_ExitMapNear()
		{
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x001E5FF3 File Offset: 0x001E43F3
		public LordJob_ExitMapNear(IntVec3 near, LocomotionUrgency locomotion, float radius = 12f, bool canDig = false, bool useAvoidGridSmart = false)
		{
			this.near = near;
			this.locomotion = locomotion;
			this.radius = radius;
			this.canDig = canDig;
			this.useAvoidGridSmart = useAvoidGridSmart;
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x001E6028 File Offset: 0x001E4428
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

		// Token: 0x0600390A RID: 14602 RVA: 0x001E6080 File Offset: 0x001E4480
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.near, "near", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
		}

		// Token: 0x04002465 RID: 9317
		private IntVec3 near;

		// Token: 0x04002466 RID: 9318
		private float radius;

		// Token: 0x04002467 RID: 9319
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04002468 RID: 9320
		private bool canDig;

		// Token: 0x04002469 RID: 9321
		private bool useAvoidGridSmart;

		// Token: 0x0400246A RID: 9322
		public const float DefaultRadius = 12f;
	}
}
