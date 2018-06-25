using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000B3A RID: 2874
	public class DutyDef : Def
	{
		// Token: 0x0400294B RID: 10571
		public ThinkNode thinkNode;

		// Token: 0x0400294C RID: 10572
		public ThinkNode constantThinkNode;

		// Token: 0x0400294D RID: 10573
		public bool alwaysShowWeapon = false;

		// Token: 0x0400294E RID: 10574
		public ThinkTreeDutyHook hook = ThinkTreeDutyHook.HighPriority;

		// Token: 0x0400294F RID: 10575
		public RandomSocialMode socialModeMax = RandomSocialMode.SuperActive;

		// Token: 0x04002950 RID: 10576
		public bool threatDisabled;
	}
}
