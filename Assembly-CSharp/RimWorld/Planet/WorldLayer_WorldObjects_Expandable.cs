using System;

namespace RimWorld.Planet
{
	// Token: 0x0200059A RID: 1434
	public class WorldLayer_WorldObjects_Expandable : WorldLayer_WorldObjects
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001B61 RID: 7009 RVA: 0x000EC8B0 File Offset: 0x000EACB0
		protected override float Alpha
		{
			get
			{
				return 1f - ExpandableWorldObjectsUtility.TransitionPct;
			}
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x000EC8D0 File Offset: 0x000EACD0
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return !worldObject.def.expandingIcon;
		}
	}
}
