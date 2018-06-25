using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000177 RID: 375
	public class LordJob_Steal : LordJob
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0004BAA8 File Offset: 0x00049EA8
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0004BAC0 File Offset: 0x00049EC0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_StealCover lordToil_StealCover = new LordToil_StealCover();
			lordToil_StealCover.avoidGridMode = AvoidGridMode.Smart;
			stateGraph.AddToil(lordToil_StealCover);
			LordToil_StealCover lordToil_StealCover2 = new LordToil_StealCover();
			lordToil_StealCover2.cover = false;
			lordToil_StealCover2.avoidGridMode = AvoidGridMode.Smart;
			stateGraph.AddToil(lordToil_StealCover2);
			Transition transition = new Transition(lordToil_StealCover, lordToil_StealCover2, false, true);
			transition.AddTrigger(new Trigger_TicksPassedAndNoRecentHarm(1200));
			stateGraph.AddTransition(transition);
			return stateGraph;
		}
	}
}
