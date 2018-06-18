using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000A09 RID: 2569
	public class TransitionAction_CheckForJobOverride : TransitionAction
	{
		// Token: 0x06003989 RID: 14729 RVA: 0x001E7A94 File Offset: 0x001E5E94
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (ownedPawns[i].CurJob != null)
				{
					ownedPawns[i].jobs.CheckForJobOverride();
				}
			}
		}
	}
}
