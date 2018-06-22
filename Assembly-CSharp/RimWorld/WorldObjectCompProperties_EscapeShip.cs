using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200027D RID: 637
	public class WorldObjectCompProperties_EscapeShip : WorldObjectCompProperties
	{
		// Token: 0x06000AE5 RID: 2789 RVA: 0x00062A64 File Offset: 0x00060E64
		public WorldObjectCompProperties_EscapeShip()
		{
			this.compClass = typeof(EscapeShipComp);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00062A80 File Offset: 0x00060E80
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return e;
			}
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_EscapeShip but it's not MapParent.";
			}
			yield break;
		}
	}
}
