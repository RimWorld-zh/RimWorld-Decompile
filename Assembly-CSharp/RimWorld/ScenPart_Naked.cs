using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000634 RID: 1588
	public class ScenPart_Naked : ScenPart_PawnModifier
	{
		// Token: 0x060020C5 RID: 8389 RVA: 0x00118CB4 File Offset: 0x001170B4
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsAreNaked".Translate(new object[]
			{
				this.context.ToStringHuman()
			}).CapitalizeFirst();
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x00118CEC File Offset: 0x001170EC
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x00118D08 File Offset: 0x00117108
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}
	}
}
