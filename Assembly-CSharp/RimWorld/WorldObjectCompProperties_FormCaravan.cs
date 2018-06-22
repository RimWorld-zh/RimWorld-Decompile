using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200027E RID: 638
	public class WorldObjectCompProperties_FormCaravan : WorldObjectCompProperties
	{
		// Token: 0x06000AE8 RID: 2792 RVA: 0x00062D10 File Offset: 0x00061110
		public WorldObjectCompProperties_FormCaravan()
		{
			this.compClass = typeof(FormCaravanComp);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00062D2C File Offset: 0x0006112C
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return e;
			}
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_FormCaravan but it's not MapParent.";
			}
			yield break;
		}
	}
}
