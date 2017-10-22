#define ENABLE_PROFILER
using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	public abstract class InspectTabBase
	{
		public string labelKey;

		protected Vector2 size;

		public string tutorTag;

		private string cachedTutorHighlightTagClosed = (string)null;

		protected abstract float PaneTopY
		{
			get;
		}

		protected abstract bool StillValid
		{
			get;
		}

		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}

		public string TutorHighlightTagClosed
		{
			get
			{
				string result;
				if (this.tutorTag == null)
				{
					result = (string)null;
				}
				else
				{
					if (this.cachedTutorHighlightTagClosed == null)
					{
						this.cachedTutorHighlightTagClosed = "ITab-" + this.tutorTag + "-Closed";
					}
					result = this.cachedTutorHighlightTagClosed;
				}
				return result;
			}
		}

		protected Rect TabRect
		{
			get
			{
				this.UpdateSize();
				float y = (float)(this.PaneTopY - 30.0 - this.size.y);
				return new Rect(0f, y, this.size.x, this.size.y);
			}
		}

		public void DoTabGUI()
		{
			Profiler.BeginSample("Inspect tab GUI");
			Rect rect = this.TabRect;
			Find.WindowStack.ImmediateWindow(235086, rect, WindowLayer.GameUI, (Action)delegate
			{
				if (this.StillValid && this.IsVisible)
				{
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.CloseTab();
					}
					try
					{
						this.FillTab();
					}
					catch (Exception ex)
					{
						Log.ErrorOnce("Exception filling tab " + base.GetType() + ": " + ex, 49827);
					}
				}
			}, true, false, 1f);
			this.ExtraOnGUI();
			Profiler.EndSample();
		}

		protected abstract void CloseTab();

		protected abstract void FillTab();

		protected virtual void ExtraOnGUI()
		{
		}

		protected virtual void UpdateSize()
		{
		}

		public virtual void OnOpen()
		{
		}

		public virtual void TabTick()
		{
		}

		public virtual void TabUpdate()
		{
		}
	}
}
