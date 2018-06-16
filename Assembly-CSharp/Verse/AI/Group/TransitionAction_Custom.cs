using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A01 RID: 2561
	public class TransitionAction_Custom : TransitionAction
	{
		// Token: 0x06003971 RID: 14705 RVA: 0x001E752B File Offset: 0x001E592B
		public TransitionAction_Custom(Action action)
		{
			this.action = action;
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x001E753B File Offset: 0x001E593B
		public TransitionAction_Custom(Action<Transition> actionWithArg)
		{
			this.actionWithArg = actionWithArg;
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x001E754B File Offset: 0x001E594B
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

		// Token: 0x0400248C RID: 9356
		public Action action;

		// Token: 0x0400248D RID: 9357
		public Action<Transition> actionWithArg;
	}
}
