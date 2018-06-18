using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088B RID: 2187
	public class PawnColumnWorker_LifeStage : PawnColumnWorker_Icon
	{
		// Token: 0x060031E3 RID: 12771 RVA: 0x001AECB4 File Offset: 0x001AD0B4
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStageRace.GetIcon(pawn);
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x001AECDC File Offset: 0x001AD0DC
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStage.LabelCap;
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x001AED04 File Offset: 0x001AD104
		protected override Vector2 GetIconSize(Pawn pawn)
		{
			return new Vector2(24f, 24f);
		}
	}
}
