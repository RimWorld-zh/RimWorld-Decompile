using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000172 RID: 370
	public class LordJob_MechanoidsDefendShip : LordJob
	{
		// Token: 0x04000350 RID: 848
		private Thing shipPart;

		// Token: 0x04000351 RID: 849
		private Faction faction;

		// Token: 0x04000352 RID: 850
		private float defendRadius;

		// Token: 0x04000353 RID: 851
		private IntVec3 defSpot;

		// Token: 0x0600079B RID: 1947 RVA: 0x0004AF9B File Offset: 0x0004939B
		public LordJob_MechanoidsDefendShip()
		{
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0004AFA4 File Offset: 0x000493A4
		public LordJob_MechanoidsDefendShip(Thing shipPart, Faction faction, float defendRadius, IntVec3 defSpot)
		{
			this.shipPart = shipPart;
			this.faction = faction;
			this.defendRadius = defendRadius;
			this.defSpot = defSpot;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0004AFCC File Offset: 0x000493CC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			StateGraph result;
			if (!this.defSpot.IsValid)
			{
				Log.Warning("LordJob_MechanoidsDefendShip defSpot is invalid. Returning graph for LordJob_AssaultColony.", false);
				stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph());
				result = stateGraph;
			}
			else
			{
				LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.defSpot, this.defendRadius);
				stateGraph.StartingToil = lordToil_DefendPoint;
				LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false);
				stateGraph.AddToil(lordToil_AssaultColony);
				LordToil_AssaultColony lordToil_AssaultColony2 = new LordToil_AssaultColony(false);
				stateGraph.AddToil(lordToil_AssaultColony2);
				Transition transition = new Transition(lordToil_DefendPoint, lordToil_AssaultColony2, false, true);
				transition.AddSource(lordToil_AssaultColony);
				transition.AddTrigger(new Trigger_PawnCannotReachMapEdge());
				stateGraph.AddTransition(transition);
				Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
				transition2.AddTrigger(new Trigger_PawnHarmed(0.5f, true, null));
				transition2.AddTrigger(new Trigger_PawnLostViolently());
				transition2.AddTrigger(new Trigger_Memo(CompSpawnerMechanoidsOnDamaged.MemoDamaged));
				transition2.AddPostAction(new TransitionAction_EndAllJobs());
				stateGraph.AddTransition(transition2);
				Transition transition3 = new Transition(lordToil_AssaultColony, lordToil_DefendPoint, false, true);
				transition3.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(1380, new string[]
				{
					CompSpawnerMechanoidsOnDamaged.MemoDamaged
				}));
				transition3.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
				stateGraph.AddTransition(transition3);
				Transition transition4 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony2, false, true);
				transition4.AddSource(lordToil_AssaultColony);
				transition4.AddTrigger(new Trigger_ThingDamageTaken(this.shipPart, 0.5f));
				transition4.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
				stateGraph.AddTransition(transition4);
				result = stateGraph;
			}
			return result;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0004B160 File Offset: 0x00049560
		public override void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.shipPart, "shipPart", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 0f, false);
			Scribe_Values.Look<IntVec3>(ref this.defSpot, "defSpot", default(IntVec3), false);
		}
	}
}
