using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A05 RID: 2565
	public class TransitionAction_WakeAll : TransitionAction
	{
		// Token: 0x0600397E RID: 14718 RVA: 0x001E77F0 File Offset: 0x001E5BF0
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
