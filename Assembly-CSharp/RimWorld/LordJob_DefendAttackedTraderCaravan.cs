using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200016C RID: 364
	public class LordJob_DefendAttackedTraderCaravan : LordJob
	{
		// Token: 0x0400033D RID: 829
		private IntVec3 defendSpot;

		// Token: 0x06000775 RID: 1909 RVA: 0x0004A3BD File Offset: 0x000487BD
		public LordJob_DefendAttackedTraderCaravan()
		{
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0004A3C6 File Offset: 0x000487C6
		public LordJob_DefendAttackedTraderCaravan(IntVec3 defendSpot)
		{
			this.defendSpot = defendSpot;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0004A3D8 File Offset: 0x000487D8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendTraderCaravan lordToil_DefendTraderCaravan = new LordToil_DefendTraderCaravan(this.defendSpot);
			stateGraph.StartingToil = lordToil_DefendTraderCaravan;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false);
			stateGraph.AddToil(lordToil_ExitMap);
			Transition transition = new Transition(lordToil_DefendTraderCaravan, lordToil_ExitMap, false, true);
			transition.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			transition.AddTrigger(new Trigger_TraderAndAllTraderCaravanGuardsLost());
			stateGraph.AddTransition(transition);
			return stateGraph;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0004A444 File Offset: 0x00048844
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendSpot, "defendSpot", default(IntVec3), false);
		}
	}
}
