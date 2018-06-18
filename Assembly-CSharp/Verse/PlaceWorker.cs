using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C67 RID: 3175
	public abstract class PlaceWorker
	{
		// Token: 0x060045BE RID: 17854 RVA: 0x0024C38C File Offset: 0x0024A78C
		public virtual AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x0024C3A6 File Offset: 0x0024A7A6
		public virtual void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x0024C3A9 File Offset: 0x0024A7A9
		public virtual void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x0024C3AC File Offset: 0x0024A7AC
		public virtual bool ForceAllowPlaceOver(BuildableDef other)
		{
			return false;
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x0024C3C4 File Offset: 0x0024A7C4
		public virtual IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield break;
		}
	}
}
