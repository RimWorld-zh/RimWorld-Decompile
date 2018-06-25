using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000861 RID: 2145
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
		// (get) Token: 0x060030A3 RID: 12451
		protected abstract float PaneTopY { get; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060030A4 RID: 12452
		protected abstract bool StillValid { get; }

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060030A5 RID: 12453 RVA: 0x001A1530 File Offset: 0x0019F930
		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060030A6 RID: 12454 RVA: 0x001A1548 File Offset: 0x0019F948
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
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x001A159C File Offset: 0x0019F99C
		protected Rect TabRect
		{
			get
			{
				this.UpdateSize();
				float y = this.PaneTopY - 30f - this.size.y;
				return new Rect(0f, y, this.size.x, this.size.y);
			}
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x001A15F4 File Offset: 0x0019F9F4
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

		// Token: 0x060030A9 RID: 12457
		protected abstract void CloseTab();

		// Token: 0x060030AA RID: 12458
		protected abstract void FillTab();

		// Token: 0x060030AB RID: 12459 RVA: 0x001A1659 File Offset: 0x0019FA59
		protected virtual void ExtraOnGUI()
		{
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x001A165C File Offset: 0x0019FA5C
		protected virtual void UpdateSize()
		{
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x001A165F File Offset: 0x0019FA5F
		public virtual void OnOpen()
		{
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x001A1662 File Offset: 0x0019FA62
		public virtual void TabTick()
		{
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x001A1665 File Offset: 0x0019FA65
		public virtual void TabUpdate()
		{
		}
	}
}
