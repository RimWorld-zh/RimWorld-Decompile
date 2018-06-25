using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000280 RID: 640
	public class WorldObjectCompProperties_FormCaravan : WorldObjectCompProperties
	{
		// Token: 0x06000AEC RID: 2796 RVA: 0x00062E60 File Offset: 0x00061260
		public WorldObjectCompProperties_FormCaravan()
		{
			this.compClass = typeof(FormCaravanComp);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00062E7C File Offset: 0x0006127C
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
