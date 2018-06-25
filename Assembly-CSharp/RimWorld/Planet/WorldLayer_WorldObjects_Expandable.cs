using System;

namespace RimWorld.Planet
{
	// Token: 0x0200059C RID: 1436
	public class WorldLayer_WorldObjects_Expandable : WorldLayer_WorldObjects
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001B65 RID: 7013 RVA: 0x000ECA00 File Offset: 0x000EAE00
		protected override float Alpha
		{
			get
			{
				return 1f - ExpandableWorldObjectsUtility.TransitionPct;
			}
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x000ECA20 File Offset: 0x000EAE20
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return !worldObject.def.expandingIcon;
		}
	}
}
