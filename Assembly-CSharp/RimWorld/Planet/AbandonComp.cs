using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200061A RID: 1562
	public class AbandonComp : WorldObjectComp
	{
		// Token: 0x06001FB7 RID: 8119 RVA: 0x00111B14 File Offset: 0x0010FF14
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
