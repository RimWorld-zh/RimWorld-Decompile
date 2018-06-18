using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A2A RID: 2602
	public class Trigger_Memo : Trigger
	{
		// Token: 0x060039D1 RID: 14801 RVA: 0x001E897F File Offset: 0x001E6D7F
		public Trigger_Memo(string memo)
		{
			this.memo = memo;
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x001E8990 File Offset: 0x001E6D90
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Memo && signal.memo == this.memo;
		}

		// Token: 0x040024B8 RID: 9400
		private string memo;
	}
}
