using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000863 RID: 2147
	public abstract class InspectTabBase
	{
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060030A4 RID: 12452
		protected abstract float PaneTopY { get; }

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x060030A5 RID: 12453
		protected abstract bool StillValid { get; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060030A6 RID: 12454 RVA: 0x001A1138 File Offset: 0x0019F538
		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x001A1150 File Offset: 0x0019F550
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

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060030A8 RID: 12456 RVA: 0x001A11A4 File Offset: 0x0019F5A4
		protected Rect TabRect
		{
			get
			{
				this.UpdateSize();
				float y = this.PaneTopY - 30f - this.size.y;
				return new Rect(0f, y, this.size.x, this.size.y);
			}
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x001A11FC File Offset: 0x0019F5FC
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

		// Token: 0x060030AA RID: 12458
		protected abstract void CloseTab();

		// Token: 0x060030AB RID: 12459
		protected abstract void FillTab();

		// Token: 0x060030AC RID: 12460 RVA: 0x001A1261 File Offset: 0x0019F661
		protected virtual void ExtraOnGUI()
		{
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x001A1264 File Offset: 0x0019F664
		protected virtual void UpdateSize()
		{
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x001A1267 File Offset: 0x0019F667
		public virtual void OnOpen()
		{
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x001A126A File Offset: 0x0019F66A
		public virtual void TabTick()
		{
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x001A126D File Offset: 0x0019F66D
		public virtual void TabUpdate()
		{
		}

		// Token: 0x04001A55 RID: 6741
		public string labelKey;

		// Token: 0x04001A56 RID: 6742
		protected Vector2 size;

		// Token: 0x04001A57 RID: 6743
		public string tutorTag;

		// Token: 0x04001A58 RID: 6744
		private string cachedTutorHighlightTagClosed = null;
	}
}
