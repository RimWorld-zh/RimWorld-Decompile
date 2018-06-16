using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200016F RID: 367
	public class LordJob_Kidnap : LordJob
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x0004ADA4 File Offset: 0x000491A4
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0004ADBC File Offset: 0x000491BC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_KidnapCover lordToil_KidnapCover = new LordToil_KidnapCover();
			lordToil_KidnapCover.avoidGridMode = AvoidGridMode.Smart;
			stateGraph.AddToil(lordToil_KidnapCover);
			LordToil_KidnapCover lordToil_KidnapCover2 = new LordToil_KidnapCover();
			lordToil_KidnapCover2.cover = false;
			lordToil_KidnapCover2.avoidGridMode = AvoidGridMode.Smart;
			stateGraph.AddToil(lordToil_KidnapCover2);
			Transition transition = new Transition(lordToil_KidnapCover, lordToil_KidnapCover2, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(1200));
			stateGraph.AddTransition(transition);
			return stateGraph;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0004AE2A File Offset: 0x0004922A
		public override void ExposeData()
		{
		}
	}
}
