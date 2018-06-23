using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000857 RID: 2135
	public class ITab_Pawn_Social : ITab
	{
		// Token: 0x04001A3A RID: 6714
		public const float Width = 540f;

		// Token: 0x06003060 RID: 12384 RVA: 0x001A5302 File Offset: 0x001A3702
		public ITab_Pawn_Social()
		{
			this.size = new Vector2(540f, 510f);
			this.labelKey = "TabSocial";
			this.tutorTag = "Social";
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06003061 RID: 12385 RVA: 0x001A5338 File Offset: 0x001A3738
		public override bool IsVisible
		{
			get
			{
				return this.SelPawnForSocialInfo.RaceProps.IsFlesh;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06003062 RID: 12386 RVA: 0x001A5360 File Offset: 0x001A3760
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

		// Token: 0x06003063 RID: 12387 RVA: 0x001A53BC File Offset: 0x001A37BC
		protected override void FillTab()
		{
			SocialCardUtility.DrawSocialCard(new Rect(0f, 0f, this.size.x, this.size.y), this.SelPawnForSocialInfo);
		}
	}
}
