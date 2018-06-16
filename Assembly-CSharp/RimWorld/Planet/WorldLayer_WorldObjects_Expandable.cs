using System;

namespace RimWorld.Planet
{
	// Token: 0x0200059E RID: 1438
	public class WorldLayer_WorldObjects_Expandable : WorldLayer_WorldObjects
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001B69 RID: 7017 RVA: 0x000EC7F0 File Offset: 0x000EABF0
		protected override float Alpha
		{
			get
			{
				return 1f - ExpandableWorldObjectsUtility.TransitionPct;
			}
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x000EC810 File Offset: 0x000EAC10
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return !worldObject.def.expandingIcon;
		}
	}
}
