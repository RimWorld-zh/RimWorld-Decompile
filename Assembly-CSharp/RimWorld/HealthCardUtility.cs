using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000815 RID: 2069
	[StaticConstructorOnStartup]
	public static class HealthCardUtility
	{
		// Token: 0x04001897 RID: 6295
		private static Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001898 RID: 6296
		private static float scrollViewHeight = 0f;

		// Token: 0x04001899 RID: 6297
		private static bool highlight = true;

		// Token: 0x0400189A RID: 6298
		private static bool onOperationTab = false;

		// Token: 0x0400189B RID: 6299
		private static Vector2 billsScrollPosition = Vector2.zero;

		// Token: 0x0400189C RID: 6300
		private static float billsScrollHeight = 1000f;

		// Token: 0x0400189D RID: 6301
		private static bool showAllHediffs = false;

		// Token: 0x0400189E RID: 6302
		private static bool showHediffsDebugInfo = false;

		// Token: 0x0400189F RID: 6303
		public const float TopPadding = 20f;

		// Token: 0x040018A0 RID: 6304
		private const float ThoughtLevelHeight = 25f;

		// Token: 0x040018A1 RID: 6305
		private const float ThoughtLevelSpacing = 4f;

		// Token: 0x040018A2 RID: 6306
		private const float IconSize = 20f;

		// Token: 0x040018A3 RID: 6307
		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x040018A4 RID: 6308
		private static readonly Color StaticHighlightColor = new Color(0.75f, 0.75f, 0.85f, 1f);

		// Token: 0x040018A5 RID: 6309
		private static readonly Color VeryPoorColor = new Color(0.4f, 0.4f, 0.4f);

		// Token: 0x040018A6 RID: 6310
		private static readonly Color PoorColor = new Color(0.55f, 0.55f, 0.55f);

		// Token: 0x040018A7 RID: 6311
		private static readonly Color WeakenedColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x040018A8 RID: 6312
		private static readonly Color EnhancedColor = new Color(0.5f, 0.5f, 0.9f);

		// Token: 0x040018A9 RID: 6313
		private static readonly Color MediumPainColor = new Color(0.9f, 0.9f, 0f);

		// Token: 0x040018AA RID: 6314
		private static readonly Color SeverePainColor = new Color(0.9f, 0.5f, 0f);

		// Token: 0x040018AB RID: 6315
		private static readonly Texture2D BleedingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Medical/Bleeding", true);

		// Token: 0x040018AC RID: 6316
		private static List<ThingDef> tmpMedicineBestToWorst = new List<ThingDef>();

		// Token: 0x06002E2B RID: 11819 RVA: 0x00186488 File Offset: 0x00184888
		public static void DrawPawnHealthCard(Rect outRect, Pawn pawn, bool allowOperations, bool showBloodLoss, Thing thingForMedBills)
		{
			if (pawn.Dead && allowOperations)
			{
				Log.Error("Called DrawPawnHealthCard with a dead pawn and allowOperations=true. Operations are disallowed on corpses.", false);
				allowOperations = false;
			}
			outRect = outRect.Rounded();
			Rect rect = new Rect(outRect.x, outRect.y, outRect.width * 0.375f, outRect.height).Rounded();
			Rect rect2 = new Rect(rect.xMax, outRect.y, outRect.width - rect.width, outRect.height);
			rect.yMin += 11f;
			HealthCardUtility.DrawHealthSummary(rect, pawn, allowOperations, thingForMedBills);
			HealthCardUtility.DrawHediffListing(rect2.ContractedBy(10f), pawn, showBloodLoss);
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x00186548 File Offset: 0x00184948
		public static void DrawHealthSummary(Rect rect, Pawn pawn, bool allowOperations, Thing thingForMedBills)
		{
			GUI.color = Color.white;
			if (!allowOperations)
			{
				HealthCardUtility.onOperationTab = false;
			}
			Widgets.DrawMenuSection(rect);
			List<TabRecord> list = new List<TabRecord>();
			list.Add(new TabRecord("HealthOverview".Translate(), delegate()
			{
				HealthCardUtility.onOperationTab = false;
			}, !HealthCardUtility.onOperationTab));
			if (allowOperations)
			{
				string label = (!pawn.RaceProps.IsMechanoid) ? "MedicalOperationsShort".Translate(new object[]
				{
					pawn.BillStack.Count
				}) : "MedicalOperationsMechanoidsShort".Translate(new object[]
				{
					pawn.BillStack.Count
				});
				list.Add(new TabRecord(label, delegate()
				{
					HealthCardUtility.onOperationTab = true;
				}, HealthCardUtility.onOperationTab));
			}
			TabDrawer.DrawTabs(rect, list, 200f);
			rect = rect.ContractedBy(9f);
			GUI.BeginGroup(rect);
			float curY = 0f;
			Text.Font = GameFont.Medium;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperCenter;
			if (HealthCardUtility.onOperationTab)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.MedicalOperations, KnowledgeAmount.FrameDisplayed);
				curY = HealthCardUtility.DrawMedOperationsTab(rect, pawn, thingForMedBills, curY);
			}
			else
			{
				curY = HealthCardUtility.DrawOverviewTab(rect, pawn, curY);
			}
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x001866CC File Offset: 0x00184ACC
		public static void DrawHediffListing(Rect rect, Pawn pawn, bool showBloodLoss)
		{
			GUI.color = Color.white;
			if (Prefs.DevMode && Current.ProgramState == ProgramState.Playing)
			{
				HealthCardUtility.DoDebugOptions(rect, pawn);
			}
			GUI.BeginGroup(rect);
			float lineHeight = Text.LineHeight;
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height - lineHeight);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, HealthCardUtility.scrollViewHeight);
			Rect rect2 = rect;
			if (viewRect.height > outRect.height)
			{
				rect2.width -= 16f;
			}
			Widgets.BeginScrollView(outRect, ref HealthCardUtility.scrollPosition, viewRect, true);
			GUI.color = Color.white;
			float num = 0f;
			HealthCardUtility.highlight = true;
			bool flag = false;
			foreach (IGrouping<BodyPartRecord, Hediff> diffs in HealthCardUtility.VisibleHediffGroupsInOrder(pawn, showBloodLoss))
			{
				flag = true;
				HealthCardUtility.DrawHediffRow(rect2, pawn, diffs, ref num);
			}
			if (!flag)
			{
				Widgets.NoneLabelCenteredVertically(new Rect(0f, 0f, viewRect.width, outRect.height), "(" + "NoHealthConditions".Translate() + ")");
				num = outRect.height - 1f;
			}
			if (Event.current.type == EventType.Layout)
			{
				HealthCardUtility.scrollViewHeight = num;
			}
			Widgets.EndScrollView();
			float bleedRateTotal = pawn.health.hediffSet.BleedRateTotal;
			if (bleedRateTotal > 0.01f)
			{
				Rect rect3 = new Rect(0f, rect.height - lineHeight, rect.width, lineHeight);
				string text = string.Concat(new string[]
				{
					"BleedingRate".Translate(),
					": ",
					bleedRateTotal.ToStringPercent(),
					"/",
					"LetterDay".Translate()
				});
				int num2 = HealthUtility.TicksUntilDeathDueToBloodLoss(pawn);
				if (num2 < 60000)
				{
					text = text + " (" + "TimeToDeath".Translate(new object[]
					{
						num2.ToStringTicksToPeriod()
					}) + ")";
				}
				else
				{
					text = text + " (" + "WontBleedOutSoon".Translate() + ")";
				}
				Widgets.Label(rect3, text);
			}
			GUI.EndGroup();
			GUI.color = Color.white;
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x00186968 File Offset: 0x00184D68
		private static IEnumerable<IGrouping<BodyPartRecord, Hediff>> VisibleHediffGroupsInOrder(Pawn pawn, bool showBloodLoss)
		{
			foreach (IGrouping<BodyPartRecord, Hediff> group in from x in HealthCardUtility.VisibleHediffs(pawn, showBloodLoss)
			group x by x.Part into x
			orderby HealthCardUtility.GetListPriority(x.First<Hediff>().Part) descending
			select x)
			{
				yield return group;
			}
			yield break;
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x0018699C File Offset: 0x00184D9C
		private static float GetListPriority(BodyPartRecord rec)
		{
			float result;
			if (rec == null)
			{
				result = 9999999f;
			}
			else
			{
				result = (float)((int)rec.height * 10000) + rec.coverageAbsWithChildren;
			}
			return result;
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x001869D8 File Offset: 0x00184DD8
		private static IEnumerable<Hediff> VisibleHediffs(Pawn pawn, bool showBloodLoss)
		{
			if (!HealthCardUtility.showAllHediffs)
			{
				List<Hediff_MissingPart> mpca = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				for (int i = 0; i < mpca.Count; i++)
				{
					yield return mpca[i];
				}
				IEnumerable<Hediff> visibleDiffs = from d in pawn.health.hediffSet.hediffs
				where !(d is Hediff_MissingPart) && d.Visible && (showBloodLoss || d.def != HediffDefOf.BloodLoss)
				select d;
				foreach (Hediff diff in visibleDiffs)
				{
					yield return diff;
				}
			}
			else
			{
				foreach (Hediff diff2 in pawn.health.hediffSet.hediffs)
				{
					yield return diff2;
				}
			}
			yield break;
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x00186A0C File Offset: 0x00184E0C
		private static float DrawMedOperationsTab(Rect leftRect, Pawn pawn, Thing thingForMedBills, float curY)
		{
			curY += 2f;
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (RecipeDef recipeDef in thingForMedBills.def.AllRecipes)
				{
					if (recipeDef.AvailableNow)
					{
						IEnumerable<ThingDef> enumerable = recipeDef.PotentiallyMissingIngredients(null, thingForMedBills.Map);
						if (!enumerable.Any((ThingDef x) => x.isTechHediff))
						{
							if (!enumerable.Any((ThingDef x) => x.IsDrug))
							{
								if (!enumerable.Any<ThingDef>() || !recipeDef.dontShowIfAnyIngredientMissing)
								{
									if (recipeDef.targetsBodyPart)
									{
										foreach (BodyPartRecord part in recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef))
										{
											list.Add(HealthCardUtility.GenerateSurgeryOption(pawn, thingForMedBills, recipeDef, enumerable, part));
										}
									}
									else
									{
										list.Add(HealthCardUtility.GenerateSurgeryOption(pawn, thingForMedBills, recipeDef, enumerable, null));
									}
								}
							}
						}
					}
				}
				return list;
			};
			Rect rect = new Rect(leftRect.x - 9f, curY, leftRect.width, leftRect.height - curY - 20f);
			((IBillGiver)thingForMedBills).BillStack.DoListing(rect, recipeOptionsMaker, ref HealthCardUtility.billsScrollPosition, ref HealthCardUtility.billsScrollHeight);
			return curY;
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x00186A9C File Offset: 0x00184E9C
		private static FloatMenuOption GenerateSurgeryOption(Pawn pawn, Thing thingForMedBills, RecipeDef recipe, IEnumerable<ThingDef> missingIngredients, BodyPartRecord part = null)
		{
			string text = recipe.Worker.GetLabelWhenUsedOn(pawn, part).CapitalizeFirst();
			if (part != null && !recipe.hideBodyPartNames)
			{
				text = text + " (" + part.Label + ")";
			}
			FloatMenuOption floatMenuOption;
			if (missingIngredients.Any<ThingDef>())
			{
				text += " (";
				bool flag = true;
				foreach (ThingDef thingDef in missingIngredients)
				{
					if (!flag)
					{
						text += ", ";
					}
					flag = false;
					text += "MissingMedicalBillIngredient".Translate(new object[]
					{
						thingDef.label
					});
				}
				text += ")";
				floatMenuOption = new FloatMenuOption(text, null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else
			{
				Action action = delegate()
				{
					Pawn pawn2 = thingForMedBills as Pawn;
					if (pawn2 != null)
					{
						Bill_Medical bill_Medical = new Bill_Medical(recipe);
						pawn2.BillStack.AddBill(bill_Medical);
						bill_Medical.Part = part;
						if (recipe.conceptLearned != null)
						{
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(recipe.conceptLearned, KnowledgeAmount.Total);
						}
						Map map = thingForMedBills.Map;
						if (!map.mapPawns.FreeColonists.Any((Pawn col) => recipe.PawnSatisfiesSkillRequirements(col)))
						{
							Bill.CreateNoPawnsWithSkillDialog(recipe);
						}
						if (!pawn2.InBed() && pawn2.RaceProps.IsFlesh)
						{
							if (pawn2.RaceProps.Humanlike)
							{
								if (!map.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(pawn, x.def) && ((Building_Bed)x).Medical))
								{
									Messages.Message("MessageNoMedicalBeds".Translate(), pawn2, MessageTypeDefOf.CautionInput, false);
								}
							}
							else if (!map.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(pawn, x.def)))
							{
								Messages.Message("MessageNoAnimalBeds".Translate(), pawn2, MessageTypeDefOf.CautionInput, false);
							}
						}
						if (pawn2.Faction != null && !pawn2.Faction.def.hidden && !pawn2.Faction.HostileTo(Faction.OfPlayer) && recipe.Worker.IsViolationOnPawn(pawn2, part, Faction.OfPlayer))
						{
							Messages.Message("MessageMedicalOperationWillAngerFaction".Translate(new object[]
							{
								pawn2.Faction
							}), pawn2, MessageTypeDefOf.CautionInput, false);
						}
						ThingDef minRequiredMedicine = HealthCardUtility.GetMinRequiredMedicine(recipe);
						if (minRequiredMedicine != null && pawn2.playerSettings != null && !pawn2.playerSettings.medCare.AllowsMedicine(minRequiredMedicine))
						{
							Messages.Message("MessageTooLowMedCare".Translate(new object[]
							{
								minRequiredMedicine.label,
								pawn2.LabelShort,
								pawn2.playerSettings.medCare.GetLabel()
							}), pawn2, MessageTypeDefOf.CautionInput, false);
						}
					}
				};
				floatMenuOption = new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			floatMenuOption.extraPartWidth = 29f;
			floatMenuOption.extraPartOnGUI = ((Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, recipe));
			return floatMenuOption;
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x00186C2C File Offset: 0x0018502C
		private static ThingDef GetMinRequiredMedicine(RecipeDef recipe)
		{
			HealthCardUtility.tmpMedicineBestToWorst.Clear();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].IsMedicine)
				{
					HealthCardUtility.tmpMedicineBestToWorst.Add(allDefsListForReading[i]);
				}
			}
			HealthCardUtility.tmpMedicineBestToWorst.SortByDescending((ThingDef x) => x.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			ThingDef thingDef = null;
			for (int j = 0; j < recipe.ingredients.Count; j++)
			{
				ThingDef thingDef2 = null;
				for (int k = 0; k < HealthCardUtility.tmpMedicineBestToWorst.Count; k++)
				{
					if (recipe.ingredients[j].filter.Allows(HealthCardUtility.tmpMedicineBestToWorst[k]))
					{
						thingDef2 = HealthCardUtility.tmpMedicineBestToWorst[k];
					}
				}
				if (thingDef2 != null && (thingDef == null || thingDef2.GetStatValueAbstract(StatDefOf.MedicalPotency, null) > thingDef.GetStatValueAbstract(StatDefOf.MedicalPotency, null)))
				{
					thingDef = thingDef2;
				}
			}
			HealthCardUtility.tmpMedicineBestToWorst.Clear();
			return thingDef;
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x00186D6C File Offset: 0x0018516C
		private static float DrawOverviewTab(Rect leftRect, Pawn pawn, float curY)
		{
			curY += 4f;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(0.9f, 0.9f, 0.9f);
			string text = "";
			if (pawn.gender != Gender.None)
			{
				text = pawn.gender.GetLabel() + " ";
			}
			text = text + pawn.def.label + ", " + "AgeIndicator".Translate(new object[]
			{
				pawn.ageTracker.AgeNumberString
			});
			Rect rect = new Rect(0f, curY, leftRect.width, 34f);
			Widgets.Label(rect, text.CapitalizeFirst());
			TooltipHandler.TipRegion(rect, () => pawn.ageTracker.AgeTooltipString, 73412);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			GUI.color = Color.white;
			curY += 34f;
			GUI.color = Color.white;
			if (pawn.IsColonist && !pawn.Dead)
			{
				bool selfTend = pawn.playerSettings.selfTend;
				Rect rect2 = new Rect(0f, curY, leftRect.width, 24f);
				Widgets.CheckboxLabeled(rect2, "SelfTend".Translate(), ref pawn.playerSettings.selfTend, false, null, null, false);
				if (pawn.playerSettings.selfTend && !selfTend)
				{
					if (pawn.story.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
					{
						pawn.playerSettings.selfTend = false;
						Messages.Message("MessageCannotSelfTendEver".Translate(new object[]
						{
							pawn.LabelShort
						}), MessageTypeDefOf.RejectInput, false);
					}
					else if (pawn.workSettings.GetPriority(WorkTypeDefOf.Doctor) == 0)
					{
						Messages.Message("MessageSelfTendUnsatisfied".Translate(new object[]
						{
							pawn.LabelShort
						}), MessageTypeDefOf.CautionInput, false);
					}
				}
				TooltipHandler.TipRegion(rect2, "SelfTendTip".Translate(new object[]
				{
					Faction.OfPlayer.def.pawnsPlural,
					0.7f.ToStringPercent()
				}).CapitalizeFirst());
				curY += 28f;
			}
			if (pawn.playerSettings != null && !pawn.Dead && Current.ProgramState == ProgramState.Playing)
			{
				Rect rect3 = new Rect(0f, curY, 140f, 28f);
				MedicalCareUtility.MedicalCareSetter(rect3, ref pawn.playerSettings.medCare);
				if (Widgets.ButtonText(new Rect(leftRect.width - 70f, curY, 70f, 28f), "MedGroupDefaults".Translate(), true, false, true))
				{
					Find.WindowStack.Add(new Dialog_MedicalDefaults());
				}
				curY += 32f;
			}
			Text.Font = GameFont.Small;
			if (pawn.def.race.IsFlesh)
			{
				Pair<string, Color> painLabel = HealthCardUtility.GetPainLabel(pawn);
				string painTip = HealthCardUtility.GetPainTip(pawn);
				curY = HealthCardUtility.DrawLeftRow(leftRect, curY, "PainLevel".Translate(), painLabel.First, painLabel.Second, painTip);
			}
			if (!pawn.Dead)
			{
				IEnumerable<PawnCapacityDef> source;
				if (pawn.def.race.Humanlike)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnHumanlikes
					select x;
				}
				else if (pawn.def.race.Animal)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnAnimals
					select x;
				}
				else
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnMechanoids
					select x;
				}
				foreach (PawnCapacityDef pawnCapacityDef in from act in source
				orderby act.listOrder
				select act)
				{
					if (PawnCapacityUtility.BodyCanEverDoCapacity(pawn.RaceProps.body, pawnCapacityDef))
					{
						PawnCapacityDef activityLocal = pawnCapacityDef;
						Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel(pawn, pawnCapacityDef);
						Func<string> textGetter = () => (!pawn.Dead) ? HealthCardUtility.GetPawnCapacityTip(pawn, activityLocal) : "";
						curY = HealthCardUtility.DrawLeftRow(leftRect, curY, pawnCapacityDef.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike).CapitalizeFirst(), efficiencyLabel.First, efficiencyLabel.Second, new TipSignal(textGetter, pawn.thingIDNumber ^ (int)pawnCapacityDef.index));
					}
				}
			}
			return curY;
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x00187308 File Offset: 0x00185708
		private static float DrawLeftRow(Rect leftRect, float curY, string leftLabel, string rightLabel, Color rightLabelColor, TipSignal tipSignal)
		{
			Rect rect = new Rect(0f, curY, leftRect.width, 20f);
			if (Mouse.IsOver(rect))
			{
				GUI.color = HealthCardUtility.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			GUI.color = Color.white;
			Widgets.Label(new Rect(0f, curY, leftRect.width * 0.65f, 30f), leftLabel);
			GUI.color = rightLabelColor;
			Widgets.Label(new Rect(leftRect.width * 0.65f, curY, leftRect.width * 0.35f, 30f), rightLabel);
			TooltipHandler.TipRegion(new Rect(0f, curY, leftRect.width, 20f), tipSignal);
			curY += 20f;
			return curY;
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x001873E0 File Offset: 0x001857E0
		private static void DrawHediffRow(Rect rect, Pawn pawn, IEnumerable<Hediff> diffs, ref float curY)
		{
			float num = rect.width * 0.375f;
			float width = rect.width - num - 20f;
			BodyPartRecord part = diffs.First<Hediff>().Part;
			float a;
			if (part == null)
			{
				a = Text.CalcHeight("WholeBody".Translate(), num);
			}
			else
			{
				a = Text.CalcHeight(part.LabelCap, num);
			}
			float b = 0f;
			float num2 = curY;
			float num3 = 0f;
			foreach (IGrouping<int, Hediff> source in from x in diffs
			group x by x.UIGroupKey)
			{
				int num4 = source.Count<Hediff>();
				string text = source.First<Hediff>().LabelCap;
				if (num4 != 1)
				{
					text = text + " x" + num4.ToString();
				}
				num3 += Text.CalcHeight(text, width);
			}
			b = num3;
			Rect rect2 = new Rect(0f, curY, rect.width, Mathf.Max(a, b));
			HealthCardUtility.DoRightRowHighlight(rect2);
			if (part != null)
			{
				GUI.color = HealthUtility.GetPartConditionLabel(pawn, part).Second;
				Widgets.Label(new Rect(0f, curY, num, 100f), part.LabelCap);
			}
			else
			{
				GUI.color = HealthUtility.DarkRedColor;
				Widgets.Label(new Rect(0f, curY, num, 100f), "WholeBody".Translate());
			}
			GUI.color = Color.white;
			foreach (IGrouping<int, Hediff> grouping in from x in diffs
			group x by x.UIGroupKey)
			{
				int num5 = 0;
				Hediff hediff = null;
				Texture2D texture2D = null;
				TextureAndColor textureAndColor = null;
				float num6 = 0f;
				foreach (Hediff hediff2 in grouping)
				{
					if (num5 == 0)
					{
						hediff = hediff2;
					}
					textureAndColor = hediff2.StateIcon;
					if (hediff2.Bleeding)
					{
						texture2D = HealthCardUtility.BleedingIcon;
					}
					num6 += hediff2.BleedRate;
					num5++;
				}
				string text2 = hediff.LabelCap;
				if (num5 != 1)
				{
					text2 = text2 + " x" + num5.ToStringCached();
				}
				GUI.color = hediff.LabelColor;
				float num7 = Text.CalcHeight(text2, width);
				Rect rect3 = new Rect(num, curY, width, num7);
				Widgets.Label(rect3, text2);
				GUI.color = Color.white;
				Rect rect4 = new Rect(rect2.xMax - 20f, curY, 20f, 20f);
				if (texture2D)
				{
					Rect position = rect4.ContractedBy(GenMath.LerpDouble(0f, 0.6f, 5f, 0f, Mathf.Min(num6, 1f)));
					GUI.DrawTexture(position, texture2D);
					rect4.x -= rect4.width;
				}
				if (textureAndColor.HasValue)
				{
					GUI.color = textureAndColor.Color;
					GUI.DrawTexture(rect4, textureAndColor.Texture);
					GUI.color = Color.white;
					rect4.x -= rect4.width;
				}
				curY += num7;
			}
			GUI.color = Color.white;
			curY = num2 + Mathf.Max(a, b);
			if (Widgets.ButtonInvisible(rect2, false))
			{
				HealthCardUtility.EntryClicked(diffs, pawn);
			}
			TooltipHandler.TipRegion(rect2, new TipSignal(() => HealthCardUtility.GetTooltip(diffs, pawn, part), (int)curY + 7857));
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x00187888 File Offset: 0x00185C88
		public static string GetPainTip(Pawn pawn)
		{
			return "PainLevel".Translate() + ": " + (pawn.health.hediffSet.PainTotal * 100f).ToString("F0") + "%";
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x001878DC File Offset: 0x00185CDC
		public static string GetPawnCapacityTip(Pawn pawn, PawnCapacityDef capacity)
		{
			List<PawnCapacityUtility.CapacityImpactor> list = new List<PawnCapacityUtility.CapacityImpactor>();
			float num = PawnCapacityUtility.CalculateCapacityLevel(pawn.health.hediffSet, capacity, list);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(capacity.LabelCap + ": " + (num * 100f).ToString("F0") + "%");
			if (list.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("AffectedBy".Translate());
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] is PawnCapacityUtility.CapacityImpactorHediff)
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[i].Readable(pawn)));
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j] is PawnCapacityUtility.CapacityImpactorBodyPartHealth)
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[j].Readable(pawn)));
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k] is PawnCapacityUtility.CapacityImpactorCapacity)
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[k].Readable(pawn)));
					}
				}
				for (int l = 0; l < list.Count; l++)
				{
					if (list[l] is PawnCapacityUtility.CapacityImpactorPain)
					{
						stringBuilder.AppendLine(string.Format("  {0}", list[l].Readable(pawn)));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x00187AA8 File Offset: 0x00185EA8
		private static string GetTooltip(IEnumerable<Hediff> diffs, Pawn pawn, BodyPartRecord part)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (part != null)
			{
				stringBuilder.Append(part.LabelCap + ": ");
				stringBuilder.AppendLine(" " + pawn.health.hediffSet.GetPartHealth(part).ToString() + " / " + part.def.GetMaxHealth(pawn).ToString());
				float num = PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, part, false, null);
				if (num != 1f)
				{
					stringBuilder.AppendLine("Efficiency".Translate() + ": " + num.ToStringPercent());
				}
			}
			else
			{
				stringBuilder.AppendLine("WholeBody".Translate());
			}
			stringBuilder.AppendLine("------------------");
			foreach (IGrouping<int, Hediff> grouping in from x in diffs
			group x by x.UIGroupKey)
			{
				foreach (Hediff hediff in grouping)
				{
					string severityLabel = hediff.SeverityLabel;
					bool flag = HealthCardUtility.showHediffsDebugInfo && !hediff.DebugString().NullOrEmpty();
					if (!hediff.Label.NullOrEmpty() || !severityLabel.NullOrEmpty() || !hediff.CapMods.NullOrEmpty<PawnCapacityModifier>() || flag)
					{
						stringBuilder.Append(hediff.LabelCap);
						if (!severityLabel.NullOrEmpty())
						{
							stringBuilder.Append(": " + severityLabel);
						}
						stringBuilder.AppendLine();
						string tipStringExtra = hediff.TipStringExtra;
						if (!tipStringExtra.NullOrEmpty())
						{
							stringBuilder.AppendLine(tipStringExtra.TrimEndNewlines().Indented("    "));
						}
						if (flag)
						{
							stringBuilder.AppendLine(hediff.DebugString().TrimEndNewlines());
						}
					}
				}
			}
			string s;
			LogEntry logEntry;
			if (HealthCardUtility.GetCombatLogInfo(diffs, out s, out logEntry))
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Cause:");
				stringBuilder.AppendLine(s.Indented("    "));
			}
			return stringBuilder.ToString().TrimEnd(new char[0]);
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x00187D80 File Offset: 0x00186180
		private static void EntryClicked(IEnumerable<Hediff> diffs, Pawn pawn)
		{
			LogEntry combatLogEntry;
			string text;
			if (HealthCardUtility.GetCombatLogInfo(diffs, out text, out combatLogEntry) && combatLogEntry != null)
			{
				if (Find.BattleLog.Battles.Any((Battle b) => b.Concerns(pawn) && b.Entries.Any((LogEntry e) => e == combatLogEntry)))
				{
					ITab_Pawn_Log tab_Pawn_Log = InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Log)) as ITab_Pawn_Log;
					if (tab_Pawn_Log != null)
					{
						tab_Pawn_Log.SeekTo(combatLogEntry);
						tab_Pawn_Log.Highlight(combatLogEntry);
					}
				}
			}
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x00187E1C File Offset: 0x0018621C
		private static bool GetCombatLogInfo(IEnumerable<Hediff> diffs, out string combatLogText, out LogEntry combatLogEntry)
		{
			combatLogText = null;
			combatLogEntry = null;
			foreach (Hediff hediff in diffs)
			{
				if ((hediff.combatLogEntry != null && hediff.combatLogEntry.Target != null) || (combatLogText.NullOrEmpty() && !hediff.combatLogText.NullOrEmpty()))
				{
					combatLogEntry = ((hediff.combatLogEntry == null) ? null : hediff.combatLogEntry.Target);
					combatLogText = hediff.combatLogText;
				}
				if (combatLogEntry != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x00187EE8 File Offset: 0x001862E8
		private static void DoRightRowHighlight(Rect rowRect)
		{
			if (HealthCardUtility.highlight)
			{
				GUI.color = HealthCardUtility.StaticHighlightColor;
				GUI.DrawTexture(rowRect, TexUI.HighlightTex);
			}
			HealthCardUtility.highlight = !HealthCardUtility.highlight;
			if (Mouse.IsOver(rowRect))
			{
				GUI.color = HealthCardUtility.HighlightColor;
				GUI.DrawTexture(rowRect, TexUI.HighlightTex);
			}
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x00187F48 File Offset: 0x00186348
		private static void DoDebugOptions(Rect rightRect, Pawn pawn)
		{
			Widgets.CheckboxLabeled(new Rect(rightRect.x, rightRect.y - 25f, 110f, 30f), "Dev: AllDiffs", ref HealthCardUtility.showAllHediffs, false, null, null, false);
			Widgets.CheckboxLabeled(new Rect(rightRect.x + 115f, rightRect.y - 25f, 120f, 30f), "DiffsDebugInfo", ref HealthCardUtility.showHediffsDebugInfo, false, null, null, false);
			if (Widgets.ButtonText(new Rect(rightRect.x + 240f, rightRect.y - 27f, 115f, 25f), "Debug info", true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("Parts hit chance (this part or any child node)", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in from x in pawn.RaceProps.body.AllParts
					orderby x.coverageAbsWithChildren descending
					select x)
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.coverageAbsWithChildren.ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("Parts hit chance (exactly this part)", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					float num = 0f;
					foreach (BodyPartRecord bodyPartRecord in from x in pawn.RaceProps.body.AllParts
					orderby x.coverageAbs descending
					select x)
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.coverageAbs.ToStringPercent());
						num += bodyPartRecord.coverageAbs;
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Total " + num.ToStringPercent());
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("Per-part efficiency", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in pawn.RaceProps.body.AllParts)
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, bodyPartRecord, false, null).ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("BodyPartGroup efficiency (of only natural parts)", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartGroupDef bodyPartGroupDef in from x in DefDatabase<BodyPartGroupDef>.AllDefs
					where pawn.RaceProps.body.AllParts.Any((BodyPartRecord y) => y.groups.Contains(x))
					select x)
					{
						stringBuilder.AppendLine(bodyPartGroupDef.LabelCap + " " + PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(pawn.health.hediffSet, bodyPartGroupDef).ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("IsSolid", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null))
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.def.IsSolid(bodyPartRecord, pawn.health.hediffSet.hediffs));
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("IsSkinCovered", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null))
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.def.IsSkinCovered(bodyPartRecord, pawn.health.hediffSet));
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("does have added parts", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null))
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(bodyPartRecord));
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("GetNotMissingParts", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null))
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap);
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("GetCoverageOfNotMissingNaturalParts", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (BodyPartRecord bodyPartRecord in from x in pawn.RaceProps.body.AllParts
					orderby pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x) descending
					select x)
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + ": " + pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(bodyPartRecord).ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("parts nutrition (assuming adult)", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					float totalCorpseNutrition = StatDefOf.Nutrition.Worker.GetValueAbstract(pawn.RaceProps.corpseDef, null);
					foreach (BodyPartRecord bodyPartRecord in from x in pawn.RaceProps.body.AllParts
					orderby FoodUtility.GetBodyPartNutrition(totalCorpseNutrition, pawn, x) descending
					select x)
					{
						stringBuilder.AppendLine(bodyPartRecord.LabelCap + ": " + FoodUtility.GetBodyPartNutrition(totalCorpseNutrition, pawn, bodyPartRecord));
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("test permanent injury pain factor probability", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					float num4 = 0f;
					int num5 = 10000;
					for (int i = 0; i < num5; i++)
					{
						float randomPainFactor = PermanentInjuryUtility.GetRandomPainFactor();
						if (randomPainFactor < 0f)
						{
							Log.Error("Pain factor < 0", false);
						}
						if (randomPainFactor == 0f)
						{
							num++;
						}
						if (randomPainFactor < 1f)
						{
							num2++;
						}
						if (randomPainFactor < 5f)
						{
							num3++;
						}
						if (randomPainFactor > num4)
						{
							num4 = randomPainFactor;
						}
					}
					stringBuilder.AppendLine("total: " + num5);
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"0: ",
						num,
						" (",
						((float)num / (float)num5).ToStringPercent(),
						")"
					}));
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"< 1: ",
						num2,
						" (",
						((float)num2 / (float)num5).ToStringPercent(),
						")"
					}));
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"< 5: ",
						num3,
						" (",
						((float)num3 / (float)num5).ToStringPercent(),
						")"
					}));
					stringBuilder.AppendLine("max: " + num4);
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("HediffGiver_Birthday chance at age", delegate()
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (HediffGiverSetDef hediffGiverSetDef in pawn.RaceProps.hediffGiverSets)
					{
						foreach (HediffGiver_Birthday hediffGiver_Birthday in hediffGiverSetDef.hediffGivers.OfType<HediffGiver_Birthday>())
						{
							HediffGiver_Birthday hLocal = hediffGiver_Birthday;
							FloatMenuOption item = new FloatMenuOption(hediffGiverSetDef.defName + " - " + hediffGiver_Birthday.hediff.defName, delegate()
							{
								StringBuilder stringBuilder = new StringBuilder();
								stringBuilder.AppendLine("% of pawns which will have at least 1 " + hLocal.hediff.label + " at age X:");
								stringBuilder.AppendLine();
								int num = 1;
								while ((float)num < pawn.RaceProps.lifeExpectancy + 20f)
								{
									stringBuilder.AppendLine(num + ": " + hLocal.DebugChanceToHaveAtAge(pawn, num).ToStringPercent());
									num++;
								}
								Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
							}, MenuOptionPriority.Default, null, null, 0f, null, null);
							list2.Add(item);
						}
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("HediffGiver_Birthday count at age", delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Average hediffs count (from HediffGiver_Birthday) at age X:");
					stringBuilder.AppendLine();
					int num = 1;
					while ((float)num < pawn.RaceProps.lifeExpectancy + 20f)
					{
						float num2 = 0f;
						foreach (HediffGiverSetDef hediffGiverSetDef in pawn.RaceProps.hediffGiverSets)
						{
							foreach (HediffGiver_Birthday hediffGiver_Birthday in hediffGiverSetDef.hediffGivers.OfType<HediffGiver_Birthday>())
							{
								num2 += hediffGiver_Birthday.DebugChanceToHaveAtAge(pawn, num);
							}
						}
						stringBuilder.AppendLine(num + ": " + num2.ToStringDecimalIfSmall());
						num++;
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x00188228 File Offset: 0x00186628
		public static Pair<string, Color> GetEfficiencyLabel(Pawn pawn, PawnCapacityDef activity)
		{
			float level = pawn.health.capacities.GetLevel(activity);
			Color second = Color.white;
			string first;
			if (level <= 0f)
			{
				first = "None".Translate();
				second = HealthUtility.DarkRedColor;
			}
			else if (level < 0.4f)
			{
				first = "VeryPoor".Translate();
				second = HealthCardUtility.VeryPoorColor;
			}
			else if (level < 0.7f)
			{
				first = "Poor".Translate();
				second = HealthCardUtility.PoorColor;
			}
			else if (level < 1f && !Mathf.Approximately(level, 1f))
			{
				first = "Weakened".Translate();
				second = HealthCardUtility.WeakenedColor;
			}
			else if (Mathf.Approximately(level, 1f))
			{
				first = "GoodCondition".Translate();
				second = HealthUtility.GoodConditionColor;
			}
			else
			{
				first = "Enhanced".Translate();
				second = HealthCardUtility.EnhancedColor;
			}
			return new Pair<string, Color>(first, second);
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x0018833C File Offset: 0x0018673C
		public static Pair<string, Color> GetPainLabel(Pawn pawn)
		{
			float painTotal = pawn.health.hediffSet.PainTotal;
			Color second = Color.white;
			string first;
			if (Mathf.Approximately(painTotal, 0f))
			{
				first = "NoPain".Translate();
				second = HealthUtility.GoodConditionColor;
			}
			else if (painTotal < 0.15f)
			{
				first = "LittlePain".Translate();
				second = Color.gray;
			}
			else if (painTotal < 0.4f)
			{
				first = "MediumPain".Translate();
				second = HealthCardUtility.MediumPainColor;
			}
			else if (painTotal < 0.8f)
			{
				first = "SeverePain".Translate();
				second = HealthCardUtility.SeverePainColor;
			}
			else
			{
				first = "ExtremePain".Translate();
				second = HealthUtility.DarkRedColor;
			}
			return new Pair<string, Color>(first, second);
		}
	}
}
