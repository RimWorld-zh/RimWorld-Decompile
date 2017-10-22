using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public static class GizmoGridDrawer
	{
		public static HashSet<KeyCode> drawnHotKeys = new HashSet<KeyCode>();

		private static float heightDrawn = 0f;

		private static int heightDrawnFrame;

		private static readonly Vector2 GizmoSpacing = new Vector2(5f, 14f);

		private static List<List<Gizmo>> gizmoGroups = new List<List<Gizmo>>();

		private static List<Gizmo> firstGizmos = new List<Gizmo>();

		public static float HeightDrawnRecently
		{
			get
			{
				if (Time.frameCount > GizmoGridDrawer.heightDrawnFrame + 2)
				{
					return 0f;
				}
				return GizmoGridDrawer.heightDrawn;
			}
		}

		public static void DrawGizmoGrid(IEnumerable<Gizmo> gizmos, float startX, out Gizmo mouseoverGizmo)
		{
			GizmoGridDrawer.gizmoGroups.Clear();
			foreach (Gizmo item in gizmos)
			{
				bool flag = false;
				int num = 0;
				while (num < GizmoGridDrawer.gizmoGroups.Count)
				{
					if (!GizmoGridDrawer.gizmoGroups[num][0].GroupsWith(item))
					{
						num++;
						continue;
					}
					flag = true;
					GizmoGridDrawer.gizmoGroups[num].Add(item);
					break;
				}
				if (!flag)
				{
					List<Gizmo> list = new List<Gizmo>();
					list.Add(item);
					GizmoGridDrawer.gizmoGroups.Add(list);
				}
			}
			GizmoGridDrawer.firstGizmos.Clear();
			for (int i = 0; i < GizmoGridDrawer.gizmoGroups.Count; i++)
			{
				List<Gizmo> source = GizmoGridDrawer.gizmoGroups[i];
				Gizmo gizmo = source.FirstOrDefault((Func<Gizmo, bool>)((Gizmo opt) => !opt.disabled));
				if (gizmo == null)
				{
					gizmo = source.FirstOrDefault();
				}
				GizmoGridDrawer.firstGizmos.Add(gizmo);
			}
			GizmoGridDrawer.drawnHotKeys.Clear();
			float num2 = (float)(UI.screenWidth - 140);
			Text.Font = GameFont.Tiny;
			float num3 = (float)(UI.screenHeight - 35);
			Vector2 gizmoSpacing = GizmoGridDrawer.GizmoSpacing;
			Vector2 topLeft = new Vector2(startX, (float)(num3 - gizmoSpacing.y - 75.0));
			mouseoverGizmo = null;
			Gizmo interactedGiz = null;
			Event ev = null;
			for (int j = 0; j < GizmoGridDrawer.firstGizmos.Count; j++)
			{
				Gizmo gizmo2 = GizmoGridDrawer.firstGizmos[j];
				if (gizmo2.Visible)
				{
					float num4 = topLeft.x + gizmo2.Width;
					Vector2 gizmoSpacing2 = GizmoGridDrawer.GizmoSpacing;
					if (num4 + gizmoSpacing2.x > num2)
					{
						topLeft.x = startX;
						float y = topLeft.y;
						Vector2 gizmoSpacing3 = GizmoGridDrawer.GizmoSpacing;
						topLeft.y = (float)(y - (75.0 + gizmoSpacing3.x));
					}
					GizmoGridDrawer.heightDrawnFrame = Time.frameCount;
					GizmoGridDrawer.heightDrawn = (float)UI.screenHeight - topLeft.y;
					GizmoResult gizmoResult = gizmo2.GizmoOnGUI(topLeft);
					if (gizmoResult.State == GizmoState.Interacted)
					{
						ev = gizmoResult.InteractEvent;
						interactedGiz = gizmo2;
					}
					if ((int)gizmoResult.State >= 1)
					{
						mouseoverGizmo = gizmo2;
					}
					float x = topLeft.x;
					float y2 = topLeft.y;
					float width = gizmo2.Width;
					Vector2 gizmoSpacing4 = GizmoGridDrawer.GizmoSpacing;
					Rect rect = new Rect(x, y2, width, (float)(75.0 + gizmoSpacing4.y));
					rect = rect.ContractedBy(-12f);
					GenUI.AbsorbClicksInRect(rect);
					float x2 = topLeft.x;
					float width2 = gizmo2.Width;
					Vector2 gizmoSpacing5 = GizmoGridDrawer.GizmoSpacing;
					topLeft.x = x2 + (width2 + gizmoSpacing5.x);
				}
			}
			if (interactedGiz != null)
			{
				List<Gizmo> list2 = GizmoGridDrawer.gizmoGroups.First((Func<List<Gizmo>, bool>)((List<Gizmo> group) => group.Contains(interactedGiz)));
				for (int k = 0; k < list2.Count; k++)
				{
					Gizmo gizmo3 = list2[k];
					if (gizmo3 != interactedGiz && !gizmo3.disabled && interactedGiz.InheritInteractionsFrom(gizmo3))
					{
						gizmo3.ProcessInput(ev);
					}
				}
				interactedGiz.ProcessInput(ev);
				Event.current.Use();
			}
		}
	}
}
