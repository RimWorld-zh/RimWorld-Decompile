using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008E7 RID: 2279
	public class WITab_Planet : WITab
	{
		// Token: 0x04001C5D RID: 7261
		private static readonly Vector2 WinSize = new Vector2(400f, 150f);

		// Token: 0x0600346A RID: 13418 RVA: 0x001C094A File Offset: 0x001BED4A
		public WITab_Planet()
		{
			this.size = WITab_Planet.WinSize;
			this.labelKey = "TabPlanet";
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x0600346B RID: 13419 RVA: 0x001C096C File Offset: 0x001BED6C
		public override bool IsVisible
		{
			get
			{
				return base.SelTileID >= 0;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x0600346C RID: 13420 RVA: 0x001C0990 File Offset: 0x001BED90
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

		// Token: 0x0600346D RID: 13421 RVA: 0x001C0A1C File Offset: 0x001BEE1C
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
