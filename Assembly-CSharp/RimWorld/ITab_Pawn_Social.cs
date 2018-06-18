using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200085B RID: 2139
	public class ITab_Pawn_Social : ITab
	{
		// Token: 0x06003067 RID: 12391 RVA: 0x001A5122 File Offset: 0x001A3522
		public ITab_Pawn_Social()
		{
			this.size = new Vector2(540f, 510f);
			this.labelKey = "TabSocial";
			this.tutorTag = "Social";
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06003068 RID: 12392 RVA: 0x001A5158 File Offset: 0x001A3558
		public override bool IsVisible
		{
			get
			{
				return this.SelPawnForSocialInfo.RaceProps.IsFlesh;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06003069 RID: 12393 RVA: 0x001A5180 File Offset: 0x001A3580
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

		// Token: 0x0600306A RID: 12394 RVA: 0x001A51DC File Offset: 0x001A35DC
		protected override void FillTab()
		{
			SocialCardUtility.DrawSocialCard(new Rect(0f, 0f, this.size.x, this.size.y), this.SelPawnForSocialInfo);
		}

		// Token: 0x04001A3C RID: 6716
		public const float Width = 540f;
	}
}
