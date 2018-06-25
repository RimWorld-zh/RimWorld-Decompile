using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public static class GizmoGridDrawer
	{
		public static HashSet<KeyCode> drawnHotKeys = new HashSet<KeyCode>();

		private static float heightDrawn;

		private static int heightDrawnFrame;

		private static readonly Vector2 GizmoSpacing = new Vector2(5f, 14f);

		private static List<List<Gizmo>> gizmoGroups = new List<List<Gizmo>>();

		private static List<Gizmo> firstGizmos = new List<Gizmo>();

		private static List<Gizmo> tmpAllGizmos = new List<Gizmo>();

		[CompilerGenerated]
		private static Func<Gizmo, Gizmo, int> <>f__am$cache0;

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

		// Note: this type is marked as 'beforefieldinit'.
		static GizmoGridDrawer()
		{
		}

		[CompilerGenerated]
		private static int <DrawGizmoGrid>m__0(Gizmo lhs, Gizmo rhs)
		{
			return lhs.order.CompareTo(rhs.order);
		}

		[CompilerGenerated]
		private sealed class <DrawGizmoGrid>c__AnonStorey0
		{
			internal Gizmo interactedGiz;

			internal Gizmo floatMenuGiz;

			public <DrawGizmoGrid>c__AnonStorey0()
			{
			}

			internal bool <>m__0(List<Gizmo> group)
			{
				return group.Contains(this.interactedGiz);
			}

			internal bool <>m__1(List<Gizmo> group)
			{
				return group.Contains(this.floatMenuGiz);
			}
		}

		[CompilerGenerated]
		private sealed class <DrawGizmoGrid>c__AnonStorey1
		{
			internal FloatMenuOption option;

			public <DrawGizmoGrid>c__AnonStorey1()
			{
			}

			internal bool <>m__0(FloatMenuOption x)
			{
				return x.Label == this.option.Label;
			}
		}

		[CompilerGenerated]
		private sealed class <DrawGizmoGrid>c__AnonStorey2
		{
			internal Action prevAction;

			internal Action localOptionAction;

			public <DrawGizmoGrid>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				this.prevAction();
				this.localOptionAction();
			}
		}
	}
}
