using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CE RID: 1998
	public class Designator_EmptySpace : Designator
	{
		// Token: 0x06002C40 RID: 11328 RVA: 0x00175878 File Offset: 0x00173C78
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x00175893 File Offset: 0x00173C93
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x0017589B File Offset: 0x00173C9B
		public override void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}
	}
}
