using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000283 RID: 643
	public class WorldObjectCompProperties_TimedForcedExit : WorldObjectCompProperties
	{
		// Token: 0x06000AF0 RID: 2800 RVA: 0x0006313A File Offset: 0x0006153A
		public WorldObjectCompProperties_TimedForcedExit()
		{
			this.compClass = typeof(TimedForcedExit);
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00063154 File Offset: 0x00061554
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
