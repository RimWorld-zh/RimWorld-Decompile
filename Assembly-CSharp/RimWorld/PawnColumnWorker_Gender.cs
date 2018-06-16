using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000888 RID: 2184
	public class PawnColumnWorker_Gender : PawnColumnWorker_Icon
	{
		// Token: 0x060031CB RID: 12747 RVA: 0x001AEB34 File Offset: 0x001ACF34
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.gender.GetIcon();
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x001AEB54 File Offset: 0x001ACF54
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.gender.GetLabel().CapitalizeFirst();
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x001AEB7C File Offset: 0x001ACF7C
		protected override Vector2 GetIconSize(Pawn pawn)
		{
			return new Vector2(24f, 24f);
		}
	}
}
