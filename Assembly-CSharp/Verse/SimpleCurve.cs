using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFB RID: 3835
	public class SimpleCurve : IEnumerable<CurvePoint>, IEnumerable
	{
		// Token: 0x04003CBA RID: 15546
		private List<CurvePoint> points = new List<CurvePoint>();

		// Token: 0x04003CBB RID: 15547
		[Unsaved]
		private SimpleCurveView view = null;

		// Token: 0x04003CBC RID: 15548
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

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x06005BB1 RID: 23473 RVA: 0x002EB718 File Offset: 0x002E9B18
		public int PointsCount
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x06005BB2 RID: 23474 RVA: 0x002EB738 File Offset: 0x002E9B38
		public List<CurvePoint> Points
		{
			get
			{
				return this.points;
			}
		}

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06005BB3 RID: 23475 RVA: 0x002EB754 File Offset: 0x002E9B54
		public bool HasView
		{
			get
			{
				return this.view != null;
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06005BB4 RID: 23476 RVA: 0x002EB778 File Offset: 0x002E9B78
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

		// Token: 0x06005BB5 RID: 23477 RVA: 0x002EB7B8 File Offset: 0x002E9BB8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06005BB6 RID: 23478 RVA: 0x002EB7D4 File Offset: 0x002E9BD4
		public IEnumerator<CurvePoint> GetEnumerator()
		{
			foreach (CurvePoint point in this.points)
			{
				yield return point;
			}
			yield break;
		}

		// Token: 0x17000EA6 RID: 3750
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

		// Token: 0x06005BB9 RID: 23481 RVA: 0x002EB82C File Offset: 0x002E9C2C
		public void SetPoints(IEnumerable<CurvePoint> newPoints)
		{
			this.points.Clear();
			foreach (CurvePoint item in newPoints)
			{
				this.points.Add(item);
			}
			this.SortPoints();
		}

		// Token: 0x06005BBA RID: 23482 RVA: 0x002EB89C File Offset: 0x002E9C9C
		public void Add(float x, float y, bool sort = true)
		{
			CurvePoint newPoint = new CurvePoint(x, y);
			this.Add(newPoint, sort);
		}

		// Token: 0x06005BBB RID: 23483 RVA: 0x002EB8BB File Offset: 0x002E9CBB
		public void Add(CurvePoint newPoint, bool sort = true)
		{
			this.points.Add(newPoint);
			if (sort)
			{
				this.SortPoints();
			}
		}

		// Token: 0x06005BBC RID: 23484 RVA: 0x002EB8D6 File Offset: 0x002E9CD6
		public void SortPoints()
		{
			this.points.Sort(SimpleCurve.CurvePointsComparer);
		}

		// Token: 0x06005BBD RID: 23485 RVA: 0x002EB8EC File Offset: 0x002E9CEC
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

		// Token: 0x06005BBE RID: 23486 RVA: 0x002EB95C File Offset: 0x002E9D5C
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

		// Token: 0x06005BBF RID: 23487 RVA: 0x002EBAE0 File Offset: 0x002E9EE0
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

		// Token: 0x06005BC0 RID: 23488 RVA: 0x002EBB88 File Offset: 0x002E9F88
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
	}
}
