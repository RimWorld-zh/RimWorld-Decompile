using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000A05 RID: 2565
	public class TransitionAction_CheckForJobOverride : TransitionAction
	{
		// Token: 0x06003983 RID: 14723 RVA: 0x001E7CD4 File Offset: 0x001E60D4
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
