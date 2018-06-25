using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C67 RID: 3175
	public abstract class PlaceWorker
	{
		// Token: 0x060045CA RID: 17866 RVA: 0x0024DB18 File Offset: 0x0024BF18
		public virtual AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x0024DB32 File Offset: 0x0024BF32
		public virtual void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x0024DB35 File Offset: 0x0024BF35
		public virtual void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x0024DB38 File Offset: 0x0024BF38
		public virtual bool ForceAllowPlaceOver(BuildableDef other)
		{
			return false;
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x0024DB50 File Offset: 0x0024BF50
		public virtual IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield break;
		}
	}
}
