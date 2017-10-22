using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Profile;
using Verse.Sound;

namespace RimWorld
{
	public class Page_CreateWorldParams : Page
	{
		private bool initialized = false;

		private string seedString;

		private float planetCoverage;

		private OverallRainfall rainfall;

		private OverallTemperature temperature;

		private static readonly float[] PlanetCoverages = new float[3]
		{
			0.3f,
			0.5f,
			1f
		};

		private static readonly float[] PlanetCoveragesDev = new float[4]
		{
			0.3f,
			0.5f,
			1f,
			0.05f
		};

		public override string PageTitle
		{
			get
			{
				return "CreateWorld".Translate();
			}
		}

		public override void PreOpen()
		{
			base.PreOpen();
			if (!this.initialized)
			{
				this.Reset();
				this.initialized = true;
			}
		}

		public override void PostOpen()
		{
			base.PostOpen();
			TutorSystem.Notify_Event("PageStart-CreateWorldParams");
		}

		public void Reset()
		{
			this.seedString = GenText.RandomSeedString();
			this.planetCoverage = (float)((Prefs.DevMode && UnityData.isEditor) ? 0.05000000074505806 : 0.30000001192092896);
			this.rainfall = OverallRainfall.Normal;
			this.temperature = OverallTemperature.Normal;
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			GUI.BeginGroup(base.GetMainRect(rect, 0f, false));
			Text.Font = GameFont.Small;
			float num = 0f;
			Widgets.Label(new Rect(0f, num, 200f, 30f), "WorldSeed".Translate());
			Rect rect2 = new Rect(200f, num, 200f, 30f);
			this.seedString = Widgets.TextField(rect2, this.seedString);
			num = (float)(num + 40.0);
			Rect rect3 = new Rect(200f, num, 200f, 30f);
			if (Widgets.ButtonText(rect3, "RandomizeSeed".Translate(), true, false, true))
			{
				SoundDefOf.TickTiny.PlayOneShotOnCamera(null);
				this.seedString = GenText.RandomSeedString();
			}
			num = (float)(num + 40.0);
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
					if (coverage <= 0.10000000149011612)
					{
						text += " (dev)";
					}
					FloatMenuOption item = new FloatMenuOption(text, (Action)delegate
					{
						if (this.planetCoverage != coverage)
						{
							this.planetCoverage = coverage;
							if (this.planetCoverage == 1.0)
							{
								Messages.Message("MessageMaxPlanetCoveragePerformanceWarning".Translate(), MessageTypeDefOf.CautionInput);
							}
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null);
					list.Add(item);
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			TooltipHandler.TipRegion(new Rect(0f, num, rect4.xMax, rect4.height), "PlanetCoverageTip".Translate());
			num = (float)(num + 40.0);
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetRainfall".Translate());
			Rect rect5 = new Rect(200f, num, 200f, 30f);
			this.rainfall = (OverallRainfall)Mathf.RoundToInt(Widgets.HorizontalSlider(rect5, (float)this.rainfall, 0f, (float)(OverallRainfallUtility.EnumValuesCount - 1), true, "PlanetRainfall_Normal".Translate(), "PlanetRainfall_Low".Translate(), "PlanetRainfall_High".Translate(), 1f));
			num = (float)(num + 40.0);
			Widgets.Label(new Rect(0f, num, 200f, 30f), "PlanetTemperature".Translate());
			Rect rect6 = new Rect(200f, num, 200f, 30f);
			this.temperature = (OverallTemperature)Mathf.RoundToInt(Widgets.HorizontalSlider(rect6, (float)this.temperature, 0f, (float)(OverallTemperatureUtility.EnumValuesCount - 1), true, "PlanetTemperature_Normal".Translate(), "PlanetTemperature_Low".Translate(), "PlanetTemperature_High".Translate(), 1f));
			GUI.EndGroup();
			base.DoBottomButtons(rect, "WorldGenerate".Translate(), "Reset".Translate(), new Action(this.Reset), true);
		}

		protected override bool CanDoNext()
		{
			bool result;
			if (!base.CanDoNext())
			{
				result = false;
			}
			else
			{
				LongEventHandler.QueueLongEvent((Action)delegate
				{
					Find.GameInitData.ResetWorldRelatedMapInitData();
					Current.Game.World = WorldGenerator.GenerateWorld(this.planetCoverage, this.seedString, this.rainfall, this.temperature);
					LongEventHandler.ExecuteWhenFinished((Action)delegate
					{
						if (base.next != null)
						{
							Find.WindowStack.Add(base.next);
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
