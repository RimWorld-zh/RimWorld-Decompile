using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF7 RID: 3831
	public class SimpleCurveView
	{
		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06005B7B RID: 23419 RVA: 0x002E9058 File Offset: 0x002E7458
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

		// Token: 0x06005B7C RID: 23420 RVA: 0x002E9082 File Offset: 0x002E7482
		public void SetDebugInput(object key, float value)
		{
			this.debugInputValues[key] = value;
		}

		// Token: 0x06005B7D RID: 23421 RVA: 0x002E9092 File Offset: 0x002E7492
		public void ClearDebugInputFrom(object key)
		{
			if (this.debugInputValues.ContainsKey(key))
			{
				this.debugInputValues.Remove(key);
			}
		}

		// Token: 0x06005B7E RID: 23422 RVA: 0x002E90B4 File Offset: 0x002E74B4
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

		// Token: 0x04003C9D RID: 15517
		public Rect rect;

		// Token: 0x04003C9E RID: 15518
		private Dictionary<object, float> debugInputValues = new Dictionary<object, float>();

		// Token: 0x04003C9F RID: 15519
		private const float ResetZoomBuffer = 0.1f;

		// Token: 0x04003CA0 RID: 15520
		private static Rect identityRect = new Rect(0f, 0f, 1f, 1f);
	}
}
