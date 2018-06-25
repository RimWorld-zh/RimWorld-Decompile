using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A04 RID: 2564
	public class TransitionAction_WakeAll : TransitionAction
	{
		// Token: 0x0600397F RID: 14719 RVA: 0x001E7F5C File Offset: 0x001E635C
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
