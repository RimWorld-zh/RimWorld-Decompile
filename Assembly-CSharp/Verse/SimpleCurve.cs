using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF8 RID: 3832
	public class SimpleCurve : IEnumerable<CurvePoint>, IEnumerable
	{
		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x06005B85 RID: 23429 RVA: 0x002E95B0 File Offset: 0x002E79B0
		public int PointsCount
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x06005B86 RID: 23430 RVA: 0x002E95D0 File Offset: 0x002E79D0
		public List<CurvePoint> Points
		{
			get
			{
				return this.points;
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06005B87 RID: 23431 RVA: 0x002E95EC File Offset: 0x002E79EC
		public bool HasView
		{
			get
			{
				return this.view != null;
			}
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x06005B88 RID: 23432 RVA: 0x002E9610 File Offset: 0x002E7A10
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

		// Token: 0x06005B89 RID: 23433 RVA: 0x002E9650 File Offset: 0x002E7A50
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06005B8A RID: 23434 RVA: 0x002E966C File Offset: 0x002E7A6C
		public IEnumerator<CurvePoint> GetEnumerator()
		{
			foreach (CurvePoint point in this.points)
			{
				yield return point;
			}
			yield break;
		}

		// Token: 0x17000EA3 RID: 3747
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

		// Token: 0x06005B8D RID: 23437 RVA: 0x002E96C4 File Offset: 0x002E7AC4
		public void SetPoints(IEnumerable<CurvePoint> newPoints)
		{
			this.points.Clear();
			foreach (CurvePoint item in newPoints)
			{
				this.points.Add(item);
			}
			this.SortPoints();
		}

		// Token: 0x06005B8E RID: 23438 RVA: 0x002E9734 File Offset: 0x002E7B34
		public void Add(float x, float y, bool sort = true)
		{
			CurvePoint newPoint = new CurvePoint(x, y);
			this.Add(newPoint, sort);
		}

		// Token: 0x06005B8F RID: 23439 RVA: 0x002E9753 File Offset: 0x002E7B53
		public void Add(CurvePoint newPoint, bool sort = true)
		{
			this.points.Add(newPoint);
			if (sort)
			{
				this.SortPoints();
			}
		}

		// Token: 0x06005B90 RID: 23440 RVA: 0x002E976E File Offset: 0x002E7B6E
		public void SortPoints()
		{
			this.points.Sort(SimpleCurve.CurvePointsComparer);
		}

		// Token: 0x06005B91 RID: 23441 RVA: 0x002E9784 File Offset: 0x002E7B84
		public void RemovePointNear(CurvePoint point)
		{
			for (int i = 0; i < this.points.Count; i++)
			{
				if ((this.points[i].Loc - point.Loc).sqrMagnitude < 0.001f)
				{
					this.points.RemoveAt(i);
					break;
				}
			}
		}

		// Token: 0x06005B92 RID: 23442 RVA: 0x002E97F4 File Offset: 0x002E7BF4
		public float Evaluate(float x)
		{
			float result;
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.", false);
				result = 0f;
			}
			else if (x <= this.points[0].x)
			{
				result = this.points[0].y;
			}
			else if (x >= this.points[this.points.Count - 1].x)
			{
				result = this.points[this.points.Count - 1].y;
			}
			else
			{
				CurvePoint curvePoint = this.points[0];
				CurvePoint curvePoint2 = this.points[this.points.Count - 1];
				for (int i = 0; i < this.points.Count; i++)
				{
					if (x <= this.points[i].x)
					{
						curvePoint2 = this.points[i];
						if (i > 0)
						{
							curvePoint = this.points[i - 1];
						}
						break;
					}
				}
				float t = (x - curvePoint.x) / (curvePoint2.x - curvePoint.x);
				result = Mathf.Lerp(curvePoint.y, curvePoint2.y, t);
			}
			return result;
		}

		// Token: 0x06005B93 RID: 23443 RVA: 0x002E9978 File Offset: 0x002E7D78
		public float PeriodProbabilityFromCumulative(float startX, float span)
		{
			float result;
			if (this.points.Count < 2)
			{
				result = 0f;
			}
			else
			{
				if (this.points[0].y != 0f)
				{
					Log.Warning("PeriodProbabilityFromCumulative should only run on curves whose first point is 0.", false);
				}
				float num = this.Evaluate(startX + span) - this.Evaluate(startX);
				if (num < 0f)
				{
					Log.Error("PeriodicProbability got negative probability from " + this + ": slope should never be negative.", false);
					num = 0f;
				}
				if (num > 1f)
				{
					num = 1f;
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06005B94 RID: 23444 RVA: 0x002E9A20 File Offset: 0x002E7E20
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			for (int i = 0; i < this.points.Count - 1; i++)
			{
				if (this.points[i + 1].x < this.points[i].x)
				{
					yield return prefix + ": points are out of order";
					break;
				}
			}
			yield break;
		}

		// Token: 0x04003CA5 RID: 15525
		private List<CurvePoint> points = new List<CurvePoint>();

		// Token: 0x04003CA6 RID: 15526
		[Unsaved]
		private SimpleCurveView view = null;

		// Token: 0x04003CA7 RID: 15527
		private static Comparison<CurvePoint> CurvePointsComparer = delegate(CurvePoint a, CurvePoint b)
		{
			int result;
			if (a.x < b.x)
			{
				result = -1;
			}
			else if (b.x < a.x)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		};
	}
}
