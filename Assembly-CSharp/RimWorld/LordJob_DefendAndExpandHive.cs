using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordJob_DefendAndExpandHive : LordJob
	{
		private bool aggressive;

		public LordJob_DefendAndExpandHive()
		{
		}

		public LordJob_DefendAndExpandHive(bool aggressive)
		{
			this.aggressive = aggressive;
		}

		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendAndExpandHive lordToil_DefendAndExpandHive = new LordToil_DefendAndExpandHive();
			lordToil_DefendAndExpandHive.distToHiveToAttack = 10f;
			stateGraph.StartingToil = lordToil_DefendAndExpandHive;
			LordToil_DefendHiveAggressively lordToil_DefendHiveAggressively = new LordToil_DefendHiveAggressively();
			lordToil_DefendHiveAggressively.distToHiveToAttack = 40f;
			stateGraph.AddToil(lordToil_DefendHiveAggressively);
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false);
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition = new Transition(lordToil_DefendAndExpandHive, (!this.aggressive) ? lordToil_DefendHiveAggressively : lordToil_AssaultColony, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(0.5f, true, null));
			transition.AddTrigger(new Trigger_PawnLostViolently());
			transition.AddTrigger(new Trigger_Memo(Hive.MemoAttackedByEnemy));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoBurnedBadly));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoDestroyed));
			transition.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition);
			Transition transition2 = new Transition(lordToil_DefendAndExpandHive, lordToil_AssaultColony, false, true);
			Transition transition3 = transition2;
			float chance = 0.5f;
			Faction parentFaction = base.Map.ParentFaction;
			transition3.AddTrigger(new Trigger_PawnHarmed(chance, false, parentFaction));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2);
			Transition transition4 = new Transition(lordToil_DefendHiveAggressively, lordToil_AssaultColony, false, true);
			Transition transition5 = transition4;
			chance = 0.5f;
			parentFaction = base.Map.ParentFaction;
			transition5.AddTrigger(new Trigger_PawnHarmed(chance, false, parentFaction));
			transition4.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition4);
			Transition transition6 = new Transition(lordToil_DefendAndExpandHive, (!this.aggressive) ? lordToil_DefendHiveAggressively : lordToil_AssaultColony, false, true);
			transition6.canMoveToSameState = true;
			transition6.AddSource(lordToil_AssaultColony);
			transition6.AddTrigger(new Trigger_Memo(Hive.MemoDestroyed));
			stateGraph.AddTransition(transition6);
			Transition transition7 = new Transition(lordToil_AssaultColony, lordToil_DefendAndExpandHive, false, true);
			transition7.AddSource(lordToil_DefendHiveAggressively);
			transition7.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(1200, new string[]
			{
				Hive.MemoAttackedByEnemy,
				Hive.MemoBurnedBadly,
				Hive.MemoDestroyed,
				HediffGiver_Heat.MemoPawnBurnedByAir
			}));
			transition7.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
			stateGraph.AddTransition(transition7);
			return stateGraph;
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.aggressive, "aggressive", false, false);
		}
	}
}
