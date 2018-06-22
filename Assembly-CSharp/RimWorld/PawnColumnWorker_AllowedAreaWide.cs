using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200088D RID: 2189
	public class PawnColumnWorker_AllowedAreaWide : PawnColumnWorker_AllowedArea
	{
		// Token: 0x06003201 RID: 12801 RVA: 0x001AF478 File Offset: 0x001AD878
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(350, this.GetMinWidth(table), this.GetMaxWidth(table));
		}
	}
}
