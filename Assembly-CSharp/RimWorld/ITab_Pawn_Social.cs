using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000859 RID: 2137
	public class ITab_Pawn_Social : ITab
	{
		// Token: 0x04001A3A RID: 6714
		public const float Width = 540f;

		// Token: 0x06003064 RID: 12388 RVA: 0x001A5452 File Offset: 0x001A3852
		public ITab_Pawn_Social()
		{
			this.size = new Vector2(540f, 510f);
			this.labelKey = "TabSocial";
			this.tutorTag = "Social";
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06003065 RID: 12389 RVA: 0x001A5488 File Offset: 0x001A3888
		public override bool IsVisible
		{
			get
			{
				return this.SelPawnForSocialInfo.RaceProps.IsFlesh;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06003066 RID: 12390 RVA: 0x001A54B0 File Offset: 0x001A38B0
		private Pawn SelPawnForSocialInfo
		{
			get
			{
				Pawn result;
				if (base.SelPawn != null)
				{
					result = base.SelPawn;
				}
				else
				{
					Corpse corpse = base.SelThing as Corpse;
					if (corpse == null)
					{
						throw new InvalidOperationException("Social tab on non-pawn non-corpse " + base.SelThing);
					}
					result = corpse.InnerPawn;
				}
				return result;
			}
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x001A550C File Offset: 0x001A390C
		protected override void FillTab()
		{
			SocialCardUtility.DrawSocialCard(new Rect(0f, 0f, this.size.x, this.size.y), this.SelPawnForSocialInfo);
		}
	}
}
