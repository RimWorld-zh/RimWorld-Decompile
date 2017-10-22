using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class EditWindow_CurveEditor : EditWindow
	{
		private const float ViewDragPanSpeed = 0.002f;

		private const float ScrollZoomSpeed = 0.025f;

		private const float PointClickDistanceLimit = 7f;

		private SimpleCurve curve;

		public List<float> debugInputValues;

		private int draggingPointIndex = -1;

		private int draggingButton = -1;

		private bool DraggingView
		{
			get
			{
				return this.draggingButton >= 0;
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 400f);
			}
		}

		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		public EditWindow_CurveEditor(SimpleCurve curve, string title)
		{
			this.curve = curve;
			base.optionalTitle = title;
		}

		public override void DoWindowContents(Rect inRect)
		{
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (widgetRow.ButtonIcon(TexButton.CenterOnPointsTex, "Center view around points."))
			{
				this.curve.View.SetViewRectAround(this.curve);
			}
			if (widgetRow.ButtonIcon(TexButton.CurveResetTex, "Reset to growth from 0 to 1."))
			{
				List<CurvePoint> list = new List<CurvePoint>();
				list.Add(new CurvePoint(0f, 0f));
				list.Add(new CurvePoint(1f, 1f));
				this.curve.SetPoints(list);
				this.curve.View.SetViewRectAround(this.curve);
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomHor1Tex, "Reset horizontal zoom to 0-1"))
			{
				this.curve.View.rect.xMin = 0f;
				this.curve.View.rect.xMax = 1f;
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomHor100Tex, "Reset horizontal zoom to 0-100"))
			{
				this.curve.View.rect.xMin = 0f;
				this.curve.View.rect.xMax = 100f;
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomHor20kTex, "Reset horizontal zoom to 0-20,000"))
			{
				this.curve.View.rect.xMin = 0f;
				this.curve.View.rect.xMax = 20000f;
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomVer1Tex, "Reset vertical zoom to 0-1"))
			{
				this.curve.View.rect.yMin = 0f;
				this.curve.View.rect.yMax = 1f;
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomVer100Tex, "Reset vertical zoom to 0-100"))
			{
				this.curve.View.rect.yMin = 0f;
				this.curve.View.rect.yMax = 100f;
			}
			if (widgetRow.ButtonIcon(TexButton.QuickZoomVer20kTex, "Reset vertical zoom to 0-20,000"))
			{
				this.curve.View.rect.yMin = 0f;
				this.curve.View.rect.yMax = 20000f;
			}
			Rect screenRect = new Rect(inRect.AtZero());
			screenRect.yMin += 26f;
			screenRect.yMax -= 24f;
			this.DoCurveEditor(screenRect);
		}

		private void DoCurveEditor(Rect screenRect)
		{
			Widgets.DrawMenuSection(screenRect, true);
			SimpleCurveDrawer.DrawCurve(screenRect, this.curve, null, null, default(Rect));
			Vector2 mousePosition = Event.current.mousePosition;
			if (Mouse.IsOver(screenRect))
			{
				Rect rect = new Rect((float)(mousePosition.x + 8.0), (float)(mousePosition.y + 18.0), 100f, 100f);
				Vector2 v = SimpleCurveDrawer.ScreenToCurveCoords(screenRect, this.curve.View.rect, mousePosition);
				Widgets.Label(rect, v.ToStringTwoDigits());
			}
			Rect rect2 = new Rect(0f, 0f, 50f, 24f)
			{
				x = screenRect.x,
				y = (float)(screenRect.y + screenRect.height / 2.0 - 12.0)
			};
			string s = Widgets.TextField(rect2, this.curve.View.rect.x.ToString());
			float num = default(float);
			if (float.TryParse(s, out num))
			{
				this.curve.View.rect.x = num;
			}
			rect2.x = screenRect.xMax - rect2.width;
			rect2.y = (float)(screenRect.y + screenRect.height / 2.0 - 12.0);
			s = Widgets.TextField(rect2, this.curve.View.rect.xMax.ToString());
			if (float.TryParse(s, out num))
			{
				this.curve.View.rect.xMax = num;
			}
			rect2.x = (float)(screenRect.x + screenRect.width / 2.0 - rect2.width / 2.0);
			rect2.y = screenRect.yMax - rect2.height;
			s = Widgets.TextField(rect2, this.curve.View.rect.y.ToString());
			if (float.TryParse(s, out num))
			{
				this.curve.View.rect.y = num;
			}
			rect2.x = (float)(screenRect.x + screenRect.width / 2.0 - rect2.width / 2.0);
			rect2.y = screenRect.y;
			s = Widgets.TextField(rect2, this.curve.View.rect.yMax.ToString());
			if (float.TryParse(s, out num))
			{
				this.curve.View.rect.yMax = num;
			}
			if (Mouse.IsOver(screenRect))
			{
				if (Event.current.type == EventType.ScrollWheel)
				{
					Vector2 delta = Event.current.delta;
					float num2 = (float)(-1.0 * delta.y * 0.02500000037252903);
					Vector2 center = this.curve.View.rect.center;
					float num3 = center.x - this.curve.View.rect.x;
					Vector2 center2 = this.curve.View.rect.center;
					float num4 = center2.y - this.curve.View.rect.y;
					this.curve.View.rect.xMin += num3 * num2;
					this.curve.View.rect.xMax -= num3 * num2;
					this.curve.View.rect.yMin += num4 * num2;
					this.curve.View.rect.yMax -= num4 * num2;
					Event.current.Use();
				}
				if (Event.current.type == EventType.MouseDown && (Event.current.button == 0 || Event.current.button == 2))
				{
					List<int> list = this.PointsNearMouse(screenRect).ToList();
					if (list.Any())
					{
						this.draggingPointIndex = list[0];
					}
					else
					{
						this.draggingPointIndex = -1;
					}
					if (this.draggingPointIndex < 0)
					{
						this.draggingButton = Event.current.button;
					}
					Event.current.Use();
				}
				if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
				{
					Vector2 mouseCurveCoords = SimpleCurveDrawer.ScreenToCurveCoords(screenRect, this.curve.View.rect, Event.current.mousePosition);
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					list2.Add(new FloatMenuOption("Add point at " + mouseCurveCoords.ToString(), (Action)delegate
					{
						this.curve.Add(new CurvePoint(mouseCurveCoords), true);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
					foreach (int item in this.PointsNearMouse(screenRect))
					{
						CurvePoint point = this.curve[item];
						list2.Add(new FloatMenuOption("Remove point at " + point.ToString(), (Action)delegate
						{
							this.curve.RemovePointNear(point);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
					Event.current.Use();
				}
			}
			if (this.draggingPointIndex >= 0)
			{
				this.curve[this.draggingPointIndex] = new CurvePoint(SimpleCurveDrawer.ScreenToCurveCoords(screenRect, this.curve.View.rect, Event.current.mousePosition));
				this.curve.SortPoints();
				if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
				{
					this.draggingPointIndex = -1;
					Event.current.Use();
				}
			}
			if (this.DraggingView)
			{
				if (Event.current.type == EventType.MouseDrag)
				{
					Vector2 delta2 = Event.current.delta;
					this.curve.View.rect.x -= (float)(delta2.x * this.curve.View.rect.width * 0.0020000000949949026);
					this.curve.View.rect.y += (float)(delta2.y * this.curve.View.rect.height * 0.0020000000949949026);
					Event.current.Use();
				}
				if (Event.current.type == EventType.MouseUp && Event.current.button == this.draggingButton)
				{
					this.draggingButton = -1;
				}
			}
		}

		private IEnumerable<int> PointsNearMouse(Rect screenRect)
		{
			GUI.BeginGroup(screenRect);
			try
			{
				for (int i = 0; i < this.curve.PointsCount; i++)
				{
					Vector2 screenPoint = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(screenRect, this.curve.View.rect, this.curve[i].Loc);
					if ((screenPoint - Event.current.mousePosition).sqrMagnitude < 49.0)
					{
						yield return i;
					}
				}
			}
			finally
			{
				((_003CPointsNearMouse_003Ec__Iterator232)/*Error near IL_0100: stateMachine*/)._003C_003E__Finally0();
			}
		}
	}
}
