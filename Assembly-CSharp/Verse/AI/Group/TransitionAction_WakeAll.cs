using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	public class TransitionAction_WakeAll : TransitionAction
	{
		public TransitionAction_WakeAll()
		{
		}

		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				RestUtility.WakeUp(ownedPawns[i]);
			}
		}
	}
}
