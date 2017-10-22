using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class SimpleCurve : IEnumerable, IEnumerable<CurvePoint>
	{
		private List<CurvePoint> points = new List<CurvePoint>();

		[Unsaved]
		private SimpleCurveView view;

		private static Comparison<CurvePoint> CurvePointsComparer = (Comparison<CurvePoint>)delegate(CurvePoint a, CurvePoint b)
		{
			if (a.x < b.x)
			{
				return -1;
			}
			if (b.x < a.x)
			{
				return 1;
			}
			return 0;
		};

		public int PointsCount
		{
			get
			{
				return this.points.Count;
			}
		}

		public IEnumerable<CurvePoint> AllPoints
		{
			get
			{
				return this.points;
			}
		}

		public bool HasView
		{
			get
			{
				return this.view != null;
			}
		}

		public SimpleCurveView View
		{
			get
			{
				if (this.view == null)
				{
					this.view = new SimpleCurveView();
					this.view.SetViewRectAround(this);
				}
				return this.view;
			}
		}

		public CurvePoint this[int i]
		{
			get
			{
				return this.points[i];
			}
			set
			{
				this.points[i] = value;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public IEnumerator<CurvePoint> GetEnumerator()
		{
			List<CurvePoint>.Enumerator enumerator = this.points.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					CurvePoint point = enumerator.Current;
					yield return point;
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}

		public void SetPoints(IEnumerable<CurvePoint> newPoints)
		{
			this.points.Clear();
			foreach (CurvePoint item in newPoints)
			{
				this.points.Add(item);
			}
			this.SortPoints();
		}

		public void Add(float x, float y, bool sort = true)
		{
			CurvePoint newPoint = new CurvePoint(x, y);
			this.Add(newPoint, sort);
		}

		public void Add(CurvePoint newPoint, bool sort = true)
		{
			this.points.Add(newPoint);
			if (sort)
			{
				this.SortPoints();
			}
		}

		public void SortPoints()
		{
			this.points.Sort(SimpleCurve.CurvePointsComparer);
		}

		public void RemovePointNear(CurvePoint point)
		{
			int num = 0;
			while (true)
			{
				if (num < this.points.Count)
				{
					if (!((this.points[num].Loc - point.Loc).sqrMagnitude < 0.0010000000474974513))
					{
						num++;
						continue;
					}
					break;
				}
				return;
			}
			this.points.RemoveAt(num);
		}

		public float Evaluate(float x)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.");
				return 0f;
			}
			if (x <= this.points[0].x)
			{
				return this.points[0].y;
			}
			if (x >= this.points[this.points.Count - 1].x)
			{
				return this.points[this.points.Count - 1].y;
			}
			CurvePoint curvePoint = this.points[0];
			CurvePoint curvePoint2 = this.points[this.points.Count - 1];
			int num = 0;
			while (num < this.points.Count)
			{
				if (!(x <= this.points[num].x))
				{
					num++;
					continue;
				}
				curvePoint2 = this.points[num];
				if (num > 0)
				{
					curvePoint = this.points[num - 1];
				}
				break;
			}
			float t = (x - curvePoint.x) / (curvePoint2.x - curvePoint.x);
			return Mathf.Lerp(curvePoint.y, curvePoint2.y, t);
		}

		public float PeriodProbabilityFromCumulative(float startX, float span)
		{
			if (this.points.Count < 2)
			{
				return 0f;
			}
			if (this.points[0].y != 0.0)
			{
				Log.Warning("PeriodProbabilityFromCumulative should only run on curves whose first point is 0.");
			}
			float num = this.Evaluate(startX + span) - this.Evaluate(startX);
			if (num < 0.0)
			{
				Log.Error("PeriodicProbability got negative probability from " + this + ": slope should never be negative.");
				num = 0f;
			}
			if (num > 1.0)
			{
				num = 1f;
			}
			return num;
		}

		public IEnumerable<string> ConfigErrors(string prefix)
		{
			int i = 0;
			while (true)
			{
				if (i < this.points.Count - 1)
				{
					if (!(this.points[i + 1].x < this.points[i].x))
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return prefix + ": points are out of order";
		}
	}
}
