using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000636 RID: 1590
	public class ScenPart_Naked : ScenPart_PawnModifier
	{
		// Token: 0x060020CA RID: 8394 RVA: 0x00118850 File Offset: 0x00116C50
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsAreNaked".Translate(new object[]
			{
				this.context.ToStringHuman()
			}).CapitalizeFirst();
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00118888 File Offset: 0x00116C88
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x001188A4 File Offset: 0x00116CA4
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}
	}
}
