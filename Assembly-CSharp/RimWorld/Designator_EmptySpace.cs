using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CC RID: 1996
	public class Designator_EmptySpace : Designator
	{
		// Token: 0x06002C3C RID: 11324 RVA: 0x00175E04 File Offset: 0x00174204
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x00175E1F File Offset: 0x0017421F
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x00175E27 File Offset: 0x00174227
		public override void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}
	}
}
