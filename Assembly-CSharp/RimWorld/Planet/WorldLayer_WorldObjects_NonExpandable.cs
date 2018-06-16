using System;

namespace RimWorld.Planet
{
	// Token: 0x020005A0 RID: 1440
	public class WorldLayer_WorldObjects_NonExpandable : WorldLayer_WorldObjects
	{
		// Token: 0x06001B77 RID: 7031 RVA: 0x000ECCA4 File Offset: 0x000EB0A4
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return worldObject.def.expandingIcon;
		}
	}
}
