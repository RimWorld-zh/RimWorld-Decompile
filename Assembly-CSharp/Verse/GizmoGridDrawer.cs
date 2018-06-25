using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6F RID: 3695
	public static class GizmoGridDrawer
	{
		// Token: 0x040039A3 RID: 14755
		public static HashSet<KeyCode> drawnHotKeys = new HashSet<KeyCode>();

		// Token: 0x040039A4 RID: 14756
		private static float heightDrawn;

		// Token: 0x040039A5 RID: 14757
		private static int heightDrawnFrame;

		// Token: 0x040039A6 RID: 14758
		private static readonly Vector2 GizmoSpacing = new Vector2(5f, 14f);

		// Token: 0x040039A7 RID: 14759
		private static List<List<Gizmo>> gizmoGroups = new List<List<Gizmo>>();

		// Token: 0x040039A8 RID: 14760
		private static List<Gizmo> firstGizmos = new List<Gizmo>();

		// Token: 0x040039A9 RID: 14761
		private static List<Gizmo> tmpAllGizmos = new List<Gizmo>();

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06005707 RID: 22279 RVA: 0x002CCA64 File Offset: 0x002CAE64
		public static float HeightDrawnRecently
		{
			get
			{
				float result;
				if (Time.frameCount > GizmoGridDrawer.heightDrawnFrame + 2)
				{
					result = 0f;
				}
				else
				{
					result = GizmoGridDrawer.heightDrawn;
				}
				return result;
			}
		}

		// Token: 0x06005708 RID: 22280 RVA: 0x002CCA9C File Offset: 0x002CAE9C
		public static void DrawGizmoGrid(IEnumerable<Gizmo> gizmos, float startX, out Gizmo mouseoverGizmo)
		{
			GizmoGridDrawer.tmpAllGizmos.Clear();
			GizmoGridDrawer.tmpAllGizmos.AddRange(gizmos);
			GizmoGridDrawer.tmpAllGizmos.SortStable((Gizmo lhs, Gizmo rhs) => lhs.order.CompareTo(rhs.order));
			GizmoGridDrawer.gizmoGroups.Clear();
			for (int i = 0; i < GizmoGridDrawer.tmpAllGizmos.Count; i++)
			{
				Gizmo gizmo = GizmoGridDrawer.tmpAllGizmos[i];
				bool flag = false;
				for (int j = 0; j < GizmoGridDrawer.gizmoGroups.Count; j++)
				{
					if (GizmoGridDrawer.gizmoGroups[j][0].GroupsWith(gizmo))
					{
						flag = true;
						GizmoGridDrawer.gizmoGroups[j].Add(gizmo);
						GizmoGridDrawer.gizmoGroups[j][0].MergeWith(gizmo);
						break;
					}
				}
				if (!flag)
				{
					List<Gizmo> list = new List<Gizmo>();
					list.Add(gizmo);
					GizmoGridDrawer.gizmoGroups.Add(list);
				}
			}
			GizmoGridDrawer.firstGizmos.Clear();
			for (int k = 0; k < GizmoGridDrawer.gizmoGroups.Count; k++)
			{
				List<Gizmo> list2 = GizmoGridDrawer.gizmoGroups[k];
				Gizmo gizmo2 = null;
				for (int l = 0; l < list2.Count; l++)
				{
					if (!list2[l].disabled)
					{
						gizmo2 = list2[l];
						break;
					}
				}
				if (gizmo2 == null)
				{
					gizmo2 = list2.FirstOrDefault<Gizmo>();
				}
				if (gizmo2 != null)
				{
					GizmoGridDrawer.firstGizmos.Add(gizmo2);
				}
			}
			GizmoGridDrawer.drawnHotKeys.Clear();
			float num = (float)(UI.screenWidth - 147);
			float maxWidth = num - startX;
			Text.Font = GameFont.Tiny;
			Vector2 topLeft = new Vector2(startX, (float)(UI.screenHeight - 35) - GizmoGridDrawer.GizmoSpacing.y - 75f);
			mouseoverGizmo = null;
			Gizmo interactedGiz = null;
			Event ev = null;
			Gizmo floatMenuGiz = null;
			for (int m = 0; m < GizmoGridDrawer.firstGizmos.Count; m++)
			{
				Gizmo gizmo3 = GizmoGridDrawer.firstGizmos[m];
				if (gizmo3.Visible)
				{
					if (topLeft.x + gizmo3.GetWidth(maxWidth) > num)
					{
						topLeft.x = startX;
						topLeft.y -= 75f + GizmoGridDrawer.GizmoSpacing.x;
					}
					GizmoGridDrawer.heightDrawnFrame = Time.frameCount;
					GizmoGridDrawer.heightDrawn = (float)UI.screenHeight - topLeft.y;
					GizmoResult gizmoResult = gizmo3.GizmoOnGUI(topLeft, maxWidth);
					if (gizmoResult.State == GizmoState.Interacted)
					{
						ev = gizmoResult.InteractEvent;
						interactedGiz = gizmo3;
					}
					else if (gizmoResult.State == GizmoState.OpenedFloatMenu)
					{
						floatMenuGiz = gizmo3;
					}
					if (gizmoResult.State >= GizmoState.Mouseover)
					{
						mouseoverGizmo = gizmo3;
					}
					Rect rect = new Rect(topLeft.x, topLeft.y, gizmo3.GetWidth(maxWidth), 75f + GizmoGridDrawer.GizmoSpacing.y);
					rect = rect.ContractedBy(-12f);
					GenUI.AbsorbClicksInRect(rect);
					topLeft.x += gizmo3.GetWidth(maxWidth) + GizmoGridDrawer.GizmoSpacing.x;
				}
			}
			if (interactedGiz != null)
			{
				List<Gizmo> list3 = GizmoGridDrawer.gizmoGroups.First((List<Gizmo> group) => group.Contains(interactedGiz));
				for (int n = 0; n < list3.Count; n++)
				{
					Gizmo gizmo4 = list3[n];
					if (gizmo4 != interactedGiz && !gizmo4.disabled && interactedGiz.InheritInteractionsFrom(gizmo4))
					{
						gizmo4.ProcessInput(ev);
					}
				}
				interactedGiz.ProcessInput(ev);
				Event.current.Use();
			}
			else if (floatMenuGiz != null)
			{
				List<FloatMenuOption> list4 = new List<FloatMenuOption>();
				foreach (FloatMenuOption item in floatMenuGiz.RightClickFloatMenuOptions)
				{
					list4.Add(item);
				}
				List<Gizmo> list5 = GizmoGridDrawer.gizmoGroups.First((List<Gizmo> group) => group.Contains(floatMenuGiz));
				for (int num2 = 0; num2 < list5.Count; num2++)
				{
					Gizmo gizmo5 = list5[num2];
					if (gizmo5 != floatMenuGiz && !gizmo5.disabled && floatMenuGiz.InheritFloatMenuInteractionsFrom(gizmo5))
					{
						using (IEnumerator<FloatMenuOption> enumerator2 = gizmo5.RightClickFloatMenuOptions.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								FloatMenuOption option = enumerator2.Current;
								FloatMenuOption floatMenuOption = list4.Find((FloatMenuOption x) => x.Label == option.Label);
								if (floatMenuOption == null)
								{
									list4.Add(option);
								}
								else if (!option.Disabled)
								{
									if (!floatMenuOption.Disabled)
									{
										Action prevAction = floatMenuOption.action;
										Action localOptionAction = option.action;
										floatMenuOption.action = delegate()
										{
											prevAction();
											localOptionAction();
										};
									}
									else if (floatMenuOption.Disabled)
									{
										list4[list4.IndexOf(floatMenuOption)] = option;
									}
								}
							}
						}
					}
				}
				Event.current.Use();
				if (list4.Any<FloatMenuOption>())
				{
					Find.WindowStack.Add(new FloatMenu(list4));
				}
			}
			GizmoGridDrawer.gizmoGroups.Clear();
			GizmoGridDrawer.firstGizmos.Clear();
			GizmoGridDrawer.tmpAllGizmos.Clear();
		}
	}
}
