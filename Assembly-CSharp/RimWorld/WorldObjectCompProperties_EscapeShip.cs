using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200027F RID: 639
	public class WorldObjectCompProperties_EscapeShip : WorldObjectCompProperties
	{
		// Token: 0x06000AE8 RID: 2792 RVA: 0x00062BB0 File Offset: 0x00060FB0
		public WorldObjectCompProperties_EscapeShip()
		{
			this.compClass = typeof(EscapeShipComp);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00062BCC File Offset: 0x00060FCC
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
