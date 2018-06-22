using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000618 RID: 1560
	public class AbandonComp : WorldObjectComp
	{
		// Token: 0x06001FB3 RID: 8115 RVA: 0x001119C4 File Offset: 0x0010FDC4
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
