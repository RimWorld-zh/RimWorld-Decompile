using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class HistoryAutoRecorderGroup : IExposable
	{
		public HistoryAutoRecorderGroupDef def;

		public List<HistoryAutoRecorder> recorders;

		private List<SimpleCurveDrawInfo> curves;

		private int cachedGraphTickCount = -1;

		public HistoryAutoRecorderGroup()
		{
			this.recorders = new List<HistoryAutoRecorder>();
			this.curves = new List<SimpleCurveDrawInfo>();
		}

		public void CreateRecorders()
		{
			List<HistoryAutoRecorderDef>.Enumerator enumerator = this.def.historyAutoRecorderDefs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					HistoryAutoRecorderDef current = enumerator.Current;
					HistoryAutoRecorder historyAutoRecorder = new HistoryAutoRecorder();
					historyAutoRecorder.def = current;
					this.recorders.Add(historyAutoRecorder);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}

		public float GetMaxDay()
		{
			float num = 0f;
			List<HistoryAutoRecorder>.Enumerator enumerator = this.recorders.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					HistoryAutoRecorder current = enumerator.Current;
					int count = current.records.Count;
					if (count != 0)
					{
						float num2 = (float)((float)((count - 1) * current.def.recordTicksFrequency) / 60000.0);
						if (num2 > num)
						{
							num = num2;
						}
					}
				}
				return num;
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}

		public void Tick()
		{
			for (int i = 0; i < this.recorders.Count; i++)
			{
				this.recorders[i].Tick();
			}
		}

		public void DrawGraph(Rect graphRect, Rect legendRect, Vector2 section, SimpleCurveDrawerStyle curveDrawerStyle, List<CurveMark> marks)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame != this.cachedGraphTickCount)
			{
				this.cachedGraphTickCount = ticksGame;
				this.curves.Clear();
				for (int i = 0; i < this.recorders.Count; i++)
				{
					HistoryAutoRecorder historyAutoRecorder = this.recorders[i];
					SimpleCurveDrawInfo simpleCurveDrawInfo = new SimpleCurveDrawInfo();
					simpleCurveDrawInfo.color = historyAutoRecorder.def.graphColor;
					simpleCurveDrawInfo.label = historyAutoRecorder.def.LabelCap;
					simpleCurveDrawInfo.curve = new SimpleCurve();
					for (int j = 0; j < historyAutoRecorder.records.Count; j++)
					{
						simpleCurveDrawInfo.curve.Add(new CurvePoint((float)((float)j * (float)historyAutoRecorder.def.recordTicksFrequency / 60000.0), historyAutoRecorder.records[j]), false);
					}
					simpleCurveDrawInfo.curve.SortPoints();
					if (historyAutoRecorder.records.Count == 1)
					{
						simpleCurveDrawInfo.curve.Add(new CurvePoint(1.66666669E-05f, historyAutoRecorder.records[0]), true);
					}
					this.curves.Add(simpleCurveDrawInfo);
				}
			}
			if (Mathf.Approximately(section.x, section.y))
			{
				section.y += 1.66666669E-05f;
			}
			curveDrawerStyle.FixedSection = section;
			curveDrawerStyle.LabelY = this.def.graphLabelY;
			curveDrawerStyle.UseFixedScale = this.def.useFixedScale;
			curveDrawerStyle.FixedScale = this.def.fixedScale;
			SimpleCurveDrawer.DrawCurves(graphRect, this.curves, curveDrawerStyle, marks, legendRect);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderGroupDef>(ref this.def, "def");
			Scribe_Collections.Look<HistoryAutoRecorder>(ref this.recorders, "recorders", LookMode.Deep, new object[0]);
		}
	}
}
