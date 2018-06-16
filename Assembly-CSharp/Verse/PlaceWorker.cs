using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C68 RID: 3176
	public abstract class PlaceWorker
	{
		// Token: 0x060045C0 RID: 17856 RVA: 0x0024C3B4 File Offset: 0x0024A7B4
		public virtual AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x0024C3CE File Offset: 0x0024A7CE
		public virtual void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x0024C3D1 File Offset: 0x0024A7D1
		public virtual void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x0024C3D4 File Offset: 0x0024A7D4
		public virtual bool ForceAllowPlaceOver(BuildableDef other)
		{
			return false;
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x0024C3EC File Offset: 0x0024A7EC
		public virtual IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield break;
		}
	}
}
