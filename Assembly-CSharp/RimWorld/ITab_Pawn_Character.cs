using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084A RID: 2122
	public class ITab_Pawn_Character : ITab
	{
		// Token: 0x0600300B RID: 12299 RVA: 0x001A214C File Offset: 0x001A054C
		public ITab_Pawn_Character()
		{
			this.size = CharacterCardUtility.PawnCardSize + new Vector2(17f, 17f) * 2f;
			this.labelKey = "TabCharacter";
			this.tutorTag = "Character";
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x001A21A0 File Offset: 0x001A05A0
		private Pawn PawnToShowInfoAbout
		{
			get
			{
				Pawn pawn = null;
				if (base.SelPawn != null)
				{
					pawn = base.SelPawn;
				}
				else
				{
					Corpse corpse = base.SelThing as Corpse;
					if (corpse != null)
					{
						pawn = corpse.InnerPawn;
					}
				}
				Pawn result;
				if (pawn == null)
				{
					Log.Error("Character tab found no selected pawn to display.", false);
					result = null;
				}
				else
				{
					result = pawn;
				}
				return result;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x0600300D RID: 12301 RVA: 0x001A2204 File Offset: 0x001A0604
		public override bool IsVisible
		{
			get
			{
				return this.PawnToShowInfoAbout.story != null;
			}
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x001A222C File Offset: 0x001A062C
		protected override void FillTab()
		{
			Rect rect = new Rect(17f, 17f, CharacterCardUtility.PawnCardSize.x, CharacterCardUtility.PawnCardSize.y);
			CharacterCardUtility.DrawCharacterCard(rect, this.PawnToShowInfoAbout, null, default(Rect));
		}
	}
}
