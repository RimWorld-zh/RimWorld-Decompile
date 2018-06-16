using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CE RID: 1998
	public class Designator_EmptySpace : Designator
	{
		// Token: 0x06002C3E RID: 11326 RVA: 0x001757E4 File Offset: 0x00173BE4
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x001757FF File Offset: 0x00173BFF
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x00175807 File Offset: 0x00173C07
		public override void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}
	}
}
