using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class TransferableOneWayWidget
	{
		private struct Section
		{
			public string title;

			public IEnumerable<TransferableOneWay> transferables;

			public List<TransferableOneWay> cachedTransferables;
		}

		private const float TopAreaHeight = 55f;

		private const float ColumnWidth = 120f;

		private const float FirstTransferableY = 6f;

		private const float RowInterval = 30f;

		public const float CountColumnWidth = 75f;

		public const float AdjustColumnWidth = 240f;

		public const float MassColumnWidth = 100f;

		private const float MarketValueColumnWidth = 100f;

		private const float ExtraSpaceAfterSectionTitle = 5f;

		private const float DaysUntilRotColumnWidth = 75f;

		public const float TopAreaWidth = 515f;

		private List<Section> sections = new List<Section>();

		private string sourceLabel;

		private string destinationLabel;

		private string sourceCountDesc;

		private bool drawMass;

		private IgnorePawnsInventoryMode ignorePawnInventoryMass = IgnorePawnsInventoryMode.DontIgnore;

		private bool includePawnsMassInMassUsage;

		private Func<float> availableMassGetter;

		private float extraHeaderSpace;

		private bool ignoreCorpseGearAndInventoryMass;

		private bool drawMarketValue;

		private int drawDaysUntilRotForTile;

		private bool transferablesCached;

		private Vector2 scrollPosition;

		private TransferableSorterDef sorter1;

		private TransferableSorterDef sorter2;

		private static List<TransferableCountToTransferStoppingPoint> stoppingPoints = new List<TransferableCountToTransferStoppingPoint>();

		protected readonly Vector2 AcceptButtonSize = new Vector2(160f, 40f);

		protected readonly Vector2 OtherBottomButtonSize = new Vector2(160f, 40f);

		public static readonly Color ItemMassColor = new Color(0.7f, 0.7f, 0.7f);

		public float TotalNumbersColumnsWidths
		{
			get
			{
				float num = 315f;
				if (this.drawMass)
				{
					num = (float)(num + 100.0);
				}
				if (this.drawMarketValue)
				{
					num = (float)(num + 100.0);
				}
				if (this.drawDaysUntilRotForTile >= 0)
				{
					num = (float)(num + 75.0);
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
					Section section = this.sections[i];
					if (section.cachedTransferables.Any())
					{
						return true;
					}
				}
				return false;
			}
		}

		public TransferableOneWayWidget(IEnumerable<TransferableOneWay> transferables, string sourceLabel, string destinationLabel, string sourceCountDesc, bool drawMass = false, IgnorePawnsInventoryMode ignorePawnInventoryMass = IgnorePawnsInventoryMode.DontIgnore, bool includePawnsMassInMassUsage = false, Func<float> availableMassGetter = null, float extraHeaderSpace = 0f, bool ignoreCorpseGearAndInventoryMass = false, bool drawMarketValue = false, int drawDaysUntilRotForTile = -1)
		{
			if (transferables != null)
			{
				this.AddSection((string)null, transferables);
			}
			this.sourceLabel = sourceLabel;
			this.destinationLabel = destinationLabel;
			this.sourceCountDesc = sourceCountDesc;
			this.drawMass = drawMass;
			this.ignorePawnInventoryMass = ignorePawnInventoryMass;
			this.includePawnsMassInMassUsage = includePawnsMassInMassUsage;
			this.availableMassGetter = availableMassGetter;
			this.extraHeaderSpace = extraHeaderSpace;
			this.ignoreCorpseGearAndInventoryMass = ignoreCorpseGearAndInventoryMass;
			this.drawMarketValue = drawMarketValue;
			this.drawDaysUntilRotForTile = drawDaysUntilRotForTile;
			this.sorter1 = TransferableSorterDefOf.Category;
			this.sorter2 = TransferableSorterDefOf.MarketValue;
		}

		public void AddSection(string title, IEnumerable<TransferableOneWay> transferables)
		{
			Section item = new Section
			{
				title = title,
				transferables = transferables,
				cachedTransferables = new List<TransferableOneWay>()
			};
			this.sections.Add(item);
			this.transferablesCached = false;
		}

		private void CacheTransferables()
		{
			this.transferablesCached = true;
			for (int i = 0; i < this.sections.Count; i++)
			{
				Section section = this.sections[i];
				List<TransferableOneWay> cachedTransferables = section.cachedTransferables;
				cachedTransferables.Clear();
				List<TransferableOneWay> obj = cachedTransferables;
				Section section2 = this.sections[i];
				obj.AddRange(section2.transferables.OrderBy((Func<TransferableOneWay, Transferable>)((TransferableOneWay tr) => tr), this.sorter1.Comparer).ThenBy((Func<TransferableOneWay, Transferable>)((TransferableOneWay tr) => tr), this.sorter2.Comparer).ThenBy((Func<TransferableOneWay, float>)((TransferableOneWay tr) => TransferableUIUtility.DefaultListOrderPriority(tr))).ToList());
			}
		}

		public void OnGUI(Rect inRect)
		{
			bool flag = default(bool);
			this.OnGUI(inRect, out flag);
		}

		public void OnGUI(Rect inRect, out bool anythingChanged)
		{
			if (!this.transferablesCached)
			{
				this.CacheTransferables();
			}
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, (Action<TransferableSorterDef>)delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheTransferables();
			}, (Action<TransferableSorterDef>)delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheTransferables();
			});
			float num = (float)(inRect.width - 515.0);
			Rect position = new Rect(inRect.x + num, inRect.y, inRect.width - num, 55f);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(0f, 0f, (float)(position.width / 2.0), position.height);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect, this.sourceLabel);
			Rect rect2 = new Rect((float)(position.width / 2.0), 0f, (float)(position.width / 2.0), position.height);
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(rect2, this.destinationLabel);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			Rect mainRect = new Rect(inRect.x, (float)(inRect.y + 55.0 + this.extraHeaderSpace), inRect.width, (float)(inRect.height - 55.0 - this.extraHeaderSpace));
			this.FillMainRect(mainRect, out anythingChanged);
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
					float num2 = num;
					Section section = this.sections[i];
					num = (float)(num2 + (float)section.cachedTransferables.Count * 30.0);
					Section section2 = this.sections[i];
					if (section2.title != null)
					{
						num = (float)(num + 30.0);
					}
				}
				float num3 = 6f;
				float availableMass = (float)(((object)this.availableMassGetter == null) ? 3.4028234663852886E+38 : this.availableMassGetter());
				Rect viewRect = new Rect(0f, 0f, (float)(mainRect.width - 16.0), num);
				Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
				float num4 = (float)(this.scrollPosition.y - 30.0);
				float num5 = this.scrollPosition.y + mainRect.height;
				for (int j = 0; j < this.sections.Count; j++)
				{
					Section section3 = this.sections[j];
					List<TransferableOneWay> cachedTransferables = section3.cachedTransferables;
					if (cachedTransferables.Any())
					{
						Section section4 = this.sections[j];
						if (section4.title != null)
						{
							float width = viewRect.width;
							Section section5 = this.sections[j];
							Widgets.ListSeparator(ref num3, width, section5.title);
							num3 = (float)(num3 + 5.0);
						}
						for (int k = 0; k < cachedTransferables.Count; k++)
						{
							if (num3 > num4 && num3 < num5)
							{
								Rect rect = new Rect(0f, num3, viewRect.width, 30f);
								int countToTransfer = cachedTransferables[k].CountToTransfer;
								this.DoRow(rect, cachedTransferables[k], k, availableMass);
								if (countToTransfer != cachedTransferables[k].CountToTransfer)
								{
									anythingChanged = true;
								}
							}
							num3 = (float)(num3 + 30.0);
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
				GUI.DrawTexture(rect, TradeUI.TradeAlternativeBGTex);
			}
			Text.Font = GameFont.Small;
			GUI.BeginGroup(rect);
			float width = rect.width;
			int maxCount = trad.MaxCount;
			Rect rect2 = new Rect((float)(width - 240.0), 0f, 240f, rect.height);
			TransferableOneWayWidget.stoppingPoints.Clear();
			if ((object)this.availableMassGetter != null && (!(trad.AnyThing is Pawn) || this.includePawnsMassInMassUsage))
			{
				float num = availableMass + this.GetMass(trad.AnyThing) * (float)trad.CountToTransfer;
				int threshold = (!(num <= 0.0)) ? Mathf.FloorToInt(num / this.GetMass(trad.AnyThing)) : 0;
				TransferableOneWayWidget.stoppingPoints.Add(new TransferableCountToTransferStoppingPoint(threshold, "M<", ">M"));
			}
			List<TransferableCountToTransferStoppingPoint> extraStoppingPoints = TransferableOneWayWidget.stoppingPoints;
			TransferableUIUtility.DoCountAdjustInterface(rect2, trad, index, 0, maxCount, false, extraStoppingPoints);
			width = (float)(width - 240.0);
			if (this.drawMarketValue)
			{
				Rect rect3 = new Rect((float)(width - 100.0), 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawMarketValue(rect3, trad);
				width = (float)(width - 100.0);
			}
			if (this.drawMass)
			{
				Rect rect4 = new Rect((float)(width - 100.0), 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawMass(rect4, trad, availableMass);
				width = (float)(width - 100.0);
			}
			if (this.drawDaysUntilRotForTile >= 0)
			{
				Rect rect5 = new Rect((float)(width - 75.0), 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawDaysUntilRot(rect5, trad);
				width = (float)(width - 75.0);
			}
			Rect rect6 = new Rect((float)(width - 75.0), 0f, 75f, rect.height);
			if (Mouse.IsOver(rect6))
			{
				Widgets.DrawHighlight(rect6);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect7 = rect6;
			rect7.xMin += 5f;
			rect7.xMax -= 5f;
			Widgets.Label(rect7, maxCount.ToStringCached());
			TooltipHandler.TipRegion(rect6, this.sourceCountDesc);
			width = (float)(width - 75.0);
			Rect idRect = new Rect(0f, 0f, width, rect.height);
			TransferableUIUtility.DrawTransferableInfo(trad, idRect, Color.white);
			GenUI.ResetLabelAlign();
			GUI.EndGroup();
		}

		private void DrawDaysUntilRot(Rect rect, TransferableOneWay trad)
		{
			if (trad.HasAnyThing && trad.ThingDef.IsNutritionGivingIngestible)
			{
				int num = 2147483647;
				for (int i = 0; i < trad.things.Count; i++)
				{
					CompRottable compRottable = trad.things[i].TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						num = Mathf.Min(num, compRottable.ApproxTicksUntilRotWhenAtTempOfTile(this.drawDaysUntilRotForTile));
					}
				}
				if (num < 36000000)
				{
					if (Mouse.IsOver(rect))
					{
						Widgets.DrawHighlight(rect);
					}
					float num2 = (float)((float)num / 60000.0);
					GUI.color = Color.yellow;
					Widgets.Label(rect, num2.ToString("0.#"));
					GUI.color = Color.white;
					TooltipHandler.TipRegion(rect, "DaysUntilRotTip".Translate());
				}
			}
		}

		private void DrawMarketValue(Rect rect, TransferableOneWay trad)
		{
			if (trad.HasAnyThing)
			{
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				Widgets.Label(rect, trad.AnyThing.GetInnerIfMinified().MarketValue.ToStringMoney());
				TooltipHandler.TipRegion(rect, "MarketValueTip".Translate());
			}
		}

		private void DrawMass(Rect rect, TransferableOneWay trad, float availableMass)
		{
			if (trad.HasAnyThing)
			{
				Thing anyThing = trad.AnyThing;
				Pawn pawn = anyThing as Pawn;
				if (pawn != null && !this.includePawnsMassInMassUsage && !MassUtility.CanEverCarryAnything(pawn))
					return;
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
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
						TooltipHandler.TipRegion(rect, (Func<string>)(() => this.GetPawnMassTip(trad, 0f, mass - gearMass - invMass, gearMass, invMass)), trad.GetHashCode() * 59);
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
					float cap = MassUtility.Capacity(pawn);
					float gearMass2 = MassUtility.GearMass(pawn);
					float invMass2 = (float)((!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass)) ? MassUtility.InventoryMass(pawn) : 0.0);
					float num = cap - gearMass2 - invMass2;
					if (num > 0.0)
					{
						GUI.color = Color.green;
					}
					else if (num < 0.0)
					{
						GUI.color = Color.red;
					}
					else
					{
						GUI.color = Color.gray;
					}
					Widgets.Label(rect, num.ToStringMassOffset());
					TooltipHandler.TipRegion(rect, (Func<string>)(() => this.GetPawnMassTip(trad, cap, 0f, gearMass2, invMass2)), trad.GetHashCode() * 59);
				}
				GUI.color = Color.white;
			}
		}

		private string GetPawnMassTip(TransferableOneWay trad, float capacity, float pawnMass, float gearMass, float invMass)
		{
			if (!trad.HasAnyThing)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (capacity != 0.0)
			{
				stringBuilder.Append("MassCapacity".Translate() + ": " + capacity.ToStringMass());
			}
			else
			{
				stringBuilder.Append("Mass".Translate() + ": " + pawnMass.ToStringMass());
			}
			if (gearMass != 0.0)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("EquipmentAndApparelMass".Translate() + ": " + gearMass.ToStringMass());
			}
			if (invMass != 0.0)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("InventoryMass".Translate() + ": " + invMass.ToStringMass());
			}
			return stringBuilder.ToString();
		}

		private float GetMass(Thing thing)
		{
			if (thing == null)
			{
				return 0f;
			}
			float num = thing.GetInnerIfMinified().GetStatValue(StatDefOf.Mass, true);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass))
				{
					num -= MassUtility.InventoryMass(pawn);
				}
			}
			else if (this.ignoreCorpseGearAndInventoryMass)
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					num -= MassUtility.GearAndInventoryMass(corpse.InnerPawn);
				}
			}
			return num;
		}
	}
}
