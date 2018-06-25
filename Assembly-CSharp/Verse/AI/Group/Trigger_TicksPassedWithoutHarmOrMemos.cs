using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI.Group
{
	// Token: 0x02000A14 RID: 2580
	public class Trigger_TicksPassedWithoutHarmOrMemos : Trigger_TicksPassed
	{
		// Token: 0x040024A6 RID: 9382
		private List<string> memos;

		// Token: 0x060039A5 RID: 14757 RVA: 0x001E8374 File Offset: 0x001E6774
		public Trigger_TicksPassedWithoutHarmOrMemos(int tickLimit, params string[] memos) : base(tickLimit)
		{
			this.memos = memos.ToList<string>();
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x001E838C File Offset: 0x001E678C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal) || this.memos.Contains(signal.memo))
			{
				base.Data.ticksPassed = 0;
			}
			return base.ActivateOn(lord, signal);
		}
	}
}
