using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200061A RID: 1562
	public class AbandonComp : WorldObjectComp
	{
		// Token: 0x06001FB6 RID: 8118 RVA: 0x00111D7C File Offset: 0x0011017C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent mapParent = this.parent as MapParent;
			if (mapParent.HasMap && mapParent.Faction == Faction.OfPlayer)
			{
				yield return SettlementAbandonUtility.AbandonCommand(mapParent);
			}
			yield break;
		}
	}
}
