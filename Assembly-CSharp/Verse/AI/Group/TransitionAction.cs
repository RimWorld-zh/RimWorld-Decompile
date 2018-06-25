using System;

namespace Verse.AI.Group
{
	public abstract class TransitionAction
	{
		protected TransitionAction()
		{
		}

		public abstract void DoAction(Transition trans);
	}
}
