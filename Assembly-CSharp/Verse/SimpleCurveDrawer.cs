using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public static class SimpleCurveDrawer
	{
		private const float PointSize = 10f;

		private static readonly Color AxisLineColor = new Color(0.2f, 0.5f, 1f, 1f);

		private static readonly Color MajorLineColor = new Color(0.2f, 0.4f, 1f, 0.6f);

		private static readonly Color MinorLineColor = new Color(0.2f, 0.3f, 1f, 0.19f);

		private const float MeasureWidth = 60f;

		private const float MeasureHeight = 30f;

		private const float MeasureLinePeekOut = 5f;

		private const float LegendCellWidth = 140f;

		private const float LegendCellHeight = 20f;

		private static readonly Texture2D CurvePoint = ContentFinder<Texture2D>.Get("UI/Widgets/Dev/CurvePoint", true);

		public static void DrawCurve(Rect rect, SimpleCurve curve, SimpleCurveDrawerStyle style = null, List<CurveMark> marks = null, Rect legendScreenRect = default(Rect))
		{
			SimpleCurveDrawInfo simpleCurveDrawInfo = new SimpleCurveDrawInfo();
			simpleCurveDrawInfo.curve = curve;
			SimpleCurveDrawer.DrawCurve(rect, simpleCurveDrawInfo, style, marks, legendScreenRect);
		}

		public static void DrawCurve(Rect rect, SimpleCurveDrawInfo curve, SimpleCurveDrawerStyle style = null, List<CurveMark> marks = null, Rect legendScreenRect = default(Rect))
		{
			if (curve.curve != null)
			{
				List<SimpleCurveDrawInfo> list = new List<SimpleCurveDrawInfo>();
				list.Add(curve);
				SimpleCurveDrawer.DrawCurves(rect, list, style, marks, legendScreenRect);
			}
		}

		public static void DrawCurves(Rect rect, List<SimpleCurveDrawInfo> curves, SimpleCurveDrawerStyle style = null, List<CurveMark> marks = null, Rect legendRect = default(Rect))
		{
			if (Event.current.type == EventType.Repaint)
			{
				if (style == null)
				{
					style = new SimpleCurveDrawerStyle();
				}
				if (curves.Count != 0)
				{
					bool flag = true;
					Rect viewRect = default(Rect);
					for (int i = 0; i < curves.Count; i++)
					{
						SimpleCurveDrawInfo simpleCurveDrawInfo = curves[i];
						if (simpleCurveDrawInfo.curve != null)
						{
							if (flag)
							{
								flag = false;
								viewRect = simpleCurveDrawInfo.curve.View.rect;
							}
							else
							{
								viewRect.xMin = Mathf.Min(viewRect.xMin, simpleCurveDrawInfo.curve.View.rect.xMin);
								viewRect.xMax = Mathf.Max(viewRect.xMax, simpleCurveDrawInfo.curve.View.rect.xMax);
								viewRect.yMin = Mathf.Min(viewRect.yMin, simpleCurveDrawInfo.curve.View.rect.yMin);
								viewRect.yMax = Mathf.Max(viewRect.yMax, simpleCurveDrawInfo.curve.View.rect.yMax);
							}
						}
					}
					if (style.UseFixedScale)
					{
						Vector2 fixedScale = style.FixedScale;
						viewRect.yMin = fixedScale.x;
						Vector2 fixedScale2 = style.FixedScale;
						viewRect.yMax = fixedScale2.y;
					}
					if (style.OnlyPositiveValues)
					{
						if (viewRect.xMin < 0.0)
						{
							viewRect.xMin = 0f;
						}
						if (viewRect.yMin < 0.0)
						{
							viewRect.yMin = 0f;
						}
					}
					if (style.UseFixedSection)
					{
						FloatRange fixedSection = style.FixedSection;
						viewRect.xMin = fixedSection.min;
						FloatRange fixedSection2 = style.FixedSection;
						viewRect.xMax = fixedSection2.max;
					}
					if (!Mathf.Approximately(viewRect.width, 0f) && !Mathf.Approximately(viewRect.height, 0f))
					{
						Rect rect2 = rect;
						if (style.DrawMeasures)
						{
							rect2.xMin += 60f;
							rect2.yMax -= 30f;
						}
						if (marks != null)
						{
							Rect rect3 = rect2;
							rect3.height = 15f;
							SimpleCurveDrawer.DrawCurveMarks(rect3, viewRect, marks);
							rect2.yMin = rect3.yMax;
						}
						if (style.DrawBackground)
						{
							GUI.color = new Color(0.302f, 0.318f, 0.365f);
							GUI.DrawTexture(rect2, BaseContent.WhiteTex);
						}
						if (style.DrawBackgroundLines)
						{
							SimpleCurveDrawer.DrawGraphBackgroundLines(rect2, viewRect);
						}
						if (style.DrawMeasures)
						{
							SimpleCurveDrawer.DrawCurveMeasures(rect, viewRect, rect2, style.MeasureLabelsXCount, style.MeasureLabelsYCount, style.XIntegersOnly, style.YIntegersOnly);
						}
						foreach (SimpleCurveDrawInfo item in curves)
						{
							SimpleCurveDrawer.DrawCurveLines(rect2, item, style.DrawPoints, viewRect, style.UseAntiAliasedLines, style.PointsRemoveOptimization);
						}
						if (style.DrawLegend)
						{
							SimpleCurveDrawer.DrawCurvesLegend(legendRect, curves);
						}
						if (style.DrawCurveMousePoint)
						{
							SimpleCurveDrawer.DrawCurveMousePoint(curves, rect2, viewRect, style.LabelX);
						}
					}
				}
			}
		}

		public static void DrawCurveLines(Rect rect, SimpleCurveDrawInfo curve, bool drawPoints, Rect viewRect, bool useAALines, bool pointsRemoveOptimization)
		{
			if (((curve.curve != null) ? curve.curve.PointsCount : 0) != 0)
			{
				Rect position = rect;
				position.yMin -= 1f;
				position.yMax += 1f;
				GUI.BeginGroup(position);
				if (Event.current.type == EventType.Repaint)
				{
					if (useAALines)
					{
						bool flag = true;
						Vector2 start = default(Vector2);
						Vector2 curvePoint = default(Vector2);
						int num = curve.curve.AllPoints.Count((Func<CurvePoint, bool>)((CurvePoint x) => x.x >= viewRect.xMin && x.x <= viewRect.xMax));
						int num2 = SimpleCurveDrawer.RemovePointsOptimizationFreq(num);
						for (int i = 0; i < curve.curve.PointsCount; i++)
						{
							CurvePoint curvePoint2 = curve.curve[i];
							if (pointsRemoveOptimization && i % num2 == 0 && i != 0 && i != num - 1)
							{
								continue;
							}
							curvePoint.x = curvePoint2.x;
							curvePoint.y = curvePoint2.y;
							Vector2 vector = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(rect, viewRect, curvePoint);
							if (flag)
							{
								flag = false;
							}
							else
							{
								if (start.x >= 0.0 && start.x <= rect.width)
								{
									goto IL_0176;
								}
								if (vector.x >= 0.0 && vector.x <= rect.width)
									goto IL_0176;
							}
							goto IL_018b;
							IL_018b:
							start = vector;
							continue;
							IL_0176:
							Widgets.DrawLine(start, vector, curve.color, 1f);
							goto IL_018b;
						}
						Vector2 start2 = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(rect, viewRect, curve.curve.AllPoints.First());
						Vector2 start3 = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(rect, viewRect, curve.curve.AllPoints.Last());
						Widgets.DrawLine(start2, new Vector2(0f, start2.y), curve.color, 1f);
						Widgets.DrawLine(start3, new Vector2(rect.width, start3.y), curve.color, 1f);
					}
					else
					{
						GUI.color = curve.color;
						float num3 = viewRect.x;
						float num4 = (float)(rect.width / 1.0);
						float num5 = viewRect.width / num4;
						while (num3 < viewRect.xMax)
						{
							num3 += num5;
							Vector2 vector2 = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(rect, viewRect, new Vector2(num3, curve.curve.Evaluate(num3)));
							GUI.DrawTexture(new Rect(vector2.x, vector2.y, 1f, 1f), BaseContent.WhiteTex);
						}
					}
					GUI.color = Color.white;
				}
				if (drawPoints)
				{
					for (int j = 0; j < curve.curve.PointsCount; j++)
					{
						CurvePoint curvePoint3 = curve.curve[j];
						Vector2 screenPoint = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(rect, viewRect, curvePoint3.Loc);
						SimpleCurveDrawer.DrawPoint(screenPoint);
					}
				}
				foreach (float debugInputValue in curve.curve.View.DebugInputValues)
				{
					float num6 = debugInputValue;
					GUI.color = new Color(0f, 1f, 0f, 0.25f);
					SimpleCurveDrawer.DrawInfiniteVerticalLine(rect, viewRect, num6);
					float y = curve.curve.Evaluate(num6);
					Vector2 curvePoint4 = new Vector2(num6, y);
					Vector2 screenPoint2 = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(rect, viewRect, curvePoint4);
					GUI.color = Color.green;
					SimpleCurveDrawer.DrawPoint(screenPoint2);
					GUI.color = Color.white;
				}
				GUI.EndGroup();
			}
		}

		public static void DrawCurveMeasures(Rect rect, Rect viewRect, Rect graphRect, int xLabelsCount, int yLabelsCount, bool xIntegersOnly, bool yIntegersOnly)
		{
			Text.Font = GameFont.Small;
			Color color = new Color(0.45f, 0.45f, 0.45f);
			Color color2 = new Color(0.7f, 0.7f, 0.7f);
			GUI.BeginGroup(rect);
			float num = default(float);
			float num2 = default(float);
			int num3 = default(int);
			SimpleCurveDrawer.CalculateMeasureStartAndInc(out num, out num2, out num3, viewRect.xMin, viewRect.xMax, xLabelsCount, xIntegersOnly);
			Text.Anchor = TextAnchor.UpperCenter;
			string b = string.Empty;
			for (int num4 = 0; num4 < num3; num4++)
			{
				float x = num + num2 * (float)num4;
				string text = x.ToString("F0");
				if (!(text == b))
				{
					b = text;
					Vector2 vector = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(graphRect, viewRect, new Vector2(x, 0f));
					float x2 = vector.x;
					float num5 = (float)(x2 + 60.0);
					float num6 = (float)(rect.height - 30.0);
					GUI.color = color;
					Widgets.DrawLineVertical(num5, num6, 5f);
					GUI.color = color2;
					Rect rect2 = new Rect((float)(num5 - 31.0), (float)(num6 + 2.0), 60f, 30f);
					Text.Font = GameFont.Tiny;
					Widgets.Label(rect2, text);
					Text.Font = GameFont.Small;
				}
			}
			float num7 = default(float);
			float num8 = default(float);
			int num9 = default(int);
			SimpleCurveDrawer.CalculateMeasureStartAndInc(out num7, out num8, out num9, viewRect.yMin, viewRect.yMax, yLabelsCount, yIntegersOnly);
			string b2 = string.Empty;
			Text.Anchor = TextAnchor.UpperRight;
			for (int num10 = 0; num10 < num9; num10++)
			{
				float y = num7 + num8 * (float)num10;
				string text2 = y.ToString("F0");
				if (!(text2 == b2))
				{
					b2 = text2;
					Vector2 vector2 = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(graphRect, viewRect, new Vector2(0f, y));
					float y2 = vector2.y;
					float num11 = y2 + (graphRect.y - rect.y);
					GUI.color = color;
					Widgets.DrawLineHorizontal(55f, num11, (float)(5.0 + graphRect.width));
					GUI.color = color2;
					Rect rect3 = new Rect(0f, (float)(num11 - 10.0), 55f, 20f);
					Text.Font = GameFont.Tiny;
					Widgets.Label(rect3, text2);
					Text.Font = GameFont.Small;
				}
			}
			GUI.EndGroup();
			GUI.color = new Color(1f, 1f, 1f);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		private static void CalculateMeasureStartAndInc(out float start, out float inc, out int count, float min, float max, int wantedCount, bool integersOnly)
		{
			if (integersOnly && GenMath.AnyIntegerInRange(min, max))
			{
				int num = Mathf.CeilToInt(min);
				int num2 = Mathf.FloorToInt(max);
				start = (float)num;
				inc = (float)Mathf.CeilToInt((float)(num2 - num + 1) / (float)wantedCount);
				count = (num2 - num) / (int)inc + 1;
			}
			else
			{
				start = min;
				inc = (max - min) / (float)wantedCount;
				count = wantedCount;
			}
		}

		public static void DrawCurvesLegend(Rect rect, List<SimpleCurveDrawInfo> curves)
		{
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.BeginGroup(rect);
			float num = 0f;
			float num2 = 0f;
			int num3 = (int)(rect.width / 140.0);
			int num4 = 0;
			foreach (SimpleCurveDrawInfo item in curves)
			{
				GUI.color = item.color;
				GUI.DrawTexture(new Rect(num, (float)(num2 + 2.0), 15f, 15f), BaseContent.WhiteTex);
				GUI.color = Color.white;
				num = (float)(num + 20.0);
				if (item.label != null)
				{
					Widgets.Label(new Rect(num, num2, 140f, 20f), item.label);
				}
				num4++;
				if (num4 == num3)
				{
					num4 = 0;
					num = 0f;
					num2 = (float)(num2 + 20.0);
				}
				else
				{
					num = (float)(num + 140.0);
				}
			}
			GUI.EndGroup();
			GUI.color = Color.white;
		}

		public static void DrawCurveMousePoint(List<SimpleCurveDrawInfo> curves, Rect screenRect, Rect viewRect, string labelX)
		{
			if (curves.Count != 0 && Mouse.IsOver(screenRect))
			{
				GUI.BeginGroup(screenRect);
				Vector2 mousePosition = Event.current.mousePosition;
				Vector2 vector = default(Vector2);
				Vector2 vector2 = default(Vector2);
				SimpleCurveDrawInfo simpleCurveDrawInfo = null;
				bool flag = false;
				foreach (SimpleCurveDrawInfo item in curves)
				{
					if (item.curve.AllPoints.Any())
					{
						Vector2 vector3 = SimpleCurveDrawer.ScreenToCurveCoords(screenRect, viewRect, mousePosition);
						vector3.y = item.curve.Evaluate(vector3.x);
						Vector2 vector4 = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(screenRect, viewRect, vector3);
						if (!flag || Vector2.Distance(vector4, mousePosition) < Vector2.Distance(vector2, mousePosition))
						{
							flag = true;
							vector = vector3;
							vector2 = vector4;
							simpleCurveDrawInfo = item;
						}
					}
				}
				if (flag)
				{
					SimpleCurveDrawer.DrawPoint(vector2);
					Rect rect = new Rect(vector2.x, vector2.y, 100f, 60f);
					Text.Anchor = TextAnchor.UpperLeft;
					if (rect.x + rect.width > screenRect.width)
					{
						rect.x -= rect.width;
						Text.Anchor = TextAnchor.UpperRight;
					}
					if (rect.y + rect.height > screenRect.height)
					{
						rect.y -= rect.height;
						if (Text.Anchor == TextAnchor.UpperLeft)
						{
							Text.Anchor = TextAnchor.LowerLeft;
						}
						else
						{
							Text.Anchor = TextAnchor.LowerRight;
						}
					}
					Widgets.Label(rect, labelX + ": " + vector.x.ToString("0.##") + "\n" + simpleCurveDrawInfo.labelY + ": " + vector.y.ToString("0.##"));
					Text.Anchor = TextAnchor.UpperLeft;
				}
				GUI.EndGroup();
			}
		}

		public static void DrawCurveMarks(Rect rect, Rect viewRect, List<CurveMark> marks)
		{
			float x = viewRect.x;
			float num = viewRect.x + viewRect.width;
			float num2 = (float)(rect.y + 5.0);
			float num3 = (float)(rect.yMax - 5.0);
			int num4;
			for (num4 = 0; num4 < marks.Count; num4++)
			{
				CurveMark curveMark = marks[num4];
				if (curveMark.X >= x && curveMark.X <= num)
				{
					GUI.color = curveMark.Color;
					Vector2 screenPoint = new Vector2(rect.x + (curveMark.X - x) / (num - x) * rect.width, (num4 % 2 != 0) ? num3 : num2);
					SimpleCurveDrawer.DrawPoint(screenPoint);
					TooltipHandler.TipRegion(new Rect((float)(screenPoint.x - 5.0), (float)(screenPoint.y - 5.0), 10f, 10f), new TipSignal(curveMark.Message));
				}
				num4++;
			}
			GUI.color = Color.white;
		}

		private static void DrawPoint(Vector2 screenPoint)
		{
			Rect position = new Rect((float)(screenPoint.x - 5.0), (float)(screenPoint.y - 5.0), 10f, 10f);
			GUI.DrawTexture(position, SimpleCurveDrawer.CurvePoint);
		}

		private static void DrawInfiniteVerticalLine(Rect rect, Rect viewRect, float curveX)
		{
			Vector2 vector = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(rect, viewRect, new Vector2(curveX, 0f));
			Widgets.DrawLineVertical(vector.x, -999f, 9999f);
		}

		private static void DrawInfiniteHorizontalLine(Rect rect, Rect viewRect, float curveY)
		{
			Vector2 vector = SimpleCurveDrawer.CurveToScreenCoordsInsideScreenRect(rect, viewRect, new Vector2(0f, curveY));
			Widgets.DrawLineHorizontal(-999f, vector.y, 9999f);
		}

		public static Vector2 CurveToScreenCoordsInsideScreenRect(Rect rect, Rect viewRect, Vector2 curvePoint)
		{
			Vector2 result = curvePoint;
			result.x -= viewRect.x;
			result.y -= viewRect.y;
			result.x *= rect.width / viewRect.width;
			result.y *= rect.height / viewRect.height;
			result.y = rect.height - result.y;
			return result;
		}

		public static Vector2 ScreenToCurveCoords(Rect rect, Rect viewRect, Vector2 screenPoint)
		{
			Vector2 loc = screenPoint;
			loc.y = rect.height - loc.y;
			loc.x /= rect.width / viewRect.width;
			loc.y /= rect.height / viewRect.height;
			loc.x += viewRect.x;
			loc.y += viewRect.y;
			return new CurvePoint(loc);
		}

		public static void DrawGraphBackgroundLines(Rect rect, Rect viewRect)
		{
			GUI.BeginGroup(rect);
			float num = 0.01f;
			while (viewRect.width / (num * 10.0) > 4.0)
			{
				num = (float)(num * 10.0);
			}
			for (float num2 = (float)Mathf.FloorToInt(viewRect.x / num) * num; num2 < viewRect.xMax; num2 += num)
			{
				if (Mathf.Abs((float)(num2 % (10.0 * num))) < 0.0010000000474974513)
				{
					GUI.color = SimpleCurveDrawer.MajorLineColor;
				}
				else
				{
					GUI.color = SimpleCurveDrawer.MinorLineColor;
				}
				SimpleCurveDrawer.DrawInfiniteVerticalLine(rect, viewRect, num2);
			}
			float num3 = 0.01f;
			while (viewRect.height / (num3 * 10.0) > 4.0)
			{
				num3 = (float)(num3 * 10.0);
			}
			for (float num4 = (float)Mathf.FloorToInt(viewRect.y / num3) * num3; num4 < viewRect.yMax; num4 += num3)
			{
				if (Mathf.Abs((float)(num4 % (10.0 * num3))) < 0.0010000000474974513)
				{
					GUI.color = SimpleCurveDrawer.MajorLineColor;
				}
				else
				{
					GUI.color = SimpleCurveDrawer.MinorLineColor;
				}
				SimpleCurveDrawer.DrawInfiniteHorizontalLine(rect, viewRect, num4);
			}
			GUI.color = SimpleCurveDrawer.AxisLineColor;
			SimpleCurveDrawer.DrawInfiniteHorizontalLine(rect, viewRect, 0f);
			SimpleCurveDrawer.DrawInfiniteVerticalLine(rect, viewRect, 0f);
			GUI.color = Color.white;
			GUI.EndGroup();
		}

		private static int RemovePointsOptimizationFreq(int count)
		{
			int result = count + 1;
			if (count > 1000)
			{
				result = 5;
			}
			if (count > 1200)
			{
				result = 4;
			}
			if (count > 1400)
			{
				result = 3;
			}
			if (count > 1900)
			{
				result = 2;
			}
			return result;
		}
	}
}
