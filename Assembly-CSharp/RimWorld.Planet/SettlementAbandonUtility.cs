using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public static class SettlementAbandonUtility
	{
		private static readonly Texture2D AbandonCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome", true);

		public static Command AbandonCommand(MapParent settlement)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandAbandonHome".Translate();
			command_Action.defaultDesc = "CommandAbandonHomeDesc".Translate();
			command_Action.icon = SettlementAbandonUtility.AbandonCommandTex;
			command_Action.action = (Action)delegate()
			{
				SettlementAbandonUtility.TryAbandonViaInterface(settlement);
			};
			if (SettlementAbandonUtility.AllColonistsThere(settlement))
			{
				command_Action.Disable("CommandAbandonHomeFailAllColonistsThere".Translate());
			}
			return command_Action;
		}

		public static bool AllColonistsThere(MapParent settlement)
		{
			return !CaravanUtility.PlayerHasAnyCaravan() && !Find.Maps.Any((Predicate<Map>)((Map x) => x.info.parent != settlement && x.mapPawns.FreeColonistsSpawned.Any()));
		}

		public static void TryAbandonViaInterface(MapParent settlement)
		{
			Map map = settlement.Map;
			if (map == null)
			{
				SettlementAbandonUtility.Abandon(settlement);
				SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				IEnumerable<Pawn> source = map.mapPawns.PawnsInFaction(Faction.OfPlayer);
				if (source.Count() != 0)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (Pawn item in from x in source
					orderby x.IsColonist descending
					select x)
					{
						if (stringBuilder2.Length > 0)
						{
							stringBuilder2.AppendLine();
						}
						stringBuilder2.Append("    " + item.LabelCap);
					}
					stringBuilder.Append("ConfirmAbandonHomeWithColonyPawns".Translate(stringBuilder2));
				}
				PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(map.mapPawns.AllPawns, PawnDiedOrDownedThoughtsKind.Banished, stringBuilder, (string)null, "\n\n" + "ConfirmAbandonHomeNegativeThoughts_Everyone".Translate(), "ConfirmAbandonHomeNegativeThoughts");
				if (stringBuilder.Length == 0)
				{
					SettlementAbandonUtility.Abandon(settlement);
					SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
				}
				else
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), (Action)delegate()
					{
						SettlementAbandonUtility.Abandon(settlement);
					}, false, (string)null));
				}
			}
		}

		private static void Abandon(MapParent settlement)
		{
			Find.WorldObjects.Remove(settlement);
			FactionBase factionBase = settlement as FactionBase;
			if (factionBase != null)
			{
				SettlementAbandonUtility.AddAbandonedBase(factionBase);
			}
			Find.GameEnder.CheckGameOver();
		}

		private static void AddAbandonedBase(FactionBase factionBase)
		{
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedFactionBase);
			worldObject.Tile = factionBase.Tile;
			worldObject.SetFaction(factionBase.Faction);
			Find.WorldObjects.Add(worldObject);
		}
	}
}
