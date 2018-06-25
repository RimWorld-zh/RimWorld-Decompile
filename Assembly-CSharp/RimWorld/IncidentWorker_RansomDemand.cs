using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200033C RID: 828
	public class IncidentWorker_RansomDemand : IncidentWorker
	{
		// Token: 0x040008E4 RID: 2276
		private const int TimeoutTicks = 60000;

		// Token: 0x040008E5 RID: 2277
		private static List<Pawn> candidates = new List<Pawn>();

		// Token: 0x06000E1C RID: 3612 RVA: 0x0007822C File Offset: 0x0007662C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return CommsConsoleUtility.PlayerHasPoweredCommsConsole(map) && this.RandomKidnappedColonist() != null && base.CanFireNowSub(parms);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00078278 File Offset: 0x00076678
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn = this.RandomKidnappedColonist();
			bool result;
			if (pawn == null)
			{
				result = false;
			}
			else
			{
				Faction faction = this.FactionWhichKidnapped(pawn);
				int num = this.RandomFee(pawn);
				ChoiceLetter_RansomDemand choiceLetter_RansomDemand = (ChoiceLetter_RansomDemand)LetterMaker.MakeLetter(this.def.letterLabel, "RansomDemand".Translate(new object[]
				{
					pawn.LabelShort,
					faction.Name,
					num
				}).AdjustedFor(pawn, "PAWN"), this.def.letterDef);
				choiceLetter_RansomDemand.title = "RansomDemandTitle".Translate(new object[]
				{
					map.Parent.Label
				});
				choiceLetter_RansomDemand.radioMode = true;
				choiceLetter_RansomDemand.kidnapped = pawn;
				choiceLetter_RansomDemand.faction = faction;
				choiceLetter_RansomDemand.map = map;
				choiceLetter_RansomDemand.fee = num;
				choiceLetter_RansomDemand.relatedFaction = faction;
				choiceLetter_RansomDemand.StartTimeout(60000);
				Find.LetterStack.ReceiveLetter(choiceLetter_RansomDemand, null);
				result = true;
			}
			return result;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00078388 File Offset: 0x00076788
		private Pawn RandomKidnappedColonist()
		{
			IncidentWorker_RansomDemand.candidates.Clear();
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				List<Pawn> kidnappedPawnsListForReading = allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading;
				for (int j = 0; j < kidnappedPawnsListForReading.Count; j++)
				{
					if (kidnappedPawnsListForReading[j].Faction == Faction.OfPlayer)
					{
						IncidentWorker_RansomDemand.candidates.Add(kidnappedPawnsListForReading[j]);
					}
				}
			}
			List<Letter> lettersListForReading = Find.LetterStack.LettersListForReading;
			for (int k = 0; k < lettersListForReading.Count; k++)
			{
				ChoiceLetter_RansomDemand choiceLetter_RansomDemand = lettersListForReading[k] as ChoiceLetter_RansomDemand;
				if (choiceLetter_RansomDemand != null)
				{
					IncidentWorker_RansomDemand.candidates.Remove(choiceLetter_RansomDemand.kidnapped);
				}
			}
			Pawn pawn;
			Pawn result;
			if (!IncidentWorker_RansomDemand.candidates.TryRandomElement(out pawn))
			{
				result = null;
			}
			else
			{
				IncidentWorker_RansomDemand.candidates.Clear();
				result = pawn;
			}
			return result;
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00078498 File Offset: 0x00076898
		private Faction FactionWhichKidnapped(Pawn pawn)
		{
			return Find.FactionManager.AllFactionsListForReading.Find((Faction x) => x.kidnapped.KidnappedPawnsListForReading.Contains(pawn));
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x000784D8 File Offset: 0x000768D8
		private int RandomFee(Pawn pawn)
		{
			return (int)(pawn.MarketValue * DiplomacyTuning.RansomFeeMarketValueFactorRange.RandomInRange);
		}
	}
}
