using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C64 RID: 3172
	public abstract class PlaceWorker
	{
		// Token: 0x060045C7 RID: 17863 RVA: 0x0024D75C File Offset: 0x0024BB5C
		public virtual AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x0024D776 File Offset: 0x0024BB76
		public virtual void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x0024D779 File Offset: 0x0024BB79
		public virtual void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x0024D77C File Offset: 0x0024BB7C
		public virtual bool ForceAllowPlaceOver(BuildableDef other)
		{
			return false;
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x0024D794 File Offset: 0x0024BB94
		public virtual IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield break;
		}
	}
}
