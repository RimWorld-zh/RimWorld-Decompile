using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006AF RID: 1711
	public class Bill_Production : Bill, IExposable
	{
		// Token: 0x060024BC RID: 9404 RVA: 0x0013AA28 File Offset: 0x00138E28
		public Bill_Production()
		{
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0013AAB0 File Offset: 0x00138EB0
		public Bill_Production(RecipeDef recipe) : base(recipe)
		{
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x060024BE RID: 9406 RVA: 0x0013AB38 File Offset: 0x00138F38
		protected override string StatusString
		{
			get
			{
				string result;
				if (this.paused)
				{
					result = " " + "Paused".Translate();
				}
				else
				{
					result = "";
				}
				return result;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x060024BF RID: 9407 RVA: 0x0013AB78 File Offset: 0x00138F78
		protected override float StatusLineMinHeight
		{
			get
			{
				return (!this.CanUnpause()) ? 0f : 24f;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x060024C0 RID: 9408 RVA: 0x0013ABA8 File Offset: 0x00138FA8
		public string RepeatInfoText
		{
			get
			{
				string result;
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					result = "Forever".Translate();
				}
				else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					result = this.repeatCount.ToString() + "x";
				}
				else
				{
					if (this.repeatMode != BillRepeatModeDefOf.TargetCount)
					{
						throw new InvalidOperationException();
					}
					result = this.recipe.WorkerCounter.CountProducts(this).ToString() + "/" + this.targetCount.ToString();
				}
				return result;
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x0013AC60 File Offset: 0x00139060
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<BillRepeatModeDef>(ref this.repeatMode, "repeatMode");
			Scribe_Values.Look<int>(ref this.repeatCount, "repeatCount", 0, false);
			Scribe_Defs.Look<BillStoreModeDef>(ref this.storeMode, "storeMode");
			Scribe_References.Look<Zone_Stockpile>(ref this.storeZone, "storeZone", false);
			Scribe_Values.Look<int>(ref this.targetCount, "targetCount", 0, false);
			Scribe_Values.Look<bool>(ref this.pauseWhenSatisfied, "pauseWhenSatisfied", false, false);
			Scribe_Values.Look<int>(ref this.unpauseWhenYouHave, "unpauseWhenYouHave", 0, false);
			Scribe_Values.Look<bool>(ref this.includeEquipped, "includeEquipped", false, false);
			Scribe_Values.Look<bool>(ref this.includeTainted, "includeTainted", false, false);
			Scribe_References.Look<Zone_Stockpile>(ref this.includeFromZone, "includeFromZone", false);
			Scribe_Values.Look<FloatRange>(ref this.hpRange, "hpRange", FloatRange.ZeroToOne, false);
			Scribe_Values.Look<QualityRange>(ref this.qualityRange, "qualityRange", QualityRange.All, false);
			Scribe_Values.Look<bool>(ref this.limitToAllowedStuff, "limitToAllowedStuff", false, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			if (this.repeatMode == null)
			{
				this.repeatMode = BillRepeatModeDefOf.RepeatCount;
			}
			if (this.storeMode == null)
			{
				this.storeMode = BillStoreModeDefOf.BestStockpile;
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x0013ADA0 File Offset: 0x001391A0
		public override BillStoreModeDef GetStoreMode()
		{
			return this.storeMode;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x0013ADBC File Offset: 0x001391BC
		public override Zone_Stockpile GetStoreZone()
		{
			return this.storeZone;
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x0013ADD7 File Offset: 0x001391D7
		public override void SetStoreMode(BillStoreModeDef mode, Zone_Stockpile zone = null)
		{
			this.storeMode = mode;
			this.storeZone = zone;
			if (this.storeMode == BillStoreModeDefOf.SpecificStockpile != (this.storeZone != null))
			{
				Log.ErrorOnce("Inconsistent bill StoreMode data set", 75645354, false);
			}
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x0013AE18 File Offset: 0x00139218
		public override bool ShouldDoNow()
		{
			if (this.repeatMode != BillRepeatModeDefOf.TargetCount)
			{
				this.paused = false;
			}
			bool result;
			if (this.suspended)
			{
				result = false;
			}
			else if (this.repeatMode == BillRepeatModeDefOf.Forever)
			{
				result = true;
			}
			else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				result = (this.repeatCount > 0);
			}
			else
			{
				if (this.repeatMode != BillRepeatModeDefOf.TargetCount)
				{
					throw new InvalidOperationException();
				}
				int num = this.recipe.WorkerCounter.CountProducts(this);
				if (this.pauseWhenSatisfied && num >= this.targetCount)
				{
					this.paused = true;
				}
				if (num <= this.unpauseWhenYouHave || !this.pauseWhenSatisfied)
				{
					this.paused = false;
				}
				result = (!this.paused && num < this.targetCount);
			}
			return result;
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x0013AF10 File Offset: 0x00139310
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				if (this.repeatCount > 0)
				{
					this.repeatCount--;
				}
				if (this.repeatCount == 0)
				{
					Messages.Message("MessageBillComplete".Translate(new object[]
					{
						this.LabelCap
					}), (Thing)this.billStack.billGiver, MessageTypeDefOf.TaskCompletion, true);
				}
			}
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x0013AF90 File Offset: 0x00139390
		protected override void DoConfigInterface(Rect baseRect, Color baseColor)
		{
			Rect rect = new Rect(28f, 32f, 100f, 30f);
			GUI.color = new Color(1f, 1f, 1f, 0.65f);
			Widgets.Label(rect, this.RepeatInfoText);
			GUI.color = baseColor;
			WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Details".Translate() + "...", null, true, false))
			{
				Find.WindowStack.Add(new Dialog_BillConfig(this, ((Thing)this.billStack.billGiver).Position));
			}
			if (widgetRow.ButtonText(this.repeatMode.LabelCap.PadRight(20), null, true, false))
			{
				BillRepeatModeUtility.MakeConfigFloatMenu(this);
			}
			if (widgetRow.ButtonIcon(TexButton.Plus, null, null))
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					this.repeatMode = BillRepeatModeDefOf.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					int num = this.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
					this.targetCount += num;
					this.unpauseWhenYouHave += num;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					this.repeatCount += GenUI.CurrentAdjustmentMultiplier();
				}
				SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
				if (TutorSystem.TutorialMode && this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					TutorSystem.Notify_Event(this.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount);
				}
			}
			if (widgetRow.ButtonIcon(TexButton.Minus, null, null))
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					this.repeatMode = BillRepeatModeDefOf.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					int num2 = this.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
					this.targetCount = Mathf.Max(0, this.targetCount - num2);
					this.unpauseWhenYouHave = Mathf.Max(0, this.unpauseWhenYouHave - num2);
				}
				else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					this.repeatCount = Mathf.Max(0, this.repeatCount - GenUI.CurrentAdjustmentMultiplier());
				}
				SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
				if (TutorSystem.TutorialMode && this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					TutorSystem.Notify_Event(this.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount);
				}
			}
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x0013B288 File Offset: 0x00139688
		private bool CanUnpause()
		{
			return this.repeatMode == BillRepeatModeDefOf.TargetCount && this.paused && this.pauseWhenSatisfied && this.recipe.WorkerCounter.CountProducts(this) < this.targetCount;
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x0013B2E0 File Offset: 0x001396E0
		public override void DoStatusLineInterface(Rect rect)
		{
			if (this.paused)
			{
				WidgetRow widgetRow = new WidgetRow(rect.xMax, rect.y, UIDirection.LeftThenUp, 99999f, 4f);
				if (widgetRow.ButtonText("Unpause".Translate(), null, true, false))
				{
					this.paused = false;
				}
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x0013B33C File Offset: 0x0013973C
		public override void ValidateSettings()
		{
			base.ValidateSettings();
			if (this.storeZone != null)
			{
				if (!this.storeZone.zoneManager.AllZones.Contains(this.storeZone))
				{
					if (this != BillUtility.Clipboard)
					{
						Messages.Message("MessageBillValidationStoreZoneDeleted".Translate(new object[]
						{
							this.LabelCap,
							this.billStack.billGiver.LabelShort.CapitalizeFirst(),
							this.storeZone.label
						}), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
					}
					this.SetStoreMode(BillStoreModeDefOf.DropOnFloor, null);
				}
				else if (base.Map != null && !base.Map.zoneManager.AllZones.Contains(this.storeZone))
				{
					if (this != BillUtility.Clipboard)
					{
						Messages.Message("MessageBillValidationStoreZoneUnavailable".Translate(new object[]
						{
							this.LabelCap,
							this.billStack.billGiver.LabelShort.CapitalizeFirst(),
							this.storeZone.label
						}), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
					}
					this.SetStoreMode(BillStoreModeDefOf.DropOnFloor, null);
				}
			}
			else if (this.storeMode == BillStoreModeDefOf.SpecificStockpile)
			{
				this.SetStoreMode(BillStoreModeDefOf.DropOnFloor, null);
				Log.ErrorOnce("Found SpecificStockpile bill store mode without associated stockpile, recovering", 46304128, false);
			}
			if (this.includeFromZone != null)
			{
				if (!this.includeFromZone.zoneManager.AllZones.Contains(this.includeFromZone))
				{
					if (this != BillUtility.Clipboard)
					{
						Messages.Message("MessageBillValidationIncludeZoneDeleted".Translate(new object[]
						{
							this.LabelCap,
							this.billStack.billGiver.LabelShort.CapitalizeFirst(),
							this.includeFromZone.label
						}), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
					}
					this.includeFromZone = null;
				}
				else if (base.Map != null && !base.Map.zoneManager.AllZones.Contains(this.includeFromZone))
				{
					if (this != BillUtility.Clipboard)
					{
						Messages.Message("MessageBillValidationIncludeZoneUnavailable".Translate(new object[]
						{
							this.LabelCap,
							this.billStack.billGiver.LabelShort.CapitalizeFirst(),
							this.includeFromZone.label
						}), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
					}
					this.includeFromZone = null;
				}
			}
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x0013B614 File Offset: 0x00139A14
		public override Bill Clone()
		{
			Bill_Production bill_Production = (Bill_Production)base.Clone();
			bill_Production.repeatMode = this.repeatMode;
			bill_Production.repeatCount = this.repeatCount;
			bill_Production.storeMode = this.storeMode;
			bill_Production.storeZone = this.storeZone;
			bill_Production.targetCount = this.targetCount;
			bill_Production.pauseWhenSatisfied = this.pauseWhenSatisfied;
			bill_Production.unpauseWhenYouHave = this.unpauseWhenYouHave;
			bill_Production.includeEquipped = this.includeEquipped;
			bill_Production.includeTainted = this.includeTainted;
			bill_Production.includeFromZone = this.includeFromZone;
			bill_Production.hpRange = this.hpRange;
			bill_Production.qualityRange = this.qualityRange;
			bill_Production.limitToAllowedStuff = this.limitToAllowedStuff;
			bill_Production.paused = this.paused;
			return bill_Production;
		}

		// Token: 0x04001441 RID: 5185
		public BillRepeatModeDef repeatMode = BillRepeatModeDefOf.RepeatCount;

		// Token: 0x04001442 RID: 5186
		public int repeatCount = 1;

		// Token: 0x04001443 RID: 5187
		private BillStoreModeDef storeMode = BillStoreModeDefOf.BestStockpile;

		// Token: 0x04001444 RID: 5188
		private Zone_Stockpile storeZone = null;

		// Token: 0x04001445 RID: 5189
		public int targetCount = 10;

		// Token: 0x04001446 RID: 5190
		public bool pauseWhenSatisfied = false;

		// Token: 0x04001447 RID: 5191
		public int unpauseWhenYouHave = 5;

		// Token: 0x04001448 RID: 5192
		public bool includeEquipped = false;

		// Token: 0x04001449 RID: 5193
		public bool includeTainted = false;

		// Token: 0x0400144A RID: 5194
		public Zone_Stockpile includeFromZone = null;

		// Token: 0x0400144B RID: 5195
		public FloatRange hpRange = FloatRange.ZeroToOne;

		// Token: 0x0400144C RID: 5196
		public QualityRange qualityRange = QualityRange.All;

		// Token: 0x0400144D RID: 5197
		public bool limitToAllowedStuff = false;

		// Token: 0x0400144E RID: 5198
		public bool paused = false;
	}
}
