using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A26 RID: 2598
	public class Trigger_Memo : Trigger
	{
		// Token: 0x060039CB RID: 14795 RVA: 0x001E8BBF File Offset: 0x001E6FBF
		public Trigger_Memo(string memo)
		{
			this.memo = memo;
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x001E8BD0 File Offset: 0x001E6FD0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Memo && signal.memo == this.memo;
		}

		// Token: 0x040024B3 RID: 9395
		private string memo;
	}
}
