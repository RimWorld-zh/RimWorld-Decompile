using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000862 RID: 2146
	[StaticConstructorOnStartup]
	public static class InspectPaneUtility
	{
		// Token: 0x06003094 RID: 12436 RVA: 0x001A617B File Offset: 0x001A457B
		public static void Reset()
		{
			InspectPaneUtility.truncatedLabelsCached.Clear();
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x001A6188 File Offset: 0x001A4588
		public static float PaneWidthFor(IInspectPane pane)
		{
			float result;
			if (pane == null)
			{
				result = 432f;
			}
			else
			{
				int num = 0;
				foreach (InspectTabBase inspectTabBase in pane.CurTabs)
				{
					if (inspectTabBase.IsVisible)
					{
						num++;
					}
				}
				result = 72f * (float)Mathf.Max(6, num);
			}
			return result;
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x001A6218 File Offset: 0x001A4618
		public static Vector2 PaneSizeFor(IInspectPane pane)
		{
			return new Vector2(InspectPaneUtility.PaneWidthFor(pane), 165f);
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x001A6240 File Offset: 0x001A4640
		public static bool CanInspectTogether(object A, object B)
		{
			Thing thing = A as Thing;
			Thing thing2 = B as Thing;
			return thing != null && thing2 != null && thing.def.category != ThingCategory.Pawn && thing.def == thing2.def;
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x001A629C File Offset: 0x001A469C
		public static string AdjustedLabelFor(IEnumerable<object> selected, Rect rect)
		{
			Zone zone = selected.First<object>() as Zone;
			string text;
			if (zone != null)
			{
				text = zone.label;
			}
			else
			{
				InspectPaneUtility.selectedThings.Clear();
				foreach (object obj in selected)
				{
					Thing thing = obj as Thing;
					if (thing != null)
					{
						InspectPaneUtility.selectedThings.Add(thing);
					}
				}
				if (InspectPaneUtility.selectedThings.Count == 1)
				{
					text = InspectPaneUtility.selectedThings[0].LabelCap;
				}
				else
				{
					IEnumerable<IGrouping<string, Thing>> source = from th in InspectPaneUtility.selectedThings
					group th by th.LabelCapNoCount into g
					select g;
					if (source.Count<IGrouping<string, Thing>>() > 1)
					{
						text = "VariousLabel".Translate();
					}
					else
					{
						text = InspectPaneUtility.selectedThings[0].LabelCapNoCount;
					}
					int num = 0;
					for (int i = 0; i < InspectPaneUtility.selectedThings.Count; i++)
					{
						num += InspectPaneUtility.selectedThings[i].stackCount;
					}
					text = text + " x" + num;
				}
				InspectPaneUtility.selectedThings.Clear();
			}
			Text.Font = GameFont.Medium;
			return text.Truncate(rect.width, InspectPaneUtility.truncatedLabelsCached);
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x001A646C File Offset: 0x001A486C
		public static void ExtraOnGUI(IInspectPane pane)
		{
			if (pane.AnythingSelected)
			{
				if (KeyBindingDefOf.SelectNextInCell.KeyDownEvent)
				{
					pane.SelectNextInCell();
				}
				pane.DrawInspectGizmos();
				InspectPaneUtility.DoTabs(pane);
			}
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x001A64A0 File Offset: 0x001A48A0
		public static void UpdateTabs(IInspectPane pane)
		{
			bool flag = false;
			foreach (InspectTabBase inspectTabBase in pane.CurTabs)
			{
				if (inspectTabBase.IsVisible)
				{
					if (inspectTabBase.GetType() == pane.OpenTabType)
					{
						inspectTabBase.TabUpdate();
						flag = true;
					}
				}
			}
			if (!flag)
			{
				pane.CloseOpenTab();
			}
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x001A6530 File Offset: 0x001A4930
		public static void InspectPaneOnGUI(Rect inRect, IInspectPane pane)
		{
			pane.RecentHeight = 165f;
			if (pane.AnythingSelected)
			{
				try
				{
					Rect rect = inRect.ContractedBy(12f);
					rect.yMin -= 4f;
					rect.yMax += 6f;
					GUI.BeginGroup(rect);
					float num = 0f;
					if (pane.ShouldShowSelectNextInCellButton)
					{
						Rect rect2 = new Rect(rect.width - 24f, 0f, 24f, 24f);
						if (Widgets.ButtonImage(rect2, TexButton.SelectOverlappingNext))
						{
							pane.SelectNextInCell();
						}
						num += 24f;
						TooltipHandler.TipRegion(rect2, "SelectNextInSquareTip".Translate(new object[]
						{
							KeyBindingDefOf.SelectNextInCell.MainKeyLabel
						}));
					}
					pane.DoInspectPaneButtons(rect, ref num);
					Rect rect3 = new Rect(0f, 0f, rect.width - num, 50f);
					string label = pane.GetLabel(rect3);
					rect3.width += 300f;
					Text.Font = GameFont.Medium;
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect3, label);
					if (pane.ShouldShowPaneContents)
					{
						Rect rect4 = rect.AtZero();
						rect4.yMin += 26f;
						pane.DoPaneContents(rect4);
					}
				}
				catch (Exception ex)
				{
					Log.Error("Exception doing inspect pane: " + ex.ToString(), false);
				}
				finally
				{
					GUI.EndGroup();
				}
			}
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x001A66FC File Offset: 0x001A4AFC
		private static void DoTabs(IInspectPane pane)
		{
			try
			{
				float y = pane.PaneTopY - 30f;
				float num = InspectPaneUtility.PaneWidthFor(pane) - 72f;
				float width = 0f;
				bool flag = false;
				foreach (InspectTabBase inspectTabBase in pane.CurTabs)
				{
					if (inspectTabBase.IsVisible)
					{
						Rect rect = new Rect(num, y, 72f, 30f);
						width = num;
						Text.Font = GameFont.Small;
						if (Widgets.ButtonText(rect, inspectTabBase.labelKey.Translate(), true, false, true))
						{
							InspectPaneUtility.InterfaceToggleTab(inspectTabBase, pane);
						}
						bool flag2 = inspectTabBase.GetType() == pane.OpenTabType;
						if (!flag2 && !inspectTabBase.TutorHighlightTagClosed.NullOrEmpty())
						{
							UIHighlighter.HighlightOpportunity(rect, inspectTabBase.TutorHighlightTagClosed);
						}
						if (flag2)
						{
							inspectTabBase.DoTabGUI();
							pane.RecentHeight = 700f;
							flag = true;
						}
						num -= 72f;
					}
				}
				if (flag)
				{
					GUI.DrawTexture(new Rect(0f, y, width, 30f), InspectPaneUtility.InspectTabButtonFillTex);
				}
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(ex.ToString(), 742783, false);
			}
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x001A688C File Offset: 0x001A4C8C
		private static bool IsOpen(InspectTabBase tab, IInspectPane pane)
		{
			return tab.GetType() == pane.OpenTabType;
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x001A68B0 File Offset: 0x001A4CB0
		private static void ToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (InspectPaneUtility.IsOpen(tab, pane) || (tab == null && pane.OpenTabType == null))
			{
				pane.OpenTabType = null;
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
			}
			else
			{
				tab.OnOpen();
				pane.OpenTabType = tab.GetType();
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x001A6914 File Offset: 0x001A4D14
		public static InspectTabBase OpenTab(Type inspectTabType)
		{
			MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			InspectTabBase inspectTabBase = (from t in mainTabWindow_Inspect.CurTabs
			where inspectTabType.IsAssignableFrom(t.GetType())
			select t).FirstOrDefault<InspectTabBase>();
			if (inspectTabBase != null)
			{
				if (Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect, true);
				}
				if (!InspectPaneUtility.IsOpen(inspectTabBase, mainTabWindow_Inspect))
				{
					InspectPaneUtility.ToggleTab(inspectTabBase, mainTabWindow_Inspect);
				}
			}
			return inspectTabBase;
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x001A69A4 File Offset: 0x001A4DA4
		private static void InterfaceToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (!TutorSystem.TutorialMode || InspectPaneUtility.IsOpen(tab, pane) || TutorSystem.AllowAction("ITab-" + tab.tutorTag + "-Open"))
			{
				InspectPaneUtility.ToggleTab(tab, pane);
			}
		}

		// Token: 0x04001A49 RID: 6729
		private static Dictionary<string, string> truncatedLabelsCached = new Dictionary<string, string>();

		// Token: 0x04001A4A RID: 6730
		public const float TabWidth = 72f;

		// Token: 0x04001A4B RID: 6731
		public const float TabHeight = 30f;

		// Token: 0x04001A4C RID: 6732
		private static readonly Texture2D InspectTabButtonFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.07450981f, 0.08627451f, 0.105882354f, 1f));

		// Token: 0x04001A4D RID: 6733
		public const float CornerButtonsSize = 24f;

		// Token: 0x04001A4E RID: 6734
		public const float PaneInnerMargin = 12f;

		// Token: 0x04001A4F RID: 6735
		public const float PaneHeight = 165f;

		// Token: 0x04001A50 RID: 6736
		private const int TabMinimum = 6;

		// Token: 0x04001A51 RID: 6737
		private static List<Thing> selectedThings = new List<Thing>();
	}
}
