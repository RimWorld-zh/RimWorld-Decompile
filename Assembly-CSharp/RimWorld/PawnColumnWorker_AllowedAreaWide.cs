using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200088F RID: 2191
	public class PawnColumnWorker_AllowedAreaWide : PawnColumnWorker_AllowedArea
	{
		// Token: 0x06003204 RID: 12804 RVA: 0x001AF820 File Offset: 0x001ADC20
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(350, this.GetMinWidth(table), this.GetMaxWidth(table));
		}
	}
}
