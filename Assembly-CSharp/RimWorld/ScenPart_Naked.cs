using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000632 RID: 1586
	public class ScenPart_Naked : ScenPart_PawnModifier
	{
		// Token: 0x060020C2 RID: 8386 RVA: 0x001188FC File Offset: 0x00116CFC
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsAreNaked".Translate(new object[]
			{
				this.context.ToStringHuman()
			}).CapitalizeFirst();
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x00118934 File Offset: 0x00116D34
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x00118950 File Offset: 0x00116D50
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}
	}
}
