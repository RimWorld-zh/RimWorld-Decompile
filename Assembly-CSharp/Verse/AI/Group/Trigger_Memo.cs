using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A29 RID: 2601
	public class Trigger_Memo : Trigger
	{
		// Token: 0x040024C4 RID: 9412
		private string memo;

		// Token: 0x060039D0 RID: 14800 RVA: 0x001E9017 File Offset: 0x001E7417
		public Trigger_Memo(string memo)
		{
			this.memo = memo;
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x001E9028 File Offset: 0x001E7428
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Memo && signal.memo == this.memo;
		}
	}
}
