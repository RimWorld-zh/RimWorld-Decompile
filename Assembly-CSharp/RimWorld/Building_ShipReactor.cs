using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068A RID: 1674
	internal class Building_ShipReactor : Building
	{
		// Token: 0x06002375 RID: 9077 RVA: 0x00130E44 File Offset: 0x0012F244
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
