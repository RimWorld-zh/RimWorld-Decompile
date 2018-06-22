using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CA RID: 1994
	public class Designator_EmptySpace : Designator
	{
		// Token: 0x06002C39 RID: 11321 RVA: 0x00175A50 File Offset: 0x00173E50
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x00175A6B File Offset: 0x00173E6B
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x00175A73 File Offset: 0x00173E73
		public override void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}
	}
}
