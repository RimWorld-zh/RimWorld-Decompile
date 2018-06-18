using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E76 RID: 3702
	public abstract class LetterWithTimeout : Letter
	{
		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06005724 RID: 22308 RVA: 0x001A0414 File Offset: 0x0019E814
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06005725 RID: 22309 RVA: 0x001A0438 File Offset: 0x0019E838
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06005726 RID: 22310 RVA: 0x001A0470 File Offset: 0x0019E870
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x001A04AA File Offset: 0x0019E8AA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", 0, false);
		}

		// Token: 0x06005728 RID: 22312 RVA: 0x001A04C5 File Offset: 0x0019E8C5
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x001A04DC File Offset: 0x0019E8DC
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

		// Token: 0x040039C2 RID: 14786
		public int disappearAtTick = -1;
	}
}
