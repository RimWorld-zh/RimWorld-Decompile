using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Dialog_ManageDrugPolicies : Window
	{
		private Vector2 scrollPosition;

		private DrugPolicy selPolicy;

		private const float TopAreaHeight = 40f;

		private const float TopButtonHeight = 35f;

		private const float TopButtonWidth = 150f;

		private const float DrugEntryRowHeight = 35f;

		private const float BottomButtonsAreaHeight = 50f;

		private const float AddEntryButtonHeight = 35f;

		private const float AddEntryButtonWidth = 150f;

		private const float CellsPadding = 4f;

		private static readonly Texture2D IconForAddiction = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/ForAddiction", true);

		private static readonly Texture2D IconForJoy = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/ForJoy", true);

		private static readonly Texture2D IconScheduled = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/Scheduled", true);

		private static readonly Regex ValidNameRegex = Outfit.ValidNameRegex;

		private const float UsageSpacing = 12f;

		private DrugPolicy SelectedPolicy
		{
			get
			{
				return this.selPolicy;
			}
			set
			{
				this.CheckSelectedPolicyHasName();
				this.selPolicy = value;
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		public Dialog_ManageDrugPolicies(DrugPolicy selectedAssignedDrugs)
		{
			base.forcePause = true;
			base.doCloseX = true;
			base.closeOnEscapeKey = true;
			base.doCloseButton = true;
			base.closeOnClickedOutside = true;
			base.absorbInputAroundWindow = true;
			this.SelectedPolicy = selectedAssignedDrugs;
		}

		private void CheckSelectedPolicyHasName()
		{
			if (this.SelectedPolicy != null && this.SelectedPolicy.label.NullOrEmpty())
			{
				this.SelectedPolicy.label = "Unnamed";
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			float num = 0f;
			Rect rect = new Rect(0f, 0f, 150f, 35f);
			num = (float)(num + 150.0);
			if (Widgets.ButtonText(rect, "SelectDrugPolicy".Translate(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (DrugPolicy allPolicy in Current.Game.drugPolicyDatabase.AllPolicies)
				{
					DrugPolicy localAssignedDrugs = allPolicy;
					list.Add(new FloatMenuOption(localAssignedDrugs.label, delegate
					{
						this.SelectedPolicy = localAssignedDrugs;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num = (float)(num + 10.0);
			Rect rect2 = new Rect(num, 0f, 150f, 35f);
			num = (float)(num + 150.0);
			if (Widgets.ButtonText(rect2, "NewDrugPolicy".Translate(), true, false, true))
			{
				this.SelectedPolicy = Current.Game.drugPolicyDatabase.MakeNewDrugPolicy();
			}
			num = (float)(num + 10.0);
			Rect rect3 = new Rect(num, 0f, 150f, 35f);
			num = (float)(num + 150.0);
			if (Widgets.ButtonText(rect3, "DeleteDrugPolicy".Translate(), true, false, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (DrugPolicy allPolicy2 in Current.Game.drugPolicyDatabase.AllPolicies)
				{
					DrugPolicy localAssignedDrugs2 = allPolicy2;
					list2.Add(new FloatMenuOption(localAssignedDrugs2.label, delegate
					{
						AcceptanceReport acceptanceReport = Current.Game.drugPolicyDatabase.TryDelete(localAssignedDrugs2);
						if (!acceptanceReport.Accepted)
						{
							Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput);
						}
						else if (localAssignedDrugs2 == this.SelectedPolicy)
						{
							this.SelectedPolicy = null;
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			float width = inRect.width;
			double num2 = inRect.height - 40.0;
			Vector2 closeButSize = base.CloseButSize;
			Rect rect4 = new Rect(0f, 40f, width, (float)(num2 - closeButSize.y)).ContractedBy(10f);
			if (this.SelectedPolicy == null)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect4, "NoDrugPolicySelected".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			else
			{
				GUI.BeginGroup(rect4);
				Rect rect5 = new Rect(0f, 0f, 200f, 30f);
				Dialog_ManageDrugPolicies.DoNameInputRect(rect5, ref this.SelectedPolicy.label);
				Rect rect6 = new Rect(0f, 40f, rect4.width, (float)(rect4.height - 45.0 - 10.0));
				this.DoPolicyConfigArea(rect6);
				GUI.EndGroup();
			}
		}

		public override void PreClose()
		{
			base.PreClose();
			this.CheckSelectedPolicyHasName();
		}

		public static void DoNameInputRect(Rect rect, ref string name)
		{
			name = Widgets.TextField(rect, name, 30, Dialog_ManageDrugPolicies.ValidNameRegex);
		}

		private void DoPolicyConfigArea(Rect rect)
		{
			Rect rect2 = rect;
			rect2.height = 54f;
			Rect rect3 = rect;
			rect3.yMin = rect2.yMax;
			rect3.height -= 50f;
			Rect rect4 = rect;
			rect4.yMin = (float)(rect4.yMax - 50.0);
			this.DoColumnLabels(rect2);
			Widgets.DrawMenuSection(rect3);
			if (this.SelectedPolicy.Count == 0)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, "NoDrugs".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			else
			{
				float height = (float)((float)this.SelectedPolicy.Count * 35.0);
				Rect viewRect = new Rect(0f, 0f, (float)(rect3.width - 16.0), height);
				Widgets.BeginScrollView(rect3, ref this.scrollPosition, viewRect, true);
				DrugPolicy selectedPolicy = this.SelectedPolicy;
				for (int i = 0; i < selectedPolicy.Count; i++)
				{
					Rect rect5 = new Rect(0f, (float)((float)i * 35.0), viewRect.width, 35f);
					this.DoEntryRow(rect5, selectedPolicy[i]);
				}
				Widgets.EndScrollView();
			}
		}

		private void CalculateColumnsWidths(Rect rect, out float addictionWidth, out float allowJoyWidth, out float scheduledWidth, out float drugNameWidth, out float frequencyWidth, out float moodThresholdWidth, out float joyThresholdWidth, out float takeToInventoryWidth)
		{
			float num = (float)(rect.width - 108.0);
			drugNameWidth = (float)(num * 0.20000000298023224);
			addictionWidth = 36f;
			allowJoyWidth = 36f;
			scheduledWidth = 36f;
			frequencyWidth = (float)(num * 0.34999999403953552);
			moodThresholdWidth = (float)(num * 0.15000000596046448);
			joyThresholdWidth = (float)(num * 0.15000000596046448);
			takeToInventoryWidth = (float)(num * 0.15000000596046448);
		}

		private void DoColumnLabels(Rect rect)
		{
			rect.width -= 16f;
			float num = default(float);
			float num2 = default(float);
			float num3 = default(float);
			float num4 = default(float);
			float num5 = default(float);
			float num6 = default(float);
			float num7 = default(float);
			float num8 = default(float);
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8);
			float x = rect.x;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect2 = new Rect((float)(x + 4.0), rect.y, num4, rect.height);
			Widgets.Label(rect2, "DrugColumnLabel".Translate());
			TooltipHandler.TipRegion(rect2, "DrugNameColumnDesc".Translate());
			x += num4;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = new Rect(x, rect.y, num2 + num2, (float)(rect.height / 2.0));
			Widgets.Label(rect3, "DrugUsageColumnLabel".Translate());
			TooltipHandler.TipRegion(rect3, "DrugUsageColumnDesc".Translate());
			Rect rect4 = new Rect(x, (float)(rect.yMax - 24.0), 24f, 24f);
			GUI.DrawTexture(rect4, Dialog_ManageDrugPolicies.IconForAddiction);
			TooltipHandler.TipRegion(rect4, "DrugUsageTipForAddiction".Translate());
			x += num;
			Rect rect5 = new Rect(x, (float)(rect.yMax - 24.0), 24f, 24f);
			GUI.DrawTexture(rect5, Dialog_ManageDrugPolicies.IconForJoy);
			TooltipHandler.TipRegion(rect5, "DrugUsageTipForJoy".Translate());
			x += num2;
			Rect rect6 = new Rect(x, (float)(rect.yMax - 24.0), 24f, 24f);
			GUI.DrawTexture(rect6, Dialog_ManageDrugPolicies.IconScheduled);
			TooltipHandler.TipRegion(rect6, "DrugUsageTipScheduled".Translate());
			x += num3;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect7 = new Rect(x, rect.y, num5, rect.height);
			Widgets.Label(rect7, "FrequencyColumnLabel".Translate());
			TooltipHandler.TipRegion(rect7, "FrequencyColumnDesc".Translate());
			x += num5;
			Rect rect8 = new Rect(x, rect.y, num6, rect.height);
			Widgets.Label(rect8, "MoodThresholdColumnLabel".Translate());
			TooltipHandler.TipRegion(rect8, "MoodThresholdColumnDesc".Translate());
			x += num6;
			Rect rect9 = new Rect(x, rect.y, num7, rect.height);
			Widgets.Label(rect9, "JoyThresholdColumnLabel".Translate());
			TooltipHandler.TipRegion(rect9, "JoyThresholdColumnDesc".Translate());
			x += num7;
			Rect rect10 = new Rect(x, rect.y, num8, rect.height);
			Widgets.Label(rect10, "TakeToInventoryColumnLabel".Translate());
			TooltipHandler.TipRegion(rect10, "TakeToInventoryColumnDesc".Translate());
			x += num8;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		private void DoEntryRow(Rect rect, DrugPolicyEntry entry)
		{
			float num = default(float);
			float num2 = default(float);
			float num3 = default(float);
			float num4 = default(float);
			float num5 = default(float);
			float num6 = default(float);
			float num7 = default(float);
			float num8 = default(float);
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8);
			Text.Anchor = TextAnchor.MiddleLeft;
			float x = rect.x;
			Widgets.Label(new Rect(x, rect.y, num4, rect.height).ContractedBy(4f), entry.drug.LabelCap);
			float num9 = x;
			Vector2 vector = Text.CalcSize(entry.drug.LabelCap);
			Widgets.InfoCardButton((float)(num9 + vector.x + 5.0), (float)(rect.y + (rect.height - 24.0) / 2.0), entry.drug);
			x += num4;
			if (entry.drug.IsAddictiveDrug)
			{
				Widgets.Checkbox(x, rect.y, ref entry.allowedForAddiction, 24f, false);
			}
			x += num;
			if (entry.drug.IsPleasureDrug)
			{
				Widgets.Checkbox(x, rect.y, ref entry.allowedForJoy, 24f, false);
			}
			x += num2;
			Widgets.Checkbox(x, rect.y, ref entry.allowScheduled, 24f, false);
			x += num3;
			if (entry.allowScheduled)
			{
				entry.daysFrequency = Widgets.FrequencyHorizontalSlider(new Rect(x, rect.y, num5, rect.height).ContractedBy(4f), entry.daysFrequency, 0.1f, 25f, true);
				x += num5;
				string label = (!(entry.onlyIfMoodBelow < 1.0)) ? "NoDrugUseRequirement".Translate() : entry.onlyIfMoodBelow.ToStringPercent();
				entry.onlyIfMoodBelow = Widgets.HorizontalSlider(new Rect(x, rect.y, num6, rect.height).ContractedBy(4f), entry.onlyIfMoodBelow, 0.01f, 1f, true, label, null, null, -1f);
				x += num6;
				string label2 = (!(entry.onlyIfJoyBelow < 1.0)) ? "NoDrugUseRequirement".Translate() : entry.onlyIfJoyBelow.ToStringPercent();
				entry.onlyIfJoyBelow = Widgets.HorizontalSlider(new Rect(x, rect.y, num7, rect.height).ContractedBy(4f), entry.onlyIfJoyBelow, 0.01f, 1f, true, label2, null, null, -1f);
				x += num7;
				Widgets.TextFieldNumeric<int>(new Rect(x, rect.y, num8, rect.height).ContractedBy(4f), ref entry.takeToInventory, ref entry.takeToInventoryTempBuffer, 0f, 15f);
				x += num8;
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}
	}
}
