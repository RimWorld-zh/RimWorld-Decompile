using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068E RID: 1678
	internal class Building_ShipReactor : Building
	{
		// Token: 0x0600237D RID: 9085 RVA: 0x00130CFC File Offset: 0x0012F0FC
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
