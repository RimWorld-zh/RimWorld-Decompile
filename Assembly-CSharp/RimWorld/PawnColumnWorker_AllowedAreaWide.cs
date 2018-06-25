using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200088F RID: 2191
	public class PawnColumnWorker_AllowedAreaWide : PawnColumnWorker_AllowedArea
	{
		// Token: 0x06003205 RID: 12805 RVA: 0x001AF5B8 File Offset: 0x001AD9B8
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(350, this.GetMinWidth(table), this.GetMaxWidth(table));
		}
	}
}
