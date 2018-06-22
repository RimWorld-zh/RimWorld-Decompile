using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000442 RID: 1090
	public class VoluntarilyJoinableLordsStarter : IExposable
	{
		// Token: 0x060012E6 RID: 4838 RVA: 0x000A34E4 File Offset: 0x000A18E4
		public VoluntarilyJoinableLordsStarter(Map map)
		{
			this.map = map;
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x000A3508 File Offset: 0x000A1908
		public bool TryStartMarriageCeremony(Pawn firstFiance, Pawn secondFiance)
		{
			IntVec3 intVec;
			bool result;
			if (!RCellFinder.TryFindMarriageSite(firstFiance, secondFiance, out intVec))
			{
				result = false;
			}
			else
			{
				LordMaker.MakeNewLord(firstFiance.Faction, new LordJob_Joinable_MarriageCeremony(firstFiance, secondFiance, intVec), this.map, null);
				Messages.Message("MessageNewMarriageCeremony".Translate(new object[]
				{
					firstFiance.LabelShort,
					secondFiance.LabelShort
				}), new TargetInfo(intVec, this.map, false), MessageTypeDefOf.PositiveEvent, true);
				this.lastLordStartTick = Find.TickManager.TicksGame;
				result = true;
			}
			return result;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x000A35A0 File Offset: 0x000A19A0
		public bool TryStartParty()
		{
			Pawn pawn = PartyUtility.FindRandomPartyOrganizer(Faction.OfPlayer, this.map);
			bool result;
			IntVec3 intVec;
			if (pawn == null)
			{
				result = false;
			}
			else if (!RCellFinder.TryFindPartySpot(pawn, out intVec))
			{
				result = false;
			}
			else
			{
				LordMaker.MakeNewLord(pawn.Faction, new LordJob_Joinable_Party(intVec, pawn), this.map, null);
				Find.LetterStack.ReceiveLetter("LetterLabelNewParty".Translate(), "LetterNewParty".Translate(new object[]
				{
					pawn.LabelShort
				}), LetterDefOf.PositiveEvent, new TargetInfo(intVec, this.map, false), null, null);
				this.lastLordStartTick = Find.TickManager.TicksGame;
				this.startPartyASAP = false;
				result = true;
			}
			return result;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x000A365F File Offset: 0x000A1A5F
		public void VoluntarilyJoinableLordsStarterTick()
		{
			this.Tick_TryStartParty();
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x000A3668 File Offset: 0x000A1A68
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLordStartTick, "lastLordStartTick", 0, false);
			Scribe_Values.Look<bool>(ref this.startPartyASAP, "startPartyASAP", false, false);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x000A3690 File Offset: 0x000A1A90
		private void Tick_TryStartParty()
		{
			if (this.map.IsPlayerHome)
			{
				if (Find.TickManager.TicksGame % 5000 == 0)
				{
					if (Rand.MTBEventOccurs(40f, 60000f, 5000f))
					{
						this.startPartyASAP = true;
					}
					if (this.startPartyASAP && Find.TickManager.TicksGame - this.lastLordStartTick >= 600000 && PartyUtility.AcceptableGameConditionsToStartParty(this.map))
					{
						this.TryStartParty();
					}
				}
			}
		}

		// Token: 0x04000B75 RID: 2933
		private Map map;

		// Token: 0x04000B76 RID: 2934
		private int lastLordStartTick = -999999;

		// Token: 0x04000B77 RID: 2935
		private bool startPartyASAP = false;

		// Token: 0x04000B78 RID: 2936
		private const int CheckStartPartyIntervalTicks = 5000;

		// Token: 0x04000B79 RID: 2937
		private const float StartPartyMTBDays = 40f;
	}
}
