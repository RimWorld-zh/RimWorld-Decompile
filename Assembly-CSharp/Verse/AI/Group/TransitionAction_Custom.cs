using System;

namespace Verse.AI.Group
{
	// Token: 0x020009FD RID: 2557
	public class TransitionAction_Custom : TransitionAction
	{
		// Token: 0x04002487 RID: 9351
		public Action action;

		// Token: 0x04002488 RID: 9352
		public Action<Transition> actionWithArg;

		// Token: 0x0600396D RID: 14701 RVA: 0x001E783F File Offset: 0x001E5C3F
		public TransitionAction_Custom(Action action)
		{
			this.action = action;
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x001E784F File Offset: 0x001E5C4F
		public TransitionAction_Custom(Action<Transition> actionWithArg)
		{
			this.actionWithArg = actionWithArg;
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x001E785F File Offset: 0x001E5C5F
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
