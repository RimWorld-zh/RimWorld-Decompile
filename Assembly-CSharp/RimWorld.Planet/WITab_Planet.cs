using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WITab_Planet : WITab
	{
		private static readonly Vector2 WinSize = new Vector2(400f, 150f);

		public override bool IsVisible
		{
			get
			{
				return base.SelTileID >= 0;
			}
		}

		private string Desc
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("PlanetSeed".Translate());
				stringBuilder.Append(": ");
				stringBuilder.AppendLine(Find.World.info.seedString);
				stringBuilder.Append("PlanetCoverageShort".Translate());
				stringBuilder.Append(": ");
				stringBuilder.AppendLine(Find.World.info.planetCoverage.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		public WITab_Planet()
		{
			base.size = WITab_Planet.WinSize;
			base.labelKey = "TabPlanet";
		}

		protected override void FillTab()
		{
			Vector2 winSize = WITab_Planet.WinSize;
			float x = winSize.x;
			Vector2 winSize2 = WITab_Planet.WinSize;
			Rect rect;
			Rect rect2 = rect = new Rect(0f, 0f, x, winSize2.y).ContractedBy(10f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, Find.World.info.name);
			Rect rect3 = rect2;
			rect3.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, this.Desc);
		}
	}
}
