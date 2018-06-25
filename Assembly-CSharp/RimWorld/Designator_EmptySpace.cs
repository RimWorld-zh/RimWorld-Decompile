using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CC RID: 1996
	public class Designator_EmptySpace : Designator
	{
		// Token: 0x06002C3D RID: 11325 RVA: 0x00175BA0 File Offset: 0x00173FA0
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x00175BBB File Offset: 0x00173FBB
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x00175BC3 File Offset: 0x00173FC3
		public override void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}
	}
}
