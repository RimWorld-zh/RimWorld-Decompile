using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public abstract class Gizmo
	{
		public bool disabled;

		public string disabledReason;

		public bool alsoClickIfOtherInGroupClicked = true;

		public float order = 0f;

		public const float Height = 75f;

		protected Gizmo()
		{
		}

		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				yield break;
			}
		}

		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth);

		public virtual void GizmoUpdateOnMouseover()
		{
		}

		public abstract float GetWidth(float maxWidth);

		public virtual void ProcessInput(Event ev)
		{
		}

		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		public virtual void MergeWith(Gizmo other)
		{
		}

		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
				}
				return false;
			}

			FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new Gizmo.<>c__Iterator0();
			}
		}
	}
}
