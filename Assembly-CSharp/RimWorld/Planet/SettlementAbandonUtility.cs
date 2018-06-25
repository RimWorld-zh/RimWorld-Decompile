using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02000605 RID: 1541
	[StaticConstructorOnStartup]
	public static class SettlementAbandonUtility
	{
		// Token: 0x0400122D RID: 4653
		private static readonly Texture2D AbandonCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome", true);

		// Token: 0x06001EE2 RID: 7906 RVA: 0x0010CDC4 File Offset: 0x0010B1C4
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
			command_Action.order = 30f;
			if (SettlementAbandonUtility.AllColonistsThere(settlement))
			{
				command_Action.Disable("CommandAbandonHomeFailAllColonistsThere".Translate());
			}
			return command_Action;
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0010CE58 File Offset: 0x0010B258
		public static bool AllColonistsThere(MapParent settlement)
		{
			return !CaravanUtility.PlayerHasAnyCaravan() && !Find.Maps.Any((Map x) => x.info.parent != settlement && x.mapPawns.FreeColonistsSpawned.Any<Pawn>());
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0010CEA0 File Offset: 0x0010B2A0
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

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0010D044 File Offset: 0x0010B444
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

		// Token: 0x06001EE6 RID: 7910 RVA: 0x0010D07C File Offset: 0x0010B47C
		private static void AddAbandonedBase(FactionBase factionBase)
		{
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedBase);
			worldObject.Tile = factionBase.Tile;
			worldObject.SetFaction(factionBase.Faction);
			Find.WorldObjects.Add(worldObject);
		}
	}
}
