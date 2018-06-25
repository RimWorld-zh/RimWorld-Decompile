using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E77 RID: 3703
	public abstract class LetterWithTimeout : Letter
	{
		// Token: 0x040039D2 RID: 14802
		public int disappearAtTick = -1;

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06005748 RID: 22344 RVA: 0x001A0744 File Offset: 0x0019EB44
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06005749 RID: 22345 RVA: 0x001A0768 File Offset: 0x0019EB68
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x0600574A RID: 22346 RVA: 0x001A07A0 File Offset: 0x0019EBA0
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x001A07DA File Offset: 0x0019EBDA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", 0, false);
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x001A07F5 File Offset: 0x0019EBF5
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x001A080C File Offset: 0x0019EC0C
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
