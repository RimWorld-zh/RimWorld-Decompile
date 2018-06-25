using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A00 RID: 2560
	public class TransitionAction_Custom : TransitionAction
	{
		// Token: 0x04002498 RID: 9368
		public Action action;

		// Token: 0x04002499 RID: 9369
		public Action<Transition> actionWithArg;

		// Token: 0x06003972 RID: 14706 RVA: 0x001E7C97 File Offset: 0x001E6097
		public TransitionAction_Custom(Action action)
		{
			this.action = action;
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x001E7CA7 File Offset: 0x001E60A7
		public TransitionAction_Custom(Action<Transition> actionWithArg)
		{
			this.actionWithArg = actionWithArg;
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x001E7CB7 File Offset: 0x001E60B7
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
