using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002AE RID: 686
	public class MainButtonDef : Def
	{
		// Token: 0x04000676 RID: 1654
		public Type workerClass = typeof(MainButtonWorker_ToggleTab);

		// Token: 0x04000677 RID: 1655
		public Type tabWindowClass;

		// Token: 0x04000678 RID: 1656
		public bool buttonVisible = true;

		// Token: 0x04000679 RID: 1657
		public int order;

		// Token: 0x0400067A RID: 1658
		public KeyCode defaultHotKey = KeyCode.None;

		// Token: 0x0400067B RID: 1659
		public bool canBeTutorDenied = true;

		// Token: 0x0400067C RID: 1660
		public bool validWithoutMap;

		// Token: 0x0400067D RID: 1661
		[Unsaved]
		public KeyBindingDef hotKey;

		// Token: 0x0400067E RID: 1662
		[Unsaved]
		public string cachedTutorTag;

		// Token: 0x0400067F RID: 1663
		[Unsaved]
		public string cachedHighlightTagClosed;

		// Token: 0x04000680 RID: 1664
		[Unsaved]
		private MainButtonWorker workerInt;

		// Token: 0x04000681 RID: 1665
		[Unsaved]
		private MainTabWindow tabWindowInt;

		// Token: 0x04000682 RID: 1666
		[Unsaved]
		private string cachedShortenedLabelCap;

		// Token: 0x04000683 RID: 1667
		[Unsaved]
		private float cachedLabelCapWidth = -1f;

		// Token: 0x04000684 RID: 1668
		[Unsaved]
		private float cachedShortenedLabelCapWidth = -1f;

		// Token: 0x04000685 RID: 1669
		public const int ButtonHeight = 35;

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x000673D0 File Offset: 0x000657D0
		public MainButtonWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (MainButtonWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0006741C File Offset: 0x0006581C
		public MainTabWindow TabWindow
		{
			get
			{
				if (this.tabWindowInt == null && this.tabWindowClass != null)
				{
					this.tabWindowInt = (MainTabWindow)Activator.CreateInstance(this.tabWindowClass);
					this.tabWindowInt.def = this;
				}
				return this.tabWindowInt;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x00067474 File Offset: 0x00065874
		public string ShortenedLabelCap
		{
			get
			{
				if (this.cachedShortenedLabelCap == null)
				{
					this.cachedShortenedLabelCap = base.LabelCap.Shorten();
				}
				return this.cachedShortenedLabelCap;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x000674AC File Offset: 0x000658AC
		public float LabelCapWidth
		{
			get
			{
				if (this.cachedLabelCapWidth < 0f)
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					this.cachedLabelCapWidth = Text.CalcSize(base.LabelCap).x;
					Text.Font = font;
				}
				return this.cachedLabelCapWidth;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x00067504 File Offset: 0x00065904
		public float ShortenedLabelCapWidth
		{
			get
			{
				if (this.cachedShortenedLabelCapWidth < 0f)
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					this.cachedShortenedLabelCapWidth = Text.CalcSize(this.ShortenedLabelCap).x;
					Text.Font = font;
				}
				return this.cachedShortenedLabelCapWidth;
			}
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0006755C File Offset: 0x0006595C
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightTagClosed = "MainTab-" + this.defName + "-Closed";
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00067580 File Offset: 0x00065980
		public void Notify_SwitchedMap()
		{
			if (this.tabWindowInt != null)
			{
				Find.WindowStack.TryRemove(this.tabWindowInt, true);
				this.tabWindowInt = null;
			}
		}
	}
}
