using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FE RID: 1534
	public static class FactionBaseDefeatUtility
	{
		// Token: 0x06001E89 RID: 7817 RVA: 0x0010AEC8 File Offset: 0x001092C8
		public static void CheckDefeated(FactionBase factionBase)
		{
			if (factionBase.Faction != Faction.OfPlayer)
			{
				Map map = factionBase.Map;
				if (map != null && FactionBaseDefeatUtility.IsDefeated(map, factionBase.Faction))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("LetterFactionBaseDefeated".Translate(new object[]
					{
						factionBase.Label,
						TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(60000)
					}));
					if (!FactionBaseDefeatUtility.HasAnyOtherBase(factionBase))
					{
						factionBase.Faction.defeated = true;
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						stringBuilder.Append("LetterFactionBaseDefeated_FactionDestroyed".Translate(new object[]
						{
							factionBase.Faction.Name
						}));
					}
					foreach (Faction faction in Find.FactionManager.AllFactions)
					{
						if (!faction.def.hidden && !faction.IsPlayer && faction != factionBase.Faction && faction.HostileTo(factionBase.Faction))
						{
							FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
							if (faction.TryAffectGoodwillWith(Faction.OfPlayer, 15, false, false, null, null))
							{
								stringBuilder.AppendLine();
								stringBuilder.AppendLine();
								stringBuilder.Append("RelationsWith".Translate(new object[]
								{
									faction.Name
								}) + ": " + 15.ToStringWithSign());
								faction.TryAppendRelationKindChangedInfo(stringBuilder, playerRelationKind, faction.PlayerRelationKind, null);
							}
						}
					}
					Find.LetterStack.ReceiveLetter("LetterLabelFactionBaseDefeated".Translate(), stringBuilder.ToString(), LetterDefOf.PositiveEvent, new GlobalTargetInfo(factionBase.Tile), factionBase.Faction, null);
					DestroyedFactionBase destroyedFactionBase = (DestroyedFactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.DestroyedFactionBase);
					destroyedFactionBase.Tile = factionBase.Tile;
					Find.WorldObjects.Add(destroyedFactionBase);
					map.info.parent = destroyedFactionBase;
					Find.WorldObjects.Remove(factionBase);
					destroyedFactionBase.GetComponent<TimedForcedExit>().StartForceExitAndRemoveMapCountdown();
					TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, new object[]
					{
						map.mapPawns.FreeColonists.RandomElement<Pawn>()
					});
				}
			}
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x0010B138 File Offset: 0x00109538
		private static bool IsDefeated(Map map, Faction faction)
		{
			List<Pawn> list = map.mapPawns.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				if (pawn.RaceProps.Humanlike && GenHostility.IsActiveThreatToPlayer(pawn))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x0010B1A0 File Offset: 0x001095A0
		private static bool HasAnyOtherBase(FactionBase defeatedFactionBase)
		{
			List<FactionBase> factionBases = Find.WorldObjects.FactionBases;
			for (int i = 0; i < factionBases.Count; i++)
			{
				FactionBase factionBase = factionBases[i];
				if (factionBase.Faction == defeatedFactionBase.Faction && factionBase != defeatedFactionBase)
				{
					return true;
				}
			}
			return false;
		}
	}
}
