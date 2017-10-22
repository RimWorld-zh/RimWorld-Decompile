using RimWorld;
using UnityEngine;

namespace Verse
{
	public class PawnUIOverlay
	{
		private Pawn pawn;

		private const float PawnLabelOffsetY = -0.6f;

		private const int PawnStatBarWidth = 32;

		private const float ActivityIconSize = 13f;

		private const float ActivityIconOffsetY = 12f;

		public PawnUIOverlay(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void DrawPawnGUIOverlay()
		{
			if (this.pawn.Spawned && !this.pawn.Map.fogGrid.IsFogged(this.pawn.Position))
			{
				if (!this.pawn.RaceProps.Humanlike)
				{
					switch (Prefs.AnimalNameMode)
					{
					case AnimalNameDisplayMode.TameAll:
					{
						if (this.pawn.Name == null)
							return;
						break;
					}
					case AnimalNameDisplayMode.TameNamed:
					{
						if (this.pawn.Name == null)
							return;
						if (this.pawn.Name.Numerical)
							return;
						break;
					}
					case AnimalNameDisplayMode.None:
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
