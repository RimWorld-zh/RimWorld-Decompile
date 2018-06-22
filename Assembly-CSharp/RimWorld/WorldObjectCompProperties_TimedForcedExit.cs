using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000281 RID: 641
	public class WorldObjectCompProperties_TimedForcedExit : WorldObjectCompProperties
	{
		// Token: 0x06000AED RID: 2797 RVA: 0x00062FEE File Offset: 0x000613EE
		public WorldObjectCompProperties_TimedForcedExit()
		{
			this.compClass = typeof(TimedForcedExit);
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x00063008 File Offset: 0x00061408
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return e;
			}
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_TimedForcedExit but it's not MapParent.";
			}
			yield break;
		}
	}
}
