using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class TransferableOneWayWidget
	{
		private List<TransferableOneWayWidget.Section> sections = new List<TransferableOneWayWidget.Section>();

		private string sourceLabel;

		private string destinationLabel;

		private string sourceCountDesc;

		private bool drawMass;

		private IgnorePawnsInventoryMode ignorePawnInventoryMass = IgnorePawnsInventoryMode.DontIgnore;

		private bool includePawnsMassInMassUsage;

		private Func<float> availableMassGetter;

		private float extraHeaderSpace;

		private bool ignoreSpawnedCorpseGearAndInventoryMass;

		private int tile;

		private bool drawMarketValue;

		private bool drawEquippedWeapon;

		private bool drawNutritionEatenPerDay;

		private bool drawItemNutrition;

		private bool drawForagedFoodPerDay;

		private bool drawDaysUntilRot;

		private bool playerPawnsReadOnly;

		private bool transferablesCached;

		private Vector2 scrollPosition;

		private TransferableSorterDef sorter1;

		private TransferableSorterDef sorter2;

		private static List<TransferableCountToTransferStoppingPoint> stoppingPoints = new List<TransferableCountToTransferStoppingPoint>();

		private const float TopAreaHeight = 37f;

		protected readonly Vector2 AcceptButtonSize = new Vector2(160f, 40f);

		protected readonly Vector2 OtherBottomButtonSize = new Vector2(160f, 40f);

		private const float ColumnWidth = 120f;

		private const float FirstTransferableY = 6f;

		private const float RowInterval = 30f;

		public const float CountColumnWidth = 75f;

		public const float AdjustColumnWidth = 240f;

		public const float MassColumnWidth = 100f;

		public static readonly Color ItemMassColor = new Color(0.7f, 0.7f, 0.7f);

		private const float MarketValueColumnWidth = 100f;

		private const float ExtraSpaceAfterSectionTitle = 5f;

		private const float DaysUntilRotColumnWidth = 75f;

		private const float NutritionEatenPerDayColumnWidth = 75f;

		private const float ItemNutritionColumnWidth = 75f;

		private const float ForagedFoodPerDayColumnWidth = 75f;

		private const float GrazeabilityInnerColumnWidth = 40f;

		private const float EquippedWeaponIconSize = 30f;

		public const float TopAreaWidth = 515f;

		private static readonly Texture2D CanGrazeIcon = ContentFinder<Texture2D>.Get("UI/Icons/CanGraze", true);

		private static readonly Texture2D PregnantIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);

		private static readonly Texture2D BondIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Bond", true);

		[TweakValue("Interface", 0f, 50f)]
		private static float PregnancyIconWidth = 24f;

		[TweakValue("Interface", 0f, 50f)]
		private static float BondIconWidth = 24f;

		[CompilerGenerated]
		private static Func<TransferableOneWay, Transferable> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<TransferableOneWay, Transferable> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<TransferableOneWay, float> <>f__am$cache2;

		public TransferableOneWayWidget(IEnumerable<TransferableOneWay> transferables, string sourceLabel, string destinationLabel, string sourceCountDesc, bool drawMass = false, IgnorePawnsInventoryMode ignorePawnInventoryMass = IgnorePawnsInventoryMode.DontIgnore, bool includePawnsMassInMassUsage = false, Func<float> availableMassGetter = null, float extraHeaderSpace = 0f, bool ignoreSpawnedCorpseGearAndInventoryMass = false, int tile = -1, bool drawMarketValue = false, bool drawEquippedWeapon = false, bool drawNutritionEatenPerDay = false, bool drawItemNutrition = false, bool drawForagedFoodPerDay = false, bool drawDaysUntilRot = false, bool playerPawnsReadOnly = false)
		{
			if (transferables != null)
			{
				this.AddSection(null, transferables);
			}
			this.sourceLabel = sourceLabel;
			this.destinationLabel = destinationLabel;
			this.sourceCountDesc = sourceCountDesc;
			this.drawMass = drawMass;
			this.ignorePawnInventoryMass = ignorePawnInventoryMass;
			this.includePawnsMassInMassUsage = includePawnsMassInMassUsage;
			this.availableMassGetter = availableMassGetter;
			this.extraHeaderSpace = extraHeaderSpace;
			this.ignoreSpawnedCorpseGearAndInventoryMass = ignoreSpawnedCorpseGearAndInventoryMass;
			this.tile = tile;
			this.drawMarketValue = drawMarketValue;
			this.drawEquippedWeapon = drawEquippedWeapon;
			this.drawNutritionEatenPerDay = drawNutritionEatenPerDay;
			this.drawItemNutrition = drawItemNutrition;
			this.drawForagedFoodPerDay = drawForagedFoodPerDay;
			this.drawDaysUntilRot = drawDaysUntilRot;
			this.playerPawnsReadOnly = playerPawnsReadOnly;
			this.sorter1 = TransferableSorterDefOf.Category;
			this.sorter2 = TransferableSorterDefOf.MarketValue;
		}

		public float TotalNumbersColumnsWidths
		{
			get
			{
				float num = 315f;
				if (this.drawMass)
				{
					num += 100f;
				}
				if (this.drawMarketValue)
				{
					num += 100f;
				}
				if (this.drawDaysUntilRot)
				{
					num += 75f;
				}
				if (this.drawItemNutrition)
				{
					num += 75f;
				}
				if (this.drawNutritionEatenPerDay)
				{
					num += 75f;
				}
				if (this.drawForagedFoodPerDay)
				{
					num += 75f;
				}
				return num;
			}
		}

		private bool AnyTransferable
		{
			get
			{
				if (!this.transferablesCached)
				{
					this.CacheTransferables();
				}
				for (int i = 0; i < this.sections.Count; i++)
				{
					if (this.sections[i].cachedTransferables.Any<TransferableOneWay>())
					{
						return true;
					}
				}
				return false;
			}
		}

		public void AddSection(string title, IEnumerable<TransferableOneWay> transferables)
		{
			TransferableOneWayWidget.Section item = default(TransferableOneWayWidget.Section);
			item.title = title;
			item.transferables = transferables;
			item.cachedTransferables = new List<TransferableOneWay>();
			this.sections.Add(item);
			this.transferablesCached = false;
		}

		private void CacheTransferables()
		{
			this.transferablesCached = true;
			for (int i = 0; i < this.sections.Count; i++)
			{
				List<TransferableOneWay> cachedTransferables = this.sections[i].cachedTransferables;
				cachedTransferables.Clear();
				cachedTransferables.AddRange(this.sections[i].transferables.OrderBy((TransferableOneWay tr) => tr, this.sorter1.Comparer).ThenBy((TransferableOneWay tr) => tr, this.sorter2.Comparer).ThenBy((TransferableOneWay tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ToList<TransferableOneWay>());
			}
		}

		public void OnGUI(Rect inRect)
		{
			bool flag;
			this.OnGUI(inRect, out flag);
		}

		public void OnGUI(Rect inRect, out bool anythingChanged)
		{
			if (!this.transferablesCached)
			{
				this.CacheTransferables();
			}
			Profiler.BeginSample("TransferableOneWayWidget.OnGUI()");
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheTransferables();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheTransferables();
			});
			if (!this.sourceLabel.NullOrEmpty() || !this.destinationLabel.NullOrEmpty())
			{
				float num = inRect.width - 515f;
				Rect position = new Rect(inRect.x + num, inRect.y, inRect.width - num, 37f);
				GUI.BeginGroup(position);
				Text.Font = GameFont.Medium;
				if (!this.sourceLabel.NullOrEmpty())
				{
					Rect rect = new Rect(0f, 0f, position.width / 2f, position.height);
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect, this.sourceLabel);
				}
				if (!this.destinationLabel.NullOrEmpty())
				{
					Rect rect2 = new Rect(position.width / 2f, 0f, position.width / 2f, position.height);
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect2, this.destinationLabel);
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.EndGroup();
			}
			Rect mainRect = new Rect(inRect.x, inRect.y + 37f + this.extraHeaderSpace, inRect.width, inRect.height - 37f - this.extraHeaderSpace);
			this.FillMainRect(mainRect, out anythingChanged);
			Profiler.EndSample();
		}

		private void FillMainRect(Rect mainRect, out bool anythingChanged)
		{
			anythingChanged = false;
			Text.Font = GameFont.Small;
			if (this.AnyTransferable)
			{
				float num = 6f;
				for (int i = 0; i < this.sections.Count; i++)
				{
					num += (float)this.sections[i].cachedTransferables.Count * 30f;
					if (this.sections[i].title != null)
					{
						num += 30f;
					}
				}
				float num2 = 6f;
				float availableMass = (this.availableMassGetter == null) ? float.MaxValue : this.availableMassGetter();
				Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, num);
				Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
				float num3 = this.scrollPosition.y - 30f;
				float num4 = this.scrollPosition.y + mainRect.height;
				for (int j = 0; j < this.sections.Count; j++)
				{
					List<TransferableOneWay> cachedTransferables = this.sections[j].cachedTransferables;
					if (cachedTransferables.Any<TransferableOneWay>())
					{
						if (this.sections[j].title != null)
						{
							Widgets.ListSeparator(ref num2, viewRect.width, this.sections[j].title);
							num2 += 5f;
						}
						for (int k = 0; k < cachedTransferables.Count; k++)
						{
							if (num2 > num3 && num2 < num4)
							{
								Rect rect = new Rect(0f, num2, viewRect.width, 30f);
								int countToTransfer = cachedTransferables[k].CountToTransfer;
								Profiler.BeginSample("DoRow()");
								this.DoRow(rect, cachedTransferables[k], k, availableMass);
								Profiler.EndSample();
								if (countToTransfer != cachedTransferables[k].CountToTransfer)
								{
									anythingChanged = true;
								}
							}
							num2 += 30f;
						}
					}
				}
				Widgets.EndScrollView();
			}
			else
			{
				GUI.color = Color.gray;
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(mainRect, "NoneBrackets".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
		}

		private void DoRow(Rect rect, TransferableOneWay trad, int index, float availableMass)
		{
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Text.Font = GameFont.Small;
			GUI.BeginGroup(rect);
			float num = rect.width;
			int maxCount = trad.MaxCount;
			Rect rect2 = new Rect(num - 240f, 0f, 240f, rect.height);
			TransferableOneWayWidget.stoppingPoints.Clear();
			if (this.availableMassGetter != null && (!(trad.AnyThing is Pawn) || this.includePawnsMassInMassUsage))
			{
				float num2 = availableMass + this.GetMass(trad.AnyThing) * (float)trad.CountToTransfer;
				int threshold = (num2 > 0f) ? Mathf.FloorToInt(num2 / this.GetMass(trad.AnyThing)) : 0;
				TransferableOneWayWidget.stoppingPoints.Add(new TransferableCountToTransferStoppingPoint(threshold, "M<", ">M"));
			}
			Pawn pawn = trad.AnyThing as Pawn;
			bool flag = pawn != null && (pawn.IsColonist || pawn.IsPrisonerOfColony);
			Profiler.BeginSample("DoCountAdjustInterface()");
			Rect rect3 = rect2;
			int min = 0;
			int max = maxCount;
			List<TransferableCountToTransferStoppingPoint> extraStoppingPoints = TransferableOneWayWidget.stoppingPoints;
			TransferableUIUtility.DoCountAdjustInterface(rect3, trad, index, min, max, false, extraStoppingPoints, this.playerPawnsReadOnly && flag);
			Profiler.EndSample();
			num -= 240f;
			if (this.drawMarketValue)
			{
				Rect rect4 = new Rect(num - 100f, 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				Profiler.BeginSample("DrawMarketValue()");
				this.DrawMarketValue(rect4, trad);
				Profiler.EndSample();
				num -= 100f;
			}
			if (this.drawMass)
			{
				Rect rect5 = new Rect(num - 100f, 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				Profiler.BeginSample("DrawMass()");
				this.DrawMass(rect5, trad, availableMass);
				Profiler.EndSample();
				num -= 100f;
			}
			if (this.drawDaysUntilRot)
			{
				Rect rect6 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				Profiler.BeginSample("DrawDaysUntilRot()");
				this.DrawDaysUntilRot(rect6, trad);
				Profiler.EndSample();
				num -= 75f;
			}
			if (this.drawItemNutrition)
			{
				Rect rect7 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				Profiler.BeginSample("DrawItemNutrition()");
				this.DrawItemNutrition(rect7, trad);
				Profiler.EndSample();
				num -= 75f;
			}
			if (this.drawForagedFoodPerDay)
			{
				Rect rect8 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				Profiler.BeginSample("DrawGrazeability()");
				bool flag2 = this.DrawGrazeability(rect8, trad);
				Profiler.EndSample();
				if (!flag2)
				{
					Profiler.BeginSample("DrawForagedFoodPerDay()");
					this.DrawForagedFoodPerDay(rect8, trad);
					Profiler.EndSample();
				}
				num -= 75f;
			}
			if (this.drawNutritionEatenPerDay)
			{
				Rect rect9 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				Profiler.BeginSample("DrawNutritionEatenPerDay()");
				this.DrawNutritionEatenPerDay(rect9, trad);
				Profiler.EndSample();
				num -= 75f;
			}
			if (this.ShouldShowCount(trad))
			{
				Rect rect10 = new Rect(num - 75f, 0f, 75f, rect.height);
				Widgets.DrawHighlightIfMouseover(rect10);
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect11 = rect10;
				rect11.xMin += 5f;
				rect11.xMax -= 5f;
				Widgets.Label(rect11, maxCount.ToStringCached());
				TooltipHandler.TipRegion(rect10, this.sourceCountDesc);
			}
			num -= 75f;
			if (this.drawEquippedWeapon)
			{
				Rect rect12 = new Rect(num - 30f, 0f, 30f, rect.height);
				Rect iconRect = new Rect(num - 30f, (rect.height - 30f) / 2f, 30f, 30f);
				Profiler.BeginSample("DrawEquippedWeapon()");
				this.DrawEquippedWeapon(rect12, iconRect, trad);
				Profiler.EndSample();
				num -= 30f;
			}
			Pawn pawn2 = trad.AnyThing as Pawn;
			if (pawn2 != null && pawn2.def.race.Animal)
			{
				Rect rect13 = new Rect(num - TransferableOneWayWidget.BondIconWidth, (rect.height - TransferableOneWayWidget.BondIconWidth) / 2f, TransferableOneWayWidget.BondIconWidth, TransferableOneWayWidget.BondIconWidth);
				num -= TransferableOneWayWidget.BondIconWidth;
				Rect rect14 = new Rect(num - TransferableOneWayWidget.PregnancyIconWidth, (rect.height - TransferableOneWayWidget.PregnancyIconWidth) / 2f, TransferableOneWayWidget.PregnancyIconWidth, TransferableOneWayWidget.PregnancyIconWidth);
				num -= TransferableOneWayWidget.PregnancyIconWidth;
				string iconTooltipText = TrainableUtility.GetIconTooltipText(pawn2);
				if (!iconTooltipText.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect13, iconTooltipText);
				}
				if (pawn2.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null) != null)
				{
					GUI.DrawTexture(rect13, TransferableOneWayWidget.BondIcon);
				}
				if (pawn2.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true))
				{
					TooltipHandler.TipRegion(rect14, PawnColumnWorker_Pregnant.GetTooltipText(pawn2));
					GUI.DrawTexture(rect14, TransferableOneWayWidget.PregnantIcon);
				}
			}
			Rect idRect = new Rect(0f, 0f, num, rect.height);
			Profiler.BeginSample("DrawTransferableInfo()");
			TransferableUIUtility.DrawTransferableInfo(trad, idRect, Color.white);
			Profiler.EndSample();
			GenUI.ResetLabelAlign();
			GUI.EndGroup();
		}

		private bool ShouldShowCount(TransferableOneWay trad)
		{
			bool result;
			if (!trad.HasAnyThing)
			{
				result = true;
			}
			else
			{
				Pawn pawn = trad.AnyThing as Pawn;
				result = (pawn == null || !pawn.RaceProps.Humanlike || trad.MaxCount != 1);
			}
			return result;
		}

		private void DrawDaysUntilRot(Rect rect, TransferableOneWay trad)
		{
			if (trad.HasAnyThing)
			{
				if (trad.ThingDef.IsNutritionGivingIngestible)
				{
					int num = int.MaxValue;
					for (int i = 0; i < trad.things.Count; i++)
					{
						CompRottable compRottable = trad.things[i].TryGetComp<CompRottable>();
						if (compRottable != null)
						{
							num = Mathf.Min(num, DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(compRottable, this.tile, null));
						}
					}
					if (num < 36000000)
					{
						Widgets.DrawHighlightIfMouseover(rect);
						float num2 = (float)num / 60000f;
						GUI.color = Color.yellow;
						Widgets.Label(rect, num2.ToString("0.#"));
						GUI.color = Color.white;
						TooltipHandler.TipRegion(rect, "DaysUntilRotTip".Translate());
					}
				}
			}
		}

		private void DrawItemNutrition(Rect rect, TransferableOneWay trad)
		{
			if (trad.HasAnyThing)
			{
				if (trad.ThingDef.IsNutritionGivingIngestible)
				{
					Widgets.DrawHighlightIfMouseover(rect);
					GUI.color = Color.green;
					Widgets.Label(rect, trad.ThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("0.##"));
					GUI.color = Color.white;
					TooltipHandler.TipRegion(rect, "ItemNutritionTip".Translate(new object[]
					{
						(1.6f * ThingDefOf.Human.race.baseHungerRate).ToString("0.##")
					}));
				}
			}
		}

		private bool DrawGrazeability(Rect rect, TransferableOneWay trad)
		{
			bool result;
			if (!trad.HasAnyThing)
			{
				result = false;
			}
			else
			{
				Pawn pawn = trad.AnyThing as Pawn;
				if (pawn == null || !VirtualPlantsUtility.CanEverEatVirtualPlants(pawn))
				{
					result = false;
				}
				else
				{
					rect.width = 40f;
					Rect position = new Rect(rect.x + (float)((int)((rect.width - 28f) / 2f)), rect.y + (float)((int)((rect.height - 28f) / 2f)), 28f, 28f);
					Widgets.DrawHighlightIfMouseover(rect);
					GUI.DrawTexture(position, TransferableOneWayWidget.CanGrazeIcon);
					TooltipHandler.TipRegion(rect, delegate()
					{
						string text = "AnimalCanGrazeTip".Translate();
						if (this.tile != -1)
						{
							text = text + "\n\n" + VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(this.tile, Find.TickManager.TicksAbs);
						}
						return text;
					}, trad.GetHashCode() ^ 1948571634);
					result = true;
				}
			}
			return result;
		}

		private void DrawForagedFoodPerDay(Rect rect, TransferableOneWay trad)
		{
			if (trad.HasAnyThing)
			{
				Pawn p = trad.AnyThing as Pawn;
				if (p != null)
				{
					bool flag;
					float foragedNutritionPerDay = ForagedFoodPerDayCalculator.GetBaseForagedNutritionPerDay(p, out flag);
					if (!flag)
					{
						Widgets.DrawHighlightIfMouseover(rect);
						GUI.color = ((foragedNutritionPerDay != 0f) ? Color.green : Color.gray);
						Widgets.Label(rect, "+" + foragedNutritionPerDay.ToString("0.##"));
						GUI.color = Color.white;
						TooltipHandler.TipRegion(rect, () => "NutritionForagedPerDayTip".Translate(new object[]
						{
							StatDefOf.ForagedNutritionPerDay.Worker.GetExplanationFull(StatRequest.For(p), StatDefOf.ForagedNutritionPerDay.toStringNumberSense, foragedNutritionPerDay)
						}), trad.GetHashCode() ^ 1958671422);
					}
				}
			}
		}

		private void DrawNutritionEatenPerDay(Rect rect, TransferableOneWay trad)
		{
			if (trad.HasAnyThing)
			{
				Pawn p = trad.AnyThing as Pawn;
				if (p != null && p.RaceProps.EatsFood && !p.Dead && p.needs.food != null)
				{
					Widgets.DrawHighlightIfMouseover(rect);
					string text = (p.needs.food.FoodFallPerTick * 60000f).ToString("0.##");
					DietCategory resolvedDietCategory = p.RaceProps.ResolvedDietCategory;
					if (resolvedDietCategory != DietCategory.Omnivorous)
					{
						text = text + " (" + resolvedDietCategory.ToStringHumanShort() + ")";
					}
					GUI.color = new Color(1f, 0.5f, 0f);
					Widgets.Label(rect, text);
					GUI.color = Color.white;
					TooltipHandler.TipRegion(rect, delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append("NoDietCategoryLetter".Translate() + " - " + DietCategory.Omnivorous.ToStringHuman());
						DietCategory[] array = (DietCategory[])Enum.GetValues(typeof(DietCategory));
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i] != DietCategory.NeverEats && array[i] != DietCategory.Omnivorous)
							{
								stringBuilder.AppendLine();
								stringBuilder.Append(array[i].ToStringHumanShort() + " - " + array[i].ToStringHuman());
							}
						}
						return "NutritionEatenPerDayTip".Translate(new object[]
						{
							ThingDefOf.MealSimple.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("0.##"),
							stringBuilder.ToString(),
							p.RaceProps.foodType.ToHumanString()
						});
					}, trad.GetHashCode() ^ 385968958);
				}
			}
		}

		private void DrawMarketValue(Rect rect, TransferableOneWay trad)
		{
			if (trad.HasAnyThing)
			{
				Widgets.DrawHighlightIfMouseover(rect);
				Widgets.Label(rect, trad.AnyThing.MarketValue.ToStringMoney("F2"));
				TooltipHandler.TipRegion(rect, "MarketValueTip".Translate());
			}
		}

		private void DrawMass(Rect rect, TransferableOneWay trad, float availableMass)
		{
			if (trad.HasAnyThing)
			{
				Thing anyThing = trad.AnyThing;
				Pawn pawn = anyThing as Pawn;
				if (pawn == null || this.includePawnsMassInMassUsage || MassUtility.CanEverCarryAnything(pawn))
				{
					Widgets.DrawHighlightIfMouseover(rect);
					if (pawn == null || this.includePawnsMassInMassUsage)
					{
						float mass = this.GetMass(anyThing);
						if (pawn != null)
						{
							float gearMass = 0f;
							float invMass = 0f;
							gearMass = MassUtility.GearMass(pawn);
							if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass))
							{
								invMass = MassUtility.InventoryMass(pawn);
							}
							TooltipHandler.TipRegion(rect, () => this.GetPawnMassTip(trad, 0f, mass - gearMass - invMass, gearMass, invMass), trad.GetHashCode() * 59);
						}
						else
						{
							TooltipHandler.TipRegion(rect, "ItemWeightTip".Translate());
						}
						if (mass > availableMass)
						{
							GUI.color = Color.red;
						}
						else
						{
							GUI.color = TransferableOneWayWidget.ItemMassColor;
						}
						Widgets.Label(rect, mass.ToStringMass());
					}
					else
					{
						float cap = MassUtility.Capacity(pawn, null);
						float gearMass = MassUtility.GearMass(pawn);
						float invMass = (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass)) ? MassUtility.InventoryMass(pawn) : 0f;
						float num = cap - gearMass - invMass;
						if (num > 0f)
						{
							GUI.color = Color.green;
						}
						else if (num < 0f)
						{
							GUI.color = Color.red;
						}
						else
						{
							GUI.color = Color.gray;
						}
						Widgets.Label(rect, num.ToStringMassOffset());
						TooltipHandler.TipRegion(rect, () => this.GetPawnMassTip(trad, cap, 0f, gearMass, invMass), trad.GetHashCode() * 59);
					}
					GUI.color = Color.white;
				}
			}
		}

		private void DrawEquippedWeapon(Rect rect, Rect iconRect, TransferableOneWay trad)
		{
			if (trad.HasAnyThing)
			{
				Pawn pawn = trad.AnyThing as Pawn;
				if (pawn != null && pawn.equipment != null && pawn.equipment.Primary != null)
				{
					ThingWithComps primary = pawn.equipment.Primary;
					Widgets.DrawHighlightIfMouseover(rect);
					Widgets.ThingIcon(iconRect, primary, 1f);
					TooltipHandler.TipRegion(rect, primary.LabelCap);
				}
			}
		}

		private string GetPawnMassTip(TransferableOneWay trad, float capacity, float pawnMass, float gearMass, float invMass)
		{
			string result;
			if (!trad.HasAnyThing)
			{
				result = "";
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (capacity != 0f)
				{
					stringBuilder.Append("MassCapacity".Translate() + ": " + capacity.ToStringMass());
				}
				else
				{
					stringBuilder.Append("Mass".Translate() + ": " + pawnMass.ToStringMass());
				}
				if (gearMass != 0f)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("EquipmentAndApparelMass".Translate() + ": " + gearMass.ToStringMass());
				}
				if (invMass != 0f)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("InventoryMass".Translate() + ": " + invMass.ToStringMass());
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		private float GetMass(Thing thing)
		{
			float result;
			if (thing == null)
			{
				result = 0f;
			}
			else
			{
				float num = thing.GetStatValue(StatDefOf.Mass, true);
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					if (InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass))
					{
						num -= MassUtility.InventoryMass(pawn);
					}
				}
				else if (this.ignoreSpawnedCorpseGearAndInventoryMass)
				{
					Corpse corpse = thing as Corpse;
					if (corpse != null && corpse.Spawned)
					{
						num -= MassUtility.GearAndInventoryMass(corpse.InnerPawn);
					}
				}
				result = num;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static TransferableOneWayWidget()
		{
		}

		[CompilerGenerated]
		private static Transferable <CacheTransferables>m__0(TransferableOneWay tr)
		{
			return tr;
		}

		[CompilerGenerated]
		private static Transferable <CacheTransferables>m__1(TransferableOneWay tr)
		{
			return tr;
		}

		[CompilerGenerated]
		private static float <CacheTransferables>m__2(TransferableOneWay tr)
		{
			return TransferableUIUtility.DefaultListOrderPriority(tr);
		}

		[CompilerGenerated]
		private void <OnGUI>m__3(TransferableSorterDef x)
		{
			this.sorter1 = x;
			this.CacheTransferables();
		}

		[CompilerGenerated]
		private void <OnGUI>m__4(TransferableSorterDef x)
		{
			this.sorter2 = x;
			this.CacheTransferables();
		}

		[CompilerGenerated]
		private string <DrawGrazeability>m__5()
		{
			string text = "AnimalCanGrazeTip".Translate();
			if (this.tile != -1)
			{
				text = text + "\n\n" + VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(this.tile, Find.TickManager.TicksAbs);
			}
			return text;
		}

		private struct Section
		{
			public string title;

			public IEnumerable<TransferableOneWay> transferables;

			public List<TransferableOneWay> cachedTransferables;
		}

		[CompilerGenerated]
		private sealed class <DrawForagedFoodPerDay>c__AnonStorey0
		{
			internal Pawn p;

			internal float foragedNutritionPerDay;

			public <DrawForagedFoodPerDay>c__AnonStorey0()
			{
			}

			internal string <>m__0()
			{
				return "NutritionForagedPerDayTip".Translate(new object[]
				{
					StatDefOf.ForagedNutritionPerDay.Worker.GetExplanationFull(StatRequest.For(this.p), StatDefOf.ForagedNutritionPerDay.toStringNumberSense, this.foragedNutritionPerDay)
				});
			}
		}

		[CompilerGenerated]
		private sealed class <DrawNutritionEatenPerDay>c__AnonStorey1
		{
			internal Pawn p;

			public <DrawNutritionEatenPerDay>c__AnonStorey1()
			{
			}

			internal string <>m__0()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("NoDietCategoryLetter".Translate() + " - " + DietCategory.Omnivorous.ToStringHuman());
				DietCategory[] array = (DietCategory[])Enum.GetValues(typeof(DietCategory));
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != DietCategory.NeverEats && array[i] != DietCategory.Omnivorous)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append(array[i].ToStringHumanShort() + " - " + array[i].ToStringHuman());
					}
				}
				return "NutritionEatenPerDayTip".Translate(new object[]
				{
					ThingDefOf.MealSimple.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("0.##"),
					stringBuilder.ToString(),
					this.p.RaceProps.foodType.ToHumanString()
				});
			}
		}

		[CompilerGenerated]
		private sealed class <DrawMass>c__AnonStorey2
		{
			internal TransferableOneWay trad;

			internal TransferableOneWayWidget $this;

			public <DrawMass>c__AnonStorey2()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DrawMass>c__AnonStorey3
		{
			internal float mass;

			public <DrawMass>c__AnonStorey3()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DrawMass>c__AnonStorey4
		{
			internal float gearMass;

			internal float invMass;

			internal TransferableOneWayWidget.<DrawMass>c__AnonStorey2 <>f__ref$2;

			internal TransferableOneWayWidget.<DrawMass>c__AnonStorey3 <>f__ref$3;

			public <DrawMass>c__AnonStorey4()
			{
			}

			internal string <>m__0()
			{
				return this.<>f__ref$2.$this.GetPawnMassTip(this.<>f__ref$2.trad, 0f, this.<>f__ref$3.mass - this.gearMass - this.invMass, this.gearMass, this.invMass);
			}
		}

		[CompilerGenerated]
		private sealed class <DrawMass>c__AnonStorey5
		{
			internal float cap;

			internal float gearMass;

			internal float invMass;

			internal TransferableOneWayWidget.<DrawMass>c__AnonStorey2 <>f__ref$2;

			public <DrawMass>c__AnonStorey5()
			{
			}

			internal string <>m__0()
			{
				return this.<>f__ref$2.$this.GetPawnMassTip(this.<>f__ref$2.trad, this.cap, 0f, this.gearMass, this.invMass);
			}
		}
	}
}
