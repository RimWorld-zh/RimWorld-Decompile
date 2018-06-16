using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200081E RID: 2078
	public static class SocialCardUtility
	{
		// Token: 0x06002E64 RID: 11876 RVA: 0x0018AEB8 File Offset: 0x001892B8
		public static void DrawSocialCard(Rect rect, Pawn pawn)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(0f, 20f, rect.width, rect.height - 20f);
			Rect rect3 = rect2.ContractedBy(10f);
			Rect rect4 = rect3;
			Rect rect5 = rect3;
			rect4.height *= 0.63f;
			rect5.y = rect4.yMax + 17f;
			rect5.yMax = rect3.yMax;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(0f, (rect4.yMax + rect5.y) / 2f, rect.width);
			GUI.color = Color.white;
			if (Prefs.DevMode)
			{
				Rect rect6 = new Rect(5f, 5f, rect.width, 22f);
				SocialCardUtility.DrawDebugOptions(rect6, pawn);
			}
			SocialCardUtility.DrawRelationsAndOpinions(rect4, pawn);
			InteractionCardUtility.DrawInteractionsLog(rect5, pawn, Find.PlayLog.AllEntries, 12);
			GUI.EndGroup();
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x0018AFDC File Offset: 0x001893DC
		private static void CheckRecache(Pawn selPawnForSocialInfo)
		{
			if (SocialCardUtility.cachedForPawn != selPawnForSocialInfo || Time.frameCount % 20 == 0)
			{
				SocialCardUtility.Recache(selPawnForSocialInfo);
			}
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x0018B000 File Offset: 0x00189400
		private static void Recache(Pawn selPawnForSocialInfo)
		{
			SocialCardUtility.cachedForPawn = selPawnForSocialInfo;
			SocialCardUtility.tmpToCache.Clear();
			foreach (Pawn pawn in selPawnForSocialInfo.relations.RelatedPawns)
			{
				if (SocialCardUtility.ShouldShowPawnRelations(pawn, selPawnForSocialInfo))
				{
					SocialCardUtility.tmpToCache.Add(pawn);
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
					Pawn pawn2 = list[i];
					if (pawn2.RaceProps.Humanlike && pawn2 != selPawnForSocialInfo && SocialCardUtility.ShouldShowPawnRelations(pawn2, selPawnForSocialInfo) && !SocialCardUtility.tmpToCache.Contains(pawn2))
					{
						if (selPawnForSocialInfo.relations.OpinionOf(pawn2) != 0 || pawn2.relations.OpinionOf(selPawnForSocialInfo) != 0)
						{
							SocialCardUtility.tmpToCache.Add(pawn2);
						}
					}
				}
			}
			SocialCardUtility.cachedEntries.RemoveAll((SocialCardUtility.CachedSocialTabEntry x) => !SocialCardUtility.tmpToCache.Contains(x.otherPawn));
			SocialCardUtility.tmpCached.Clear();
			for (int j = 0; j < SocialCardUtility.cachedEntries.Count; j++)
			{
				SocialCardUtility.tmpCached.Add(SocialCardUtility.cachedEntries[j].otherPawn);
			}
			foreach (Pawn pawn3 in SocialCardUtility.tmpToCache)
			{
				if (!SocialCardUtility.tmpCached.Contains(pawn3))
				{
					SocialCardUtility.CachedSocialTabEntry cachedSocialTabEntry = new SocialCardUtility.CachedSocialTabEntry();
					cachedSocialTabEntry.otherPawn = pawn3;
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

		// Token: 0x06002E67 RID: 11879 RVA: 0x0018B2A0 File Offset: 0x001896A0
		private static bool ShouldShowPawnRelations(Pawn pawn, Pawn selPawnForSocialInfo)
		{
			return SocialCardUtility.showAllRelations || pawn.relations.everSeenByPlayer;
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x0018B2E0 File Offset: 0x001896E0
		private static void RecacheEntry(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			entry.opinionOfMe = entry.otherPawn.relations.OpinionOf(selPawnForSocialInfo);
			entry.opinionOfOtherPawn = selPawnForSocialInfo.relations.OpinionOf(entry.otherPawn);
			entry.relations.Clear();
			foreach (PawnRelationDef item in selPawnForSocialInfo.GetRelations(entry.otherPawn))
			{
				entry.relations.Add(item);
			}
			entry.relations.Sort((PawnRelationDef a, PawnRelationDef b) => b.importance.CompareTo(a.importance));
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x0018B3AC File Offset: 0x001897AC
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
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, SocialCardUtility.listScrollViewHeight);
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
			if (!SocialCardUtility.cachedEntries.Any<SocialCardUtility.CachedSocialTabEntry>())
			{
				GUI.color = Color.gray;
				Text.Anchor = TextAnchor.UpperCenter;
				Rect rect3 = new Rect(0f, 0f, rect2.width, 30f);
				Widgets.Label(rect3, "NoRelationships".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (Event.current.type == EventType.Layout)
			{
				SocialCardUtility.listScrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			GUI.color = Color.white;
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x0018B57C File Offset: 0x0018997C
		private static void DrawPawnRow(float y, float width, SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			float rowHeight = SocialCardUtility.GetRowHeight(entry, width, selPawnForSocialInfo);
			Rect rect = new Rect(0f, y, width, rowHeight);
			Pawn otherPawn = entry.otherPawn;
			if (Mouse.IsOver(rect))
			{
				GUI.color = SocialCardUtility.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			TooltipHandler.TipRegion(rect, () => SocialCardUtility.GetPawnRowTooltip(entry, selPawnForSocialInfo), entry.otherPawn.thingIDNumber * 13 + selPawnForSocialInfo.thingIDNumber);
			if (Widgets.ButtonInvisible(rect, false))
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (otherPawn.Dead)
					{
						Messages.Message("MessageCantSelectDeadPawn".Translate(new object[]
						{
							otherPawn.LabelShort
						}).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
					}
					else if (otherPawn.SpawnedOrAnyParentSpawned || otherPawn.IsCaravanMember())
					{
						CameraJumper.TryJumpAndSelect(otherPawn);
					}
					else
					{
						Messages.Message("MessageCantSelectOffMapPawn".Translate(new object[]
						{
							otherPawn.LabelShort
						}).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
					}
				}
				else if (Find.GameInitData.startingAndOptionalPawns.Contains(otherPawn))
				{
					Page_ConfigureStartingPawns page_ConfigureStartingPawns = Find.WindowStack.WindowOfType<Page_ConfigureStartingPawns>();
					if (page_ConfigureStartingPawns != null)
					{
						page_ConfigureStartingPawns.SelectPawn(otherPawn);
						SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
					}
				}
			}
			float width2;
			float width3;
			float width4;
			float width5;
			float width6;
			SocialCardUtility.CalculateColumnsWidths(width, out width2, out width3, out width4, out width5, out width6);
			Rect rect2 = new Rect(5f, y + 3f, width2, rowHeight - 3f);
			SocialCardUtility.DrawRelationLabel(entry, rect2, selPawnForSocialInfo);
			Rect rect3 = new Rect(rect2.xMax, y + 3f, width3, rowHeight - 3f);
			SocialCardUtility.DrawPawnLabel(otherPawn, rect3);
			Rect rect4 = new Rect(rect3.xMax, y + 3f, width4, rowHeight - 3f);
			SocialCardUtility.DrawMyOpinion(entry, rect4, selPawnForSocialInfo);
			Rect rect5 = new Rect(rect4.xMax, y + 3f, width5, rowHeight - 3f);
			SocialCardUtility.DrawHisOpinion(entry, rect5, selPawnForSocialInfo);
			Rect rect6 = new Rect(rect5.xMax, y + 3f, width6, rowHeight - 3f);
			SocialCardUtility.DrawPawnSituationLabel(entry.otherPawn, rect6, selPawnForSocialInfo);
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x0018B804 File Offset: 0x00189C04
		private static float GetRowHeight(SocialCardUtility.CachedSocialTabEntry entry, float rowWidth, Pawn selPawnForSocialInfo)
		{
			float width;
			float width2;
			float num;
			float num2;
			float num3;
			SocialCardUtility.CalculateColumnsWidths(rowWidth, out width, out width2, out num, out num2, out num3);
			float num4 = 0f;
			num4 = Mathf.Max(num4, Text.CalcHeight(SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo), width));
			num4 = Mathf.Max(num4, Text.CalcHeight(SocialCardUtility.GetPawnLabel(entry.otherPawn), width2));
			return num4 + 3f;
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x0018B86C File Offset: 0x00189C6C
		private static void CalculateColumnsWidths(float rowWidth, out float relationsWidth, out float pawnLabelWidth, out float myOpinionWidth, out float hisOpinionWidth, out float pawnSituationLabelWidth)
		{
			float num = rowWidth - 10f;
			relationsWidth = num * 0.23f;
			pawnLabelWidth = num * 0.41f;
			myOpinionWidth = num * 0.075f;
			hisOpinionWidth = num * 0.085f;
			pawnSituationLabelWidth = num * 0.2f;
			if (myOpinionWidth < 25f)
			{
				pawnLabelWidth -= 25f - myOpinionWidth;
				myOpinionWidth = 25f;
			}
			if (hisOpinionWidth < 35f)
			{
				pawnLabelWidth -= 35f - hisOpinionWidth;
				hisOpinionWidth = 35f;
			}
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x0018B8F8 File Offset: 0x00189CF8
		private static void DrawRelationLabel(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			string relationsString = SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo);
			if (!relationsString.NullOrEmpty())
			{
				GUI.color = SocialCardUtility.RelationLabelColor;
				Widgets.Label(rect, relationsString);
			}
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x0018B92C File Offset: 0x00189D2C
		private static void DrawPawnLabel(Pawn pawn, Rect rect)
		{
			GUI.color = SocialCardUtility.PawnLabelColor;
			Widgets.Label(rect, SocialCardUtility.GetPawnLabel(pawn));
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x0018B948 File Offset: 0x00189D48
		private static void DrawMyOpinion(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			if (entry.otherPawn.RaceProps.Humanlike && selPawnForSocialInfo.RaceProps.Humanlike)
			{
				int opinionOfOtherPawn = entry.opinionOfOtherPawn;
				GUI.color = SocialCardUtility.OpinionLabelColor(opinionOfOtherPawn);
				Widgets.Label(rect, opinionOfOtherPawn.ToStringWithSign());
			}
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x0018B9A0 File Offset: 0x00189DA0
		private static void DrawHisOpinion(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			if (entry.otherPawn.RaceProps.Humanlike && selPawnForSocialInfo.RaceProps.Humanlike)
			{
				int opinionOfMe = entry.opinionOfMe;
				Color color = SocialCardUtility.OpinionLabelColor(opinionOfMe);
				GUI.color = new Color(color.r, color.g, color.b, 0.4f);
				Widgets.Label(rect, "(" + opinionOfMe.ToStringWithSign() + ")");
			}
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x0018BA28 File Offset: 0x00189E28
		private static void DrawPawnSituationLabel(Pawn pawn, Rect rect, Pawn selPawnForSocialInfo)
		{
			GUI.color = Color.gray;
			string label = SocialCardUtility.GetPawnSituationLabel(pawn, selPawnForSocialInfo).Truncate(rect.width, null);
			Widgets.Label(rect, label);
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x0018BA5C File Offset: 0x00189E5C
		private static Color OpinionLabelColor(int opinion)
		{
			Color result;
			if (Mathf.Abs(opinion) < 10)
			{
				result = Color.gray;
			}
			else if (opinion < 0)
			{
				result = Color.red;
			}
			else
			{
				result = Color.green;
			}
			return result;
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x0018BAA0 File Offset: 0x00189EA0
		private static string GetPawnLabel(Pawn pawn)
		{
			string result;
			if (pawn.Name != null)
			{
				result = pawn.Name.ToStringFull;
			}
			else
			{
				result = pawn.LabelCapNoCount;
			}
			return result;
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x0018BAD8 File Offset: 0x00189ED8
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
				if (faction != fromPOV.Faction)
				{
					if (faction == null || fromPOV.Faction == null)
					{
						result = "Neutral".Translate();
					}
					else
					{
						switch (faction.RelationKindWith(fromPOV.Faction))
						{
						case FactionRelationKind.Hostile:
							result = "Hostile".Translate() + ", " + faction.Name;
							break;
						case FactionRelationKind.Neutral:
							result = "Neutral".Translate() + ", " + faction.Name;
							break;
						case FactionRelationKind.Ally:
							result = "Ally".Translate() + ", " + faction.Name;
							break;
						default:
							result = "";
							break;
						}
					}
				}
				else
				{
					result = "";
				}
			}
			return result;
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x0018BC44 File Offset: 0x0018A044
		private static string GetRelationsString(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			string text = "";
			string result;
			if (entry.relations.Count == 0)
			{
				if (entry.opinionOfOtherPawn < -20)
				{
					result = "Rival".Translate();
				}
				else if (entry.opinionOfOtherPawn > 20)
				{
					result = "Friend".Translate();
				}
				else
				{
					result = "Acquaintance".Translate();
				}
			}
			else
			{
				for (int i = 0; i < entry.relations.Count; i++)
				{
					PawnRelationDef pawnRelationDef = entry.relations[i];
					if (!text.NullOrEmpty())
					{
						text = text + ", " + pawnRelationDef.GetGenderSpecificLabel(entry.otherPawn);
					}
					else
					{
						text = pawnRelationDef.GetGenderSpecificLabelCap(entry.otherPawn);
					}
				}
				result = text;
			}
			return result;
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x0018BD20 File Offset: 0x0018A120
		private static string GetPawnRowTooltip(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (entry.otherPawn.RaceProps.Humanlike && selPawnForSocialInfo.RaceProps.Humanlike)
			{
				stringBuilder.AppendLine(selPawnForSocialInfo.relations.OpinionExplanation(entry.otherPawn));
				stringBuilder.AppendLine();
				stringBuilder.Append("SomeonesOpinionOfMe".Translate(new object[]
				{
					entry.otherPawn.LabelShort
				}));
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

		// Token: 0x06002E77 RID: 11895 RVA: 0x0018BE90 File Offset: 0x0018A290
		private static void DrawDebugOptions(Rect rect, Pawn pawn)
		{
			GUI.BeginGroup(rect);
			Widgets.CheckboxLabeled(new Rect(0f, 0f, 145f, 22f), "Dev: AllRelations", ref SocialCardUtility.showAllRelations, false, null, null, false);
			if (Widgets.ButtonText(new Rect(150f, 0f, 115f, 22f), "Debug info", true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("AttractionTo", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("My gender: " + pawn.gender);
					stringBuilder.AppendLine("My age: " + pawn.ageTracker.AgeBiologicalYears);
					stringBuilder.AppendLine();
					IOrderedEnumerable<Pawn> orderedEnumerable = from x in pawn.Map.mapPawns.AllPawnsSpawned
					where x.def == pawn.def
					orderby pawn.relations.SecondaryRomanceChanceFactor(x) descending
					select x;
					foreach (Pawn pawn2 in orderedEnumerable)
					{
						if (pawn2 != pawn)
						{
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								pawn2.LabelShort,
								" (",
								pawn2.gender,
								", age: ",
								pawn2.ageTracker.AgeBiologicalYears,
								", compat: ",
								pawn.relations.CompatibilityWith(pawn2).ToString("F2"),
								"): ",
								pawn.relations.SecondaryRomanceChanceFactor(pawn2).ToStringPercent("F0"),
								"        [vs ",
								pawn2.relations.SecondaryRomanceChanceFactor(pawn).ToStringPercent("F0"),
								"]"
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("CompatibilityTo", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("My age: " + pawn.ageTracker.AgeBiologicalYears);
					stringBuilder.AppendLine();
					IOrderedEnumerable<Pawn> orderedEnumerable = from x in pawn.Map.mapPawns.AllPawnsSpawned
					where x.def == pawn.def
					orderby pawn.relations.CompatibilityWith(x) descending
					select x;
					foreach (Pawn pawn2 in orderedEnumerable)
					{
						if (pawn2 != pawn)
						{
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								pawn2.LabelShort,
								" (",
								pawn2.KindLabel,
								", age: ",
								pawn2.ageTracker.AgeBiologicalYears,
								"): ",
								pawn.relations.CompatibilityWith(pawn2).ToString("0.##")
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				if (pawn.RaceProps.Humanlike)
				{
					list.Add(new FloatMenuOption("Interaction chance", delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendLine("(selected pawn is the initiator)");
						stringBuilder.AppendLine("(\"fight chance\" is the chance that the receiver will start social fight)");
						stringBuilder.AppendLine("Interaction chance (real chance, not just weights):");
						IOrderedEnumerable<Pawn> orderedEnumerable = from x in pawn.Map.mapPawns.AllPawnsSpawned
						where x.RaceProps.Humanlike
						orderby (x.Faction != null) ? x.Faction.loadID : -1
						select x;
						using (IEnumerator<Pawn> enumerator = orderedEnumerable.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Pawn c = enumerator.Current;
								if (c != pawn)
								{
									stringBuilder.AppendLine();
									stringBuilder.AppendLine(string.Concat(new object[]
									{
										c.LabelShort,
										" (",
										c.KindLabel,
										", ",
										c.gender,
										", age: ",
										c.ageTracker.AgeBiologicalYears,
										", compat: ",
										pawn.relations.CompatibilityWith(c).ToString("F2"),
										", attr: ",
										pawn.relations.SecondaryRomanceChanceFactor(c).ToStringPercent("F0"),
										"):"
									}));
									List<InteractionDef> list2 = (from x in DefDatabase<InteractionDef>.AllDefs
									where x.Worker.RandomSelectionWeight(pawn, c) > 0f
									orderby x.Worker.RandomSelectionWeight(pawn, c) descending
									select x).ToList<InteractionDef>();
									float num = list2.Sum((InteractionDef x) => x.Worker.RandomSelectionWeight(pawn, c));
									foreach (InteractionDef interactionDef in list2)
									{
										float f = c.interactions.SocialFightChance(interactionDef, pawn);
										float f2 = interactionDef.Worker.RandomSelectionWeight(pawn, c) / num;
										stringBuilder.AppendLine(string.Concat(new string[]
										{
											"  ",
											interactionDef.defName,
											": ",
											f2.ToStringPercent(),
											" (fight chance: ",
											f.ToStringPercent("F2"),
											")"
										}));
										if (interactionDef == InteractionDefOf.RomanceAttempt)
										{
											stringBuilder.AppendLine("    success chance: " + ((InteractionWorker_RomanceAttempt)interactionDef.Worker).SuccessChance(pawn, c).ToStringPercent());
										}
										else if (interactionDef == InteractionDefOf.MarriageProposal)
										{
											stringBuilder.AppendLine("    acceptance chance: " + ((InteractionWorker_MarriageProposal)interactionDef.Worker).AcceptanceChance(pawn, c).ToStringPercent());
										}
									}
								}
							}
						}
						Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
					list.Add(new FloatMenuOption("Lovin' MTB", delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendLine("Lovin' MTB hours with pawn X.");
						stringBuilder.AppendLine("Assuming both pawns are in bed and are partners.");
						stringBuilder.AppendLine();
						IOrderedEnumerable<Pawn> orderedEnumerable = from x in pawn.Map.mapPawns.AllPawnsSpawned
						where x.def == pawn.def
						orderby pawn.relations.SecondaryRomanceChanceFactor(x) descending
						select x;
						foreach (Pawn pawn2 in orderedEnumerable)
						{
							if (pawn2 != pawn)
							{
								stringBuilder.AppendLine(string.Concat(new object[]
								{
									pawn2.LabelShort,
									" (",
									pawn2.KindLabel,
									", age: ",
									pawn2.ageTracker.AgeBiologicalYears,
									"): ",
									LovePartnerRelationUtility.GetLovinMtbHours(pawn, pawn2).ToString("F1"),
									" h"
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				list.Add(new FloatMenuOption("Test per pawns pair compatibility factor probability", delegate()
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
						if (num11 < -3f)
						{
							num++;
						}
						else if (num11 < -2f)
						{
							num2++;
						}
						else if (num11 < -1f)
						{
							num3++;
						}
						else if (num11 < 0f)
						{
							num4++;
						}
						else if (num11 < 1f)
						{
							num5++;
						}
						else if (num11 < 2f)
						{
							num6++;
						}
						else if (num11 < 3f)
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
					stringBuilder.AppendLine("< -3: " + ((float)num / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< -2: " + ((float)num2 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< -1: " + ((float)num3 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 0: " + ((float)num4 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 1: " + ((float)num5 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 2: " + ((float)num6 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("< 3: " + ((float)num7 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine("> 3: " + ((float)num8 / 10000f).ToStringPercent("F2"));
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("trials: " + 10000);
					stringBuilder.AppendLine("min: " + num10);
					stringBuilder.AppendLine("max: " + num9);
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Find.WindowStack.Add(new FloatMenu(list));
			}
			GUI.EndGroup();
		}

		// Token: 0x040018D1 RID: 6353
		private static Vector2 listScrollPosition = Vector2.zero;

		// Token: 0x040018D2 RID: 6354
		private static float listScrollViewHeight = 0f;

		// Token: 0x040018D3 RID: 6355
		private static bool showAllRelations;

		// Token: 0x040018D4 RID: 6356
		private static List<SocialCardUtility.CachedSocialTabEntry> cachedEntries = new List<SocialCardUtility.CachedSocialTabEntry>();

		// Token: 0x040018D5 RID: 6357
		private static Pawn cachedForPawn;

		// Token: 0x040018D6 RID: 6358
		private const float TopPadding = 20f;

		// Token: 0x040018D7 RID: 6359
		private static readonly Color RelationLabelColor = new Color(0.6f, 0.6f, 0.6f);

		// Token: 0x040018D8 RID: 6360
		private static readonly Color PawnLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);

		// Token: 0x040018D9 RID: 6361
		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x040018DA RID: 6362
		private const float RowTopPadding = 3f;

		// Token: 0x040018DB RID: 6363
		private const float RowLeftRightPadding = 5f;

		// Token: 0x040018DC RID: 6364
		private static SocialCardUtility.CachedSocialTabEntryComparer CachedEntriesComparer = new SocialCardUtility.CachedSocialTabEntryComparer();

		// Token: 0x040018DD RID: 6365
		private static HashSet<Pawn> tmpCached = new HashSet<Pawn>();

		// Token: 0x040018DE RID: 6366
		private static HashSet<Pawn> tmpToCache = new HashSet<Pawn>();

		// Token: 0x0200081F RID: 2079
		private class CachedSocialTabEntry
		{
			// Token: 0x040018E1 RID: 6369
			public Pawn otherPawn;

			// Token: 0x040018E2 RID: 6370
			public int opinionOfOtherPawn;

			// Token: 0x040018E3 RID: 6371
			public int opinionOfMe;

			// Token: 0x040018E4 RID: 6372
			public List<PawnRelationDef> relations = new List<PawnRelationDef>();
		}

		// Token: 0x02000820 RID: 2080
		private class CachedSocialTabEntryComparer : IComparer<SocialCardUtility.CachedSocialTabEntry>
		{
			// Token: 0x06002E7D RID: 11901 RVA: 0x0018C104 File Offset: 0x0018A504
			public int Compare(SocialCardUtility.CachedSocialTabEntry a, SocialCardUtility.CachedSocialTabEntry b)
			{
				bool flag = a.relations.Any<PawnRelationDef>();
				bool flag2 = b.relations.Any<PawnRelationDef>();
				int result;
				if (flag != flag2)
				{
					result = flag2.CompareTo(flag);
				}
				else
				{
					if (flag && flag2)
					{
						float num = float.MinValue;
						for (int i = 0; i < a.relations.Count; i++)
						{
							if (a.relations[i].importance > num)
							{
								num = a.relations[i].importance;
							}
						}
						float num2 = float.MinValue;
						for (int j = 0; j < b.relations.Count; j++)
						{
							if (b.relations[j].importance > num2)
							{
								num2 = b.relations[j].importance;
							}
						}
						if (num != num2)
						{
							return num2.CompareTo(num);
						}
					}
					if (a.opinionOfOtherPawn != b.opinionOfOtherPawn)
					{
						result = b.opinionOfOtherPawn.CompareTo(a.opinionOfOtherPawn);
					}
					else
					{
						result = a.otherPawn.thingIDNumber.CompareTo(b.otherPawn.thingIDNumber);
					}
				}
				return result;
			}
		}
	}
}
