using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088B RID: 2187
	public class PawnColumnWorker_LifeStage : PawnColumnWorker_Icon
	{
		// Token: 0x060031E1 RID: 12769 RVA: 0x001AEBEC File Offset: 0x001ACFEC
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStageRace.GetIcon(pawn);
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x001AEC14 File Offset: 0x001AD014
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStage.LabelCap;
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x001AEC3C File Offset: 0x001AD03C
		protected override Vector2 GetIconSize(Pawn pawn)
		{
			return new Vector2(24f, 24f);
		}
	}
}
