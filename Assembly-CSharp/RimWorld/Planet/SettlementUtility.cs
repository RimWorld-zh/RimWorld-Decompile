using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000604 RID: 1540
	public class SettlementUtility
	{
		// Token: 0x06001EE7 RID: 7911 RVA: 0x0010CDA8 File Offset: 0x0010B1A8
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

		// Token: 0x06001EE8 RID: 7912 RVA: 0x0010CE38 File Offset: 0x0010B238
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

		// Token: 0x06001EE9 RID: 7913 RVA: 0x0010CE98 File Offset: 0x0010B298
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

		// Token: 0x06001EEA RID: 7914 RVA: 0x0010CF7C File Offset: 0x0010B37C
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
