using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000889 RID: 2185
	public class PawnColumnWorker_LifeStage : PawnColumnWorker_Icon
	{
		// Token: 0x060031DF RID: 12767 RVA: 0x001AF244 File Offset: 0x001AD644
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStageRace.GetIcon(pawn);
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x001AF26C File Offset: 0x001AD66C
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStage.LabelCap;
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x001AF294 File Offset: 0x001AD694
		protected override Vector2 GetIconSize(Pawn pawn)
		{
			return new Vector2(24f, 24f);
		}
	}
}
