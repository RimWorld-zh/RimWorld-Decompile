using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF7 RID: 3831
	public class SimpleCurveView
	{
		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x06005BA3 RID: 23459 RVA: 0x002EB08C File Offset: 0x002E948C
		public IEnumerable<float> DebugInputValues
		{
			get
			{
				if (this.debugInputValues == null)
				{
					yield break;
				}
				foreach (float val in this.debugInputValues.Values)
				{
					yield return val;
				}
				yield break;
			}
		}

		// Token: 0x06005BA4 RID: 23460 RVA: 0x002EB0B6 File Offset: 0x002E94B6
		public void SetDebugInput(object key, float value)
		{
			this.debugInputValues[key] = value;
		}

		// Token: 0x06005BA5 RID: 23461 RVA: 0x002EB0C6 File Offset: 0x002E94C6
		public void ClearDebugInputFrom(object key)
		{
			if (this.debugInputValues.ContainsKey(key))
			{
				this.debugInputValues.Remove(key);
			}
		}

		// Token: 0x06005BA6 RID: 23462 RVA: 0x002EB0E8 File Offset: 0x002E94E8
		public void SetViewRectAround(SimpleCurve curve)
		{
			if (curve.PointsCount == 0)
			{
				this.rect = SimpleCurveView.identityRect;
			}
			else
			{
				this.rect.xMin = (from pt in curve.Points
				select pt.Loc.x).Min();
				this.rect.xMax = (from pt in curve.Points
				select pt.Loc.x).Max();
				this.rect.yMin = (from pt in curve.Points
				select pt.Loc.y).Min();
				this.rect.yMax = (from pt in curve.Points
				select pt.Loc.y).Max();
				if (Mathf.Approximately(this.rect.width, 0f))
				{
					this.rect.width = this.rect.xMin * 2f;
				}
				if (Mathf.Approximately(this.rect.height, 0f))
				{
					this.rect.height = this.rect.yMin * 2f;
				}
				if (Mathf.Approximately(this.rect.width, 0f))
				{
					this.rect.width = 1f;
				}
				if (Mathf.Approximately(this.rect.height, 0f))
				{
					this.rect.height = 1f;
				}
				float width = this.rect.width;
				float height = this.rect.height;
				this.rect.xMin = this.rect.xMin - width * 0.1f;
				this.rect.xMax = this.rect.xMax + width * 0.1f;
				this.rect.yMin = this.rect.yMin - height * 0.1f;
				this.rect.yMax = this.rect.yMax + height * 0.1f;
			}
		}

		// Token: 0x04003CB0 RID: 15536
		public Rect rect;

		// Token: 0x04003CB1 RID: 15537
		private Dictionary<object, float> debugInputValues = new Dictionary<object, float>();

		// Token: 0x04003CB2 RID: 15538
		private const float ResetZoomBuffer = 0.1f;

		// Token: 0x04003CB3 RID: 15539
		private static Rect identityRect = new Rect(0f, 0f, 1f, 1f);
	}
}
