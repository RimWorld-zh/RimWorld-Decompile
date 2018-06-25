using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200016D RID: 365
	public class LordJob_DefendBase : LordJob
	{
		// Token: 0x0400033E RID: 830
		private Faction faction;

		// Token: 0x0400033F RID: 831
		private IntVec3 baseCenter;

		// Token: 0x06000779 RID: 1913 RVA: 0x0004A46C File Offset: 0x0004886C
		public LordJob_DefendBase()
		{
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0004A475 File Offset: 0x00048875
		public LordJob_DefendBase(Faction faction, IntVec3 baseCenter)
		{
			this.faction = faction;
			this.baseCenter = baseCenter;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0004A48C File Offset: 0x0004888C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendBase lordToil_DefendBase = new LordToil_DefendBase(this.baseCenter);
			stateGraph.StartingToil = lordToil_DefendBase;
			LordToil_DefendBase lordToil_DefendBase2 = new LordToil_DefendBase(this.baseCenter);
			stateGraph.AddToil(lordToil_DefendBase2);
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(true);
			lordToil_AssaultColony.avoidGridMode = AvoidGridMode.Smart;
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition = new Transition(lordToil_DefendBase, lordToil_DefendBase2, false, true);
			transition.AddSource(lordToil_AssaultColony);
			transition.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			stateGraph.AddTransition(transition);
			Transition transition2 = new Transition(lordToil_DefendBase2, lordToil_DefendBase, false, true);
			transition2.AddTrigger(new Trigger_BecamePlayerEnemy());
			stateGraph.AddTransition(transition2);
			Transition transition3 = new Transition(lordToil_DefendBase, lordToil_AssaultColony, false, true);
			transition3.AddTrigger(new Trigger_FractionPawnsLost(0.2f));
			transition3.AddTrigger(new Trigger_PawnHarmed(0.4f, false, null));
			transition3.AddTrigger(new Trigger_ChanceOnTickInteval(2500, 0.03f));
			transition3.AddTrigger(new Trigger_TicksPassed(251999));
			transition3.AddTrigger(new Trigger_UrgentlyHungry());
			transition3.AddTrigger(new Trigger_ChanceOnPlayerHarmNPCBuilding(0.4f));
			transition3.AddPostAction(new TransitionAction_WakeAll());
			string message = "MessageDefendersAttacking".Translate(new object[]
			{
				this.faction.def.pawnsPlural,
				this.faction.Name,
				Faction.OfPlayer.def.pawnsPlural
			}).CapitalizeFirst();
			transition3.AddPreAction(new TransitionAction_Message(message, MessageTypeDefOf.ThreatBig, null, 1f));
			stateGraph.AddTransition(transition3);
			return stateGraph;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0004A61C File Offset: 0x00048A1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.baseCenter, "baseCenter", default(IntVec3), false);
		}
	}
}
