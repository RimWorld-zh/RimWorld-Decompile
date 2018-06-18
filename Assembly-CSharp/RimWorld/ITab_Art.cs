using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084A RID: 2122
	public class ITab_Art : ITab
	{
		// Token: 0x06003004 RID: 12292 RVA: 0x001A14FE File Offset: 0x0019F8FE
		public ITab_Art()
		{
			this.size = ITab_Art.WinSize;
			this.labelKey = "TabArt";
			this.tutorTag = "Art";
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06003005 RID: 12293 RVA: 0x001A1528 File Offset: 0x0019F928
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

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x001A1570 File Offset: 0x0019F970
		public override bool IsVisible
		{
			get
			{
				return this.SelectedCompArt != null && this.SelectedCompArt.Active;
			}
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x001A15A0 File Offset: 0x0019F9A0
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

		// Token: 0x040019F3 RID: 6643
		private static string cachedImageDescription;

		// Token: 0x040019F4 RID: 6644
		private static CompArt cachedImageSource;

		// Token: 0x040019F5 RID: 6645
		private static TaleReference cachedTaleRef;

		// Token: 0x040019F6 RID: 6646
		private static readonly Vector2 WinSize = new Vector2(400f, 300f);
	}
}
