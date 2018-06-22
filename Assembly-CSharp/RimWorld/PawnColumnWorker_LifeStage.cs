using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000887 RID: 2183
	public class PawnColumnWorker_LifeStage : PawnColumnWorker_Icon
	{
		// Token: 0x060031DC RID: 12764 RVA: 0x001AEE9C File Offset: 0x001AD29C
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStageRace.GetIcon(pawn);
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x001AEEC4 File Offset: 0x001AD2C4
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStage.LabelCap;
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x001AEEEC File Offset: 0x001AD2EC
		protected override Vector2 GetIconSize(Pawn pawn)
		{
			return new Vector2(24f, 24f);
		}
	}
}
