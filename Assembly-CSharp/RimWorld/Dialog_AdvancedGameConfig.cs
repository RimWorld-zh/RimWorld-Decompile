using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F5 RID: 2037
	public class Dialog_AdvancedGameConfig : Window
	{
		// Token: 0x040017C1 RID: 6081
		private int selTile = -1;

		// Token: 0x040017C2 RID: 6082
		private const float ColumnWidth = 200f;

		// Token: 0x040017C3 RID: 6083
		private static readonly int[] MapSizes = new int[]
		{
			200,
			225,
			250,
			275,
			300,
			325
		};

		// Token: 0x040017C4 RID: 6084
		private static readonly int[] TestMapSizes = new int[]
		{
			350,
			400
		};

		// Token: 0x06002D2E RID: 11566 RVA: 0x0017BBFE File Offset: 0x00179FFE
		public Dialog_AdvancedGameConfig(int selTile)
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.selTile = selTile;
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002D2F RID: 11567 RVA: 0x0017BC2C File Offset: 0x0017A02C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 500f);
			}
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x0017BC50 File Offset: 0x0017A050
		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = 200f;
			listing_Standard.Begin(inRect.AtZero());
			Text.Font = GameFont.Medium;
			listing_Standard.Label("MapSize".Translate(), -1f, null);
			Text.Font = GameFont.Small;
			IEnumerable<int> enumerable = Dialog_AdvancedGameConfig.MapSizes.AsEnumerable<int>();
			if (Prefs.TestMapSizes)
			{
				enumerable = enumerable.Concat(Dialog_AdvancedGameConfig.TestMapSizes);
			}
			foreach (int num in enumerable)
			{
				if (num == 200)
				{
					listing_Standard.Label("MapSizeSmall".Translate(), -1f, null);
				}
				else if (num == 250)
				{
					listing_Standard.Gap(10f);
					listing_Standard.Label("MapSizeMedium".Translate(), -1f, null);
				}
				else if (num == 300)
				{
					listing_Standard.Gap(10f);
					listing_Standard.Label("MapSizeLarge".Translate(), -1f, null);
				}
				else if (num == 350)
				{
					listing_Standard.Gap(10f);
					listing_Standard.Label("MapSizeExtreme".Translate(), -1f, null);
				}
				string label = "MapSizeDesc".Translate(new object[]
				{
					num,
					num * num
				});
				if (listing_Standard.RadioButton(label, Find.GameInitData.mapSize == num, 0f))
				{
					Find.GameInitData.mapSize = num;
				}
			}
			listing_Standard.NewColumn();
			Text.Font = GameFont.Medium;
			listing_Standard.Label("MapStartSeason".Translate(), -1f, null);
			Text.Font = GameFont.Small;
			listing_Standard.Label("", -1f, null);
			if (listing_Standard.RadioButton("MapStartSeasonDefault".Translate(), Find.GameInitData.startingSeason == Season.Undefined, 0f))
			{
				Find.GameInitData.startingSeason = Season.Undefined;
			}
			if (listing_Standard.RadioButton(Season.Spring.LabelCap(), Find.GameInitData.startingSeason == Season.Spring, 0f))
			{
				Find.GameInitData.startingSeason = Season.Spring;
			}
			if (listing_Standard.RadioButton(Season.Summer.LabelCap(), Find.GameInitData.startingSeason == Season.Summer, 0f))
			{
				Find.GameInitData.startingSeason = Season.Summer;
			}
			if (listing_Standard.RadioButton(Season.Fall.LabelCap(), Find.GameInitData.startingSeason == Season.Fall, 0f))
			{
				Find.GameInitData.startingSeason = Season.Fall;
			}
			if (listing_Standard.RadioButton(Season.Winter.LabelCap(), Find.GameInitData.startingSeason == Season.Winter, 0f))
			{
				Find.GameInitData.startingSeason = Season.Winter;
			}
			listing_Standard.NewColumn();
			Text.Font = GameFont.Medium;
			listing_Standard.Label("Notice".Translate(), -1f, null);
			Text.Font = GameFont.Small;
			listing_Standard.Label("", -1f, null);
			bool flag = false;
			if (this.selTile >= 0 && Find.GameInitData.startingSeason != Season.Undefined)
			{
				float y = Find.WorldGrid.LongLatOf(this.selTile).y;
				if (GenTemperature.AverageTemperatureAtTileForTwelfth(this.selTile, Find.GameInitData.startingSeason.GetFirstTwelfth(y)) < 3f)
				{
					listing_Standard.Label("MapTemperatureDangerWarning".Translate(), -1f, null);
					flag = true;
				}
			}
			if (Find.GameInitData.mapSize > 280)
			{
				listing_Standard.Label("MapSizePerformanceWarning".Translate(), -1f, null);
				flag = true;
			}
			if (!flag)
			{
				listing_Standard.None();
			}
			listing_Standard.End();
		}
	}
}
