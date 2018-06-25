using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000444 RID: 1092
	public class VoluntarilyJoinableLordsStarter : IExposable
	{
		// Token: 0x04000B78 RID: 2936
		private Map map;

		// Token: 0x04000B79 RID: 2937
		private int lastLordStartTick = -999999;

		// Token: 0x04000B7A RID: 2938
		private bool startPartyASAP = false;

		// Token: 0x04000B7B RID: 2939
		private const int CheckStartPartyIntervalTicks = 5000;

		// Token: 0x04000B7C RID: 2940
		private const float StartPartyMTBDays = 40f;

		// Token: 0x060012E9 RID: 4841 RVA: 0x000A3834 File Offset: 0x000A1C34
		public VoluntarilyJoinableLordsStarter(Map map)
		{
			this.map = map;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x000A3858 File Offset: 0x000A1C58
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

		// Token: 0x060012EB RID: 4843 RVA: 0x000A38F0 File Offset: 0x000A1CF0
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

		// Token: 0x060012EC RID: 4844 RVA: 0x000A39AF File Offset: 0x000A1DAF
		public void VoluntarilyJoinableLordsStarterTick()
		{
			this.Tick_TryStartParty();
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x000A39B8 File Offset: 0x000A1DB8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLordStartTick, "lastLordStartTick", 0, false);
			Scribe_Values.Look<bool>(ref this.startPartyASAP, "startPartyASAP", false, false);
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x000A39E0 File Offset: 0x000A1DE0
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
	}
}
