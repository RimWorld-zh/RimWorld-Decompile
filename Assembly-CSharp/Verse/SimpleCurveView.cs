using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class SimpleCurveView
	{
		public Rect rect;

		private Dictionary<object, float> debugInputValues = new Dictionary<object, float>();

		private const float ResetZoomBuffer = 0.1f;

		private static Rect identityRect = new Rect(0f, 0f, 1f, 1f);

		public IEnumerable<float> DebugInputValues
		{
			get
			{
				if (this.debugInputValues != null)
				{
					using (Dictionary<object, float>.ValueCollection.Enumerator enumerator = this.debugInputValues.Values.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							float val = enumerator.Current;
							yield return val;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_00d0:
				/*Error near IL_00d1: Unexpected return in MoveNext()*/;
			}
		}

		public void SetDebugInput(object key, float value)
		{
			this.debugInputValues[key] = value;
		}

		public void ClearDebugInputFrom(object key)
		{
			if (this.debugInputValues.ContainsKey(key))
			{
				this.debugInputValues.Remove(key);
			}
		}

		public void SetViewRectAround(SimpleCurve curve)
		{
			if (!curve.AllPoints.Any())
			{
				this.rect = SimpleCurveView.identityRect;
			}
			else
			{
				this.rect.xMin = curve.AllPoints.Select((Func<CurvePoint, float>)delegate(CurvePoint pt)
				{
					Vector2 loc4 = pt.Loc;
					return loc4.x;
				}).Min();
				this.rect.xMax = curve.AllPoints.Select((Func<CurvePoint, float>)delegate(CurvePoint pt)
				{
					Vector2 loc3 = pt.Loc;
					return loc3.x;
				}).Max();
				this.rect.yMin = curve.AllPoints.Select((Func<CurvePoint, float>)delegate(CurvePoint pt)
				{
					Vector2 loc2 = pt.Loc;
					return loc2.y;
				}).Min();
				this.rect.yMax = curve.AllPoints.Select((Func<CurvePoint, float>)delegate(CurvePoint pt)
				{
					Vector2 loc = pt.Loc;
					return loc.y;
				}).Max();
				if (Mathf.Approximately(this.rect.width, 0f))
				{
					this.rect.width = (float)(this.rect.xMin * 2.0);
				}
				if (Mathf.Approximately(this.rect.height, 0f))
				{
					this.rect.height = (float)(this.rect.yMin * 2.0);
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
				this.rect.xMin -= (float)(width * 0.10000000149011612);
				this.rect.xMax += (float)(width * 0.10000000149011612);
				this.rect.yMin -= (float)(height * 0.10000000149011612);
				this.rect.yMax += (float)(height * 0.10000000149011612);
			}
		}
	}
}
