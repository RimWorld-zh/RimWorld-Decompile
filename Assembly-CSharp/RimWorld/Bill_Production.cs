using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Bill_Production : Bill, IExposable
	{
		public BillRepeatModeDef repeatMode = BillRepeatModeDefOf.RepeatCount;

		public int repeatCount = 1;

		public int targetCount = 10;

		public BillStoreModeDef storeMode = BillStoreModeDefOf.BestStockpile;

		public bool pauseWhenSatisfied = false;

		public int unpauseWhenYouHave = 5;

		public bool paused = false;

		protected override string StatusString
		{
			get
			{
				return (!this.paused) ? "" : (" " + "Paused".Translate());
			}
		}

		protected override float StatusLineMinHeight
		{
			get
			{
				return (float)((!this.CanUnpause()) ? 0.0 : 24.0);
			}
		}

		public string RepeatInfoText
		{
			get
			{
				string result;
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					result = "Forever".Translate();
					goto IL_00a8;
				}
				if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					result = this.repeatCount.ToString() + "x";
					goto IL_00a8;
				}
				if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					result = base.recipe.WorkerCounter.CountProducts(this).ToString() + "/" + this.targetCount.ToString();
					goto IL_00a8;
				}
				throw new InvalidOperationException();
				IL_00a8:
				return result;
			}
		}

		public Bill_Production()
		{
		}

		public Bill_Production(RecipeDef recipe) : base(recipe)
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.repeatCount, "repeatCount", 0, false);
			Scribe_Values.Look<int>(ref this.targetCount, "targetCount", 0, false);
			Scribe_Defs.Look<BillRepeatModeDef>(ref this.repeatMode, "repeatMode");
			Scribe_Defs.Look<BillStoreModeDef>(ref this.storeMode, "storeMode");
			Scribe_Values.Look<bool>(ref this.pauseWhenSatisfied, "pauseWhenSatisfied", false, false);
			Scribe_Values.Look<int>(ref this.unpauseWhenYouHave, "unpauseWhenYouHave", 0, false);
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

		public override BillStoreModeDef GetStoreMode()
		{
			return this.storeMode;
		}

		public override bool ShouldDoNow()
		{
			if (this.repeatMode != BillRepeatModeDefOf.TargetCount)
			{
				this.paused = false;
			}
			bool result;
			if (base.suspended)
			{
				result = false;
				goto IL_00e8;
			}
			if (this.repeatMode == BillRepeatModeDefOf.Forever)
			{
				result = true;
				goto IL_00e8;
			}
			if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				result = (this.repeatCount > 0);
				goto IL_00e8;
			}
			if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
			{
				int num = base.recipe.WorkerCounter.CountProducts(this);
				if (this.pauseWhenSatisfied && num >= this.targetCount)
				{
					this.paused = true;
				}
				if (num <= this.unpauseWhenYouHave || !this.pauseWhenSatisfied)
				{
					this.paused = false;
				}
				result = (!this.paused && num < this.targetCount);
				goto IL_00e8;
			}
			throw new InvalidOperationException();
			IL_00e8:
			return result;
		}

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
					Messages.Message("MessageBillComplete".Translate(this.LabelCap), (Thing)base.billStack.billGiver, MessageTypeDefOf.TaskCompletion);
				}
			}
		}

		protected override void DoConfigInterface(Rect baseRect, Color baseColor)
		{
			Rect rect = new Rect(28f, 32f, 100f, 30f);
			GUI.color = new Color(1f, 1f, 1f, 0.65f);
			Widgets.Label(rect, this.RepeatInfoText);
			GUI.color = baseColor;
			WidgetRow widgetRow = new WidgetRow(baseRect.xMax, (float)(baseRect.y + 29.0), UIDirection.LeftThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Details".Translate() + "...", (string)null, true, false))
			{
				Find.WindowStack.Add(new Dialog_BillConfig(this, ((Thing)base.billStack.billGiver).Position));
			}
			if (widgetRow.ButtonText(this.repeatMode.LabelCap.PadRight(20), (string)null, true, false))
			{
				BillRepeatModeUtility.MakeConfigFloatMenu(this);
			}
			if (widgetRow.ButtonIcon(TexButton.Plus, (string)null))
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					this.repeatMode = BillRepeatModeDefOf.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					int num = base.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
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
					TutorSystem.Notify_Event(base.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount);
				}
			}
			if (widgetRow.ButtonIcon(TexButton.Minus, (string)null))
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					this.repeatMode = BillRepeatModeDefOf.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					int num2 = base.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
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
					TutorSystem.Notify_Event(base.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount);
				}
			}
		}

		private bool CanUnpause()
		{
			return this.repeatMode == BillRepeatModeDefOf.TargetCount && this.paused && this.pauseWhenSatisfied && base.recipe.WorkerCounter.CountProducts(this) < this.targetCount;
		}

		public override void DoStatusLineInterface(Rect rect)
		{
			if (this.paused)
			{
				WidgetRow widgetRow = new WidgetRow(rect.xMax, rect.y, UIDirection.LeftThenUp, 99999f, 4f);
				if (widgetRow.ButtonText("Unpause".Translate(), (string)null, true, false))
				{
					this.paused = false;
				}
			}
		}
	}
}
