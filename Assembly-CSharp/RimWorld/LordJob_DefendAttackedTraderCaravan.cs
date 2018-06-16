using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200016C RID: 364
	public class LordJob_DefendAttackedTraderCaravan : LordJob
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x0004A3D5 File Offset: 0x000487D5
		public LordJob_DefendAttackedTraderCaravan()
		{
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0004A3DE File Offset: 0x000487DE
		public LordJob_DefendAttackedTraderCaravan(IntVec3 defendSpot)
		{
			this.defendSpot = defendSpot;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0004A3F0 File Offset: 0x000487F0
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

		// Token: 0x06000779 RID: 1913 RVA: 0x0004A45C File Offset: 0x0004885C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendSpot, "defendSpot", default(IntVec3), false);
		}

		// Token: 0x0400033C RID: 828
		private IntVec3 defendSpot;
	}
}
