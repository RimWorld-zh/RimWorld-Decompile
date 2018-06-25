using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200027A RID: 634
	public class WorldObjectCompProperties_Abandon : WorldObjectCompProperties
	{
		// Token: 0x06000AE0 RID: 2784 RVA: 0x00062600 File Offset: 0x00060A00
		public WorldObjectCompProperties_Abandon()
		{
			this.compClass = typeof(AbandonComp);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0006261C File Offset: 0x00060A1C
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return e;
			}
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_Abandon but it's not MapParent.";
			}
			yield break;
		}
	}
}
