using System;

namespace Verse.AI.Group
{
	// Token: 0x020009FF RID: 2559
	public class TransitionAction_Custom : TransitionAction
	{
		// Token: 0x04002488 RID: 9352
		public Action action;

		// Token: 0x04002489 RID: 9353
		public Action<Transition> actionWithArg;

		// Token: 0x06003971 RID: 14705 RVA: 0x001E796B File Offset: 0x001E5D6B
		public TransitionAction_Custom(Action action)
		{
			this.action = action;
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x001E797B File Offset: 0x001E5D7B
		public TransitionAction_Custom(Action<Transition> actionWithArg)
		{
			this.actionWithArg = actionWithArg;
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x001E798B File Offset: 0x001E5D8B
		public override void DoAction(Transition trans)
		{
			if (this.actionWithArg != null)
			{
				this.actionWithArg(trans);
			}
			if (this.action != null)
			{
				this.action();
			}
		}
	}
}
