using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x0200085F RID: 2143
	public abstract class InspectTabBase
	{
		// Token: 0x04001A53 RID: 6739
		public string labelKey;

		// Token: 0x04001A54 RID: 6740
		protected Vector2 size;

		// Token: 0x04001A55 RID: 6741
		public string tutorTag;

		// Token: 0x04001A56 RID: 6742
		private string cachedTutorHighlightTagClosed = null;

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x0600309F RID: 12447
		protected abstract float PaneTopY { get; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060030A0 RID: 12448
		protected abstract bool StillValid { get; }

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060030A1 RID: 12449 RVA: 0x001A13E0 File Offset: 0x0019F7E0
		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060030A2 RID: 12450 RVA: 0x001A13F8 File Offset: 0x0019F7F8
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

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x060030A3 RID: 12451 RVA: 0x001A144C File Offset: 0x0019F84C
		protected Rect TabRect
		{
			get
			{
				this.UpdateSize();
				float y = this.PaneTopY - 30f - this.size.y;
				return new Rect(0f, y, this.size.x, this.size.y);
			}
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x001A14A4 File Offset: 0x0019F8A4
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

		// Token: 0x060030A5 RID: 12453
		protected abstract void CloseTab();

		// Token: 0x060030A6 RID: 12454
		protected abstract void FillTab();

		// Token: 0x060030A7 RID: 12455 RVA: 0x001A1509 File Offset: 0x0019F909
		protected virtual void ExtraOnGUI()
		{
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x001A150C File Offset: 0x0019F90C
		protected virtual void UpdateSize()
		{
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x001A150F File Offset: 0x0019F90F
		public virtual void OnOpen()
		{
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x001A1512 File Offset: 0x0019F912
		public virtual void TabTick()
		{
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x001A1515 File Offset: 0x0019F915
		public virtual void TabUpdate()
		{
		}
	}
}
