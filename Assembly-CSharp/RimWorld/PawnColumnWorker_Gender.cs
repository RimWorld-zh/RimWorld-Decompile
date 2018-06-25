using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000886 RID: 2182
	public class PawnColumnWorker_Gender : PawnColumnWorker_Icon
	{
		// Token: 0x060031C9 RID: 12745 RVA: 0x001AF18C File Offset: 0x001AD58C
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.gender.GetIcon();
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x001AF1AC File Offset: 0x001AD5AC
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.gender.GetLabel().CapitalizeFirst();
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x001AF1D4 File Offset: 0x001AD5D4
		protected override Vector2 GetIconSize(Pawn pawn)
		{
			return new Vector2(24f, 24f);
		}
	}
}
