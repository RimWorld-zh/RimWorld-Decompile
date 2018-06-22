using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200085E RID: 2142
	[StaticConstructorOnStartup]
	public static class InspectPaneUtility
	{
		// Token: 0x0600308D RID: 12429 RVA: 0x001A635B File Offset: 0x001A475B
		public static void Reset()
		{
			InspectPaneUtility.truncatedLabelsCached.Clear();
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x001A6368 File Offset: 0x001A4768
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

		// Token: 0x0600308F RID: 12431 RVA: 0x001A63F8 File Offset: 0x001A47F8
		public static Vector2 PaneSizeFor(IInspectPane pane)
		{
			return new Vector2(InspectPaneUtility.PaneWidthFor(pane), 165f);
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x001A6420 File Offset: 0x001A4820
		public static bool CanInspectTogether(object A, object B)
		{
			Thing thing = A as Thing;
			Thing thing2 = B as Thing;
			return thing != null && thing2 != null && thing.def.category != ThingCategory.Pawn && thing.def == thing2.def;
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x001A647C File Offset: 0x001A487C
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

		// Token: 0x06003092 RID: 12434 RVA: 0x001A664C File Offset: 0x001A4A4C
		public static void ExtraOnGUI(IInspectPane pane)
		{
			if (pane.AnythingSelected)
			{
				if (KeyBindingDefOf.SelectNextInCell.KeyDownEvent)
				{
					pane.SelectNextInCell();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					pane.DrawInspectGizmos();
				}
				InspectPaneUtility.DoTabs(pane);
			}
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x001A6688 File Offset: 0x001A4A88
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

		// Token: 0x06003094 RID: 12436 RVA: 0x001A6718 File Offset: 0x001A4B18
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

		// Token: 0x06003095 RID: 12437 RVA: 0x001A68E4 File Offset: 0x001A4CE4
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

		// Token: 0x06003096 RID: 12438 RVA: 0x001A6A74 File Offset: 0x001A4E74
		private static bool IsOpen(InspectTabBase tab, IInspectPane pane)
		{
			return tab.GetType() == pane.OpenTabType;
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x001A6A98 File Offset: 0x001A4E98
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

		// Token: 0x06003098 RID: 12440 RVA: 0x001A6AFC File Offset: 0x001A4EFC
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

		// Token: 0x06003099 RID: 12441 RVA: 0x001A6B8C File Offset: 0x001A4F8C
		private static void InterfaceToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (!TutorSystem.TutorialMode || InspectPaneUtility.IsOpen(tab, pane) || TutorSystem.AllowAction("ITab-" + tab.tutorTag + "-Open"))
			{
				InspectPaneUtility.ToggleTab(tab, pane);
			}
		}

		// Token: 0x04001A47 RID: 6727
		private static Dictionary<string, string> truncatedLabelsCached = new Dictionary<string, string>();

		// Token: 0x04001A48 RID: 6728
		public const float TabWidth = 72f;

		// Token: 0x04001A49 RID: 6729
		public const float TabHeight = 30f;

		// Token: 0x04001A4A RID: 6730
		private static readonly Texture2D InspectTabButtonFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.07450981f, 0.08627451f, 0.105882354f, 1f));

		// Token: 0x04001A4B RID: 6731
		public const float CornerButtonsSize = 24f;

		// Token: 0x04001A4C RID: 6732
		public const float PaneInnerMargin = 12f;

		// Token: 0x04001A4D RID: 6733
		public const float PaneHeight = 165f;

		// Token: 0x04001A4E RID: 6734
		private const int TabMinimum = 6;

		// Token: 0x04001A4F RID: 6735
		private static List<Thing> selectedThings = new List<Thing>();
	}
}
