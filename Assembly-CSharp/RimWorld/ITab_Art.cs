using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000846 RID: 2118
	public class ITab_Art : ITab
	{
		// Token: 0x06002FFD RID: 12285 RVA: 0x001A16DE File Offset: 0x0019FADE
		public ITab_Art()
		{
			this.size = ITab_Art.WinSize;
			this.labelKey = "TabArt";
			this.tutorTag = "Art";
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06002FFE RID: 12286 RVA: 0x001A1708 File Offset: 0x0019FB08
		private CompArt SelectedCompArt
		{
			get
			{
				Thing thing = Find.Selector.SingleSelectedThing;
				MinifiedThing minifiedThing = thing as MinifiedThing;
				if (minifiedThing != null)
				{
					thing = minifiedThing.InnerThing;
				}
				CompArt result;
				if (thing == null)
				{
					result = null;
				}
				else
				{
					result = thing.TryGetComp<CompArt>();
				}
				return result;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002FFF RID: 12287 RVA: 0x001A1750 File Offset: 0x0019FB50
		public override bool IsVisible
		{
			get
			{
				return this.SelectedCompArt != null && this.SelectedCompArt.Active;
			}
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x001A1780 File Offset: 0x0019FB80
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, ITab_Art.WinSize.x, ITab_Art.WinSize.y).ContractedBy(10f);
			Rect rect2 = rect;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, this.SelectedCompArt.Title);
			if (ITab_Art.cachedImageSource != this.SelectedCompArt || ITab_Art.cachedTaleRef != this.SelectedCompArt.TaleRef)
			{
				ITab_Art.cachedImageDescription = this.SelectedCompArt.GenerateImageDescription();
				ITab_Art.cachedImageSource = this.SelectedCompArt;
				ITab_Art.cachedTaleRef = this.SelectedCompArt.TaleRef;
			}
			Rect rect3 = rect;
			rect3.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, ITab_Art.cachedImageDescription);
		}

		// Token: 0x040019F1 RID: 6641
		private static string cachedImageDescription;

		// Token: 0x040019F2 RID: 6642
		private static CompArt cachedImageSource;

		// Token: 0x040019F3 RID: 6643
		private static TaleReference cachedTaleRef;

		// Token: 0x040019F4 RID: 6644
		private static readonly Vector2 WinSize = new Vector2(400f, 300f);
	}
}
