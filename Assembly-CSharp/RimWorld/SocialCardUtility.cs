using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public static class SocialCardUtility
	{
		private class CachedSocialTabEntry
		{
			public Pawn otherPawn;

			public int opinionOfOtherPawn;

			public int opinionOfMe;

			public List<PawnRelationDef> relations = new List<PawnRelationDef>();
		}

		private class CachedSocialTabEntryComparer : IComparer<CachedSocialTabEntry>
		{
			public int Compare(CachedSocialTabEntry a, CachedSocialTabEntry b)
			{
				bool flag = a.relations.Any();
				bool flag2 = b.relations.Any();
				int result;
				if (flag != flag2)
				{
					result = flag2.CompareTo(flag);
				}
				else
				{
					if (flag && flag2)
					{
						float num = -3.40282347E+38f;
						for (int i = 0; i < a.relations.Count; i++)
						{
							if (a.relations[i].importance > num)
							{
								num = a.relations[i].importance;
							}
						}
						float num2 = -3.40282347E+38f;
						for (int j = 0; j < b.relations.Count; j++)
						{
							if (b.relations[j].importance > num2)
							{
								num2 = b.relations[j].importance;
							}
						}
						if (num != num2)
						{
							result = num2.CompareTo(num);
							goto IL_0144;
						}
					}
					result = ((a.opinionOfOtherPawn == b.opinionOfOtherPawn) ? a.otherPawn.thingIDNumber.CompareTo(b.otherPawn.thingIDNumber) : b.opinionOfOtherPawn.CompareTo(a.opinionOfOtherPawn));
				}
				goto IL_0144;
				IL_0144:
				return result;
			}
		}

		private static Vector2 listScrollPosition = Vector2.zero;

		private static float listScrollViewHeight = 0f;

		private static bool showAllRelations;

		private static List<CachedSocialTabEntry> cachedEntries = new List<CachedSocialTabEntry>();

		private static Pawn cachedForPawn;

		private const float TopPadding = 20f;

		private static readonly Color RelationLabelColor = new Color(0.6f, 0.6f, 0.6f);

		private static readonly Color PawnLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);

		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		private const float RowTopPadding = 3f;

		private const float RowLeftRightPadding = 5f;

		private static CachedSocialTabEntryComparer CachedEntriesComparer = new CachedSocialTabEntryComparer();

		private static HashSet<Pawn> tmpCached = new HashSet<Pawn>();

		private static HashSet<Pawn> tmpToCache = new HashSet<Pawn>();

		public static void DrawSocialCard(Rect rect, Pawn pawn)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(0f, 20f, rect.width, (float)(rect.height - 20.0));
			Rect rect3;
			Rect rect4 = rect3 = rect2.ContractedBy(10f);
			Rect rect5 = rect4;
			rect3.height *= 0.63f;
			rect5.y = (float)(rect3.yMax + 17.0);
			rect5.yMax = rect4.yMax;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(0f, (float)((rect3.yMax + rect5.y) / 2.0), rect.width);
			GUI.color = Color.white;
			if (Prefs.DevMode)
			{
				Rect rect6 = new Rect(5f, 5f, rect.width, 22f);
				SocialCardUtility.DrawDebugOptions(rect6, pawn);
			}
			SocialCardUtility.DrawRelationsAndOpinions(rect3, pawn);
			InteractionCardUtility.DrawInteractionsLog(rect5, pawn, Find.PlayLog.AllEntries, 12);
			GUI.EndGroup();
		}

		private static void CheckRecache(Pawn selPawnForSocialInfo)
		{
			if (((SocialCardUtility.cachedForPawn == selPawnForSocialInfo) ? (Time.frameCount % 20) : 0) != 0)
				return;
			SocialCardUtility.Recache(selPawnForSocialInfo);
		}

		private static void Recache(Pawn selPawnForSocialInfo)
		{
			SocialCardUtility.cachedForPawn = selPawnForSocialInfo;
			SocialCardUtility.tmpToCache.Clear();
			foreach (Pawn relatedPawn in selPawnForSocialInfo.relations.RelatedPawns)
			{
				if (SocialCardUtility.ShouldShowPawnRelations(relatedPawn, selPawnForSocialInfo))
				{
					SocialCardUtility.tmpToCache.Add(relatedPawn);
				}
			}
			List<Pawn> list = null;
			if (selPawnForSocialInfo.MapHeld != null)
			{
				list = selPawnForSocialInfo.MapHeld.mapPawns.AllPawnsSpawned;
			}
			else if (selPawnForSocialInfo.IsCaravanMember())
			{
				list = selPawnForSocialInfo.GetCaravan().PawnsListForReading;
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i];
					if ((pawn.RaceProps.Humanlike ? ((pawn != selPawnForSocialInfo) ? (SocialCardUtility.ShouldShowPawnRelations(pawn, selPawnForSocialInfo) ? ((!SocialCardUtility.tmpToCache.Contains(pawn)) ? ((selPawnForSocialInfo.relations.OpinionOf(pawn) != 0) ? 1 : pawn.relations.OpinionOf(selPawnForSocialInfo)) : 0) : 0) : 0) : 0) != 0)
					{
						SocialCardUtility.tmpToCache.Add(pawn);
					}
				}
			}
			SocialCardUtility.cachedEntries.RemoveAll((Predicate<CachedSocialTabEntry>)((CachedSocialTabEntry x) => !SocialCardUtility.tmpToCache.Contains(x.otherPawn)));
			SocialCardUtility.tmpCached.Clear();
			for (int j = 0; j < SocialCardUtility.cachedEntries.Count; j++)
			{
				SocialCardUtility.tmpCached.Add(SocialCardUtility.cachedEntries[j].otherPawn);
			}
			foreach (Pawn item in SocialCardUtility.tmpToCache)
			{
				if (!SocialCardUtility.tmpCached.Contains(item))
				{
					CachedSocialTabEntry cachedSocialTabEntry = new CachedSocialTabEntry();
					cachedSocialTabEntry.otherPawn = item;
					SocialCardUtility.cachedEntries.Add(cachedSocialTabEntry);
				}
			}
			SocialCardUtility.tmpCached.Clear();
			SocialCardUtility.tmpToCache.Clear();
			for (int k = 0; k < SocialCardUtility.cachedEntries.Count; k++)
			{
				SocialCardUtility.RecacheEntry(SocialCardUtility.cachedEntries[k], selPawnForSocialInfo);
			}
			SocialCardUtility.cachedEntries.Sort(SocialCardUtility.CachedEntriesComparer);
		}

		private static bool ShouldShowPawnRelations(Pawn pawn, Pawn selPawnForSocialInfo)
		{
			return (byte)(SocialCardUtility.showAllRelations ? 1 : (pawn.relations.everSeenByPlayer ? 1 : 0)) != 0;
		}

		private static void RecacheEntry(CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			entry.opinionOfMe = entry.otherPawn.relations.OpinionOf(selPawnForSocialInfo);
			entry.opinionOfOtherPawn = selPawnForSocialInfo.relations.OpinionOf(entry.otherPawn);
			entry.relations.Clear();
			foreach (PawnRelationDef relation in selPawnForSocialInfo.GetRelations(entry.otherPawn))
			{
				entry.relations.Add(relation);
			}
			entry.relations.Sort((Comparison<PawnRelationDef>)((PawnRelationDef a, PawnRelationDef b) => b.importance.CompareTo(a.importance)));
		}

		public static void DrawRelationsAndOpinions(Rect rect, Pawn selPawnForSocialInfo)
		{
			SocialCardUtility.CheckRecache(selPawnForSocialInfo);
			if (Current.ProgramState != ProgramState.Playing)
			{
				SocialCardUtility.showAllRelations = false;
			}
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height);
			Rect viewRect = new Rect(0f, 0f, (float)(rect.width - 16.0), SocialCardUtility.listScrollViewHeight);
			Rect rect2 = rect;
			if (viewRect.height > outRect.height)
			{
				rect2.width -= 16f;
			}
			Widgets.BeginScrollView(outRect, ref SocialCardUtility.listScrollPosition, viewRect, true);
			float num = 0f;
			float y = SocialCardUtility.listScrollPosition.y;
			float num2 = SocialCardUtility.listScrollPosition.y + outRect.height;
			for (int i = 0; i < SocialCardUtility.cachedEntries.Count; i++)
			{
				float rowHeight = SocialCardUtility.GetRowHeight(SocialCardUtility.cachedEntries[i], rect2.width, selPawnForSocialInfo);
				if (num > y - rowHeight && num < num2)
				{
					SocialCardUtility.DrawPawnRow(num, rect2.width, SocialCardUtility.cachedEntries[i], selPawnForSocialInfo);
				}
				num += rowHeight;
			}
			if (!SocialCardUtility.cachedEntries.Any())
			{
				GUI.color = Color.gray;
				Text.Anchor = TextAnchor.UpperCenter;
				Rect rect3 = new Rect(0f, 0f, rect2.width, 30f);
				Widgets.Label(rect3, "NoRelationships".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (Event.current.type == EventType.Layout)
			{
				SocialCardUtility.listScrollViewHeight = (float)(num + 30.0);
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			GUI.color = Color.white;
		}

		private static void DrawPawnRow(float y, float width, CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			float rowHeight = SocialCardUtility.GetRowHeight(entry, width, selPawnForSocialInfo);
			Rect rect = new Rect(0f, y, width, rowHeight);
			Pawn otherPawn = entry.otherPawn;
			if (Mouse.IsOver(rect))
			{
				GUI.color = SocialCardUtility.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			TooltipHandler.TipRegion(rect, (Func<string>)(() => SocialCardUtility.GetPawnRowTooltip(entry, selPawnForSocialInfo)), entry.otherPawn.thingIDNumber * 13 + selPawnForSocialInfo.thingIDNumber);
			if (Widgets.ButtonInvisible(rect, false))
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (otherPawn.Dead)
					{
						Messages.Message("MessageCantSelectDeadPawn".Translate(otherPawn.LabelShort).CapitalizeFirst(), MessageTypeDefOf.RejectInput);
					}
					else if (otherPawn.SpawnedOrAnyParentSpawned || otherPawn.IsCaravanMember())
					{
						CameraJumper.TryJumpAndSelect((Thing)otherPawn);
					}
					else
					{
						Messages.Message("MessageCantSelectOffMapPawn".Translate(otherPawn.LabelShort).CapitalizeFirst(), MessageTypeDefOf.RejectInput);
					}
				}
				else if (Find.GameInitData.startingPawns.Contains(otherPawn))
				{
					Page_ConfigureStartingPawns page_ConfigureStartingPawns = Find.WindowStack.WindowOfType<Page_ConfigureStartingPawns>();
					if (page_ConfigureStartingPawns != null)
					{
						page_ConfigureStartingPawns.SelectPawn(otherPawn);
						SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
					}
				}
			}
			float width2 = default(float);
			float width3 = default(float);
			float width4 = default(float);
			float width5 = default(float);
			float width6 = default(float);
			SocialCardUtility.CalculateColumnsWidths(width, out width2, out width3, out width4, out width5, out width6);
			Rect rect2 = new Rect(5f, (float)(y + 3.0), width2, (float)(rowHeight - 3.0));
			SocialCardUtility.DrawRelationLabel(entry, rect2, selPawnForSocialInfo);
			Rect rect3 = new Rect(rect2.xMax, (float)(y + 3.0), width3, (float)(rowHeight - 3.0));
			SocialCardUtility.DrawPawnLabel(otherPawn, rect3);
			Rect rect4 = new Rect(rect3.xMax, (float)(y + 3.0), width4, (float)(rowHeight - 3.0));
			SocialCardUtility.DrawMyOpinion(entry, rect4, selPawnForSocialInfo);
			Rect rect5 = new Rect(rect4.xMax, (float)(y + 3.0), width5, (float)(rowHeight - 3.0));
			SocialCardUtility.DrawHisOpinion(entry, rect5, selPawnForSocialInfo);
			Rect rect6 = new Rect(rect5.xMax, (float)(y + 3.0), width6, (float)(rowHeight - 3.0));
			SocialCardUtility.DrawPawnSituationLabel(entry.otherPawn, rect6, selPawnForSocialInfo);
		}

		private static float GetRowHeight(CachedSocialTabEntry entry, float rowWidth, Pawn selPawnForSocialInfo)
		{
			float width = default(float);
			float width2 = default(float);
			float num = default(float);
			float num2 = default(float);
			float num3 = default(float);
			SocialCardUtility.CalculateColumnsWidths(rowWidth, out width, out width2, out num, out num2, out num3);
			float a = 0f;
			a = Mathf.Max(a, Text.CalcHeight(SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo), width));
			a = Mathf.Max(a, Text.CalcHeight(SocialCardUtility.GetPawnLabel(entry.otherPawn), width2));
			return (float)(a + 3.0);
		}

		private static void CalculateColumnsWidths(float rowWidth, out float relationsWidth, out float pawnLabelWidth, out float myOpinionWidth, out float hisOpinionWidth, out float pawnSituationLabelWidth)
		{
			float num = (float)(rowWidth - 10.0);
			relationsWidth = (float)(num * 0.23000000417232513);
			pawnLabelWidth = (float)(num * 0.40999999642372131);
			myOpinionWidth = (float)(num * 0.075000002980232239);
			hisOpinionWidth = (float)(num * 0.085000000894069672);
			pawnSituationLabelWidth = (float)(num * 0.20000000298023224);
			if (myOpinionWidth < 25.0)
			{
				pawnLabelWidth -= (float)(25.0 - myOpinionWidth);
				myOpinionWidth = 25f;
			}
			if (hisOpinionWidth < 35.0)
			{
				pawnLabelWidth -= (float)(35.0 - hisOpinionWidth);
				hisOpinionWidth = 35f;
			}
		}

		private static void DrawRelationLabel(CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			string relationsString = SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo);
			if (!relationsString.NullOrEmpty())
			{
				GUI.color = SocialCardUtility.RelationLabelColor;
				Widgets.Label(rect, relationsString);
			}
		}

		private static void DrawPawnLabel(Pawn pawn, Rect rect)
		{
			GUI.color = SocialCardUtility.PawnLabelColor;
			Widgets.Label(rect, SocialCardUtility.GetPawnLabel(pawn));
		}

		private static void DrawMyOpinion(CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			if (entry.otherPawn.RaceProps.Humanlike && selPawnForSocialInfo.RaceProps.Humanlike)
			{
				int opinionOfOtherPawn = entry.opinionOfOtherPawn;
				GUI.color = SocialCardUtility.OpinionLabelColor(opinionOfOtherPawn);
				Widgets.Label(rect, opinionOfOtherPawn.ToStringWithSign());
			}
		}

		private static void DrawHisOpinion(CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			if (entry.otherPawn.RaceProps.Humanlike && selPawnForSocialInfo.RaceProps.Humanlike)
			{
				int opinionOfMe = entry.opinionOfMe;
				Color color = SocialCardUtility.OpinionLabelColor(opinionOfMe);
				GUI.color = new Color(color.r, color.g, color.b, 0.4f);
				Widgets.Label(rect, "(" + opinionOfMe.ToStringWithSign() + ")");
			}
		}

		private static void DrawPawnSituationLabel(Pawn pawn, Rect rect, Pawn selPawnForSocialInfo)
		{
			GUI.color = Color.gray;
			string label = SocialCardUtility.GetPawnSituationLabel(pawn, selPawnForSocialInfo).Truncate(rect.width, null);
			Widgets.Label(rect, label);
		}

		private static Color OpinionLabelColor(int opinion)
		{
			return (Mathf.Abs(opinion) >= 10) ? ((opinion >= 0) ? Color.green : Color.red) : Color.gray;
		}

		private static string GetPawnLabel(Pawn pawn)
		{
			return (pawn.Name == null) ? pawn.LabelCapNoCount : pawn.Name.ToStringFull;
		}

		public static string GetPawnSituationLabel(Pawn pawn, Pawn fromPOV)
		{
			string result;
			if (pawn.Dead)
			{
				result = "Dead".Translate();
			}
			else if (pawn.Destroyed)
			{
				result = "Missing".Translate();
			}
			else if (PawnUtility.IsKidnappedPawn(pawn))
			{
				result = "Kidnapped".Translate();
			}
			else if (pawn.kindDef == PawnKindDefOf.Slave)
			{
				result = "Slave".Translate();
			}
			else if (PawnUtility.IsFactionLeader(pawn))
			{
				result = "FactionLeader".Translate();
			}
			else
			{
				Faction faction = pawn.Faction;
				result = ((faction == fromPOV.Faction) ? "" : ((faction == null || fromPOV.Faction == null) ? "Neutral".Translate() : (faction.HostileTo(fromPOV.Faction) ? ("Hostile".Translate() + ", " + faction.Name) : ("Neutral".Translate() + ", " + faction.Name))));
			}
			return result;
		}

		private static string GetRelationsString(CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			string text = "";
			string result;
			if (entry.relations.Count == 0)
			{
				result = ((entry.opinionOfOtherPawn >= -20) ? ((entry.opinionOfOtherPawn <= 20) ? "Acquaintance".Translate() : "Friend".Translate()) : "Rival".Translate());
			}
			else
			{
				for (int i = 0; i < entry.relations.Count; i++)
				{
					PawnRelationDef pawnRelationDef = entry.relations[i];
					text = (text.NullOrEmpty() ? pawnRelationDef.GetGenderSpecificLabelCap(entry.otherPawn) : (text + ", " + pawnRelationDef.GetGenderSpecificLabel(entry.otherPawn)));
				}
				result = text;
			}
			return result;
		}

		private static string GetPawnRowTooltip(CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (entry.otherPawn.RaceProps.Humanlike && selPawnForSocialInfo.RaceProps.Humanlike)
			{
				stringBuilder.AppendLine(selPawnForSocialInfo.relations.OpinionExplanation(entry.otherPawn));
				stringBuilder.AppendLine();
				stringBuilder.Append("SomeonesOpinionOfMe".Translate(entry.otherPawn.LabelShort));
				stringBuilder.Append(": ");
				stringBuilder.Append(entry.opinionOfMe.ToStringWithSign());
			}
			else
			{
				stringBuilder.AppendLine(entry.otherPawn.LabelCapNoCount);
				string pawnSituationLabel = SocialCardUtility.GetPawnSituationLabel(entry.otherPawn, selPawnForSocialInfo);
				if (!pawnSituationLabel.NullOrEmpty())
				{
					stringBuilder.AppendLine(pawnSituationLabel);
				}
				stringBuilder.AppendLine("--------------");
				stringBuilder.Append(SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo));
			}
			if (Prefs.DevMode)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("(debug) Compatibility: " + selPawnForSocialInfo.relations.CompatibilityWith(entry.otherPawn).ToString("F2"));
				stringBuilder.Append("(debug) RomanceChanceFactor: " + selPawnForSocialInfo.relations.SecondaryRomanceChanceFactor(entry.otherPawn).ToString("F2"));
			}
			return stringBuilder.ToString();
		}

		private static void DrawDebugOptions(Rect rect, Pawn pawn)
		{
			GUI.BeginGroup(rect);
			Widgets.CheckboxLabeled(new Rect(0f, 0f, 145f, 22f), "Dev: AllRelations", ref SocialCardUtility.showAllRelations, false);
			if (Widgets.ButtonText(new Rect(150f, 0f, 115f, 22f), "Debug info", true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("AttractionTo", (Action)delegate()
				{
					StringBuilder stringBuilder5 = new StringBuilder();
					stringBuilder5.AppendLine("My gender: " + pawn.gender);
					stringBuilder5.AppendLine("My age: " + pawn.ageTracker.AgeBiologicalYears);
					stringBuilder5.AppendLine();
					IOrderedEnumerable<Pawn> orderedEnumerable4 = from x in pawn.Map.mapPawns.AllPawnsSpawned
					where x.def == pawn.def
					orderby pawn.relations.SecondaryRomanceChanceFactor(x) descending
					select x;
					foreach (Pawn item in orderedEnumerable4)
					{
						if (item != pawn)
						{
							stringBuilder5.AppendLine(item.LabelShort + " (" + item.gender + ", age: " + item.ageTracker.AgeBiologicalYears + ", compat: " + pawn.relations.CompatibilityWith(item).ToString("F2") + "): " + pawn.relations.SecondaryRomanceChanceFactor(item).ToStringPercent("F0") + "        [vs " + item.relations.SecondaryRomanceChanceFactor(pawn).ToStringPercent("F0") + "]");
						}
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder5.ToString(), (string)null, null, (string)null, null, (string)null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("CompatibilityTo", (Action)delegate()
				{
					StringBuilder stringBuilder4 = new StringBuilder();
					stringBuilder4.AppendLine("My age: " + pawn.ageTracker.AgeBiologicalYears);
					stringBuilder4.AppendLine();
					IOrderedEnumerable<Pawn> orderedEnumerable3 = from x in pawn.Map.mapPawns.AllPawnsSpawned
					where x.def == pawn.def
					orderby pawn.relations.CompatibilityWith(x) descending
					select x;
					foreach (Pawn item in orderedEnumerable3)
					{
						if (item != pawn)
						{
							stringBuilder4.AppendLine(item.LabelShort + " (" + item.KindLabel + ", age: " + item.ageTracker.AgeBiologicalYears + "): " + pawn.relations.CompatibilityWith(item).ToString("0.##"));
						}
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder4.ToString(), (string)null, null, (string)null, null, (string)null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				if (pawn.RaceProps.Humanlike)
				{
					list.Add(new FloatMenuOption("Interaction chance", (Action)delegate()
					{
						StringBuilder stringBuilder3 = new StringBuilder();
						stringBuilder3.AppendLine("(selected pawn is the initiator)");
						stringBuilder3.AppendLine("(\"fight chance\" is the chance that the receiver will start social fight)");
						stringBuilder3.AppendLine("Interaction chance (real chance, not just weights):");
						IOrderedEnumerable<Pawn> orderedEnumerable2 = from x in pawn.Map.mapPawns.AllPawnsSpawned
						where x.RaceProps.Humanlike
						orderby (x.Faction != null) ? x.Faction.loadID : (-1)
						select x;
						foreach (Pawn item in orderedEnumerable2)
						{
							if (item != pawn)
							{
								stringBuilder3.AppendLine();
								stringBuilder3.AppendLine(item.LabelShort + " (" + item.KindLabel + ", " + item.gender + ", age: " + item.ageTracker.AgeBiologicalYears + ", compat: " + pawn.relations.CompatibilityWith(item).ToString("F2") + ", attr: " + pawn.relations.SecondaryRomanceChanceFactor(item).ToStringPercent("F0") + "):");
								List<InteractionDef> list2 = (from x in DefDatabase<InteractionDef>.AllDefs
								where x.Worker.RandomSelectionWeight(pawn, item) > 0.0
								orderby x.Worker.RandomSelectionWeight(pawn, item) descending
								select x).ToList();
								float num12 = list2.Sum((Func<InteractionDef, float>)((InteractionDef x) => x.Worker.RandomSelectionWeight(pawn, item)));
								foreach (InteractionDef item2 in list2)
								{
									float f = item.interactions.SocialFightChance(item2, pawn);
									float f2 = item2.Worker.RandomSelectionWeight(pawn, item) / num12;
									stringBuilder3.AppendLine("  " + item2.defName + ": " + f2.ToStringPercent() + " (fight chance: " + f.ToStringPercent("F2") + ")");
									if (item2 == InteractionDefOf.RomanceAttempt)
									{
										stringBuilder3.AppendLine("    success chance: " + ((InteractionWorker_RomanceAttempt)item2.Worker).SuccessChance(pawn, item).ToStringPercent());
									}
									else if (item2 == InteractionDefOf.MarriageProposal)
									{
										stringBuilder3.AppendLine("    acceptance chance: " + ((InteractionWorker_MarriageProposal)item2.Worker).AcceptanceChance(pawn, item).ToStringPercent());
									}
								}
							}
						}
						Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder3.ToString(), (string)null, null, (string)null, null, (string)null, false));
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
					list.Add(new FloatMenuOption("Lovin' MTB", (Action)delegate()
					{
						StringBuilder stringBuilder2 = new StringBuilder();
						stringBuilder2.AppendLine("Lovin' MTB hours with pawn X.");
						stringBuilder2.AppendLine("Assuming both pawns are in bed and are partners.");
						stringBuilder2.AppendLine();
						IOrderedEnumerable<Pawn> orderedEnumerable = from x in pawn.Map.mapPawns.AllPawnsSpawned
						where x.def == pawn.def
						orderby pawn.relations.SecondaryRomanceChanceFactor(x) descending
						select x;
						foreach (Pawn item in orderedEnumerable)
						{
							if (item != pawn)
							{
								stringBuilder2.AppendLine(item.LabelShort + " (" + item.KindLabel + ", age: " + item.ageTracker.AgeBiologicalYears + "): " + LovePartnerRelationUtility.GetLovinMtbHours(pawn, item).ToString("F1") + " h");
							}
						}
						Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder2.ToString(), (string)null, null, (string)null, null, (string)null, false));
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				list.Add(new FloatMenuOption("Test per pawns pair compatibility factor probability", (Action)delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					int num7 = 0;
					int num8 = 0;
					float num9 = -999999f;
					float num10 = 999999f;
					for (int i = 0; i < 10000; i++)
					{
						int otherPawnID = Rand.RangeInclusive(0, 30000);
						float num11 = pawn.relations.ConstantPerPawnsPairCompatibilityOffset(otherPawnID);
						if (num11 < -3.0)
						{
							num++;
						}
						else if (num11 < -2.0)
						{
							num2++;
						}
						else if (num11 < -1.0)
						{
							num3++;
						}
						else if (num11 < 0.0)
						{
							num4++;
						}
						else if (num11 < 1.0)
						{
							num5++;
						}
						else if (num11 < 2.0)
						{
							num6++;
						}
						else if (num11 < 3.0)
						{
							num7++;
						}
						else
						{
							num8++;
						}
						if (num11 > num9)
						{
							num9 = num11;
						}
						else if (num11 < num10)
						{
							num10 = num11;
						}
					}
					stringBuilder.AppendLine("< -3: " + ((float)((float)num / 10000.0)).ToStringPercent("F2"));
					stringBuilder.AppendLine("< -2: " + ((float)((float)num2 / 10000.0)).ToStringPercent("F2"));
					stringBuilder.AppendLine("< -1: " + ((float)((float)num3 / 10000.0)).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 0: " + ((float)((float)num4 / 10000.0)).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 1: " + ((float)((float)num5 / 10000.0)).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 2: " + ((float)((float)num6 / 10000.0)).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 3: " + ((float)((float)num7 / 10000.0)).ToStringPercent("F2"));
					stringBuilder.AppendLine("> 3: " + ((float)((float)num8 / 10000.0)).ToStringPercent("F2"));
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("trials: " + 10000);
					stringBuilder.AppendLine("min: " + num10);
					stringBuilder.AppendLine("max: " + num9);
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), (string)null, null, (string)null, null, (string)null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Find.WindowStack.Add(new FloatMenu(list));
			}
			GUI.EndGroup();
		}
	}
}
