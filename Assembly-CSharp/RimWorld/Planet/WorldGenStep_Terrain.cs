using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x020005C7 RID: 1479
	public class WorldGenStep_Terrain : WorldGenStep
	{
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x000F3E94 File Offset: 0x000F2294
		public override int SeedPart
		{
			get
			{
				return 83469557;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001C6C RID: 7276 RVA: 0x000F3EB0 File Offset: 0x000F22B0
		private static float FreqMultiplier
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000F3ECA File Offset: 0x000F22CA
		public override void GenerateFresh(string seed)
		{
			this.GenerateGridIntoWorld();
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000F3ED3 File Offset: 0x000F22D3
		public override void GenerateFromScribe(string seed)
		{
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000F3EEC File Offset: 0x000F22EC
		private void GenerateGridIntoWorld()
		{
			Find.World.grid = new WorldGrid();
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
			this.SetupElevationNoise();
			this.SetupTemperatureOffsetNoise();
			this.SetupRainfallNoise();
			this.SetupHillinessNoise();
			this.SetupSwampinessNoise();
			List<Tile> tiles = Find.WorldGrid.tiles;
			tiles.Clear();
			int tilesCount = Find.WorldGrid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				Tile item = this.GenerateTileFor(i);
				tiles.Add(item);
			}
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x000F3F7C File Offset: 0x000F237C
		private void SetupElevationNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase lhs = new Perlin((double)(0.035f * freqMultiplier), 2.0, 0.40000000596046448, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase = new RidgedMultifractal((double)(0.012f * freqMultiplier), 2.0, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase2 = new Perlin((double)(0.12f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase3 = new Perlin((double)(0.01f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.High);
			float num;
			if (Find.World.PlanetCoverage < 0.55f)
			{
				ModuleBase moduleBase4 = new DistanceFromPlanetViewCenter(Find.WorldGrid.viewCenter, Find.WorldGrid.viewAngle, true);
				moduleBase4 = new ScaleBias(2.0, -1.0, moduleBase4);
				moduleBase3 = new Blend(moduleBase3, moduleBase4, new Const(0.40000000596046448));
				num = Rand.Range(-0.4f, -0.35f);
			}
			else
			{
				num = Rand.Range(0.15f, 0.25f);
			}
			NoiseDebugUI.StorePlanetNoise(moduleBase3, "elevContinents");
			moduleBase2 = new ScaleBias(0.5, 0.5, moduleBase2);
			moduleBase = new Multiply(moduleBase, moduleBase2);
			float num2 = Rand.Range(0.4f, 0.6f);
			this.noiseElevation = new Blend(lhs, moduleBase, new Const((double)num2));
			this.noiseElevation = new Blend(this.noiseElevation, moduleBase3, new Const((double)num));
			if (Find.World.PlanetCoverage < 0.9999f)
			{
				this.noiseElevation = new ConvertToIsland(Find.WorldGrid.viewCenter, Find.WorldGrid.viewAngle, this.noiseElevation);
			}
			this.noiseElevation = new ScaleBias(0.5, 0.5, this.noiseElevation);
			this.noiseElevation = new Power(this.noiseElevation, new Const(3.0));
			NoiseDebugUI.StorePlanetNoise(this.noiseElevation, "noiseElevation");
			this.noiseElevation = new ScaleBias((double)WorldGenStep_Terrain.ElevationRange.Span, (double)WorldGenStep_Terrain.ElevationRange.min, this.noiseElevation);
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000F41F4 File Offset: 0x000F25F4
		private void SetupTemperatureOffsetNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			this.noiseTemperatureOffset = new Perlin((double)(0.018f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			this.noiseTemperatureOffset = new Multiply(this.noiseTemperatureOffset, new Const(4.0));
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000F425C File Offset: 0x000F265C
		private void SetupRainfallNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase moduleBase = new Perlin((double)(0.015f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			NoiseDebugUI.StorePlanetNoise(moduleBase, "basePerlin");
			ModuleBase moduleBase2 = new AbsLatitudeCurve(new SimpleCurve
			{
				{
					0f,
					1.12f,
					true
				},
				{
					25f,
					0.94f,
					true
				},
				{
					45f,
					0.7f,
					true
				},
				{
					70f,
					0.3f,
					true
				},
				{
					80f,
					0.05f,
					true
				},
				{
					90f,
					0.05f,
					true
				}
			}, 100f);
			NoiseDebugUI.StorePlanetNoise(moduleBase2, "latCurve");
			this.noiseRainfall = new Multiply(moduleBase, moduleBase2);
			float num = 0.000222222225f;
			float num2 = -500f * num;
			ModuleBase moduleBase3 = new ScaleBias((double)num, (double)num2, this.noiseElevation);
			moduleBase3 = new ScaleBias(-1.0, 1.0, moduleBase3);
			moduleBase3 = new Clamp(0.0, 1.0, moduleBase3);
			NoiseDebugUI.StorePlanetNoise(moduleBase3, "elevationRainfallEffect");
			this.noiseRainfall = new Multiply(this.noiseRainfall, moduleBase3);
			Func<double, double> processor = delegate(double val)
			{
				if (val < 0.0)
				{
					val = 0.0;
				}
				if (val < 0.12)
				{
					val = (val + 0.12) / 2.0;
					if (val < 0.03)
					{
						val = (val + 0.03) / 2.0;
					}
				}
				return val;
			};
			this.noiseRainfall = new Arbitrary(this.noiseRainfall, processor);
			this.noiseRainfall = new Power(this.noiseRainfall, new Const(1.5));
			this.noiseRainfall = new Clamp(0.0, 999.0, this.noiseRainfall);
			NoiseDebugUI.StorePlanetNoise(this.noiseRainfall, "noiseRainfall before mm");
			this.noiseRainfall = new ScaleBias(4000.0, 0.0, this.noiseRainfall);
			SimpleCurve rainfallCurve = Find.World.info.overallRainfall.GetRainfallCurve();
			if (rainfallCurve != null)
			{
				this.noiseRainfall = new CurveSimple(this.noiseRainfall, rainfallCurve);
			}
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000F44A4 File Offset: 0x000F28A4
		private void SetupHillinessNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			this.noiseMountainLines = new Perlin((double)(0.025f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase = new Perlin((double)(0.06f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			this.noiseMountainLines = new Abs(this.noiseMountainLines);
			this.noiseMountainLines = new OneMinus(this.noiseMountainLines);
			moduleBase = new Filter(moduleBase, -0.3f, 1f);
			this.noiseMountainLines = new Multiply(this.noiseMountainLines, moduleBase);
			this.noiseMountainLines = new OneMinus(this.noiseMountainLines);
			NoiseDebugUI.StorePlanetNoise(this.noiseMountainLines, "noiseMountainLines");
			this.noiseHillsPatchesMacro = new Perlin((double)(0.032f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			this.noiseHillsPatchesMicro = new Perlin((double)(0.19f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000F45E4 File Offset: 0x000F29E4
		private void SetupSwampinessNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase moduleBase = new Perlin((double)(0.09f * freqMultiplier), 2.0, 0.40000000596046448, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase2 = new RidgedMultifractal((double)(0.025f * freqMultiplier), 2.0, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			moduleBase2 = new ScaleBias(0.5, 0.5, moduleBase2);
			this.noiseSwampiness = new Multiply(moduleBase, moduleBase2);
			InverseLerp rhs = new InverseLerp(this.noiseElevation, WorldGenStep_Terrain.SwampinessMaxElevation.max, WorldGenStep_Terrain.SwampinessMaxElevation.min);
			this.noiseSwampiness = new Multiply(this.noiseSwampiness, rhs);
			InverseLerp rhs2 = new InverseLerp(this.noiseRainfall, WorldGenStep_Terrain.SwampinessMinRainfall.min, WorldGenStep_Terrain.SwampinessMinRainfall.max);
			this.noiseSwampiness = new Multiply(this.noiseSwampiness, rhs2);
			NoiseDebugUI.StorePlanetNoise(this.noiseSwampiness, "noiseSwampiness");
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x000F4710 File Offset: 0x000F2B10
		private Tile GenerateTileFor(int tileID)
		{
			Tile tile = new Tile();
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tileID);
			tile.elevation = this.noiseElevation.GetValue(tileCenter);
			float value = this.noiseMountainLines.GetValue(tileCenter);
			if (value > 0.235f || tile.elevation <= 0f)
			{
				if (tile.elevation > 0f && this.noiseHillsPatchesMicro.GetValue(tileCenter) > 0.46f && this.noiseHillsPatchesMacro.GetValue(tileCenter) > -0.3f)
				{
					if (Rand.Bool)
					{
						tile.hilliness = Hilliness.SmallHills;
					}
					else
					{
						tile.hilliness = Hilliness.LargeHills;
					}
				}
				else
				{
					tile.hilliness = Hilliness.Flat;
				}
			}
			else if (value > 0.12f)
			{
				switch (Rand.Range(0, 4))
				{
				case 0:
					tile.hilliness = Hilliness.Flat;
					break;
				case 1:
					tile.hilliness = Hilliness.SmallHills;
					break;
				case 2:
					tile.hilliness = Hilliness.LargeHills;
					break;
				case 3:
					tile.hilliness = Hilliness.Mountainous;
					break;
				}
			}
			else if (value > 0.0363f)
			{
				tile.hilliness = Hilliness.Mountainous;
			}
			else
			{
				tile.hilliness = Hilliness.Impassable;
			}
			float num = WorldGenStep_Terrain.BaseTemperatureAtLatitude(Find.WorldGrid.LongLatOf(tileID).y);
			num -= WorldGenStep_Terrain.TemperatureReductionAtElevation(tile.elevation);
			num += this.noiseTemperatureOffset.GetValue(tileCenter);
			SimpleCurve temperatureCurve = Find.World.info.overallTemperature.GetTemperatureCurve();
			if (temperatureCurve != null)
			{
				num = temperatureCurve.Evaluate(num);
			}
			tile.temperature = num;
			tile.rainfall = this.noiseRainfall.GetValue(tileCenter);
			if (float.IsNaN(tile.rainfall))
			{
				float value2 = this.noiseRainfall.GetValue(tileCenter);
				Log.ErrorOnce(value2 + " rain bad at " + tileID, 694822, false);
			}
			if (tile.hilliness == Hilliness.Flat || tile.hilliness == Hilliness.SmallHills)
			{
				tile.swampiness = this.noiseSwampiness.GetValue(tileCenter);
			}
			tile.biome = this.BiomeFrom(tile, tileID);
			return tile;
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000F4960 File Offset: 0x000F2D60
		private BiomeDef BiomeFrom(Tile ws, int tileID)
		{
			List<BiomeDef> allDefsListForReading = DefDatabase<BiomeDef>.AllDefsListForReading;
			BiomeDef biomeDef = null;
			float num = 0f;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				BiomeDef biomeDef2 = allDefsListForReading[i];
				if (biomeDef2.implemented)
				{
					float score = biomeDef2.Worker.GetScore(ws, tileID);
					if (score > num || biomeDef == null)
					{
						biomeDef = biomeDef2;
						num = score;
					}
				}
			}
			return biomeDef;
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x000F49E0 File Offset: 0x000F2DE0
		private static float FertilityFactorFromTemperature(float temp)
		{
			float result;
			if (temp < -15f)
			{
				result = 0f;
			}
			else if (temp < 30f)
			{
				result = Mathf.InverseLerp(-15f, 30f, temp);
			}
			else if (temp < 50f)
			{
				result = Mathf.InverseLerp(50f, 30f, temp);
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000F4A58 File Offset: 0x000F2E58
		private static float BaseTemperatureAtLatitude(float lat)
		{
			float x = Mathf.Abs(lat) / 90f;
			return WorldGenStep_Terrain.AvgTempByLatitudeCurve.Evaluate(x);
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000F4A88 File Offset: 0x000F2E88
		private static float TemperatureReductionAtElevation(float elev)
		{
			float result;
			if (elev < 250f)
			{
				result = 0f;
			}
			else
			{
				float t = (elev - 250f) / 4750f;
				result = Mathf.Lerp(0f, 40f, t);
			}
			return result;
		}

		// Token: 0x04001105 RID: 4357
		[Unsaved]
		private ModuleBase noiseElevation;

		// Token: 0x04001106 RID: 4358
		[Unsaved]
		private ModuleBase noiseTemperatureOffset;

		// Token: 0x04001107 RID: 4359
		[Unsaved]
		private ModuleBase noiseRainfall;

		// Token: 0x04001108 RID: 4360
		[Unsaved]
		private ModuleBase noiseSwampiness;

		// Token: 0x04001109 RID: 4361
		[Unsaved]
		private ModuleBase noiseMountainLines;

		// Token: 0x0400110A RID: 4362
		[Unsaved]
		private ModuleBase noiseHillsPatchesMicro;

		// Token: 0x0400110B RID: 4363
		[Unsaved]
		private ModuleBase noiseHillsPatchesMacro;

		// Token: 0x0400110C RID: 4364
		private const float ElevationFrequencyMicro = 0.035f;

		// Token: 0x0400110D RID: 4365
		private const float ElevationFrequencyMacro = 0.012f;

		// Token: 0x0400110E RID: 4366
		private const float ElevationMacroFactorFrequency = 0.12f;

		// Token: 0x0400110F RID: 4367
		private const float ElevationContinentsFrequency = 0.01f;

		// Token: 0x04001110 RID: 4368
		private const float MountainLinesFrequency = 0.025f;

		// Token: 0x04001111 RID: 4369
		private const float MountainLinesHolesFrequency = 0.06f;

		// Token: 0x04001112 RID: 4370
		private const float HillsPatchesFrequencyMicro = 0.19f;

		// Token: 0x04001113 RID: 4371
		private const float HillsPatchesFrequencyMacro = 0.032f;

		// Token: 0x04001114 RID: 4372
		private const float SwampinessFrequencyMacro = 0.025f;

		// Token: 0x04001115 RID: 4373
		private const float SwampinessFrequencyMicro = 0.09f;

		// Token: 0x04001116 RID: 4374
		private static readonly FloatRange SwampinessMaxElevation = new FloatRange(650f, 750f);

		// Token: 0x04001117 RID: 4375
		private static readonly FloatRange SwampinessMinRainfall = new FloatRange(725f, 900f);

		// Token: 0x04001118 RID: 4376
		private static readonly FloatRange ElevationRange = new FloatRange(-500f, 5000f);

		// Token: 0x04001119 RID: 4377
		private const float TemperatureOffsetFrequency = 0.018f;

		// Token: 0x0400111A RID: 4378
		private const float TemperatureOffsetFactor = 4f;

		// Token: 0x0400111B RID: 4379
		private static readonly SimpleCurve AvgTempByLatitudeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 30f),
				true
			},
			{
				new CurvePoint(0.1f, 29f),
				true
			},
			{
				new CurvePoint(0.5f, 7f),
				true
			},
			{
				new CurvePoint(1f, -37f),
				true
			}
		};

		// Token: 0x0400111C RID: 4380
		private const float ElevationTempReductionStartAlt = 250f;

		// Token: 0x0400111D RID: 4381
		private const float ElevationTempReductionEndAlt = 5000f;

		// Token: 0x0400111E RID: 4382
		private const float MaxElevationTempReduction = 40f;

		// Token: 0x0400111F RID: 4383
		private const float RainfallOffsetFrequency = 0.013f;

		// Token: 0x04001120 RID: 4384
		private const float RainfallPower = 1.5f;

		// Token: 0x04001121 RID: 4385
		private const float RainfallFactor = 4000f;

		// Token: 0x04001122 RID: 4386
		private const float RainfallStartFallAltitude = 500f;

		// Token: 0x04001123 RID: 4387
		private const float RainfallFinishFallAltitude = 5000f;

		// Token: 0x04001124 RID: 4388
		private const float FertilityTempMinimum = -15f;

		// Token: 0x04001125 RID: 4389
		private const float FertilityTempOptimal = 30f;

		// Token: 0x04001126 RID: 4390
		private const float FertilityTempMaximum = 50f;
	}
}
