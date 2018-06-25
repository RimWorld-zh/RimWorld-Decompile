using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000860 RID: 2144
	[StaticConstructorOnStartup]
	public static class InspectPaneUtility
	{
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

		// Token: 0x06003091 RID: 12433 RVA: 0x001A64AB File Offset: 0x001A48AB
		public static void Reset()
		{
			InspectPaneUtility.truncatedLabelsCached.Clear();
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x001A64B8 File Offset: 0x001A48B8
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

		// Token: 0x06003093 RID: 12435 RVA: 0x001A6548 File Offset: 0x001A4948
		public static Vector2 PaneSizeFor(IInspectPane pane)
		{
			return new Vector2(InspectPaneUtility.PaneWidthFor(pane), 165f);
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x001A6570 File Offset: 0x001A4970
		public static bool CanInspectTogether(object A, object B)
		{
			Thing thing = A as Thing;
			Thing thing2 = B as Thing;
			return thing != null && thing2 != null && thing.def.category != ThingCategory.Pawn && thing.def == thing2.def;
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x001A65CC File Offset: 0x001A49CC
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

		// Token: 0x06003096 RID: 12438 RVA: 0x001A679C File Offset: 0x001A4B9C
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

		// Token: 0x06003097 RID: 12439 RVA: 0x001A67D8 File Offset: 0x001A4BD8
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

		// Token: 0x06003098 RID: 12440 RVA: 0x001A6868 File Offset: 0x001A4C68
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

		// Token: 0x06003099 RID: 12441 RVA: 0x001A6A34 File Offset: 0x001A4E34
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

		// Token: 0x0600309A RID: 12442 RVA: 0x001A6BC4 File Offset: 0x001A4FC4
		private static bool IsOpen(InspectTabBase tab, IInspectPane pane)
		{
			return tab.GetType() == pane.OpenTabType;
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x001A6BE8 File Offset: 0x001A4FE8
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

		// Token: 0x0600309C RID: 12444 RVA: 0x001A6C4C File Offset: 0x001A504C
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

		// Token: 0x0600309D RID: 12445 RVA: 0x001A6CDC File Offset: 0x001A50DC
		private static void InterfaceToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (!TutorSystem.TutorialMode || InspectPaneUtility.IsOpen(tab, pane) || TutorSystem.AllowAction("ITab-" + tab.tutorTag + "-Open"))
			{
				InspectPaneUtility.ToggleTab(tab, pane);
			}
		}
	}
}
