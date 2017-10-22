using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ITab_Art : ITab
	{
		private static string cachedImageDescription;

		private static CompArt cachedImageSource;

		private static TaleReference cachedTaleRef;

		private static readonly Vector2 WinSize = new Vector2(400f, 300f);

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
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompArt>();
			}
		}

		public override bool IsVisible
		{
			get
			{
				return this.SelectedCompArt != null && this.SelectedCompArt.Active;
			}
		}

		public ITab_Art()
		{
			base.size = ITab_Art.WinSize;
			base.labelKey = "TabArt";
			base.tutorTag = "Art";
		}

		protected override void FillTab()
		{
			Vector2 winSize = ITab_Art.WinSize;
			float x = winSize.x;
			Vector2 winSize2 = ITab_Art.WinSize;
			Rect rect;
			Rect rect2 = rect = new Rect(0f, 0f, x, winSize2.y).ContractedBy(10f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.SelectedCompArt.Title);
			if (ITab_Art.cachedImageSource != this.SelectedCompArt || ITab_Art.cachedTaleRef != this.SelectedCompArt.TaleRef)
			{
				ITab_Art.cachedImageDescription = this.SelectedCompArt.GenerateImageDescription();
				ITab_Art.cachedImageSource = this.SelectedCompArt;
				ITab_Art.cachedTaleRef = this.SelectedCompArt.TaleRef;
			}
			Rect rect3 = rect2;
			rect3.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, ITab_Art.cachedImageDescription);
		}
	}
}
