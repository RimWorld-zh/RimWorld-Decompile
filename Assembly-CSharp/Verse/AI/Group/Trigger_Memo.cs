using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A28 RID: 2600
	public class Trigger_Memo : Trigger
	{
		// Token: 0x040024B4 RID: 9396
		private string memo;

		// Token: 0x060039CF RID: 14799 RVA: 0x001E8CEB File Offset: 0x001E70EB
		public Trigger_Memo(string memo)
		{
			this.memo = memo;
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x001E8CFC File Offset: 0x001E70FC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Memo && signal.memo == this.memo;
		}
	}
}
