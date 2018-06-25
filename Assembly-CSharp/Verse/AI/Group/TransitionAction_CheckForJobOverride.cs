using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000A08 RID: 2568
	public class TransitionAction_CheckForJobOverride : TransitionAction
	{
		// Token: 0x06003988 RID: 14728 RVA: 0x001E812C File Offset: 0x001E652C
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
