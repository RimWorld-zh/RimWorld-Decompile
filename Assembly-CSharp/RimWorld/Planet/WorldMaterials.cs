using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public static class WorldMaterials
	{
		public static readonly Material WorldTerrain = MatLoader.LoadMat("World/WorldTerrain", 3500);

		public static readonly Material WorldIce = MatLoader.LoadMat("World/WorldIce", 3500);

		public static readonly Material WorldOcean = MatLoader.LoadMat("World/WorldOcean", 3500);

		public static readonly Material UngeneratedPlanetParts = MatLoader.LoadMat("World/UngeneratedPlanetParts", 3500);

		public static readonly Material Rivers = MatLoader.LoadMat("World/Rivers", 3530);

		public static readonly Material RiversBorder = MatLoader.LoadMat("World/RiversBorder", 3520);

		public static readonly Material Roads = MatLoader.LoadMat("World/Roads", 3540);

		public static int DebugTileRenderQueue = 3510;

		public static int WorldObjectRenderQueue = 3550;

		public static int WorldLineRenderQueue = 3590;

		public static int DynamicObjectRenderQueue = 3600;

		public static int FeatureNameRenderQueue = 3610;

		public static readonly Material MouseTile = MaterialPool.MatFrom("World/MouseTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		public static readonly Material SelectedTile = MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		public static readonly Material CurrentMapTile = MaterialPool.MatFrom("World/CurrentMapTile", ShaderDatabase.WorldOverlayTransparent, 3560);

		public static readonly Material Stars = MatLoader.LoadMat("World/Stars", -1);

		public static readonly Material Sun = MatLoader.LoadMat("World/Sun", -1);

		public static readonly Material PlanetGlow = MatLoader.LoadMat("World/PlanetGlow", -1);

		public static readonly Material SmallHills = MaterialPool.MatFrom("World/Hills/SmallHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		public static readonly Material LargeHills = MaterialPool.MatFrom("World/Hills/LargeHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		public static readonly Material Mountains = MaterialPool.MatFrom("World/Hills/Mountains", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		public static readonly Material ImpassableMountains = MaterialPool.MatFrom("World/Hills/Impassable", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		public static readonly Material VertexColor = MatLoader.LoadMat("World/WorldVertexColor", -1);

		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent, 3560);

		private static int NumMatsPerMode = 50;

		public static Material OverlayModeMatOcean = SolidColorMaterials.NewSolidColorMaterial(new Color(0.09f, 0.18f, 0.2f), ShaderDatabase.Transparent);

		private static Material[] matsFertility;

		private static readonly Color[] FertilitySpectrum = new Color[]
		{
			new Color(0f, 1f, 0f, 0f),
			new Color(0f, 1f, 0f, 0.5f)
		};

		private const float TempRange = 50f;

		private static Material[] matsTemperature;

		private static readonly Color[] TemperatureSpectrum = new Color[]
		{
			new Color(1f, 1f, 1f),
			new Color(0f, 0f, 1f),
			new Color(0.25f, 0.25f, 1f),
			new Color(0.6f, 0.6f, 1f),
			new Color(0.5f, 0.5f, 0.5f),
			new Color(0.5f, 0.3f, 0f),
			new Color(1f, 0.6f, 0.18f),
			new Color(1f, 0f, 0f)
		};

		private const float ElevationMax = 5000f;

		private static Material[] matsElevation;

		private static readonly Color[] ElevationSpectrum = new Color[]
		{
			new Color(0.224f, 0.18f, 0.15f),
			new Color(0.447f, 0.369f, 0.298f),
			new Color(0.6f, 0.6f, 0.6f),
			new Color(1f, 1f, 1f)
		};

		private const float RainfallMax = 5000f;

		private static Material[] matsRainfall;

		private static readonly Color[] RainfallSpectrum = new Color[]
		{
			new Color(0.9f, 0.9f, 0.9f),
			GenColor.FromBytes(190, 190, 190, 255),
			new Color(0.58f, 0.58f, 0.58f),
			GenColor.FromBytes(196, 112, 110, 255),
			GenColor.FromBytes(200, 179, 150, 255),
			GenColor.FromBytes(255, 199, 117, 255),
			GenColor.FromBytes(255, 255, 84, 255),
			GenColor.FromBytes(145, 255, 253, 255),
			GenColor.FromBytes(0, 255, 0, 255),
			GenColor.FromBytes(63, 198, 55, 255),
			GenColor.FromBytes(13, 150, 5, 255),
			GenColor.FromBytes(5, 112, 94, 255)
		};

		static WorldMaterials()
		{
			WorldMaterials.GenerateMats(ref WorldMaterials.matsFertility, WorldMaterials.FertilitySpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsTemperature, WorldMaterials.TemperatureSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsElevation, WorldMaterials.ElevationSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsRainfall, WorldMaterials.RainfallSpectrum, WorldMaterials.NumMatsPerMode);
		}

		public static Material CurTargetingMat
		{
			get
			{
				WorldMaterials.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return WorldMaterials.TargetSquareMatSingle;
			}
		}

		private static void GenerateMats(ref Material[] mats, Color[] colorSpectrum, int numMats)
		{
			mats = new Material[numMats];
			for (int i = 0; i < numMats; i++)
			{
				mats[i] = MatsFromSpectrum.Get(colorSpectrum, (float)i / (float)numMats);
			}
		}

		public static Material MatForFertilityOverlay(float fert)
		{
			int value = Mathf.FloorToInt(fert * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsFertility[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		public static Material MatForTemperature(float temp)
		{
			float num = Mathf.InverseLerp(-50f, 50f, temp);
			int value = Mathf.FloorToInt(num * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsTemperature[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		public static Material MatForElevation(float elev)
		{
			float num = Mathf.InverseLerp(0f, 5000f, elev);
			int value = Mathf.FloorToInt(num * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsElevation[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		public static Material MatForRainfallOverlay(float rain)
		{
			float num = Mathf.InverseLerp(0f, 5000f, rain);
			int value = Mathf.FloorToInt(num * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsRainfall[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}
	}
}
