using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E78 RID: 3704
	public abstract class LetterWithTimeout : Letter
	{
		// Token: 0x040039DA RID: 14810
		public int disappearAtTick = -1;

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06005748 RID: 22344 RVA: 0x001A09AC File Offset: 0x0019EDAC
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06005749 RID: 22345 RVA: 0x001A09D0 File Offset: 0x0019EDD0
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x0600574A RID: 22346 RVA: 0x001A0A08 File Offset: 0x0019EE08
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x001A0A42 File Offset: 0x0019EE42
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", 0, false);
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x001A0A5D File Offset: 0x0019EE5D
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x001A0A74 File Offset: 0x0019EE74
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
	}
}
