using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084C RID: 2124
	public class ITab_Pawn_Character : ITab
	{
		// Token: 0x0600300F RID: 12303 RVA: 0x001A1BB4 File Offset: 0x0019FFB4
		public ITab_Pawn_Character()
		{
			this.size = CharacterCardUtility.PawnCardSize + new Vector2(17f, 17f) * 2f;
			this.labelKey = "TabCharacter";
			this.tutorTag = "Character";
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06003010 RID: 12304 RVA: 0x001A1C08 File Offset: 0x001A0008
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
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x001A1C6C File Offset: 0x001A006C
		public override bool IsVisible
		{
			get
			{
				return this.PawnToShowInfoAbout.story != null;
			}
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x001A1C94 File Offset: 0x001A0094
		protected override void FillTab()
		{
			Rect rect = new Rect(17f, 17f, CharacterCardUtility.PawnCardSize.x, CharacterCardUtility.PawnCardSize.y);
			CharacterCardUtility.DrawCharacterCard(rect, this.PawnToShowInfoAbout, null, default(Rect));
		}
	}
}
