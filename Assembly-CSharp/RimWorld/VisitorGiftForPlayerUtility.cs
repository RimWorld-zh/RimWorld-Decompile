using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000565 RID: 1381
	[HasDebugOutput]
	public class VisitorGiftForPlayerUtility
	{
		// Token: 0x06001A1D RID: 6685 RVA: 0x000E2804 File Offset: 0x000E0C04
		public static float ChanceToLeaveGift(Faction faction, Map map)
		{
			float result;
			if (faction.IsPlayer)
			{
				result = 0f;
			}
			else
			{
				result = VisitorGiftForPlayerUtility.PlayerWealthChanceFactor(map) * VisitorGiftForPlayerUtility.FactionRelationsChanceFactor(faction) * 0.75f;
			}
			return result;
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x000E2844 File Offset: 0x000E0C44
		public static List<Thing> GenerateGifts(Faction faction, Map map)
		{
			return ThingSetMakerDefOf.VisitorGift.root.Generate();
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x000E2868 File Offset: 0x000E0C68
		private static float PlayerWealthChanceFactor(Map map)
		{
			return VisitorGiftForPlayerUtility.PlayerWealthChanceFactorCurve.Evaluate(map.wealthWatcher.WealthTotal);
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x000E2894 File Offset: 0x000E0C94
		private static float FactionRelationsChanceFactor(Faction faction)
		{
			float result;
			if (faction.HostileTo(Faction.OfPlayer))
			{
				result = 0f;
			}
			else
			{
				result = VisitorGiftForPlayerUtility.GoodwillChanceFactorCurve.Evaluate((float)faction.PlayerGoodwill);
			}
			return result;
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x000E28D8 File Offset: 0x000E0CD8
		[DebugOutput]
		private static void VisitorGiftChance()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Current wealth factor (wealth=" + Find.CurrentMap.wealthWatcher.WealthTotal.ToString("F0") + "): ");
			stringBuilder.AppendLine(VisitorGiftForPlayerUtility.PlayerWealthChanceFactor(Find.CurrentMap).ToStringPercent());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Chance per faction:");
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.IsPlayer && !faction.HostileTo(Faction.OfPlayer) && !faction.def.hidden)
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						faction.Name,
						" (",
						faction.PlayerGoodwill.ToStringWithSign(),
						", ",
						faction.PlayerRelationKind.GetLabel(),
						")"
					}));
					stringBuilder.Append(": " + VisitorGiftForPlayerUtility.ChanceToLeaveGift(faction, Find.CurrentMap).ToStringPercent());
					stringBuilder.AppendLine(" (rels factor: " + VisitorGiftForPlayerUtility.FactionRelationsChanceFactor(faction).ToStringPercent() + ")");
				}
			}
			int num = 0;
			for (int i = 0; i < 6; i++)
			{
				Dictionary<IIncidentTarget, int> dictionary;
				int[] array;
				List<Pair<IncidentDef, IncidentParms>> list;
				int num2;
				StorytellerUtility.DebugGetFutureIncidents(60, true, out dictionary, out array, out list, out num2, null, null);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].First == IncidentDefOf.VisitorGroup || list[j].First == IncidentDefOf.TraderCaravanArrival)
					{
						Faction faction2 = list[j].Second.faction ?? Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined);
						if (Rand.Chance(VisitorGiftForPlayerUtility.ChanceToLeaveGift(faction2, Find.CurrentMap)))
						{
							num++;
						}
					}
				}
			}
			float num3 = (float)num / 6f;
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Calculated number of gifts received on average within the next 1 year");
			stringBuilder.AppendLine("(assuming current wealth and faction relations)");
			stringBuilder.Append("  = " + num3.ToString("0.##"));
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x000E2B7C File Offset: 0x000E0F7C
		public static void CheckGiveGift(List<Pawn> pawns, Faction faction)
		{
			if (pawns.Any<Pawn>())
			{
				Pawn pawn = null;
				for (int i = 0; i < pawns.Count; i++)
				{
					if (pawns[i].RaceProps.Humanlike && pawns[i].Faction == faction)
					{
						pawn = pawns[i];
						break;
					}
				}
				if (pawn == null)
				{
					for (int j = 0; j < pawns.Count; j++)
					{
						if (pawns[j].Faction == faction)
						{
							pawn = pawns[j];
							break;
						}
					}
				}
				if (pawn == null)
				{
					pawn = pawns[0];
				}
				List<Thing> list = VisitorGiftForPlayerUtility.GenerateGifts(faction, pawn.Map);
				TargetInfo target = TargetInfo.Invalid;
				for (int k = 0; k < list.Count; k++)
				{
					if (GenPlace.TryPlaceThing(list[k], pawn.Position, pawn.Map, ThingPlaceMode.Near, null, null))
					{
						target = list[k];
					}
					else
					{
						list[k].Destroy(DestroyMode.Vanish);
					}
				}
				if (target.IsValid)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelVisitorsGaveGift".Translate(), "LetterVisitorsGaveGift".Translate(), LetterDefOf.PositiveEvent, target, faction, null);
				}
			}
		}

		// Token: 0x04000F49 RID: 3913
		private const float ExtraChanceFactor = 0.75f;

		// Token: 0x04000F4A RID: 3914
		private static readonly SimpleCurve PlayerWealthChanceFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(6000f, 1f),
				true
			},
			{
				new CurvePoint(80000f, 0.08f),
				true
			}
		};

		// Token: 0x04000F4B RID: 3915
		private static readonly SimpleCurve GoodwillChanceFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 0f),
				true
			},
			{
				new CurvePoint(100f, 1f),
				true
			}
		};
	}
}
