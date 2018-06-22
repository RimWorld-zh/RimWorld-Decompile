using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A01 RID: 2561
	public class TransitionAction_WakeAll : TransitionAction
	{
		// Token: 0x0600397A RID: 14714 RVA: 0x001E7B04 File Offset: 0x001E5F04
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
