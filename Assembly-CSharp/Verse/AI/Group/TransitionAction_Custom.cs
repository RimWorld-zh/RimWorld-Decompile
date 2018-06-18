using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A01 RID: 2561
	public class TransitionAction_Custom : TransitionAction
	{
		// Token: 0x06003973 RID: 14707 RVA: 0x001E75FF File Offset: 0x001E59FF
		public TransitionAction_Custom(Action action)
		{
			this.action = action;
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x001E760F File Offset: 0x001E5A0F
		public TransitionAction_Custom(Action<Transition> actionWithArg)
		{
			this.actionWithArg = actionWithArg;
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x001E761F File Offset: 0x001E5A1F
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
