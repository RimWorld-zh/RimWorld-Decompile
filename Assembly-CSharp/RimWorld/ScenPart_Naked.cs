using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000636 RID: 1590
	public class ScenPart_Naked : ScenPart_PawnModifier
	{
		// Token: 0x060020C8 RID: 8392 RVA: 0x001187D8 File Offset: 0x00116BD8
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsAreNaked".Translate(new object[]
			{
				this.context.ToStringHuman()
			}).CapitalizeFirst();
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x00118810 File Offset: 0x00116C10
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x0011882C File Offset: 0x00116C2C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}
	}
}
