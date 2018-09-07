using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public class SimpleCurveView
	{
		public Rect rect;

		private Dictionary<object, float> debugInputValues = new Dictionary<object, float>();

		private const float ResetZoomBuffer = 0.1f;

		private static Rect identityRect = new Rect(0f, 0f, 1f, 1f);

		[CompilerGenerated]
		private static Func<CurvePoint, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<CurvePoint, float> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<CurvePoint, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<CurvePoint, float> <>f__am$cache3;

		public SimpleCurveView()
		{
		}

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
			if (curve.PointsCount == 0)
			{
				this.rect = SimpleCurveView.identityRect;
				return;
			}
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

		// Note: this type is marked as 'beforefieldinit'.
		static SimpleCurveView()
		{
		}

		[CompilerGenerated]
		private static float <SetViewRectAround>m__0(CurvePoint pt)
		{
			return pt.Loc.x;
		}

		[CompilerGenerated]
		private static float <SetViewRectAround>m__1(CurvePoint pt)
		{
			return pt.Loc.x;
		}

		[CompilerGenerated]
		private static float <SetViewRectAround>m__2(CurvePoint pt)
		{
			return pt.Loc.y;
		}

		[CompilerGenerated]
		private static float <SetViewRectAround>m__3(CurvePoint pt)
		{
			return pt.Loc.y;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<float>, IEnumerator, IDisposable, IEnumerator<float>
		{
			internal Dictionary<object, float>.ValueCollection.Enumerator $locvar0;

			internal float <val>__1;

			internal SimpleCurveView $this;

			internal float $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (this.debugInputValues == null)
					{
						return false;
					}
					enumerator = this.debugInputValues.Values.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						val = enumerator.Current;
						this.$current = val;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			float IEnumerator<float>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<float>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<float> IEnumerable<float>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				SimpleCurveView.<>c__Iterator0 <>c__Iterator = new SimpleCurveView.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}
	}
}
