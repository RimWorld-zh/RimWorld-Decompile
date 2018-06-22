using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FA RID: 1530
	public class DestroyedFactionBase : MapParent
	{
		// Token: 0x06001E78 RID: 7800 RVA: 0x00108E74 File Offset: 0x00107274
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			bool result;
			if (!base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				alsoRemoveWorldObject = true;
				result = true;
			}
			else
			{
				alsoRemoveWorldObject = false;
				result = false;
			}
			return result;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x00108EB0 File Offset: 0x001072B0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (base.HasMap && Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return SettleInExistingMapUtility.SettleCommand(base.Map, false);
			}
			yield break;
		}
	}
}
