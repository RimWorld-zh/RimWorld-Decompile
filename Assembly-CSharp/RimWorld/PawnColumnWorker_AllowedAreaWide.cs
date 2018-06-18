using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000891 RID: 2193
	public class PawnColumnWorker_AllowedAreaWide : PawnColumnWorker_AllowedArea
	{
		// Token: 0x06003208 RID: 12808 RVA: 0x001AF290 File Offset: 0x001AD690
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(350, this.GetMinWidth(table), this.GetMaxWidth(table));
		}
	}
}
