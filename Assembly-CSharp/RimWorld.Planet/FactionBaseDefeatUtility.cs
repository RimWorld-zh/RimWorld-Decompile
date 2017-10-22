using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	public static class FactionBaseDefeatUtility
	{
		public static void CheckDefeated(FactionBase factionBase)
		{
			if (factionBase.Faction != Faction.OfPlayer)
			{
				Map map = factionBase.Map;
				if (map != null && FactionBaseDefeatUtility.IsDefeated(map, factionBase.Faction))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("LetterFactionBaseDefeated".Translate(factionBase.Label, TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(60000)));
					if (!FactionBaseDefeatUtility.HasAnyOtherBase(factionBase))
					{
						factionBase.Faction.defeated = true;
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						stringBuilder.Append("LetterFactionBaseDefeated_FactionDestroyed".Translate(factionBase.Faction.Name));
					}
					Find.LetterStack.ReceiveLetter("LetterLabelFactionBaseDefeated".Translate(), stringBuilder.ToString(), LetterDefOf.PositiveEvent, new GlobalTargetInfo(factionBase.Tile), (string)null);
					DestroyedFactionBase destroyedFactionBase = (DestroyedFactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.DestroyedFactionBase);
					destroyedFactionBase.Tile = factionBase.Tile;
					Find.WorldObjects.Add(destroyedFactionBase);
					map.info.parent = destroyedFactionBase;
					Find.WorldObjects.Remove(factionBase);
					((WorldObject)destroyedFactionBase).GetComponent<TimedForcedExit>().StartForceExitAndRemoveMapCountdown();
					TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, map.mapPawns.FreeColonists.RandomElement());
				}
			}
		}

		private static bool IsDefeated(Map map, Faction faction)
		{
			List<Pawn> list = map.mapPawns.SpawnedPawnsInFaction(faction);
			int num = 0;
			bool result;
			while (true)
			{
				if (num < list.Count)
				{
					Pawn pawn = list[num];
					if (pawn.RaceProps.Humanlike && GenHostility.IsActiveThreatToPlayer(pawn))
					{
						result = false;
						break;
					}
					num++;
					continue;
				}
				result = true;
				break;
			}
			return result;
		}

		private static bool HasAnyOtherBase(FactionBase defeatedFactionBase)
		{
			List<FactionBase> factionBases = Find.WorldObjects.FactionBases;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < factionBases.Count)
				{
					FactionBase factionBase = factionBases[num];
					if (factionBase.Faction == defeatedFactionBase.Faction && factionBase != defeatedFactionBase)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
