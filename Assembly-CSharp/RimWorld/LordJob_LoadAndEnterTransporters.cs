using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000170 RID: 368
	public class LordJob_LoadAndEnterTransporters : LordJob
	{
		// Token: 0x0400034F RID: 847
		public int transportersGroup = -1;

		// Token: 0x06000793 RID: 1939 RVA: 0x0004AE19 File Offset: 0x00049219
		public LordJob_LoadAndEnterTransporters()
		{
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0004AE29 File Offset: 0x00049229
		public LordJob_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0004AE40 File Offset: 0x00049240
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0004AE56 File Offset: 0x00049256
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", 0, false);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0004AE6C File Offset: 0x0004926C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_LoadAndEnterTransporters lordToil_LoadAndEnterTransporters = new LordToil_LoadAndEnterTransporters(this.transportersGroup);
			stateGraph.StartingToil = lordToil_LoadAndEnterTransporters;
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(lordToil_LoadAndEnterTransporters, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_PawnLost());
			transition.AddPreAction(new TransitionAction_Message("MessageFailedToLoadTransportersBecauseColonistLost".Translate(), MessageTypeDefOf.NegativeEvent, null, 1f));
			transition.AddPreAction(new TransitionAction_Custom(new Action(this.CancelLoadingProcess)));
			stateGraph.AddTransition(transition);
			return stateGraph;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0004AF00 File Offset: 0x00049300
		private void CancelLoadingProcess()
		{
			List<Thing> list = this.lord.Map.listerThings.ThingsInGroup(ThingRequestGroup.Transporter);
			for (int i = 0; i < list.Count; i++)
			{
				CompTransporter compTransporter = list[i].TryGetComp<CompTransporter>();
				if (compTransporter.groupID == this.transportersGroup)
				{
					compTransporter.CancelLoad();
					break;
				}
			}
		}
	}
}
