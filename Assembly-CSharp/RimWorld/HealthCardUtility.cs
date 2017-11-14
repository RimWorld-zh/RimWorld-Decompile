using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class HealthCardUtility
	{
		private static Vector2 scrollPosition = Vector2.zero;

		private static float scrollViewHeight = 0f;

		private static bool highlight = true;

		private static bool onOperationTab = false;

		private static Vector2 billsScrollPosition = Vector2.zero;

		private static float billsScrollHeight = 1000f;

		private static bool showAllHediffs = false;

		private static bool showHediffsDebugInfo = false;

		public const float TopPadding = 20f;

		private const float ThoughtLevelHeight = 25f;

		private const float ThoughtLevelSpacing = 4f;

		private const float IconSize = 20f;

		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		private static readonly Color StaticHighlightColor = new Color(0.75f, 0.75f, 0.85f, 1f);

		private static readonly Color VeryPoorColor = new Color(0.4f, 0.4f, 0.4f);

		private static readonly Color PoorColor = new Color(0.55f, 0.55f, 0.55f);

		private static readonly Color WeakenedColor = new Color(0.7f, 0.7f, 0.7f);

		private static readonly Color EnhancedColor = new Color(0.5f, 0.5f, 0.9f);

		private static readonly Color MediumPainColor = new Color(0.9f, 0.9f, 0f);

		private static readonly Color SeverePainColor = new Color(0.9f, 0.5f, 0f);

		private static readonly Texture2D BleedingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Medical/Bleeding", true);

		private static List<ThingDef> tmpMedicineBestToWorst = new List<ThingDef>();

		public static void DrawPawnHealthCard(Rect outRect, Pawn pawn, bool allowOperations, bool showBloodLoss, Thing thingForMedBills)
		{
			if (pawn.Dead && allowOperations)
			{
				Log.Error("Called DrawPawnHealthCard with a dead pawn and allowOperations=true. Operations are disallowed on corpses.");
				allowOperations = false;
			}
			outRect = outRect.Rounded();
			Rect rect = new Rect(outRect.x, outRect.y, (float)(outRect.width * 0.375), outRect.height).Rounded();
			Rect rect2 = new Rect(rect.xMax, outRect.y, outRect.width - rect.width, outRect.height);
			rect.yMin += 11f;
			HealthCardUtility.DrawHealthSummary(rect, pawn, allowOperations, thingForMedBills);
			HealthCardUtility.DrawHediffListing(rect2.ContractedBy(10f), pawn, showBloodLoss);
		}

		public static void DrawHealthSummary(Rect rect, Pawn pawn, bool allowOperations, Thing thingForMedBills)
		{
			GUI.color = Color.white;
			if (!allowOperations)
			{
				HealthCardUtility.onOperationTab = false;
			}
			Widgets.DrawMenuSection(rect);
			List<TabRecord> list = new List<TabRecord>();
			list.Add(new TabRecord("HealthOverview".Translate(), delegate
			{
				HealthCardUtility.onOperationTab = false;
			}, !HealthCardUtility.onOperationTab));
			if (allowOperations)
			{
				string label = (!pawn.RaceProps.IsMechanoid) ? "MedicalOperationsShort".Translate(pawn.BillStack.Count) : "MedicalOperationsMechanoidsShort".Translate(pawn.BillStack.Count);
				list.Add(new TabRecord(label, delegate
				{
					HealthCardUtility.onOperationTab = true;
				}, HealthCardUtility.onOperationTab));
			}
			TabDrawer.DrawTabs(rect, list);
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
			Rect viewRect = new Rect(0f, 0f, (float)(rect.width - 16.0), HealthCardUtility.scrollViewHeight);
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
			foreach (IGrouping<BodyPartRecord, Hediff> item in HealthCardUtility.VisibleHediffGroupsInOrder(pawn, showBloodLoss))
			{
				flag = true;
				HealthCardUtility.DrawHediffRow(rect2, pawn, item, ref num);
			}
			if (!flag)
			{
				GUI.color = Color.gray;
				Text.Anchor = TextAnchor.UpperCenter;
				Rect rect3 = new Rect(0f, 0f, viewRect.width, 30f);
				Widgets.Label(rect3, "NoInjuries".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (Event.current.type == EventType.Layout)
			{
				HealthCardUtility.scrollViewHeight = num;
			}
			Widgets.EndScrollView();
			float bleedRateTotal = pawn.health.hediffSet.BleedRateTotal;
			if (bleedRateTotal > 0.0099999997764825821)
			{
				Rect rect4 = new Rect(0f, rect.height - lineHeight, rect.width, lineHeight);
				string str = "BleedingRate".Translate() + ": " + bleedRateTotal.ToStringPercent() + "/" + "LetterDay".Translate();
				int num2 = HealthUtility.TicksUntilDeathDueToBloodLoss(pawn);
				str = ((num2 >= 60000) ? (str + " (" + "WontBleedOutSoon".Translate() + ")") : (str + " (" + "TimeToDeath".Translate(num2.ToStringTicksToPeriod(true, false, true)) + ")"));
				Widgets.Label(rect4, str);
			}
			GUI.EndGroup();
			GUI.color = Color.white;
		}

		private static IEnumerable<IGrouping<BodyPartRecord, Hediff>> VisibleHediffGroupsInOrder(Pawn pawn, bool showBloodLoss)
		{
			using (IEnumerator<IGrouping<BodyPartRecord, Hediff>> enumerator = (from x in HealthCardUtility.VisibleHediffs(pawn, showBloodLoss)
			group x by x.Part into x
			orderby HealthCardUtility.GetListPriority(x.First().Part) descending
			select x).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					IGrouping<BodyPartRecord, Hediff> group = enumerator.Current;
					yield return group;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_0103:
			/*Error near IL_0104: Unexpected return in MoveNext()*/;
		}

		private static float GetListPriority(BodyPartRecord rec)
		{
			if (rec == null)
			{
				return 9999999f;
			}
			return (float)((int)rec.height * 10000) + rec.coverageAbsWithChildren;
		}

		private static IEnumerable<Hediff> VisibleHediffs(Pawn pawn, bool showBloodLoss)
		{
			_003CVisibleHediffs_003Ec__Iterator1 _003CVisibleHediffs_003Ec__Iterator = (_003CVisibleHediffs_003Ec__Iterator1)/*Error near IL_003c: stateMachine*/;
			if (!HealthCardUtility.showAllHediffs)
			{
				List<Hediff_MissingPart> mpca = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				int i = 0;
				if (i < mpca.Count)
				{
					yield return (Hediff)mpca[i];
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IEnumerable<Hediff> visibleDiffs = pawn.health.hediffSet.hediffs.Where(delegate(Hediff d)
				{
					if (d is Hediff_MissingPart)
					{
						return false;
					}
					if (!d.Visible)
					{
						return false;
					}
					if (!showBloodLoss && d.def == HediffDefOf.BloodLoss)
					{
						return false;
					}
					return true;
				});
				using (IEnumerator<Hediff> enumerator = visibleDiffs.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Hediff diff2 = enumerator.Current;
						yield return diff2;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			else
			{
				using (List<Hediff>.Enumerator enumerator2 = pawn.health.hediffSet.hediffs.GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						Hediff diff = enumerator2.Current;
						yield return diff;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_022c:
			/*Error near IL_022d: Unexpected return in MoveNext()*/;
		}

		private static float DrawMedOperationsTab(Rect leftRect, Pawn pawn, Thing thingForMedBills, float curY)
		{
			curY = (float)(curY + 2.0);
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (RecipeDef allRecipe in thingForMedBills.def.AllRecipes)
				{
					if (allRecipe.AvailableNow)
					{
						IEnumerable<ThingDef> enumerable = allRecipe.PotentiallyMissingIngredients(null, thingForMedBills.Map);
						if (!enumerable.Any((ThingDef x) => x.isBodyPartOrImplant) && !enumerable.Any((ThingDef x) => x.IsDrug) && (!enumerable.Any() || !allRecipe.dontShowIfAnyIngredientMissing))
						{
							if (allRecipe.targetsBodyPart)
							{
								foreach (BodyPartRecord item in allRecipe.Worker.GetPartsToApplyOn(pawn, allRecipe))
								{
									list.Add(HealthCardUtility.GenerateSurgeryOption(pawn, thingForMedBills, allRecipe, enumerable, item));
								}
							}
							else
							{
								list.Add(HealthCardUtility.GenerateSurgeryOption(pawn, thingForMedBills, allRecipe, enumerable, null));
							}
						}
					}
				}
				return list;
			};
			Rect rect = new Rect((float)(leftRect.x - 9.0), curY, leftRect.width, (float)(leftRect.height - curY - 20.0));
			((IBillGiver)thingForMedBills).BillStack.DoListing(rect, recipeOptionsMaker, ref HealthCardUtility.billsScrollPosition, ref HealthCardUtility.billsScrollHeight);
			return curY;
		}

		private static FloatMenuOption GenerateSurgeryOption(Pawn pawn, Thing thingForMedBills, RecipeDef recipe, IEnumerable<ThingDef> missingIngredients, BodyPartRecord part = null)
		{
			string text = recipe.Worker.GetLabelWhenUsedOn(pawn, part);
			if (part != null && !recipe.hideBodyPartNames)
			{
				text = text + " (" + part.def.label + ")";
			}
			FloatMenuOption floatMenuOption;
			if (missingIngredients.Any())
			{
				text += " (";
				bool flag = true;
				foreach (ThingDef missingIngredient in missingIngredients)
				{
					if (!flag)
					{
						text += ", ";
					}
					flag = false;
					text += "MissingMedicalBillIngredient".Translate(missingIngredient.label);
				}
				text += ")";
				floatMenuOption = new FloatMenuOption(text, null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else
			{
				Action action = delegate
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
									Messages.Message("MessageNoMedicalBeds".Translate(), pawn2, MessageTypeDefOf.CautionInput);
								}
							}
							else if (!map.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(pawn, x.def)))
							{
								Messages.Message("MessageNoAnimalBeds".Translate(), pawn2, MessageTypeDefOf.CautionInput);
							}
						}
						if (pawn2.Faction != null && !pawn2.Faction.def.hidden && !pawn2.Faction.HostileTo(Faction.OfPlayer) && recipe.Worker.IsViolationOnPawn(pawn2, part, Faction.OfPlayer))
						{
							Messages.Message("MessageMedicalOperationWillAngerFaction".Translate(pawn2.Faction), pawn2, MessageTypeDefOf.CautionInput);
						}
						ThingDef minRequiredMedicine = HealthCardUtility.GetMinRequiredMedicine(recipe);
						if (minRequiredMedicine != null && pawn2.playerSettings != null && !pawn2.playerSettings.medCare.AllowsMedicine(minRequiredMedicine))
						{
							Messages.Message("MessageTooLowMedCare".Translate(minRequiredMedicine.label, pawn2.LabelShort, pawn2.playerSettings.medCare.GetLabel()), pawn2, MessageTypeDefOf.CautionInput);
						}
					}
				};
				floatMenuOption = new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			floatMenuOption.extraPartWidth = 29f;
			floatMenuOption.extraPartOnGUI = ((Rect rect) => Widgets.InfoCardButton((float)(rect.x + 5.0), (float)(rect.y + (rect.height - 24.0) / 2.0), recipe));
			return floatMenuOption;
		}

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

		private static float DrawOverviewTab(Rect leftRect, Pawn pawn, float curY)
		{
			curY = (float)(curY + 4.0);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(0.9f, 0.9f, 0.9f);
			string str = string.Empty;
			if (pawn.gender != 0)
			{
				str = pawn.gender.GetLabel() + " ";
			}
			str = str + pawn.def.label + ", " + "AgeIndicator".Translate(pawn.ageTracker.AgeNumberString);
			Rect rect = new Rect(0f, curY, leftRect.width, 34f);
			Widgets.Label(rect, str.CapitalizeFirst());
			TooltipHandler.TipRegion(rect, () => pawn.ageTracker.AgeTooltipString, 73412);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			GUI.color = Color.white;
			curY = (float)(curY + 34.0);
			GUI.color = Color.white;
			if (pawn.IsColonist && !pawn.Dead)
			{
				bool selfTend = pawn.playerSettings.selfTend;
				Rect rect2 = new Rect(0f, curY, leftRect.width, 24f);
				Widgets.CheckboxLabeled(rect2, "SelfTend".Translate(), ref pawn.playerSettings.selfTend, false);
				if (pawn.playerSettings.selfTend && !selfTend)
				{
					if (pawn.story.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
					{
						pawn.playerSettings.selfTend = false;
						Messages.Message("MessageCannotSelfTendEver".Translate(pawn.LabelShort), MessageTypeDefOf.RejectInput);
					}
					else if (pawn.workSettings.GetPriority(WorkTypeDefOf.Doctor) == 0)
					{
						Messages.Message("MessageSelfTendUnsatisfied".Translate(pawn.LabelShort), MessageTypeDefOf.CautionInput);
					}
				}
				TooltipHandler.TipRegion(rect2, "SelfTendTip".Translate(Faction.OfPlayer.def.pawnsPlural, 0.7f.ToStringPercent()).CapitalizeFirst());
				curY = (float)(curY + 28.0);
			}
			if (pawn.playerSettings != null && !pawn.Dead && Current.ProgramState == ProgramState.Playing)
			{
				Rect rect3 = new Rect(0f, curY, 140f, 28f);
				MedicalCareUtility.MedicalCareSetter(rect3, ref pawn.playerSettings.medCare);
				if (Widgets.ButtonText(new Rect((float)(leftRect.width - 70.0), curY, 70f, 28f), "MedGroupDefaults".Translate(), true, false, true))
				{
					Find.WindowStack.Add(new Dialog_MedicalDefaults());
				}
				curY = (float)(curY + 32.0);
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
				IEnumerable<PawnCapacityDef> source = (!pawn.def.race.Humanlike) ? ((!pawn.def.race.Animal) ? (from x in DefDatabase<PawnCapacityDef>.AllDefs
				where x.showOnMechanoids
				select x) : DefDatabase<PawnCapacityDef>.AllDefs.Where((PawnCapacityDef x) => x.showOnAnimals)) : (from x in DefDatabase<PawnCapacityDef>.AllDefs
				where x.showOnHumanlikes
				select x);
				{
					foreach (PawnCapacityDef item in from act in source
					orderby act.listOrder
					select act)
					{
						if (PawnCapacityUtility.BodyCanEverDoCapacity(pawn.RaceProps.body, item))
						{
							PawnCapacityDef activityLocal = item;
							Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel(pawn, item);
							Func<string> textGetter = () => (!pawn.Dead) ? HealthCardUtility.GetPawnCapacityTip(pawn, activityLocal) : string.Empty;
							curY = HealthCardUtility.DrawLeftRow(leftRect, curY, item.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike).CapitalizeFirst(), efficiencyLabel.First, efficiencyLabel.Second, new TipSignal(textGetter, pawn.thingIDNumber ^ item.index));
						}
					}
					return curY;
				}
			}
			return curY;
		}

		private static float DrawLeftRow(Rect leftRect, float curY, string leftLabel, string rightLabel, Color rightLabelColor, TipSignal tipSignal)
		{
			Rect rect = new Rect(0f, curY, leftRect.width, 20f);
			if (Mouse.IsOver(rect))
			{
				GUI.color = HealthCardUtility.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			GUI.color = Color.white;
			Widgets.Label(new Rect(0f, curY, (float)(leftRect.width * 0.64999997615814209), 30f), leftLabel);
			GUI.color = rightLabelColor;
			Widgets.Label(new Rect((float)(leftRect.width * 0.64999997615814209), curY, (float)(leftRect.width * 0.34999999403953552), 30f), rightLabel);
			TooltipHandler.TipRegion(new Rect(0f, curY, leftRect.width, 20f), tipSignal);
			curY = (float)(curY + 20.0);
			return curY;
		}

		private static void DrawHediffRow(Rect rect, Pawn pawn, IEnumerable<Hediff> diffs, ref float curY)
		{
			float num = (float)(rect.width * 0.375);
			float width = (float)(rect.width - num - 20.0);
			BodyPartRecord part = diffs.First().Part;
			float a = (part != null) ? Text.CalcHeight(part.def.LabelCap, num) : Text.CalcHeight("WholeBody".Translate(), num);
			float num2 = 0f;
			float num3 = curY;
			float num4 = 0f;
			foreach (IGrouping<int, Hediff> item in from x in diffs
			group x by x.UIGroupKey)
			{
				int num5 = item.Count();
				string text = item.First().LabelCap;
				if (num5 != 1)
				{
					text = text + " x" + num5.ToString();
				}
				num4 += Text.CalcHeight(text, width);
			}
			num2 = num4;
			Rect rect2 = new Rect(0f, curY, rect.width, Mathf.Max(a, num2));
			HealthCardUtility.DoRightRowHighlight(rect2);
			if (part != null)
			{
				GUI.color = HealthUtility.GetPartConditionLabel(pawn, part).Second;
				Widgets.Label(new Rect(0f, curY, num, 100f), part.def.LabelCap);
			}
			else
			{
				GUI.color = HealthUtility.DarkRedColor;
				Widgets.Label(new Rect(0f, curY, num, 100f), "WholeBody".Translate());
			}
			GUI.color = Color.white;
			foreach (IGrouping<int, Hediff> item2 in from x in diffs
			group x by x.UIGroupKey)
			{
				int num6 = 0;
				Hediff hediff = null;
				Texture2D texture2D = null;
				TextureAndColor textureAndColor = null;
				float num7 = 0f;
				foreach (Hediff item3 in item2)
				{
					if (num6 == 0)
					{
						hediff = item3;
					}
					textureAndColor = item3.StateIcon;
					if (item3.Bleeding)
					{
						texture2D = HealthCardUtility.BleedingIcon;
					}
					num7 += item3.BleedRate;
					num6++;
				}
				string text2 = hediff.LabelCap;
				if (num6 != 1)
				{
					text2 = text2 + " x" + num6.ToStringCached();
				}
				GUI.color = hediff.LabelColor;
				float num8 = Text.CalcHeight(text2, width);
				Rect rect3 = new Rect(num, curY, width, num8);
				Widgets.Label(rect3, text2);
				GUI.color = Color.white;
				Rect rect4 = new Rect((float)(rect2.xMax - 20.0), curY, 20f, 20f);
				if ((bool)texture2D)
				{
					Rect position = rect4.ContractedBy(GenMath.LerpDouble(0f, 0.6f, 5f, 0f, Mathf.Min(num7, 1f)));
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
				curY += num8;
			}
			GUI.color = Color.white;
			curY = num3 + Mathf.Max(a, num2);
			TooltipHandler.TipRegion(rect2, new TipSignal(() => HealthCardUtility.GetTooltip(diffs, pawn, part), (int)curY + 7857));
		}

		public static string GetPainTip(Pawn pawn)
		{
			return "PainLevel".Translate() + ": " + ((float)(pawn.health.hediffSet.PainTotal * 100.0)).ToString("F0") + "%";
		}

		public static string GetPawnCapacityTip(Pawn pawn, PawnCapacityDef capacity)
		{
			List<PawnCapacityUtility.CapacityImpactor> list = new List<PawnCapacityUtility.CapacityImpactor>();
			float num = PawnCapacityUtility.CalculateCapacityLevel(pawn.health.hediffSet, capacity, list);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(capacity.LabelCap + ": " + ((float)(num * 100.0)).ToString("F0") + "%");
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

		private static string GetTooltip(IEnumerable<Hediff> diffs, Pawn pawn, BodyPartRecord part)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (part != null)
			{
				stringBuilder.Append(part.def.LabelCap + ": ");
				stringBuilder.AppendLine(" " + pawn.health.hediffSet.GetPartHealth(part).ToString() + " / " + part.def.GetMaxHealth(pawn).ToString());
				float num = PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, part, false, null);
				if (num != 1.0)
				{
					stringBuilder.AppendLine("Efficiency".Translate() + ": " + num.ToStringPercent());
				}
			}
			else
			{
				stringBuilder.AppendLine("WholeBody".Translate());
			}
			stringBuilder.AppendLine("------------------");
			foreach (IGrouping<int, Hediff> item in from x in diffs
			group x by x.UIGroupKey)
			{
				foreach (Hediff item2 in item)
				{
					string severityLabel = item2.SeverityLabel;
					bool flag = HealthCardUtility.showHediffsDebugInfo && !item2.DebugString().NullOrEmpty();
					if (!item2.Label.NullOrEmpty() || !severityLabel.NullOrEmpty() || !item2.CapMods.NullOrEmpty() || flag)
					{
						stringBuilder.Append(item2.LabelCap);
						if (!severityLabel.NullOrEmpty())
						{
							stringBuilder.Append(": " + severityLabel);
						}
						stringBuilder.AppendLine();
						string tipStringExtra = item2.TipStringExtra;
						if (!tipStringExtra.NullOrEmpty())
						{
							stringBuilder.AppendLine(tipStringExtra.TrimEndNewlines().Indented());
						}
						if (flag)
						{
							stringBuilder.AppendLine(item2.DebugString().TrimEndNewlines());
						}
					}
				}
			}
			return stringBuilder.ToString().TrimEnd();
		}

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

		private static void DoDebugOptions(Rect rightRect, Pawn pawn)
		{
			Widgets.CheckboxLabeled(new Rect(rightRect.x, (float)(rightRect.y - 25.0), 110f, 30f), "Dev: AllDiffs", ref HealthCardUtility.showAllHediffs, false);
			Widgets.CheckboxLabeled(new Rect((float)(rightRect.x + 115.0), (float)(rightRect.y - 25.0), 120f, 30f), "DiffsDebugInfo", ref HealthCardUtility.showHediffsDebugInfo, false);
			if (Widgets.ButtonText(new Rect((float)(rightRect.x + 240.0), (float)(rightRect.y - 27.0), 115f, 25f), "Debug info", true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("Parts hit chance (this part or any child node)", delegate
				{
					StringBuilder stringBuilder13 = new StringBuilder();
					foreach (BodyPartRecord item2 in from x in pawn.RaceProps.body.AllParts
					orderby x.coverageAbsWithChildren descending
					select x)
					{
						stringBuilder13.AppendLine(item2.def.LabelCap + " " + item2.coverageAbsWithChildren.ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder13.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("Parts hit chance (exactly this part)", delegate
				{
					StringBuilder stringBuilder12 = new StringBuilder();
					float num9 = 0f;
					foreach (BodyPartRecord item3 in from x in pawn.RaceProps.body.AllParts
					orderby x.coverageAbs descending
					select x)
					{
						stringBuilder12.AppendLine(item3.def.LabelCap + " " + item3.coverageAbs.ToStringPercent());
						num9 += item3.coverageAbs;
					}
					stringBuilder12.AppendLine();
					stringBuilder12.AppendLine("Total " + num9.ToStringPercent());
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder12.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("Per-part efficiency", delegate
				{
					StringBuilder stringBuilder11 = new StringBuilder();
					foreach (BodyPartRecord allPart in pawn.RaceProps.body.AllParts)
					{
						stringBuilder11.AppendLine(allPart.def.LabelCap + " " + PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, allPart, false, null).ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder11.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("BodyPartGroup efficiency (of only natural parts)", delegate
				{
					StringBuilder stringBuilder10 = new StringBuilder();
					foreach (BodyPartGroupDef item4 in from x in DefDatabase<BodyPartGroupDef>.AllDefs
					where pawn.RaceProps.body.AllParts.Any((BodyPartRecord y) => y.groups.Contains(x))
					select x)
					{
						stringBuilder10.AppendLine(item4.LabelCap + " " + PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(pawn.health.hediffSet, item4).ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder10.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("IsSolid", delegate
				{
					StringBuilder stringBuilder9 = new StringBuilder();
					foreach (BodyPartRecord notMissingPart in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined))
					{
						stringBuilder9.AppendLine(notMissingPart.def.LabelCap + " " + notMissingPart.def.IsSolid(notMissingPart, pawn.health.hediffSet.hediffs));
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder9.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("IsSkinCovered", delegate
				{
					StringBuilder stringBuilder8 = new StringBuilder();
					foreach (BodyPartRecord notMissingPart2 in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined))
					{
						stringBuilder8.AppendLine(notMissingPart2.def.LabelCap + " " + notMissingPart2.def.IsSkinCovered(notMissingPart2, pawn.health.hediffSet));
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder8.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("does have added parts", delegate
				{
					StringBuilder stringBuilder7 = new StringBuilder();
					foreach (BodyPartRecord notMissingPart3 in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined))
					{
						stringBuilder7.AppendLine(notMissingPart3.def.LabelCap + " " + pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(notMissingPart3));
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder7.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("GetNotMissingParts", delegate
				{
					StringBuilder stringBuilder6 = new StringBuilder();
					foreach (BodyPartRecord notMissingPart4 in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined))
					{
						stringBuilder6.AppendLine(notMissingPart4.def.LabelCap);
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder6.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("GetCoverageOfNotMissingNaturalParts", delegate
				{
					StringBuilder stringBuilder5 = new StringBuilder();
					foreach (BodyPartRecord item5 in from x in pawn.RaceProps.body.AllParts
					orderby pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x) descending
					select x)
					{
						stringBuilder5.AppendLine(item5.def.LabelCap + ": " + pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(item5).ToStringPercent());
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder5.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("parts nutrition", delegate
				{
					StringBuilder stringBuilder4 = new StringBuilder();
					foreach (BodyPartRecord item6 in from x in pawn.RaceProps.body.AllParts
					orderby FoodUtility.GetBodyPartNutrition(pawn, x) descending
					select x)
					{
						stringBuilder4.AppendLine(item6.def.LabelCap + ": " + FoodUtility.GetBodyPartNutrition(pawn, item6));
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder4.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("test old injury pain factor probability", delegate
				{
					StringBuilder stringBuilder3 = new StringBuilder();
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					float num7 = 0f;
					int num8 = 10000;
					for (int i = 0; i < num8; i++)
					{
						float randomPainFactor = OldInjuryUtility.GetRandomPainFactor();
						if (randomPainFactor < 0.0)
						{
							Log.Error("Pain factor < 0");
						}
						if (randomPainFactor == 0.0)
						{
							num4++;
						}
						if (randomPainFactor < 1.0)
						{
							num5++;
						}
						if (randomPainFactor < 5.0)
						{
							num6++;
						}
						if (randomPainFactor > num7)
						{
							num7 = randomPainFactor;
						}
					}
					stringBuilder3.AppendLine("total: " + num8);
					stringBuilder3.AppendLine("0: " + num4 + " (" + ((float)num4 / (float)num8).ToStringPercent() + ")");
					stringBuilder3.AppendLine("< 1: " + num5 + " (" + ((float)num5 / (float)num8).ToStringPercent() + ")");
					stringBuilder3.AppendLine("< 5: " + num6 + " (" + ((float)num6 / (float)num8).ToStringPercent() + ")");
					stringBuilder3.AppendLine("max: " + num7);
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder3.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				HediffGiver_Birthday hLocal;
				list.Add(new FloatMenuOption("HediffGiver_Birthday chance at age", delegate
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (HediffGiverSetDef hediffGiverSet in pawn.RaceProps.hediffGiverSets)
					{
						foreach (HediffGiver_Birthday item7 in hediffGiverSet.hediffGivers.OfType<HediffGiver_Birthday>())
						{
							hLocal = item7;
							FloatMenuOption item = new FloatMenuOption(hediffGiverSet.defName + " - " + item7.hediff.defName, delegate
							{
								StringBuilder stringBuilder2 = new StringBuilder();
								stringBuilder2.AppendLine("% of pawns which will have at least 1 " + hLocal.hediff.label + " at age X:");
								stringBuilder2.AppendLine();
								int num3 = 1;
								while ((float)num3 < pawn.RaceProps.lifeExpectancy + 20.0)
								{
									stringBuilder2.AppendLine(num3 + ": " + hLocal.DebugChanceToHaveAtAge(pawn, num3).ToStringPercent());
									num3++;
								}
								Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder2.ToString(), null, null, null, null, null, false));
							}, MenuOptionPriority.Default, null, null, 0f, null, null);
							list2.Add(item);
						}
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list.Add(new FloatMenuOption("HediffGiver_Birthday count at age", delegate
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Average hediffs count (from HediffGiver_Birthday) at age X:");
					stringBuilder.AppendLine();
					int num = 1;
					while ((float)num < pawn.RaceProps.lifeExpectancy + 20.0)
					{
						float num2 = 0f;
						foreach (HediffGiverSetDef hediffGiverSet2 in pawn.RaceProps.hediffGiverSets)
						{
							foreach (HediffGiver_Birthday item8 in hediffGiverSet2.hediffGivers.OfType<HediffGiver_Birthday>())
							{
								num2 += item8.DebugChanceToHaveAtAge(pawn, num);
							}
						}
						stringBuilder.AppendLine(num + ": " + num2.ToStringDecimalIfSmall());
						num++;
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false));
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		public static Pair<string, Color> GetEfficiencyLabel(Pawn pawn, PawnCapacityDef activity)
		{
			float level = pawn.health.capacities.GetLevel(activity);
			string empty = string.Empty;
			Color white = Color.white;
			if (level <= 0.0)
			{
				empty = "None".Translate();
				white = HealthUtility.DarkRedColor;
			}
			else if (level < 0.40000000596046448)
			{
				empty = "VeryPoor".Translate();
				white = HealthCardUtility.VeryPoorColor;
			}
			else if (level < 0.699999988079071)
			{
				empty = "Poor".Translate();
				white = HealthCardUtility.PoorColor;
			}
			else if (level < 1.0 && !Mathf.Approximately(level, 1f))
			{
				empty = "Weakened".Translate();
				white = HealthCardUtility.WeakenedColor;
			}
			else if (Mathf.Approximately(level, 1f))
			{
				empty = "GoodCondition".Translate();
				white = HealthUtility.GoodConditionColor;
			}
			else
			{
				empty = "Enhanced".Translate();
				white = HealthCardUtility.EnhancedColor;
			}
			return new Pair<string, Color>(empty, white);
		}

		public static Pair<string, Color> GetPainLabel(Pawn pawn)
		{
			float painTotal = pawn.health.hediffSet.PainTotal;
			string empty = string.Empty;
			Color white = Color.white;
			if (Mathf.Approximately(painTotal, 0f))
			{
				empty = "NoPain".Translate();
				white = HealthUtility.GoodConditionColor;
			}
			else if (painTotal < 0.15000000596046448)
			{
				empty = "LittlePain".Translate();
				white = Color.gray;
			}
			else if (painTotal < 0.40000000596046448)
			{
				empty = "MediumPain".Translate();
				white = HealthCardUtility.MediumPainColor;
			}
			else if (painTotal < 0.800000011920929)
			{
				empty = "SeverePain".Translate();
				white = HealthCardUtility.SeverePainColor;
			}
			else
			{
				empty = "ExtremePain".Translate();
				white = HealthUtility.DarkRedColor;
			}
			return new Pair<string, Color>(empty, white);
		}
	}
}
