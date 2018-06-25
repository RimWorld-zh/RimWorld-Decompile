using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084A RID: 2122
	public class ITab_Pawn_Character : ITab
	{
		// Token: 0x0600300C RID: 12300 RVA: 0x001A1EE4 File Offset: 0x001A02E4
		public ITab_Pawn_Character()
		{
			this.size = CharacterCardUtility.PawnCardSize + new Vector2(17f, 17f) * 2f;
			this.labelKey = "TabCharacter";
			this.tutorTag = "Character";
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x0600300D RID: 12301 RVA: 0x001A1F38 File Offset: 0x001A0338
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
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x001A1F9C File Offset: 0x001A039C
		public override bool IsVisible
		{
			get
			{
				return this.PawnToShowInfoAbout.story != null;
			}
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x001A1FC4 File Offset: 0x001A03C4
		protected override void FillTab()
		{
			Rect rect = new Rect(17f, 17f, CharacterCardUtility.PawnCardSize.x, CharacterCardUtility.PawnCardSize.y);
			CharacterCardUtility.DrawCharacterCard(rect, this.PawnToShowInfoAbout, null, default(Rect));
		}
	}
}
