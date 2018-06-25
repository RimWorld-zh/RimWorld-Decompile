using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FC RID: 1532
	public class DestroyedFactionBase : MapParent
	{
		// Token: 0x06001E7C RID: 7804 RVA: 0x00108FC4 File Offset: 0x001073C4
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

		// Token: 0x06001E7D RID: 7805 RVA: 0x00109000 File Offset: 0x00107400
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
