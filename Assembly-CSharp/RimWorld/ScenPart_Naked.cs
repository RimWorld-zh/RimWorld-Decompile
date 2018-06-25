using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000634 RID: 1588
	public class ScenPart_Naked : ScenPart_PawnModifier
	{
		// Token: 0x060020C6 RID: 8390 RVA: 0x00118A4C File Offset: 0x00116E4C
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsAreNaked".Translate(new object[]
			{
				this.context.ToStringHuman()
			}).CapitalizeFirst();
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x00118A84 File Offset: 0x00116E84
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x00118AA0 File Offset: 0x00116EA0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}
	}
}
