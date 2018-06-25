using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000174 RID: 372
	public class LordJob_Siege : LordJob
	{
		// Token: 0x04000358 RID: 856
		private Faction faction;

		// Token: 0x04000359 RID: 857
		private IntVec3 siegeSpot;

		// Token: 0x0400035A RID: 858
		private float blueprintPoints;

		// Token: 0x060007A7 RID: 1959 RVA: 0x0004B462 File Offset: 0x00049862
		public LordJob_Siege()
		{
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0004B46B File Offset: 0x0004986B
		public LordJob_Siege(Faction faction, IntVec3 siegeSpot, float blueprintPoints)
		{
			this.faction = faction;
			this.siegeSpot = siegeSpot;
			this.blueprintPoints = blueprintPoints;
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0004B48C File Offset: 0x0004988C
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0004B4A4 File Offset: 0x000498A4
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.siegeSpot).CreateGraph()).StartingToil;
			LordToil_Siege lordToil_Siege = new LordToil_Siege(this.siegeSpot, this.blueprintPoints);
			stateGraph.AddToil(lordToil_Siege);
			LordToil startingToil2 = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph()).StartingToil;
			Transition transition = new Transition(startingToil, lordToil_Siege, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			transition.AddTrigger(new Trigger_TicksPassed(5000));
			stateGraph.AddTransition(transition);
			Transition transition2 = new Transition(lordToil_Siege, startingToil2, false, true);
			transition2.AddTrigger(new Trigger_Memo("NoBuilders"));
			transition2.AddTrigger(new Trigger_Memo("NoArtillery"));
			transition2.AddTrigger(new Trigger_PawnHarmed(0.08f, false, null));
			transition2.AddTrigger(new Trigger_FractionPawnsLost(0.3f));
			transition2.AddTrigger(new Trigger_TicksPassed((int)(60000f * Rand.Range(1.5f, 3f))));
			transition2.AddPreAction(new TransitionAction_Message("MessageSiegersAssaulting".Translate(new object[]
			{
				this.faction.def.pawnsPlural,
				this.faction
			}), MessageTypeDefOf.ThreatBig, null, 1f));
			transition2.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition2);
			return stateGraph;
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0004B61C File Offset: 0x00049A1C
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.siegeSpot, "siegeSpot", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.blueprintPoints, "blueprintPoints", 0f, false);
		}
	}
}
