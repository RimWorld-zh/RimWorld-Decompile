using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000803 RID: 2051
	[StaticConstructorOnStartup]
	public class Dialog_ManageDrugPolicies : Window
	{
		// Token: 0x06002DBA RID: 11706 RVA: 0x00180A0F File Offset: 0x0017EE0F
		public Dialog_ManageDrugPolicies(DrugPolicy selectedAssignedDrugs)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.SelectedPolicy = selectedAssignedDrugs;
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002DBB RID: 11707 RVA: 0x00180A44 File Offset: 0x0017EE44
		// (set) Token: 0x06002DBC RID: 11708 RVA: 0x00180A5F File Offset: 0x0017EE5F
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

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002DBD RID: 11709 RVA: 0x00180A70 File Offset: 0x0017EE70
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x00180A94 File Offset: 0x0017EE94
		private void CheckSelectedPolicyHasName()
		{
			if (this.SelectedPolicy != null && this.SelectedPolicy.label.NullOrEmpty())
			{
				this.SelectedPolicy.label = "Unnamed";
			}
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x00180AC8 File Offset: 0x0017EEC8
		public override void DoWindowContents(Rect inRect)
		{
			float num = 0f;
			Rect rect = new Rect(0f, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect, "SelectDrugPolicy".Translate(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (DrugPolicy localAssignedDrugs3 in Current.Game.drugPolicyDatabase.AllPolicies)
				{
					DrugPolicy localAssignedDrugs = localAssignedDrugs3;
					list.Add(new FloatMenuOption(localAssignedDrugs.label, delegate()
					{
						this.SelectedPolicy = localAssignedDrugs;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num += 10f;
			Rect rect2 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect2, "NewDrugPolicy".Translate(), true, false, true))
			{
				this.SelectedPolicy = Current.Game.drugPolicyDatabase.MakeNewDrugPolicy();
			}
			num += 10f;
			Rect rect3 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect3, "DeleteDrugPolicy".Translate(), true, false, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (DrugPolicy localAssignedDrugs2 in Current.Game.drugPolicyDatabase.AllPolicies)
				{
					DrugPolicy localAssignedDrugs = localAssignedDrugs2;
					list2.Add(new FloatMenuOption(localAssignedDrugs.label, delegate()
					{
						AcceptanceReport acceptanceReport = Current.Game.drugPolicyDatabase.TryDelete(localAssignedDrugs);
						if (!acceptanceReport.Accepted)
						{
							Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
						}
						else if (localAssignedDrugs == this.SelectedPolicy)
						{
							this.SelectedPolicy = null;
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Rect rect4 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - this.CloseButSize.y).ContractedBy(10f);
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
				Rect rect6 = new Rect(0f, 40f, rect4.width, rect4.height - 45f - 10f);
				this.DoPolicyConfigArea(rect6);
				GUI.EndGroup();
			}
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x00180E00 File Offset: 0x0017F200
		public override void PostOpen()
		{
			base.PostOpen();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x00180E14 File Offset: 0x0017F214
		public override void PreClose()
		{
			base.PreClose();
			this.CheckSelectedPolicyHasName();
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x00180E23 File Offset: 0x0017F223
		public static void DoNameInputRect(Rect rect, ref string name)
		{
			name = Widgets.TextField(rect, name, 30, Dialog_ManageDrugPolicies.ValidNameRegex);
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x00180E38 File Offset: 0x0017F238
		private void DoPolicyConfigArea(Rect rect)
		{
			Rect rect2 = rect;
			rect2.height = 54f;
			Rect rect3 = rect;
			rect3.yMin = rect2.yMax;
			rect3.height -= 50f;
			Rect rect4 = rect;
			rect4.yMin = rect4.yMax - 50f;
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
				float height = (float)this.SelectedPolicy.Count * 35f;
				Rect viewRect = new Rect(0f, 0f, rect3.width - 16f, height);
				Widgets.BeginScrollView(rect3, ref this.scrollPosition, viewRect, true);
				DrugPolicy selectedPolicy = this.SelectedPolicy;
				for (int i = 0; i < selectedPolicy.Count; i++)
				{
					Rect rect5 = new Rect(0f, (float)i * 35f, viewRect.width, 35f);
					this.DoEntryRow(rect5, selectedPolicy[i]);
				}
				Widgets.EndScrollView();
			}
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x00180F84 File Offset: 0x0017F384
		private void CalculateColumnsWidths(Rect rect, out float addictionWidth, out float allowJoyWidth, out float scheduledWidth, out float drugNameWidth, out float frequencyWidth, out float moodThresholdWidth, out float joyThresholdWidth, out float takeToInventoryWidth)
		{
			float num = rect.width - 108f;
			drugNameWidth = num * 0.2f;
			addictionWidth = 36f;
			allowJoyWidth = 36f;
			scheduledWidth = 36f;
			frequencyWidth = num * 0.35f;
			moodThresholdWidth = num * 0.15f;
			joyThresholdWidth = num * 0.15f;
			takeToInventoryWidth = num * 0.15f;
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x00180FE8 File Offset: 0x0017F3E8
		private void DoColumnLabels(Rect rect)
		{
			rect.width -= 16f;
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			float num7;
			float num8;
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8);
			float num9 = rect.x;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect2 = new Rect(num9 + 4f, rect.y, num4, rect.height);
			Widgets.Label(rect2, "DrugColumnLabel".Translate());
			TooltipHandler.TipRegion(rect2, "DrugNameColumnDesc".Translate());
			num9 += num4;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = new Rect(num9, rect.y, num2 + num2, rect.height / 2f);
			Widgets.Label(rect3, "DrugUsageColumnLabel".Translate());
			TooltipHandler.TipRegion(rect3, "DrugUsageColumnDesc".Translate());
			Rect rect4 = new Rect(num9, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect4, Dialog_ManageDrugPolicies.IconForAddiction);
			TooltipHandler.TipRegion(rect4, "DrugUsageTipForAddiction".Translate());
			num9 += num;
			Rect rect5 = new Rect(num9, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect5, Dialog_ManageDrugPolicies.IconForJoy);
			TooltipHandler.TipRegion(rect5, "DrugUsageTipForJoy".Translate());
			num9 += num2;
			Rect rect6 = new Rect(num9, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect6, Dialog_ManageDrugPolicies.IconScheduled);
			TooltipHandler.TipRegion(rect6, "DrugUsageTipScheduled".Translate());
			num9 += num3;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect7 = new Rect(num9, rect.y, num5, rect.height);
			Widgets.Label(rect7, "FrequencyColumnLabel".Translate());
			TooltipHandler.TipRegion(rect7, "FrequencyColumnDesc".Translate());
			num9 += num5;
			Rect rect8 = new Rect(num9, rect.y, num6, rect.height);
			Widgets.Label(rect8, "MoodThresholdColumnLabel".Translate());
			TooltipHandler.TipRegion(rect8, "MoodThresholdColumnDesc".Translate());
			num9 += num6;
			Rect rect9 = new Rect(num9, rect.y, num7, rect.height);
			Widgets.Label(rect9, "JoyThresholdColumnLabel".Translate());
			TooltipHandler.TipRegion(rect9, "JoyThresholdColumnDesc".Translate());
			num9 += num7;
			Rect rect10 = new Rect(num9, rect.y, num8, rect.height);
			Widgets.Label(rect10, "TakeToInventoryColumnLabel".Translate());
			TooltipHandler.TipRegion(rect10, "TakeToInventoryColumnDesc".Translate());
			num9 += num8;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x001812C8 File Offset: 0x0017F6C8
		private void DoEntryRow(Rect rect, DrugPolicyEntry entry)
		{
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			float num7;
			float num8;
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8);
			Text.Anchor = TextAnchor.MiddleLeft;
			float num9 = rect.x;
			Widgets.Label(new Rect(num9, rect.y, num4, rect.height).ContractedBy(4f), entry.drug.LabelCap);
			Widgets.InfoCardButton(num9 + Text.CalcSize(entry.drug.LabelCap).x + 5f, rect.y + (rect.height - 24f) / 2f, entry.drug);
			num9 += num4;
			if (entry.drug.IsAddictiveDrug)
			{
				Widgets.Checkbox(num9, rect.y, ref entry.allowedForAddiction, 24f, false, true, null, null);
			}
			num9 += num;
			if (entry.drug.IsPleasureDrug)
			{
				Widgets.Checkbox(num9, rect.y, ref entry.allowedForJoy, 24f, false, true, null, null);
			}
			num9 += num2;
			Widgets.Checkbox(num9, rect.y, ref entry.allowScheduled, 24f, false, true, null, null);
			num9 += num3;
			if (entry.allowScheduled)
			{
				entry.daysFrequency = Widgets.FrequencyHorizontalSlider(new Rect(num9, rect.y, num5, rect.height).ContractedBy(4f), entry.daysFrequency, 0.1f, 25f, true);
				num9 += num5;
				string label;
				if (entry.onlyIfMoodBelow < 1f)
				{
					label = entry.onlyIfMoodBelow.ToStringPercent();
				}
				else
				{
					label = "NoDrugUseRequirement".Translate();
				}
				entry.onlyIfMoodBelow = Widgets.HorizontalSlider(new Rect(num9, rect.y, num6, rect.height).ContractedBy(4f), entry.onlyIfMoodBelow, 0.01f, 1f, true, label, null, null, -1f);
				num9 += num6;
				string label2;
				if (entry.onlyIfJoyBelow < 1f)
				{
					label2 = entry.onlyIfJoyBelow.ToStringPercent();
				}
				else
				{
					label2 = "NoDrugUseRequirement".Translate();
				}
				entry.onlyIfJoyBelow = Widgets.HorizontalSlider(new Rect(num9, rect.y, num7, rect.height).ContractedBy(4f), entry.onlyIfJoyBelow, 0.01f, 1f, true, label2, null, null, -1f);
				num9 += num7;
				Widgets.TextFieldNumeric<int>(new Rect(num9, rect.y, num8, rect.height).ContractedBy(4f), ref entry.takeToInventory, ref entry.takeToInventoryTempBuffer, 0f, 15f);
				num9 += num8;
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x04001839 RID: 6201
		private Vector2 scrollPosition;

		// Token: 0x0400183A RID: 6202
		private DrugPolicy selPolicy;

		// Token: 0x0400183B RID: 6203
		private const float TopAreaHeight = 40f;

		// Token: 0x0400183C RID: 6204
		private const float TopButtonHeight = 35f;

		// Token: 0x0400183D RID: 6205
		private const float TopButtonWidth = 150f;

		// Token: 0x0400183E RID: 6206
		private const float DrugEntryRowHeight = 35f;

		// Token: 0x0400183F RID: 6207
		private const float BottomButtonsAreaHeight = 50f;

		// Token: 0x04001840 RID: 6208
		private const float AddEntryButtonHeight = 35f;

		// Token: 0x04001841 RID: 6209
		private const float AddEntryButtonWidth = 150f;

		// Token: 0x04001842 RID: 6210
		private const float CellsPadding = 4f;

		// Token: 0x04001843 RID: 6211
		private static readonly Texture2D IconForAddiction = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/ForAddiction", true);

		// Token: 0x04001844 RID: 6212
		private static readonly Texture2D IconForJoy = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/ForJoy", true);

		// Token: 0x04001845 RID: 6213
		private static readonly Texture2D IconScheduled = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/Scheduled", true);

		// Token: 0x04001846 RID: 6214
		private static readonly Regex ValidNameRegex = Outfit.ValidNameRegex;

		// Token: 0x04001847 RID: 6215
		private const float UsageSpacing = 12f;
	}
}
