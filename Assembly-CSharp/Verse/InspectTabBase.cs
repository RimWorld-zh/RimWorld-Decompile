using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	public abstract class InspectTabBase
	{
		public string labelKey;

		protected Vector2 size;

		public string tutorTag;

		private string cachedTutorHighlightTagClosed = null;

		protected InspectTabBase()
		{
		}

		protected abstract float PaneTopY { get; }

		protected abstract bool StillValid { get; }

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
					result = null;
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
				float y = this.PaneTopY - 30f - this.size.y;
				return new Rect(0f, y, this.size.x, this.size.y);
			}
		}

		public void DoTabGUI()
		{
			Profiler.BeginSample("Inspect tab GUI");
			Rect rect = this.TabRect;
			Find.WindowStack.ImmediateWindow(235086, rect, WindowLayer.GameUI, delegate
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
						Log.ErrorOnce(string.Concat(new object[]
						{
							"Exception filling tab ",
							this.GetType(),
							": ",
							ex
						}), 49827, false);
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

		public virtual void Notify_ClearingAllMapsMemory()
		{
		}

		[CompilerGenerated]
		private sealed class <DoTabGUI>c__AnonStorey0
		{
			internal Rect rect;

			internal InspectTabBase $this;

			public <DoTabGUI>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				if (this.$this.StillValid && this.$this.IsVisible)
				{
					if (Widgets.CloseButtonFor(this.rect.AtZero()))
					{
						this.$this.CloseTab();
					}
					try
					{
						this.$this.FillTab();
					}
					catch (Exception ex)
					{
						Log.ErrorOnce(string.Concat(new object[]
						{
							"Exception filling tab ",
							this.$this.GetType(),
							": ",
							ex
						}), 49827, false);
					}
				}
			}
		}
	}
}
