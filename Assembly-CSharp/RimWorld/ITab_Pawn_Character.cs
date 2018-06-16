using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084C RID: 2124
	public class ITab_Pawn_Character : ITab
	{
		// Token: 0x0600300D RID: 12301 RVA: 0x001A1AEC File Offset: 0x0019FEEC
		public ITab_Pawn_Character()
		{
			this.size = CharacterCardUtility.PawnCardSize + new Vector2(17f, 17f) * 2f;
			this.labelKey = "TabCharacter";
			this.tutorTag = "Character";
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x001A1B40 File Offset: 0x0019FF40
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

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x0600300F RID: 12303 RVA: 0x001A1BA4 File Offset: 0x0019FFA4
		public override bool IsVisible
		{
			get
			{
				return this.PawnToShowInfoAbout.story != null;
			}
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x001A1BCC File Offset: 0x0019FFCC
		protected override void FillTab()
		{
			Rect rect = new Rect(17f, 17f, CharacterCardUtility.PawnCardSize.x, CharacterCardUtility.PawnCardSize.y);
			CharacterCardUtility.DrawCharacterCard(rect, this.PawnToShowInfoAbout, null, default(Rect));
		}
	}
}
