using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E77 RID: 3703
	public abstract class LetterWithTimeout : Letter
	{
		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06005726 RID: 22310 RVA: 0x001A034C File Offset: 0x0019E74C
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06005727 RID: 22311 RVA: 0x001A0370 File Offset: 0x0019E770
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06005728 RID: 22312 RVA: 0x001A03A8 File Offset: 0x0019E7A8
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x001A03E2 File Offset: 0x0019E7E2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", 0, false);
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x001A03FD File Offset: 0x0019E7FD
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		// Token: 0x0600572B RID: 22315 RVA: 0x001A0414 File Offset: 0x0019E814
		protected override string PostProcessedLabel()
		{
			string text = base.PostProcessedLabel();
			if (this.TimeoutActive)
			{
				int num = Mathf.RoundToInt((float)(this.disappearAtTick - Find.TickManager.TicksGame) / 2500f);
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					" (",
					num,
					"LetterHour".Translate(),
					")"
				});
			}
			return text;
		}

		// Token: 0x040039C4 RID: 14788
		public int disappearAtTick = -1;
	}
}
