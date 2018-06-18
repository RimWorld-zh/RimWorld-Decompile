using System;

namespace RimWorld.Planet
{
	// Token: 0x0200059E RID: 1438
	public class WorldLayer_WorldObjects_Expandable : WorldLayer_WorldObjects
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001B6A RID: 7018 RVA: 0x000EC85C File Offset: 0x000EAC5C
		protected override float Alpha
		{
			get
			{
				return 1f - ExpandableWorldObjectsUtility.TransitionPct;
			}
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x000EC87C File Offset: 0x000EAC7C
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return !worldObject.def.expandingIcon;
		}
	}
}
