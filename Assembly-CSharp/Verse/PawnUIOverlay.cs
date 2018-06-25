using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE9 RID: 3305
	public class PawnUIOverlay
	{
		// Token: 0x0400314A RID: 12618
		private Pawn pawn;

		// Token: 0x0400314B RID: 12619
		private const float PawnLabelOffsetY = -0.6f;

		// Token: 0x0400314C RID: 12620
		private const int PawnStatBarWidth = 32;

		// Token: 0x0400314D RID: 12621
		private const float ActivityIconSize = 13f;

		// Token: 0x0400314E RID: 12622
		private const float ActivityIconOffsetY = 12f;

		// Token: 0x060048CC RID: 18636 RVA: 0x002639EC File Offset: 0x00261DEC
		public PawnUIOverlay(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x002639FC File Offset: 0x00261DFC
		public void DrawPawnGUIOverlay()
		{
			if (this.pawn.Spawned && !this.pawn.Map.fogGrid.IsFogged(this.pawn.Position))
			{
				if (!this.pawn.RaceProps.Humanlike)
				{
					AnimalNameDisplayMode animalNameMode = Prefs.AnimalNameMode;
					if (animalNameMode == AnimalNameDisplayMode.None)
					{
						return;
					}
					if (animalNameMode != AnimalNameDisplayMode.TameAll)
					{
						if (animalNameMode == AnimalNameDisplayMode.TameNamed)
						{
							if (this.pawn.Name == null || this.pawn.Name.Numerical)
							{
								return;
							}
						}
					}
					else if (this.pawn.Name == null)
					{
						return;
					}
				}
				Vector2 pos = GenMapUI.LabelDrawPosFor(this.pawn, -0.6f);
				GenMapUI.DrawPawnLabel(this.pawn, pos, 1f, 9999f, null, GameFont.Tiny, true, true);
				if (this.pawn.CanTradeNow)
				{
					this.pawn.Map.overlayDrawer.DrawOverlay(this.pawn, OverlayTypes.QuestionMark);
				}
			}
		}
	}
}
