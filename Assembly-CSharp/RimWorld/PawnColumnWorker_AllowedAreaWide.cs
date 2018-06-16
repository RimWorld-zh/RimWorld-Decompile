using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000891 RID: 2193
	public class PawnColumnWorker_AllowedAreaWide : PawnColumnWorker_AllowedArea
	{
		// Token: 0x06003206 RID: 12806 RVA: 0x001AF1C8 File Offset: 0x001AD5C8
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(350, this.GetMinWidth(table), this.GetMaxWidth(table));
		}
	}
}
