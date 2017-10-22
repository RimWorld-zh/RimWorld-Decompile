using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordJob_Joinable_MarriageCeremony : LordJob_VoluntarilyJoinable
	{
		public Pawn firstPawn;

		public Pawn secondPawn;

		private IntVec3 spot;

		private Trigger_TicksPassed afterPartyTimeoutTrigger;

		private const int TicksPerPartyPulse = 2200;

		public override bool LostImportantReferenceDuringLoading
		{
			get
			{
				return this.firstPawn == null || this.secondPawn == null;
			}
		}

		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		public LordJob_Joinable_MarriageCeremony()
		{
		}

		public LordJob_Joinable_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
		}

		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Party lordToil_Party = new LordToil_Party(this.spot, 2200);
			stateGraph.AddToil(lordToil_Party);
			LordToil_MarriageCeremony lordToil_MarriageCeremony = new LordToil_MarriageCeremony(this.firstPawn, this.secondPawn, this.spot);
			stateGraph.AddToil(lordToil_MarriageCeremony);
			LordToil_Party lordToil_Party2 = new LordToil_Party(this.spot, 2200);
			stateGraph.AddToil(lordToil_Party2);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(lordToil_Party, lordToil_MarriageCeremony);
			transition.AddTrigger(new Trigger_TickCondition((Func<bool>)(() => base.lord.ticksInToil >= 5000 && this.AreFiancesInPartyArea()), 1));
			transition.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyStarts".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort), MessageTypeDefOf.PositiveEvent, (Thing)this.firstPawn));
			stateGraph.AddTransition(transition);
			Transition transition2 = new Transition(lordToil_MarriageCeremony, lordToil_Party2);
			transition2.AddTrigger(new Trigger_TickCondition((Func<bool>)(() => this.firstPawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, this.secondPawn)), 1));
			transition2.AddPreAction(new TransitionAction_Message("MessageNewlyMarried".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort), MessageTypeDefOf.PositiveEvent, new TargetInfo(this.spot, base.Map, false)));
			transition2.AddPreAction(new TransitionAction_Custom((Action)delegate
			{
				this.AddAttendedWeddingThoughts();
			}));
			stateGraph.AddTransition(transition2);
			Transition transition3 = new Transition(lordToil_Party2, lordToil_End);
			transition3.AddTrigger(new Trigger_TickCondition((Func<bool>)(() => this.ShouldAfterPartyBeCalledOff()), 1));
			transition3.AddTrigger(new Trigger_PawnLostViolently());
			transition3.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyCalledOff".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false)));
			stateGraph.AddTransition(transition3);
			this.afterPartyTimeoutTrigger = new Trigger_TicksPassed(7500);
			Transition transition4 = new Transition(lordToil_Party2, lordToil_End);
			transition4.AddTrigger(this.afterPartyTimeoutTrigger);
			transition4.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyAfterPartyFinished".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort), MessageTypeDefOf.PositiveEvent, (Thing)this.firstPawn));
			stateGraph.AddTransition(transition4);
			Transition transition5 = new Transition(lordToil_MarriageCeremony, lordToil_End);
			transition5.AddSource(lordToil_Party);
			transition5.AddTrigger(new Trigger_TickCondition((Func<bool>)(() => base.lord.ticksInToil >= 120000 && (this.firstPawn.Drafted || this.secondPawn.Drafted || !this.firstPawn.Position.InHorDistOf(this.spot, 4f) || !this.secondPawn.Position.InHorDistOf(this.spot, 4f))), 1));
			transition5.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyCalledOff".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false)));
			stateGraph.AddTransition(transition5);
			Transition transition6 = new Transition(lordToil_MarriageCeremony, lordToil_End);
			transition6.AddSource(lordToil_Party);
			transition6.AddTrigger(new Trigger_TickCondition((Func<bool>)(() => this.ShouldCeremonyBeCalledOff()), 1));
			transition6.AddTrigger(new Trigger_PawnLostViolently());
			transition6.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyCalledOff".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false)));
			stateGraph.AddTransition(transition6);
			return stateGraph;
		}

		private bool AreFiancesInPartyArea()
		{
			return (byte)((base.lord.ownedPawns.Contains(this.firstPawn) && base.lord.ownedPawns.Contains(this.secondPawn)) ? ((this.firstPawn.Map == base.Map && PartyUtility.InPartyArea(this.firstPawn.Position, this.spot, base.Map)) ? ((this.secondPawn.Map == base.Map && PartyUtility.InPartyArea(this.secondPawn.Position, this.spot, base.Map)) ? 1 : 0) : 0) : 0) != 0;
		}

		private bool ShouldCeremonyBeCalledOff()
		{
			return (byte)((this.firstPawn.Destroyed || this.secondPawn.Destroyed) ? 1 : ((!this.firstPawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.secondPawn)) ? 1 : ((this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) ? 1 : ((!MarriageCeremonyUtility.AcceptableGameConditionsToContinueCeremony(base.Map) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.firstPawn) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.secondPawn)) ? 1 : 0)))) != 0;
		}

		private bool ShouldAfterPartyBeCalledOff()
		{
			return (byte)((this.firstPawn.Destroyed || this.secondPawn.Destroyed) ? 1 : ((this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) ? 1 : ((!PartyUtility.AcceptableGameConditionsToContinueParty(base.Map)) ? 1 : 0))) != 0;
		}

		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			float result;
			if (this.IsFiance(p))
			{
				result = (float)(MarriageCeremonyUtility.FianceCanContinueCeremony(p) ? VoluntarilyJoinableLordJobJoinPriorities.MarriageCeremonyFiance : 0.0);
			}
			else if (this.IsGuest(p))
			{
				if (!MarriageCeremonyUtility.ShouldGuestKeepAttendingCeremony(p))
				{
					result = 0f;
				}
				else
				{
					if (!base.lord.ownedPawns.Contains(p))
					{
						if (this.IsCeremonyAboutToEnd())
						{
							result = 0f;
							goto IL_00e4;
						}
						LordToil_MarriageCeremony lordToil_MarriageCeremony = base.lord.CurLordToil as LordToil_MarriageCeremony;
						IntVec3 intVec = default(IntVec3);
						if (lordToil_MarriageCeremony != null && !SpectatorCellFinder.TryFindSpectatorCellFor(p, lordToil_MarriageCeremony.Data.spectateRect, base.Map, out intVec, lordToil_MarriageCeremony.Data.spectateRectAllowedSides, 1, (List<IntVec3>)null))
						{
							result = 0f;
							goto IL_00e4;
						}
					}
					result = VoluntarilyJoinableLordJobJoinPriorities.MarriageCeremonyGuest;
				}
			}
			else
			{
				result = 0f;
			}
			goto IL_00e4;
			IL_00e4:
			return result;
		}

		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.firstPawn, "firstPawn", false);
			Scribe_References.Look<Pawn>(ref this.secondPawn, "secondPawn", false);
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
		}

		public override string GetReport()
		{
			return "LordReportAttendingMarriageCeremony".Translate();
		}

		private bool IsCeremonyAboutToEnd()
		{
			return (byte)((this.afterPartyTimeoutTrigger.TicksLeft < 1200) ? 1 : 0) != 0;
		}

		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		private bool IsGuest(Pawn p)
		{
			return p.RaceProps.Humanlike && p != this.firstPawn && p != this.secondPawn && (p.Faction == this.firstPawn.Faction || p.Faction == this.secondPawn.Faction);
		}

		private void AddAttendedWeddingThoughts()
		{
			List<Pawn> ownedPawns = base.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (ownedPawns[i] != this.firstPawn && ownedPawns[i] != this.secondPawn && (this.firstPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f) || this.secondPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f)))
				{
					ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AttendedWedding, null);
				}
			}
		}
	}
}
