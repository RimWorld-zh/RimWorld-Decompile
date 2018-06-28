using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

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

		public static bool AllColonistsThere(MapParent settlement)
		{
			return !CaravanUtility.PlayerHasAnyCaravan() && !Find.Maps.Any((Map x) => x.info.parent != settlement && x.mapPawns.FreeColonistsSpawned.Any<Pawn>());
		}

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

		private static void Abandon(MapParent settlement)
		{
			Find.WorldObjects.Remove(settlement);
			Settlement settlement2 = settlement as Settlement;
			if (settlement2 != null)
			{
				SettlementAbandonUtility.AddAbandonedSettlement(settlement2);
			}
			Find.GameEnder.CheckOrUpdateGameOver();
		}

		private static void AddAbandonedSettlement(Settlement factionBase)
		{
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedSettlement);
			worldObject.Tile = factionBase.Tile;
			worldObject.SetFaction(factionBase.Faction);
			Find.WorldObjects.Add(worldObject);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SettlementAbandonUtility()
		{
		}

		[CompilerGenerated]
		private static bool <TryAbandonViaInterface>m__0(Pawn x)
		{
			return x.IsColonist;
		}

		[CompilerGenerated]
		private sealed class <AbandonCommand>c__AnonStorey0
		{
			internal MapParent settlement;

			public <AbandonCommand>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				SettlementAbandonUtility.TryAbandonViaInterface(this.settlement);
			}
		}

		[CompilerGenerated]
		private sealed class <AllColonistsThere>c__AnonStorey1
		{
			internal MapParent settlement;

			public <AllColonistsThere>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Map x)
			{
				return x.info.parent != this.settlement && x.mapPawns.FreeColonistsSpawned.Any<Pawn>();
			}
		}

		[CompilerGenerated]
		private sealed class <TryAbandonViaInterface>c__AnonStorey2
		{
			internal MapParent settlement;

			public <TryAbandonViaInterface>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				SettlementAbandonUtility.Abandon(this.settlement);
			}
		}
	}
}
