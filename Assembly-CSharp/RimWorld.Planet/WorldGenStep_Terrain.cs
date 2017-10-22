using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	public class WorldGenStep_Terrain : WorldGenStep
	{
		private ModuleBase noiseElevation;

		private ModuleBase noiseTemperatureOffset;

		private ModuleBase noiseRainfall;

		private ModuleBase noiseSwampiness;

		private ModuleBase noiseMountainLines;

		private ModuleBase noiseHillsPatchesMicro;

		private ModuleBase noiseHillsPatchesMacro;

		private const float ElevationFrequencyMicro = 0.035f;

		private const float ElevationFrequencyMacro = 0.012f;

		private const float ElevationMacroFactorFrequency = 0.12f;

		private const float ElevationContinentsFrequency = 0.01f;

		private const float MountainLinesFrequency = 0.025f;

		private const float MountainLinesHolesFrequency = 0.06f;

		private const float HillsPatchesFrequencyMicro = 0.19f;

		private const float HillsPatchesFrequencyMacro = 0.032f;

		private const float SwampinessFrequencyMacro = 0.025f;

		private const float SwampinessFrequencyMicro = 0.09f;

		private static readonly FloatRange SwampinessMaxElevation = new FloatRange(650f, 750f);

		private static readonly FloatRange SwampinessMinRainfall = new FloatRange(725f, 900f);

		private static readonly FloatRange ElevationRange = new FloatRange(-500f, 5000f);

		private const float TemperatureOffsetFrequency = 0.018f;

		private const float TemperatureOffsetFactor = 4f;

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

		private const float ElevationTempReductionStartAlt = 250f;

		private const float ElevationTempReductionEndAlt = 5000f;

		private const float MaxElevationTempReduction = 40f;

		private const float RainfallOffsetFrequency = 0.013f;

		private const float RainfallPower = 1.5f;

		private const float RainfallFactor = 4000f;

		private const float RainfallStartFallAltitude = 500f;

		private const float RainfallFinishFallAltitude = 5000f;

		private const float FertilityTempMinimum = -15f;

		private const float FertilityTempOptimal = 30f;

		private const float FertilityTempMaximum = 50f;

		private static float FreqMultiplier
		{
			get
			{
				return 1f;
			}
		}

		public override void GenerateFresh(string seed)
		{
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateGridIntoWorld();
			Rand.RandomizeStateFromTime();
		}

		public override void GenerateFromScribe(string seed)
		{
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
		}

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
			for (int num = 0; num < tilesCount; num++)
			{
				Tile item = this.GenerateTileFor(num);
				tiles.Add(item);
			}
		}

		private void SetupElevationNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase lhs = new Perlin(0.035000000149011612 * freqMultiplier, 2.0, 0.40000000596046448, 6, Rand.Range(0, 2147483647), QualityMode.High);
			ModuleBase lhs2 = new RidgedMultifractal(0.012000000104308128 * freqMultiplier, 2.0, 6, Rand.Range(0, 2147483647), QualityMode.High);
			ModuleBase input = new Perlin(0.11999999731779099 * freqMultiplier, 2.0, 0.5, 5, Rand.Range(0, 2147483647), QualityMode.High);
			ModuleBase moduleBase = new Perlin(0.0099999997764825821 * freqMultiplier, 2.0, 0.5, 5, Rand.Range(0, 2147483647), QualityMode.High);
			float num;
			if (Find.World.PlanetCoverage < 0.550000011920929)
			{
				ModuleBase input2 = new DistanceFromPlanetViewCenter(Find.WorldGrid.viewCenter, Find.WorldGrid.viewAngle, true);
				input2 = new ScaleBias(2.0, -1.0, input2);
				moduleBase = new Blend(moduleBase, input2, new Const(0.40000000596046448));
				num = Rand.Range(-0.4f, -0.35f);
			}
			else
			{
				num = Rand.Range(0.15f, 0.25f);
			}
			NoiseDebugUI.StorePlanetNoise(moduleBase, "elevContinents");
			input = new ScaleBias(0.5, 0.5, input);
			lhs2 = new Multiply(lhs2, input);
			float num2 = Rand.Range(0.4f, 0.6f);
			this.noiseElevation = new Blend(lhs, lhs2, new Const((double)num2));
			this.noiseElevation = new Blend(this.noiseElevation, moduleBase, new Const((double)num));
			if (Find.World.PlanetCoverage < 0.99989998340606689)
			{
				this.noiseElevation = new ConvertToIsland(Find.WorldGrid.viewCenter, Find.WorldGrid.viewAngle, this.noiseElevation);
			}
			this.noiseElevation = new ScaleBias(0.5, 0.5, this.noiseElevation);
			this.noiseElevation = new Power(this.noiseElevation, new Const(3.0));
			NoiseDebugUI.StorePlanetNoise(this.noiseElevation, "noiseElevation");
			double scale = (double)WorldGenStep_Terrain.ElevationRange.Span;
			FloatRange elevationRange = WorldGenStep_Terrain.ElevationRange;
			this.noiseElevation = new ScaleBias(scale, (double)elevationRange.min, this.noiseElevation);
		}

		private void SetupTemperatureOffsetNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			this.noiseTemperatureOffset = new Perlin(0.017999999225139618 * freqMultiplier, 2.0, 0.5, 6, Rand.Range(0, 2147483647), QualityMode.High);
			this.noiseTemperatureOffset = new Multiply(this.noiseTemperatureOffset, new Const(4.0));
		}

		private void SetupRainfallNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase input = new Perlin(0.014999999664723873 * freqMultiplier, 2.0, 0.5, 6, Rand.Range(0, 2147483647), QualityMode.High);
			input = new ScaleBias(0.5, 0.5, input);
			NoiseDebugUI.StorePlanetNoise(input, "basePerlin");
			SimpleCurve simpleCurve = new SimpleCurve();
			simpleCurve.Add(0f, 1.12f, true);
			simpleCurve.Add(25f, 0.94f, true);
			simpleCurve.Add(45f, 0.7f, true);
			simpleCurve.Add(70f, 0.3f, true);
			simpleCurve.Add(80f, 0.05f, true);
			simpleCurve.Add(90f, 0.05f, true);
			ModuleBase moduleBase = new AbsLatitudeCurve(simpleCurve, 100f);
			NoiseDebugUI.StorePlanetNoise(moduleBase, "latCurve");
			this.noiseRainfall = new Multiply(input, moduleBase);
			float num = 0.000222222225f;
			float num2 = (float)(-500.0 * num);
			ModuleBase input2 = new ScaleBias((double)num, (double)num2, this.noiseElevation);
			input2 = new ScaleBias(-1.0, 1.0, input2);
			input2 = new Clamp(0.0, 1.0, input2);
			NoiseDebugUI.StorePlanetNoise(input2, "elevationRainfallEffect");
			this.noiseRainfall = new Multiply(this.noiseRainfall, input2);
			Func<double, double> processor = (Func<double, double>)delegate(double val)
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

		private void SetupHillinessNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			this.noiseMountainLines = new Perlin(0.02500000037252903 * freqMultiplier, 2.0, 0.5, 6, Rand.Range(0, 2147483647), QualityMode.High);
			ModuleBase module = new Perlin(0.059999998658895493 * freqMultiplier, 2.0, 0.5, 6, Rand.Range(0, 2147483647), QualityMode.High);
			this.noiseMountainLines = new Abs(this.noiseMountainLines);
			this.noiseMountainLines = new OneMinus(this.noiseMountainLines);
			module = new Filter(module, -0.3f, 1f);
			this.noiseMountainLines = new Multiply(this.noiseMountainLines, module);
			this.noiseMountainLines = new OneMinus(this.noiseMountainLines);
			NoiseDebugUI.StorePlanetNoise(this.noiseMountainLines, "noiseMountainLines");
			this.noiseHillsPatchesMacro = new Perlin(0.032000001519918442 * freqMultiplier, 2.0, 0.5, 5, Rand.Range(0, 2147483647), QualityMode.Medium);
			this.noiseHillsPatchesMicro = new Perlin(0.18999999761581421 * freqMultiplier, 2.0, 0.5, 6, Rand.Range(0, 2147483647), QualityMode.High);
		}

		private void SetupSwampinessNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase input = new Perlin(0.090000003576278687 * freqMultiplier, 2.0, 0.40000000596046448, 6, Rand.Range(0, 2147483647), QualityMode.High);
			ModuleBase input2 = new RidgedMultifractal(0.02500000037252903 * freqMultiplier, 2.0, 6, Rand.Range(0, 2147483647), QualityMode.High);
			input = new ScaleBias(0.5, 0.5, input);
			input2 = new ScaleBias(0.5, 0.5, input2);
			this.noiseSwampiness = new Multiply(input, input2);
			ModuleBase module = this.noiseElevation;
			FloatRange swampinessMaxElevation = WorldGenStep_Terrain.SwampinessMaxElevation;
			float max = swampinessMaxElevation.max;
			FloatRange swampinessMaxElevation2 = WorldGenStep_Terrain.SwampinessMaxElevation;
			InverseLerp rhs = new InverseLerp(module, max, swampinessMaxElevation2.min);
			this.noiseSwampiness = new Multiply(this.noiseSwampiness, rhs);
			ModuleBase module2 = this.noiseRainfall;
			FloatRange swampinessMinRainfall = WorldGenStep_Terrain.SwampinessMinRainfall;
			float min = swampinessMinRainfall.min;
			FloatRange swampinessMinRainfall2 = WorldGenStep_Terrain.SwampinessMinRainfall;
			InverseLerp rhs2 = new InverseLerp(module2, min, swampinessMinRainfall2.max);
			this.noiseSwampiness = new Multiply(this.noiseSwampiness, rhs2);
			NoiseDebugUI.StorePlanetNoise(this.noiseSwampiness, "noiseSwampiness");
		}

		private Tile GenerateTileFor(int tileID)
		{
			Tile tile = new Tile();
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tileID);
			tile.elevation = this.noiseElevation.GetValue(tileCenter);
			float value = this.noiseMountainLines.GetValue(tileCenter);
			if (value > 0.23499999940395355 || tile.elevation <= 0.0)
			{
				if (tile.elevation > 0.0 && this.noiseHillsPatchesMicro.GetValue(tileCenter) > 0.46000000834465027 && this.noiseHillsPatchesMacro.GetValue(tileCenter) > -0.30000001192092896)
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
			else if (value > 0.11999999731779099)
			{
				switch (Rand.Range(0, 4))
				{
				case 0:
				{
					tile.hilliness = Hilliness.Flat;
					break;
				}
				case 1:
				{
					tile.hilliness = Hilliness.SmallHills;
					break;
				}
				case 2:
				{
					tile.hilliness = Hilliness.LargeHills;
					break;
				}
				case 3:
				{
					tile.hilliness = Hilliness.Mountainous;
					break;
				}
				}
			}
			else if (value > 0.036299999803304672)
			{
				tile.hilliness = Hilliness.Mountainous;
			}
			else
			{
				tile.hilliness = Hilliness.Impassable;
			}
			Vector2 vector = Find.WorldGrid.LongLatOf(tileID);
			float num = WorldGenStep_Terrain.BaseTemperatureAtLatitude(vector.y);
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
				Log.ErrorOnce(value2 + " rain bad at " + tileID, 694822);
			}
			if (tile.hilliness == Hilliness.Flat || tile.hilliness == Hilliness.SmallHills)
			{
				tile.swampiness = this.noiseSwampiness.GetValue(tileCenter);
			}
			tile.biome = this.BiomeFrom(tile);
			return tile;
		}

		private BiomeDef BiomeFrom(Tile ws)
		{
			List<BiomeDef> allDefsListForReading = DefDatabase<BiomeDef>.AllDefsListForReading;
			BiomeDef biomeDef = null;
			float num = 0f;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				BiomeDef biomeDef2 = allDefsListForReading[i];
				if (biomeDef2.implemented)
				{
					float score = biomeDef2.Worker.GetScore(ws);
					if (score > num || biomeDef == null)
					{
						biomeDef = biomeDef2;
						num = score;
					}
				}
			}
			return biomeDef;
		}

		private static float FertilityFactorFromTemperature(float temp)
		{
			return (float)((!(temp < -15.0)) ? ((!(temp < 30.0)) ? ((!(temp < 50.0)) ? 0.0 : Mathf.InverseLerp(50f, 30f, temp)) : Mathf.InverseLerp(-15f, 30f, temp)) : 0.0);
		}

		private static float BaseTemperatureAtLatitude(float lat)
		{
			float x = (float)(Mathf.Abs(lat) / 90.0);
			return WorldGenStep_Terrain.AvgTempByLatitudeCurve.Evaluate(x);
		}

		private static float TemperatureReductionAtElevation(float elev)
		{
			float result;
			if (elev < 250.0)
			{
				result = 0f;
			}
			else
			{
				float t = (float)((elev - 250.0) / 4750.0);
				result = Mathf.Lerp(0f, 40f, t);
			}
			return result;
		}
	}
}
