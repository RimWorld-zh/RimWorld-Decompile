using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000888 RID: 2184
	public class PawnColumnWorker_Gender : PawnColumnWorker_Icon
	{
		// Token: 0x060031CD RID: 12749 RVA: 0x001AEBFC File Offset: 0x001ACFFC
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.gender.GetIcon();
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x001AEC1C File Offset: 0x001AD01C
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.gender.GetLabel().CapitalizeFirst();
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x001AEC44 File Offset: 0x001AD044
		protected override Vector2 GetIconSize(Pawn pawn)
		{
			return new Vector2(24f, 24f);
		}
	}
}
