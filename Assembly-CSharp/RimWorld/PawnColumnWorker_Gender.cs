using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000884 RID: 2180
	public class PawnColumnWorker_Gender : PawnColumnWorker_Icon
	{
		// Token: 0x060031C6 RID: 12742 RVA: 0x001AEDE4 File Offset: 0x001AD1E4
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.gender.GetIcon();
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x001AEE04 File Offset: 0x001AD204
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.gender.GetLabel().CapitalizeFirst();
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x001AEE2C File Offset: 0x001AD22C
		protected override Vector2 GetIconSize(Pawn pawn)
		{
			return new Vector2(24f, 24f);
		}
	}
}
