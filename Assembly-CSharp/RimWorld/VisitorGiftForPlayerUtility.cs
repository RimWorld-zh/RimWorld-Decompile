using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public class VisitorGiftForPlayerUtility
	{
		[CompilerGenerated]
		private static Func<Thing, string> <>f__am$cache0;

		public VisitorGiftForPlayerUtility()
		{
		}

		public static float ChanceToLeaveGift(Faction faction, Map map)
		{
			if (faction.IsPlayer)
			{
				return 0f;
			}
			return 0.25f * VisitorGiftForPlayerUtility.PlayerWealthChanceFactor(map) * VisitorGiftForPlayerUtility.FactionRelationsChanceFactor(faction);
		}

		public static List<Thing> GenerateGifts(Faction faction, Map map)
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = new FloatRange?(DiplomacyTuning.VisitorGiftTotalMarketValueRangeBase * DiplomacyTuning.VisitorGiftTotalMarketValueFactorFromPlayerWealthCurve.Evaluate(map.wealthWatcher.WealthTotal));
			return ThingSetMakerDefOf.VisitorGift.root.Generate(parms);
		}

		private static float PlayerWealthChanceFactor(Map map)
		{
			return DiplomacyTuning.VisitorGiftChanceFactorFromPlayerWealthCurve.Evaluate(map.wealthWatcher.WealthTotal);
		}

		private static float FactionRelationsChanceFactor(Faction faction)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				return 0f;
			}
			return DiplomacyTuning.VisitorGiftChanceFactorFromGoodwillCurve.Evaluate((float)faction.PlayerGoodwill);
		}

		public static void GiveGift(List<Pawn> possibleGivers, Faction faction)
		{
			if (possibleGivers.NullOrEmpty<Pawn>())
			{
				return;
			}
			Pawn pawn = null;
			for (int i = 0; i < possibleGivers.Count; i++)
			{
				if (possibleGivers[i].RaceProps.Humanlike && possibleGivers[i].Faction == faction)
				{
					pawn = possibleGivers[i];
					break;
				}
			}
			if (pawn == null)
			{
				for (int j = 0; j < possibleGivers.Count; j++)
				{
					if (possibleGivers[j].Faction == faction)
					{
						pawn = possibleGivers[j];
						break;
					}
				}
			}
			if (pawn == null)
			{
				pawn = possibleGivers[0];
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
				LetterStack letterStack = Find.LetterStack;
				string label = "LetterLabelVisitorsGaveGift".Translate(new object[]
				{
					pawn.Faction.Name
				});
				string key = "LetterVisitorsGaveGift";
				object[] array = new object[2];
				array[0] = pawn.Faction.def.pawnsPlural;
				array[1] = (from g in list
				select g.LabelCap).ToLineList("   -");
				letterStack.ReceiveLetter(label, key.Translate(array).AdjustedFor(pawn, "PAWN"), LetterDefOf.PositiveEvent, target, faction, null);
			}
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

		[CompilerGenerated]
		private static string <GiveGift>m__0(Thing g)
		{
			return g.LabelCap;
		}
	}
}
