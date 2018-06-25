using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068C RID: 1676
	internal class Building_ShipReactor : Building
	{
		// Token: 0x06002379 RID: 9081 RVA: 0x00130F94 File Offset: 0x0012F394
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return c;
			}
			foreach (Gizmo c2 in ShipUtility.ShipStartupGizmos(this))
			{
				yield return c2;
			}
			yield break;
		}
	}
}
