using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Profile;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000831 RID: 2097
	public class Page_CreateWorldParams : Page
	{
		// Token: 0x04001990 RID: 6544
		private bool initialized = false;

		// Token: 0x04001991 RID: 6545
		private string seedString;

		// Token: 0x04001992 RID: 6546
		private float planetCoverage;

		// Token: 0x04001993 RID: 6547
		private OverallRainfall rainfall;

		// Token: 0x04001994 RID: 6548
		private OverallTemperature temperature;

		// Token: 0x04001995 RID: 6549
		private static readonly float[] PlanetCoverages = new float[]
		{
			0.3f,
			0.5f,
			1f
		};

		// Token: 0x04001996 RID: 6550
		private static readonly float[] PlanetCoveragesDev = new float[]
		{
			0.3f,
			0.5f,
			1f,
			0.05f
		};

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002F52 RID: 12114 RVA: 0x00194F20 File Offset: 0x00193320
		public override string PageTitle
		{
			get
			{
				return "CreateWorld".Translate();
			}
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x00194F3F File Offset: 0x0019333F
		public override void PreOpen()
		{
			base.PreOpen();
			if (!this.initialized)
			{
				this.Reset();
				this.initialized = true;
			}
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x00194F62 File Offset: 0x00193362
		public override void PostOpen()
		{
			base.PostOpen();
			TutorSystem.Notify_Event("PageStart-CreateWorldParams");
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x00194F7C File Offset: 0x0019337C
		public void Reset()
		{
			this.seedString = GenText.RandomSeedString();
			this.planetCoverage = ((Prefs.DevMode && UnityData.isEditor) ? 0.05f : 0.3f);
			this.rainfall = OverallRainfall.Normal;
			this.temperature = OverallTemperature.Normal;
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x00194FCC File Offset: 0x001933CC
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			GUI.BeginGroup(base.GetMainRect(rect, 0f, false));
			Text.Font = GameFont.Small;
			float num = 0f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "WorldSeed".Translate());
			Rect rect2 = new Rect(200f, num, 200f, 30f);
			this.seedString = Widgets.TextField(rect2, this.seedString);
			num += 40f;
			Rect rect3 = new Rect(200f, num, 200f, 30f);
			if (Widgets.ButtonText(rect3, "RandomizeSeed".Translate(), true, false, true))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.seedString = GenText.RandomSeedString();
			}
			num += 40f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetCoverage".Translate());
			Rect rect4 = new Rect(200f, num, 200f, 30f);
			if (Widgets.ButtonText(rect4, this.planetCoverage.ToStringPercent(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				float[] array = (!Prefs.DevMode) ? Page_CreateWorldParams.PlanetCoverages : Page_CreateWorldParams.PlanetCoveragesDev;
				for (int i = 0; i < array.Length; i++)
				{
					float coverage = array[i];
					string text = coverage.ToStringPercent();
					if (coverage <= 0.1f)
					{
						text += " (dev)";
					}
					FloatMenuOption item = new FloatMenuOption(text, delegate()
					{
						if (this.planetCoverage != coverage)
						{
							this.planetCoverage = coverage;
							if (this.planetCoverage == 1f)
							{
								Messages.Message("MessageMaxPlanetCoveragePerformanceWarning".Translate(), MessageTypeDefOf.CautionInput, false);
							}
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null);
					list.Add(item);
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			TooltipHandler.TipRegion(new Rect(0f, num, rect4.xMax, rect4.height), "PlanetCoverageTip".Translate());
			num += 40f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetRainfall".Translate());
			Rect rect5 = new Rect(200f, num, 200f, 30f);
			this.rainfall = (OverallRainfall)Mathf.RoundToInt(Widgets.HorizontalSlider(rect5, (float)this.rainfall, 0f, (float)(OverallRainfallUtility.EnumValuesCount - 1), true, "PlanetRainfall_Normal".Translate(), "PlanetRainfall_Low".Translate(), "PlanetRainfall_High".Translate(), 1f));
			num += 40f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetTemperature".Translate());
			Rect rect6 = new Rect(200f, num, 200f, 30f);
			this.temperature = (OverallTemperature)Mathf.RoundToInt(Widgets.HorizontalSlider(rect6, (float)this.temperature, 0f, (float)(OverallTemperatureUtility.EnumValuesCount - 1), true, "PlanetTemperature_Normal".Translate(), "PlanetTemperature_Low".Translate(), "PlanetTemperature_High".Translate(), 1f));
			GUI.EndGroup();
			base.DoBottomButtons(rect, "WorldGenerate".Translate(), "Reset".Translate(), new Action(this.Reset), true);
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x00195338 File Offset: 0x00193738
		protected override bool CanDoNext()
		{
			bool result;
			if (!base.CanDoNext())
			{
				result = false;
			}
			else
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					Find.GameInitData.ResetWorldRelatedMapInitData();
					Current.Game.World = WorldGenerator.GenerateWorld(this.planetCoverage, this.seedString, this.rainfall, this.temperature);
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						if (this.next != null)
						{
							Find.WindowStack.Add(this.next);
						}
						MemoryUtility.UnloadUnusedUnityAssets();
						Find.World.renderer.RegenerateAllLayersNow();
						this.Close(true);
					});
				}, "GeneratingWorld", true, null);
				result = false;
			}
			return result;
		}
	}
}
