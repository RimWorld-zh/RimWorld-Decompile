using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public class VisitorGiftForPlayerUtility
	{
		private const float ExtraChanceFactor = 0.75f;

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

		public VisitorGiftForPlayerUtility()
		{
		}

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

		public static List<Thing> GenerateGifts(Faction faction, Map map)
		{
			return ThingSetMakerDefOf.VisitorGift.root.Generate();
		}

		private static float PlayerWealthChanceFactor(Map map)
		{
			return VisitorGiftForPlayerUtility.PlayerWealthChanceFactorCurve.Evaluate(map.wealthWatcher.WealthTotal);
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static VisitorGiftForPlayerUtility()
		{
		}
	}
}
