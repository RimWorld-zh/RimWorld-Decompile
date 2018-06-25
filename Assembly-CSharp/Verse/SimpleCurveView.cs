using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFB RID: 3835
	public class SimpleCurveView
	{
		// Token: 0x04003CBA RID: 15546
		public Rect rect;

		// Token: 0x04003CBB RID: 15547
		private Dictionary<object, float> debugInputValues = new Dictionary<object, float>();

		// Token: 0x04003CBC RID: 15548
		private const float ResetZoomBuffer = 0.1f;

		// Token: 0x04003CBD RID: 15549
		private static Rect identityRect = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06005BA7 RID: 23463 RVA: 0x002EB3E0 File Offset: 0x002E97E0
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

		// Token: 0x06005BA8 RID: 23464 RVA: 0x002EB40A File Offset: 0x002E980A
		public void SetDebugInput(object key, float value)
		{
			this.debugInputValues[key] = value;
		}

		// Token: 0x06005BA9 RID: 23465 RVA: 0x002EB41A File Offset: 0x002E981A
		public void ClearDebugInputFrom(object key)
		{
			if (this.debugInputValues.ContainsKey(key))
			{
				this.debugInputValues.Remove(key);
			}
		}

		// Token: 0x06005BAA RID: 23466 RVA: 0x002EB43C File Offset: 0x002E983C
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
	}
}
