using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E75 RID: 3701
	public abstract class LetterWithTimeout : Letter
	{
		// Token: 0x040039D2 RID: 14802
		public int disappearAtTick = -1;

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06005744 RID: 22340 RVA: 0x001A05F4 File Offset: 0x0019E9F4
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06005745 RID: 22341 RVA: 0x001A0618 File Offset: 0x0019EA18
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06005746 RID: 22342 RVA: 0x001A0650 File Offset: 0x0019EA50
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x001A068A File Offset: 0x0019EA8A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", 0, false);
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x001A06A5 File Offset: 0x0019EAA5
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x001A06BC File Offset: 0x0019EABC
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
