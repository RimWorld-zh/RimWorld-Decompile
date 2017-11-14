using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordJob_LoadAndEnterTransporters : LordJob
	{
		public int transportersGroup = -1;

		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		public LordJob_LoadAndEnterTransporters()
		{
		}

		public LordJob_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", 0, false);
		}

		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_LoadAndEnterTransporters firstSource = (LordToil_LoadAndEnterTransporters)(stateGraph.StartingToil = new LordToil_LoadAndEnterTransporters(this.transportersGroup));
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(firstSource, lordToil_End);
			transition.AddTrigger(new Trigger_PawnLost());
			transition.AddPreAction(new TransitionAction_Message("MessageFailedToLoadTransportersBecauseColonistLost".Translate(), MessageTypeDefOf.NegativeEvent));
			transition.AddPreAction(new TransitionAction_Custom(this.CancelLoadingProcess));
			stateGraph.AddTransition(transition);
			return stateGraph;
		}

		private void CancelLoadingProcess()
		{
			List<Thing> list = base.lord.Map.listerThings.ThingsInGroup(ThingRequestGroup.Transporter);
			int num = 0;
			CompTransporter compTransporter;
			while (true)
			{
				if (num < list.Count)
				{
					compTransporter = list[num].TryGetComp<CompTransporter>();
					if (compTransporter.groupID != this.transportersGroup)
					{
						num++;
						continue;
					}
					break;
				}
				return;
			}
			compTransporter.CancelLoad();
		}
	}
}
