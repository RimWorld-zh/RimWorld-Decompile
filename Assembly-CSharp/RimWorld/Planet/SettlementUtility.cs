using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000608 RID: 1544
	public class SettlementUtility
	{
		// Token: 0x06001EEE RID: 7918 RVA: 0x0010CCDC File Offset: 0x0010B0DC
		public static bool IsPlayerAttackingAnySettlementOf(Faction faction)
		{
			bool result;
			if (faction == Faction.OfPlayer)
			{
				result = false;
			}
			else if (!faction.HostileTo(Faction.OfPlayer))
			{
				result = false;
			}
			else
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Settlement settlement = maps[i].info.parent as Settlement;
					if (settlement != null && settlement.Faction == faction)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x0010CD6C File Offset: 0x0010B16C
		public static void Attack(Caravan caravan, Settlement settlement)
		{
			if (!settlement.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					SettlementUtility.AttackNow(caravan, settlement);
				}, "GeneratingMapForNewEncounter", false, null);
			}
			else
			{
				SettlementUtility.AttackNow(caravan, settlement);
			}
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0010CDCC File Offset: 0x0010B1CC
		private static void AttackNow(Caravan caravan, Settlement settlement)
		{
			Pawn t = caravan.PawnsListForReading[0];
			bool flag = !settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(settlement.Tile, null);
			string label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			string text = "LetterCaravanEnteredEnemyBase".Translate(new object[]
			{
				caravan.Label,
				settlement.Label
			}).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked(settlement, ref text);
			if (flag)
			{
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsSettlement".Translate(new object[]
				{
					Faction.OfPlayer.def.pawnsPlural
				}), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, t, settlement.Faction, null);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, true, null);
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x0010CEB0 File Offset: 0x0010B2B0
		public static void AffectRelationsOnAttacked(Settlement settlement, ref string letterText)
		{
			if (settlement.Faction != null && settlement.Faction != Faction.OfPlayer)
			{
				FactionRelationKind playerRelationKind = settlement.Faction.PlayerRelationKind;
				if (!settlement.Faction.HostileTo(Faction.OfPlayer))
				{
					settlement.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
				}
				else if (settlement.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -50, false, false, null, null))
				{
					if (!letterText.NullOrEmpty())
					{
						letterText += "\n\n";
					}
					letterText = letterText + "RelationsWith".Translate(new object[]
					{
						settlement.Faction.Name
					}) + ": " + -50.ToStringWithSign();
				}
				settlement.Faction.TryAppendRelationKindChangedInfo(ref letterText, playerRelationKind, settlement.Faction.PlayerRelationKind, null);
			}
		}
	}
}
