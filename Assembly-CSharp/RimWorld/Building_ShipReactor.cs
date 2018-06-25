using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068C RID: 1676
	internal class Building_ShipReactor : Building
	{
		// Token: 0x06002378 RID: 9080 RVA: 0x001311FC File Offset: 0x0012F5FC
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
