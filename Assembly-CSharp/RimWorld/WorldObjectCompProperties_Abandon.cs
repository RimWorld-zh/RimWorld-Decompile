using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000278 RID: 632
	public class WorldObjectCompProperties_Abandon : WorldObjectCompProperties
	{
		// Token: 0x06000ADC RID: 2780 RVA: 0x000624B0 File Offset: 0x000608B0
		public WorldObjectCompProperties_Abandon()
		{
			this.compClass = typeof(AbandonComp);
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x000624CC File Offset: 0x000608CC
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
