using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI.Group
{
	// Token: 0x02000A12 RID: 2578
	public class Trigger_TicksPassedWithoutHarmOrMemos : Trigger_TicksPassed
	{
		// Token: 0x060039A1 RID: 14753 RVA: 0x001E8248 File Offset: 0x001E6648
		public Trigger_TicksPassedWithoutHarmOrMemos(int tickLimit, params string[] memos) : base(tickLimit)
		{
			this.memos = memos.ToList<string>();
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x001E8260 File Offset: 0x001E6660
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal) || this.memos.Contains(signal.memo))
			{
				base.Data.ticksPassed = 0;
			}
			return base.ActivateOn(lord, signal);
		}

		// Token: 0x040024A5 RID: 9381
		private List<string> memos;
	}
}
