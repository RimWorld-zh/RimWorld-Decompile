using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000848 RID: 2120
	public class ITab_Pawn_Character : ITab
	{
		// Token: 0x06003008 RID: 12296 RVA: 0x001A1D94 File Offset: 0x001A0194
		public ITab_Pawn_Character()
		{
			this.size = CharacterCardUtility.PawnCardSize + new Vector2(17f, 17f) * 2f;
			this.labelKey = "TabCharacter";
			this.tutorTag = "Character";
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06003009 RID: 12297 RVA: 0x001A1DE8 File Offset: 0x001A01E8
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
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x001A1E4C File Offset: 0x001A024C
		public override bool IsVisible
		{
			get
			{
				return this.PawnToShowInfoAbout.story != null;
			}
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x001A1E74 File Offset: 0x001A0274
		protected override void FillTab()
		{
			Rect rect = new Rect(17f, 17f, CharacterCardUtility.PawnCardSize.x, CharacterCardUtility.PawnCardSize.y);
			CharacterCardUtility.DrawCharacterCard(rect, this.PawnToShowInfoAbout, null, default(Rect));
		}
	}
}
