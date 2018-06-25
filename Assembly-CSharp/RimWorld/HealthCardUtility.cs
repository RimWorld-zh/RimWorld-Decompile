using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

		[CompilerGenerated]
		private static Action <>f__am$cache0;

		[CompilerGenerated]
		private static Action <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<PawnCapacityDef, bool> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<PawnCapacityDef, bool> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<PawnCapacityDef, bool> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<PawnCapacityDef, int> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<Hediff, int> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<Hediff, int> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<Hediff, int> <>f__am$cache9;

		[CompilerGenerated]
		private static Action <>f__am$cacheA;

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

		public static string GetPainTip(Pawn pawn)
		{
			return "PainLevel".Translate() + ": " + (pawn.health.hediffSet.PainTotal * 100f).ToString("F0") + "%";
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static HealthCardUtility()
		{
		}

		[CompilerGenerated]
		private static void <DrawHealthSummary>m__0()
		{
			HealthCardUtility.onOperationTab = false;
		}

		[CompilerGenerated]
		private static void <DrawHealthSummary>m__1()
		{
			HealthCardUtility.onOperationTab = true;
		}

		[CompilerGenerated]
		private static float <GetMinRequiredMedicine>m__2(ThingDef x)
		{
			return x.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
		}

		[CompilerGenerated]
		private static bool <DrawOverviewTab>m__3(PawnCapacityDef x)
		{
			return x.showOnHumanlikes;
		}

		[CompilerGenerated]
		private static bool <DrawOverviewTab>m__4(PawnCapacityDef x)
		{
			return x.showOnAnimals;
		}

		[CompilerGenerated]
		private static bool <DrawOverviewTab>m__5(PawnCapacityDef x)
		{
			return x.showOnMechanoids;
		}

		[CompilerGenerated]
		private static int <DrawOverviewTab>m__6(PawnCapacityDef act)
		{
			return act.listOrder;
		}

		[CompilerGenerated]
		private static int <DrawHediffRow>m__7(Hediff x)
		{
			return x.UIGroupKey;
		}

		[CompilerGenerated]
		private static int <DrawHediffRow>m__8(Hediff x)
		{
			return x.UIGroupKey;
		}

		[CompilerGenerated]
		private static int <GetTooltip>m__9(Hediff x)
		{
			return x.UIGroupKey;
		}

		[CompilerGenerated]
		private static void <DoDebugOptions>m__A()
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
		}

		[CompilerGenerated]
		private sealed class <VisibleHediffGroupsInOrder>c__Iterator0 : IEnumerable, IEnumerable<IGrouping<BodyPartRecord, Hediff>>, IEnumerator, IDisposable, IEnumerator<IGrouping<BodyPartRecord, Hediff>>
		{
			internal Pawn pawn;

			internal bool showBloodLoss;

			internal IEnumerator<IGrouping<BodyPartRecord, Hediff>> $locvar0;

			internal IGrouping<BodyPartRecord, Hediff> <group>__1;

			internal IGrouping<BodyPartRecord, Hediff> $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<Hediff, BodyPartRecord> <>f__am$cache0;

			private static Func<IGrouping<BodyPartRecord, Hediff>, float> <>f__am$cache1;

			[DebuggerHidden]
			public <VisibleHediffGroupsInOrder>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = (from x in HealthCardUtility.VisibleHediffs(pawn, showBloodLoss)
					group x by x.Part into x
					orderby HealthCardUtility.GetListPriority(x.First<Hediff>().Part) descending
					select x).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						group = enumerator.Current;
						this.$current = group;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			IGrouping<BodyPartRecord, Hediff> IEnumerator<IGrouping<BodyPartRecord, Hediff>>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<System.Linq.IGrouping<Verse.BodyPartRecord,Verse.Hediff>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IGrouping<BodyPartRecord, Hediff>> IEnumerable<IGrouping<BodyPartRecord, Hediff>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HealthCardUtility.<VisibleHediffGroupsInOrder>c__Iterator0 <VisibleHediffGroupsInOrder>c__Iterator = new HealthCardUtility.<VisibleHediffGroupsInOrder>c__Iterator0();
				<VisibleHediffGroupsInOrder>c__Iterator.pawn = pawn;
				<VisibleHediffGroupsInOrder>c__Iterator.showBloodLoss = showBloodLoss;
				return <VisibleHediffGroupsInOrder>c__Iterator;
			}

			private static BodyPartRecord <>m__0(Hediff x)
			{
				return x.Part;
			}

			private static float <>m__1(IGrouping<BodyPartRecord, Hediff> x)
			{
				return HealthCardUtility.GetListPriority(x.First<Hediff>().Part);
			}
		}

		[CompilerGenerated]
		private sealed class <VisibleHediffs>c__Iterator1 : IEnumerable, IEnumerable<Hediff>, IEnumerator, IDisposable, IEnumerator<Hediff>
		{
			internal Pawn pawn;

			internal List<Hediff_MissingPart> <mpca>__1;

			internal int <i>__2;

			internal bool showBloodLoss;

			internal IEnumerable<Hediff> <visibleDiffs>__1;

			internal IEnumerator<Hediff> $locvar0;

			internal Hediff <diff>__3;

			internal List<Hediff>.Enumerator $locvar1;

			internal Hediff <diff>__4;

			internal Hediff $current;

			internal bool $disposing;

			internal int $PC;

			private HealthCardUtility.<VisibleHediffs>c__Iterator1.<VisibleHediffs>c__AnonStorey2 $locvar2;

			[DebuggerHidden]
			public <VisibleHediffs>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (HealthCardUtility.showAllHediffs)
					{
						enumerator2 = pawn.health.hediffSet.hediffs.GetEnumerator();
						num = 4294967293u;
						goto Block_6;
					}
					mpca = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
					i = 0;
					break;
				case 1u:
					i++;
					break;
				case 2u:
					Block_5:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							diff = enumerator.Current;
							this.$current = diff;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					goto IL_230;
				case 3u:
					goto IL_1BE;
				default:
					return false;
				}
				if (i >= mpca.Count)
				{
					visibleDiffs = from d in pawn.health.hediffSet.hediffs
					where !(d is Hediff_MissingPart) && d.Visible && (<VisibleHediffs>c__AnonStorey.showBloodLoss || d.def != HediffDefOf.BloodLoss)
					select d;
					enumerator = visibleDiffs.GetEnumerator();
					num = 4294967293u;
					goto Block_5;
				}
				this.$current = mpca[i];
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
				Block_6:
				try
				{
					IL_1BE:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						diff2 = enumerator2.Current;
						this.$current = diff2;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				IL_230:
				this.$PC = -1;
				return false;
			}

			Hediff IEnumerator<Hediff>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 3u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Hediff>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Hediff> IEnumerable<Hediff>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HealthCardUtility.<VisibleHediffs>c__Iterator1 <VisibleHediffs>c__Iterator = new HealthCardUtility.<VisibleHediffs>c__Iterator1();
				<VisibleHediffs>c__Iterator.pawn = pawn;
				<VisibleHediffs>c__Iterator.showBloodLoss = showBloodLoss;
				return <VisibleHediffs>c__Iterator;
			}

			private sealed class <VisibleHediffs>c__AnonStorey2
			{
				internal bool showBloodLoss;

				internal HealthCardUtility.<VisibleHediffs>c__Iterator1 <>f__ref$1;

				public <VisibleHediffs>c__AnonStorey2()
				{
				}

				internal bool <>m__0(Hediff d)
				{
					return !(d is Hediff_MissingPart) && d.Visible && (this.showBloodLoss || d.def != HediffDefOf.BloodLoss);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DrawMedOperationsTab>c__AnonStorey3
		{
			internal Thing thingForMedBills;

			internal Pawn pawn;

			private static Func<ThingDef, bool> <>f__am$cache0;

			private static Func<ThingDef, bool> <>f__am$cache1;

			public <DrawMedOperationsTab>c__AnonStorey3()
			{
			}

			internal List<FloatMenuOption> <>m__0()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (RecipeDef recipeDef in this.thingForMedBills.def.AllRecipes)
				{
					if (recipeDef.AvailableNow)
					{
						IEnumerable<ThingDef> enumerable = recipeDef.PotentiallyMissingIngredients(null, this.thingForMedBills.Map);
						if (!enumerable.Any((ThingDef x) => x.isTechHediff))
						{
							if (!enumerable.Any((ThingDef x) => x.IsDrug))
							{
								if (!enumerable.Any<ThingDef>() || !recipeDef.dontShowIfAnyIngredientMissing)
								{
									if (recipeDef.targetsBodyPart)
									{
										foreach (BodyPartRecord part in recipeDef.Worker.GetPartsToApplyOn(this.pawn, recipeDef))
										{
											list.Add(HealthCardUtility.GenerateSurgeryOption(this.pawn, this.thingForMedBills, recipeDef, enumerable, part));
										}
									}
									else
									{
										list.Add(HealthCardUtility.GenerateSurgeryOption(this.pawn, this.thingForMedBills, recipeDef, enumerable, null));
									}
								}
							}
						}
					}
				}
				return list;
			}

			private static bool <>m__1(ThingDef x)
			{
				return x.isTechHediff;
			}

			private static bool <>m__2(ThingDef x)
			{
				return x.IsDrug;
			}
		}

		[CompilerGenerated]
		private sealed class <GenerateSurgeryOption>c__AnonStorey4
		{
			internal Thing thingForMedBills;

			internal RecipeDef recipe;

			internal BodyPartRecord part;

			internal Pawn pawn;

			public <GenerateSurgeryOption>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				Pawn pawn = this.thingForMedBills as Pawn;
				if (pawn != null)
				{
					Bill_Medical bill_Medical = new Bill_Medical(this.recipe);
					pawn.BillStack.AddBill(bill_Medical);
					bill_Medical.Part = this.part;
					if (this.recipe.conceptLearned != null)
					{
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(this.recipe.conceptLearned, KnowledgeAmount.Total);
					}
					Map map = this.thingForMedBills.Map;
					if (!map.mapPawns.FreeColonists.Any((Pawn col) => this.recipe.PawnSatisfiesSkillRequirements(col)))
					{
						Bill.CreateNoPawnsWithSkillDialog(this.recipe);
					}
					if (!pawn.InBed() && pawn.RaceProps.IsFlesh)
					{
						if (pawn.RaceProps.Humanlike)
						{
							if (!map.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(this.pawn, x.def) && ((Building_Bed)x).Medical))
							{
								Messages.Message("MessageNoMedicalBeds".Translate(), pawn, MessageTypeDefOf.CautionInput, false);
							}
						}
						else if (!map.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(this.pawn, x.def)))
						{
							Messages.Message("MessageNoAnimalBeds".Translate(), pawn, MessageTypeDefOf.CautionInput, false);
						}
					}
					if (pawn.Faction != null && !pawn.Faction.def.hidden && !pawn.Faction.HostileTo(Faction.OfPlayer) && this.recipe.Worker.IsViolationOnPawn(pawn, this.part, Faction.OfPlayer))
					{
						Messages.Message("MessageMedicalOperationWillAngerFaction".Translate(new object[]
						{
							pawn.Faction
						}), pawn, MessageTypeDefOf.CautionInput, false);
					}
					ThingDef minRequiredMedicine = HealthCardUtility.GetMinRequiredMedicine(this.recipe);
					if (minRequiredMedicine != null && pawn.playerSettings != null && !pawn.playerSettings.medCare.AllowsMedicine(minRequiredMedicine))
					{
						Messages.Message("MessageTooLowMedCare".Translate(new object[]
						{
							minRequiredMedicine.label,
							pawn.LabelShort,
							pawn.playerSettings.medCare.GetLabel()
						}), pawn, MessageTypeDefOf.CautionInput, false);
					}
				}
			}

			internal bool <>m__1(Rect rect)
			{
				return Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, this.recipe);
			}

			internal bool <>m__2(Pawn col)
			{
				return this.recipe.PawnSatisfiesSkillRequirements(col);
			}

			internal bool <>m__3(Building x)
			{
				return x is Building_Bed && RestUtility.CanUseBedEver(this.pawn, x.def) && ((Building_Bed)x).Medical;
			}

			internal bool <>m__4(Building x)
			{
				return x is Building_Bed && RestUtility.CanUseBedEver(this.pawn, x.def);
			}
		}

		[CompilerGenerated]
		private sealed class <DrawOverviewTab>c__AnonStorey5
		{
			internal Pawn pawn;

			public <DrawOverviewTab>c__AnonStorey5()
			{
			}

			internal string <>m__0()
			{
				return this.pawn.ageTracker.AgeTooltipString;
			}
		}

		[CompilerGenerated]
		private sealed class <DrawOverviewTab>c__AnonStorey6
		{
			internal PawnCapacityDef activityLocal;

			internal HealthCardUtility.<DrawOverviewTab>c__AnonStorey5 <>f__ref$5;

			public <DrawOverviewTab>c__AnonStorey6()
			{
			}

			internal string <>m__0()
			{
				return (!this.<>f__ref$5.pawn.Dead) ? HealthCardUtility.GetPawnCapacityTip(this.<>f__ref$5.pawn, this.activityLocal) : "";
			}
		}

		[CompilerGenerated]
		private sealed class <DrawHediffRow>c__AnonStorey7
		{
			internal IEnumerable<Hediff> diffs;

			internal Pawn pawn;

			internal BodyPartRecord part;

			public <DrawHediffRow>c__AnonStorey7()
			{
			}

			internal string <>m__0()
			{
				return HealthCardUtility.GetTooltip(this.diffs, this.pawn, this.part);
			}
		}

		[CompilerGenerated]
		private sealed class <EntryClicked>c__AnonStorey8
		{
			internal Pawn pawn;

			internal LogEntry combatLogEntry;

			public <EntryClicked>c__AnonStorey8()
			{
			}

			internal bool <>m__0(Battle b)
			{
				return b.Concerns(this.pawn) && b.Entries.Any((LogEntry e) => e == this.combatLogEntry);
			}

			internal bool <>m__1(LogEntry e)
			{
				return e == this.combatLogEntry;
			}
		}

		[CompilerGenerated]
		private sealed class <DoDebugOptions>c__AnonStorey9
		{
			internal Pawn pawn;

			private static Func<BodyPartRecord, float> <>f__am$cache0;

			private static Func<BodyPartRecord, float> <>f__am$cache1;

			public <DoDebugOptions>c__AnonStorey9()
			{
			}

			internal void <>m__0()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (BodyPartRecord bodyPartRecord in from x in this.pawn.RaceProps.body.AllParts
				orderby x.coverageAbsWithChildren descending
				select x)
				{
					stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.coverageAbsWithChildren.ToStringPercent());
				}
				Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
			}

			internal void <>m__1()
			{
				StringBuilder stringBuilder = new StringBuilder();
				float num = 0f;
				foreach (BodyPartRecord bodyPartRecord in from x in this.pawn.RaceProps.body.AllParts
				orderby x.coverageAbs descending
				select x)
				{
					stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.coverageAbs.ToStringPercent());
					num += bodyPartRecord.coverageAbs;
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Total " + num.ToStringPercent());
				Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
			}

			internal void <>m__2()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (BodyPartRecord bodyPartRecord in this.pawn.RaceProps.body.AllParts)
				{
					stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + PawnCapacityUtility.CalculatePartEfficiency(this.pawn.health.hediffSet, bodyPartRecord, false, null).ToStringPercent());
				}
				Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
			}

			internal void <>m__3()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (BodyPartGroupDef bodyPartGroupDef in from x in DefDatabase<BodyPartGroupDef>.AllDefs
				where this.pawn.RaceProps.body.AllParts.Any((BodyPartRecord y) => y.groups.Contains(x))
				select x)
				{
					stringBuilder.AppendLine(bodyPartGroupDef.LabelCap + " " + PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(this.pawn.health.hediffSet, bodyPartGroupDef).ToStringPercent());
				}
				Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
			}

			internal void <>m__4()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (BodyPartRecord bodyPartRecord in this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null))
				{
					stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.def.IsSolid(bodyPartRecord, this.pawn.health.hediffSet.hediffs));
				}
				Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
			}

			internal void <>m__5()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (BodyPartRecord bodyPartRecord in this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null))
				{
					stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + bodyPartRecord.def.IsSkinCovered(bodyPartRecord, this.pawn.health.hediffSet));
				}
				Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
			}

			internal void <>m__6()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (BodyPartRecord bodyPartRecord in this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null))
				{
					stringBuilder.AppendLine(bodyPartRecord.LabelCap + " " + this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(bodyPartRecord));
				}
				Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
			}

			internal void <>m__7()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (BodyPartRecord bodyPartRecord in this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null))
				{
					stringBuilder.AppendLine(bodyPartRecord.LabelCap);
				}
				Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
			}

			internal void <>m__8()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (BodyPartRecord bodyPartRecord in from x in this.pawn.RaceProps.body.AllParts
				orderby this.pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x) descending
				select x)
				{
					stringBuilder.AppendLine(bodyPartRecord.LabelCap + ": " + this.pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(bodyPartRecord).ToStringPercent());
				}
				Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
			}

			internal void <>m__9()
			{
				StringBuilder stringBuilder = new StringBuilder();
				float totalCorpseNutrition = StatDefOf.Nutrition.Worker.GetValueAbstract(this.pawn.RaceProps.corpseDef, null);
				foreach (BodyPartRecord bodyPartRecord in from x in this.pawn.RaceProps.body.AllParts
				orderby FoodUtility.GetBodyPartNutrition(totalCorpseNutrition, this.pawn, x) descending
				select x)
				{
					stringBuilder.AppendLine(bodyPartRecord.LabelCap + ": " + FoodUtility.GetBodyPartNutrition(totalCorpseNutrition, this.pawn, bodyPartRecord));
				}
				Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
			}

			internal void <>m__A()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (HediffGiverSetDef hediffGiverSetDef in this.pawn.RaceProps.hediffGiverSets)
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
							while ((float)num < this.pawn.RaceProps.lifeExpectancy + 20f)
							{
								stringBuilder.AppendLine(num + ": " + hLocal.DebugChanceToHaveAtAge(this.pawn, num).ToStringPercent());
								num++;
							}
							Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
						}, MenuOptionPriority.Default, null, null, 0f, null, null);
						list.Add(item);
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}

			internal void <>m__B()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Average hediffs count (from HediffGiver_Birthday) at age X:");
				stringBuilder.AppendLine();
				int num = 1;
				while ((float)num < this.pawn.RaceProps.lifeExpectancy + 20f)
				{
					float num2 = 0f;
					foreach (HediffGiverSetDef hediffGiverSetDef in this.pawn.RaceProps.hediffGiverSets)
					{
						foreach (HediffGiver_Birthday hediffGiver_Birthday in hediffGiverSetDef.hediffGivers.OfType<HediffGiver_Birthday>())
						{
							num2 += hediffGiver_Birthday.DebugChanceToHaveAtAge(this.pawn, num);
						}
					}
					stringBuilder.AppendLine(num + ": " + num2.ToStringDecimalIfSmall());
					num++;
				}
				Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
			}

			private static float <>m__C(BodyPartRecord x)
			{
				return x.coverageAbsWithChildren;
			}

			private static float <>m__D(BodyPartRecord x)
			{
				return x.coverageAbs;
			}

			internal bool <>m__E(BodyPartGroupDef x)
			{
				return this.pawn.RaceProps.body.AllParts.Any((BodyPartRecord y) => y.groups.Contains(x));
			}

			internal float <>m__F(BodyPartRecord x)
			{
				return this.pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x);
			}

			private sealed class <DoDebugOptions>c__AnonStoreyB
			{
				internal float totalCorpseNutrition;

				internal HealthCardUtility.<DoDebugOptions>c__AnonStorey9 <>f__ref$9;

				public <DoDebugOptions>c__AnonStoreyB()
				{
				}

				internal float <>m__0(BodyPartRecord x)
				{
					return FoodUtility.GetBodyPartNutrition(this.totalCorpseNutrition, this.<>f__ref$9.pawn, x);
				}
			}

			private sealed class <DoDebugOptions>c__AnonStoreyC
			{
				internal HediffGiver_Birthday hLocal;

				internal HealthCardUtility.<DoDebugOptions>c__AnonStorey9 <>f__ref$9;

				public <DoDebugOptions>c__AnonStoreyC()
				{
				}

				internal void <>m__0()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("% of pawns which will have at least 1 " + this.hLocal.hediff.label + " at age X:");
					stringBuilder.AppendLine();
					int num = 1;
					while ((float)num < this.<>f__ref$9.pawn.RaceProps.lifeExpectancy + 20f)
					{
						stringBuilder.AppendLine(num + ": " + this.hLocal.DebugChanceToHaveAtAge(this.<>f__ref$9.pawn, num).ToStringPercent());
						num++;
					}
					Find.WindowStack.Add(new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null));
				}
			}

			private sealed class <DoDebugOptions>c__AnonStoreyA
			{
				internal BodyPartGroupDef x;

				internal HealthCardUtility.<DoDebugOptions>c__AnonStorey9 <>f__ref$9;

				public <DoDebugOptions>c__AnonStoreyA()
				{
				}

				internal bool <>m__0(BodyPartRecord y)
				{
					return y.groups.Contains(this.x);
				}
			}
		}
	}
}
