using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02000607 RID: 1543
	[StaticConstructorOnStartup]
	public static class SettlementAbandonUtility
	{
		// Token: 0x06001EE6 RID: 7910 RVA: 0x0010C94C File Offset: 0x0010AD4C
		public static Command AbandonCommand(MapParent settlement)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandAbandonHome".Translate();
			command_Action.defaultDesc = "CommandAbandonHomeDesc".Translate();
			command_Action.icon = SettlementAbandonUtility.AbandonCommandTex;
			command_Action.action = delegate()
			{
				SettlementAbandonUtility.TryAbandonViaInterface(settlement);
			};
			if (SettlementAbandonUtility.AllColonistsThere(settlement))
			{
				command_Action.Disable("CommandAbandonHomeFailAllColonistsThere".Translate());
			}
			return command_Action;
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x0010C9D4 File Offset: 0x0010ADD4
		public static bool AllColonistsThere(MapParent settlement)
		{
			return !CaravanUtility.PlayerHasAnyCaravan() && !Find.Maps.Any((Map x) => x.info.parent != settlement && x.mapPawns.FreeColonistsSpawned.Any<Pawn>());
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x0010CA1C File Offset: 0x0010AE1C
		public static void TryAbandonViaInterface(MapParent settlement)
		{
			Map map = settlement.Map;
			if (map == null)
			{
				SettlementAbandonUtility.Abandon(settlement);
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				IEnumerable<Pawn> source = map.mapPawns.PawnsInFaction(Faction.OfPlayer);
				if (source.Count<Pawn>() != 0)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (Pawn pawn in from x in source
					orderby x.IsColonist descending
					select x)
					{
						if (stringBuilder2.Length > 0)
						{
							stringBuilder2.AppendLine();
						}
						stringBuilder2.Append("    " + pawn.LabelCap);
					}
					stringBuilder.Append("ConfirmAbandonHomeWithColonyPawns".Translate(new object[]
					{
						stringBuilder2
					}));
				}
				PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(map.mapPawns.AllPawns, PawnDiedOrDownedThoughtsKind.Banished, stringBuilder, null, "\n\n" + "ConfirmAbandonHomeNegativeThoughts_Everyone".Translate(), "ConfirmAbandonHomeNegativeThoughts");
				if (stringBuilder.Length == 0)
				{
					SettlementAbandonUtility.Abandon(settlement);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), delegate
					{
						SettlementAbandonUtility.Abandon(settlement);
					}, false, null));
				}
			}
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x0010CBC0 File Offset: 0x0010AFC0
		private static void Abandon(MapParent settlement)
		{
			Find.WorldObjects.Remove(settlement);
			FactionBase factionBase = settlement as FactionBase;
			if (factionBase != null)
			{
				SettlementAbandonUtility.AddAbandonedBase(factionBase);
			}
			Find.GameEnder.CheckOrUpdateGameOver();
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x0010CBF8 File Offset: 0x0010AFF8
		private static void AddAbandonedBase(FactionBase factionBase)
		{
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedBase);
			worldObject.Tile = factionBase.Tile;
			worldObject.SetFaction(factionBase.Faction);
			Find.WorldObjects.Add(worldObject);
		}

		// Token: 0x0400122C RID: 4652
		private static readonly Texture2D AbandonCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome", true);
	}
}
