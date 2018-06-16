using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000169 RID: 361
	public class LordJob_AssaultColony : LordJob
	{
		// Token: 0x06000768 RID: 1896 RVA: 0x00049986 File Offset: 0x00047D86
		public LordJob_AssaultColony()
		{
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x000499B4 File Offset: 0x00047DB4
		public LordJob_AssaultColony(Faction assaulterFaction, bool canKidnap = true, bool canTimeoutOrFlee = true, bool sappers = false, bool useAvoidGridSmart = false, bool canSteal = true)
		{
			this.assaulterFaction = assaulterFaction;
			this.canKidnap = canKidnap;
			this.canTimeoutOrFlee = canTimeoutOrFlee;
			this.sappers = sappers;
			this.useAvoidGridSmart = useAvoidGridSmart;
			this.canSteal = canSteal;
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00049A18 File Offset: 0x00047E18
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00049A30 File Offset: 0x00047E30
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil lordToil = null;
			if (this.sappers)
			{
				lordToil = new LordToil_AssaultColonySappers();
				if (this.useAvoidGridSmart)
				{
					lordToil.avoidGridMode = AvoidGridMode.Smart;
				}
				stateGraph.AddToil(lordToil);
				Transition transition = new Transition(lordToil, lordToil, true, true);
				transition.AddTrigger(new Trigger_PawnLost());
				stateGraph.AddTransition(transition);
				Transition transition2 = new Transition(lordToil, lordToil, true, false);
				transition2.AddTrigger(new Trigger_PawnHarmed(1f, false, null));
				transition2.AddPostAction(new TransitionAction_CheckForJobOverride());
				stateGraph.AddTransition(transition2);
			}
			LordToil lordToil2 = new LordToil_AssaultColony(false);
			if (this.useAvoidGridSmart)
			{
				lordToil2.avoidGridMode = AvoidGridMode.Smart;
			}
			stateGraph.AddToil(lordToil2);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false);
			lordToil_ExitMap.avoidGridMode = AvoidGridMode.Smart;
			stateGraph.AddToil(lordToil_ExitMap);
			if (this.sappers)
			{
				Transition transition3 = new Transition(lordToil, lordToil2, false, true);
				transition3.AddTrigger(new Trigger_NoFightingSappers());
				stateGraph.AddTransition(transition3);
			}
			if (this.assaulterFaction.def.humanlikeFaction)
			{
				if (this.canTimeoutOrFlee)
				{
					Transition transition4 = new Transition(lordToil2, lordToil_ExitMap, false, true);
					if (lordToil != null)
					{
						transition4.AddSource(lordToil);
					}
					transition4.AddTrigger(new Trigger_TicksPassed((!this.sappers) ? LordJob_AssaultColony.AssaultTimeBeforeGiveUp.RandomInRange : LordJob_AssaultColony.SapTimeBeforeGiveUp.RandomInRange));
					transition4.AddPreAction(new TransitionAction_Message("MessageRaidersGivenUpLeaving".Translate(new object[]
					{
						this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(),
						this.assaulterFaction.Name
					}), null, 1f));
					stateGraph.AddTransition(transition4);
					Transition transition5 = new Transition(lordToil2, lordToil_ExitMap, false, true);
					if (lordToil != null)
					{
						transition5.AddSource(lordToil);
					}
					FloatRange floatRange = new FloatRange(0.25f, 0.35f);
					float randomInRange = floatRange.RandomInRange;
					transition5.AddTrigger(new Trigger_FractionColonyDamageTaken(randomInRange, 900f));
					transition5.AddPreAction(new TransitionAction_Message("MessageRaidersSatisfiedLeaving".Translate(new object[]
					{
						this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(),
						this.assaulterFaction.Name
					}), null, 1f));
					stateGraph.AddTransition(transition5);
				}
				if (this.canKidnap)
				{
					LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Kidnap().CreateGraph()).StartingToil;
					Transition transition6 = new Transition(lordToil2, startingToil, false, true);
					if (lordToil != null)
					{
						transition6.AddSource(lordToil);
					}
					transition6.AddPreAction(new TransitionAction_Message("MessageRaidersKidnapping".Translate(new object[]
					{
						this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(),
						this.assaulterFaction.Name
					}), null, 1f));
					transition6.AddTrigger(new Trigger_KidnapVictimPresent());
					stateGraph.AddTransition(transition6);
				}
				if (this.canSteal)
				{
					LordToil startingToil2 = stateGraph.AttachSubgraph(new LordJob_Steal().CreateGraph()).StartingToil;
					Transition transition7 = new Transition(lordToil2, startingToil2, false, true);
					if (lordToil != null)
					{
						transition7.AddSource(lordToil);
					}
					transition7.AddPreAction(new TransitionAction_Message("MessageRaidersStealing".Translate(new object[]
					{
						this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(),
						this.assaulterFaction.Name
					}), null, 1f));
					transition7.AddTrigger(new Trigger_HighValueThingsAround());
					stateGraph.AddTransition(transition7);
				}
			}
			Transition transition8 = new Transition(lordToil2, lordToil_ExitMap, false, true);
			if (lordToil != null)
			{
				transition8.AddSource(lordToil);
			}
			transition8.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			transition8.AddPreAction(new TransitionAction_Message("MessageRaidersLeaving".Translate(new object[]
			{
				this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(),
				this.assaulterFaction.Name
			}), null, 1f));
			stateGraph.AddTransition(transition8);
			return stateGraph;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00049E44 File Offset: 0x00048244
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.assaulterFaction, "assaulterFaction", false);
			Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", true, false);
			Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
			Scribe_Values.Look<bool>(ref this.sappers, "sappers", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
			Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", true, false);
		}

		// Token: 0x04000331 RID: 817
		private Faction assaulterFaction;

		// Token: 0x04000332 RID: 818
		private bool canKidnap = true;

		// Token: 0x04000333 RID: 819
		private bool canTimeoutOrFlee = true;

		// Token: 0x04000334 RID: 820
		private bool sappers = false;

		// Token: 0x04000335 RID: 821
		private bool useAvoidGridSmart = false;

		// Token: 0x04000336 RID: 822
		private bool canSteal = true;

		// Token: 0x04000337 RID: 823
		private static readonly IntRange AssaultTimeBeforeGiveUp = new IntRange(26000, 38000);

		// Token: 0x04000338 RID: 824
		private static readonly IntRange SapTimeBeforeGiveUp = new IntRange(33000, 38000);
	}
}
