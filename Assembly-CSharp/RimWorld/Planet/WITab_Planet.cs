using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008E5 RID: 2277
	public class WITab_Planet : WITab
	{
		// Token: 0x04001C5D RID: 7261
		private static readonly Vector2 WinSize = new Vector2(400f, 150f);

		// Token: 0x06003466 RID: 13414 RVA: 0x001C080A File Offset: 0x001BEC0A
		public WITab_Planet()
		{
			this.size = WITab_Planet.WinSize;
			this.labelKey = "TabPlanet";
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06003467 RID: 13415 RVA: 0x001C082C File Offset: 0x001BEC2C
		public override bool IsVisible
		{
			get
			{
				return base.SelTileID >= 0;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06003468 RID: 13416 RVA: 0x001C0850 File Offset: 0x001BEC50
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

		// Token: 0x06003469 RID: 13417 RVA: 0x001C08DC File Offset: 0x001BECDC
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, WITab_Planet.WinSize.x, WITab_Planet.WinSize.y).ContractedBy(10f);
			Rect rect2 = rect;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, Find.World.info.name);
			Rect rect3 = rect;
			rect3.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, this.Desc);
		}
	}
}
