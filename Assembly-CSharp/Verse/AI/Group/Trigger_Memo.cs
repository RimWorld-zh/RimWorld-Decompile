using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A2A RID: 2602
	public class Trigger_Memo : Trigger
	{
		// Token: 0x060039CF RID: 14799 RVA: 0x001E88AB File Offset: 0x001E6CAB
		public Trigger_Memo(string memo)
		{
			this.memo = memo;
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x001E88BC File Offset: 0x001E6CBC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Memo && signal.memo == this.memo;
		}

		// Token: 0x040024B8 RID: 9400
		private string memo;
	}
}
