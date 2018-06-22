using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200059D RID: 1437
	[StaticConstructorOnStartup]
	public static class WorldMaterials
	{
		// Token: 0x06001B70 RID: 7024 RVA: 0x000ECD84 File Offset: 0x000EB184
		static WorldMaterials()
		{
			WorldMaterials.GenerateMats(ref WorldMaterials.matsFertility, WorldMaterials.FertilitySpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsTemperature, WorldMaterials.TemperatureSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsElevation, WorldMaterials.ElevationSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsRainfall, WorldMaterials.RainfallSpectrum, WorldMaterials.NumMatsPerMode);
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001B71 RID: 7025 RVA: 0x000ED354 File Offset: 0x000EB754
		public static Material CurTargetingMat
		{
			get
			{
				WorldMaterials.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return WorldMaterials.TargetSquareMatSingle;
			}
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x000ED380 File Offset: 0x000EB780
		private static void GenerateMats(ref Material[] mats, Color[] colorSpectrum, int numMats)
		{
			mats = new Material[numMats];
			for (int i = 0; i < numMats; i++)
			{
				mats[i] = MatsFromSpectrum.Get(colorSpectrum, (float)i / (float)numMats);
			}
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x000ED3BC File Offset: 0x000EB7BC
		public static Material MatForFertilityOverlay(float fert)
		{
			int value = Mathf.FloorToInt(fert * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsFertility[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x000ED3F8 File Offset: 0x000EB7F8
		public static Material MatForTemperature(float temp)
		{
			float num = Mathf.InverseLerp(-50f, 50f, temp);
			int value = Mathf.FloorToInt(num * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsTemperature[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x000ED440 File Offset: 0x000EB840
		public static Material MatForElevation(float elev)
		{
			float num = Mathf.InverseLerp(0f, 5000f, elev);
			int value = Mathf.FloorToInt(num * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsElevation[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x000ED488 File Offset: 0x000EB888
		public static Material MatForRainfallOverlay(float rain)
		{
			float num = Mathf.InverseLerp(0f, 5000f, rain);
			int value = Mathf.FloorToInt(num * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsRainfall[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x0400102D RID: 4141
		public static readonly Material WorldTerrain = MatLoader.LoadMat("World/WorldTerrain", 3500);

		// Token: 0x0400102E RID: 4142
		public static readonly Material WorldIce = MatLoader.LoadMat("World/WorldIce", 3500);

		// Token: 0x0400102F RID: 4143
		public static readonly Material WorldOcean = MatLoader.LoadMat("World/WorldOcean", 3500);

		// Token: 0x04001030 RID: 4144
		public static readonly Material UngeneratedPlanetParts = MatLoader.LoadMat("World/UngeneratedPlanetParts", 3500);

		// Token: 0x04001031 RID: 4145
		public static readonly Material Rivers = MatLoader.LoadMat("World/Rivers", 3530);

		// Token: 0x04001032 RID: 4146
		public static readonly Material RiversBorder = MatLoader.LoadMat("World/RiversBorder", 3520);

		// Token: 0x04001033 RID: 4147
		public static readonly Material Roads = MatLoader.LoadMat("World/Roads", 3540);

		// Token: 0x04001034 RID: 4148
		public static int DebugTileRenderQueue = 3510;

		// Token: 0x04001035 RID: 4149
		public static int WorldObjectRenderQueue = 3550;

		// Token: 0x04001036 RID: 4150
		public static int WorldLineRenderQueue = 3590;

		// Token: 0x04001037 RID: 4151
		public static int DynamicObjectRenderQueue = 3600;

		// Token: 0x04001038 RID: 4152
		public static int FeatureNameRenderQueue = 3610;

		// Token: 0x04001039 RID: 4153
		public static readonly Material MouseTile = MaterialPool.MatFrom("World/MouseTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x0400103A RID: 4154
		public static readonly Material SelectedTile = MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x0400103B RID: 4155
		public static readonly Material CurrentMapTile = MaterialPool.MatFrom("World/CurrentMapTile", ShaderDatabase.WorldOverlayTransparent, 3560);

		// Token: 0x0400103C RID: 4156
		public static readonly Material Stars = MatLoader.LoadMat("World/Stars", -1);

		// Token: 0x0400103D RID: 4157
		public static readonly Material Sun = MatLoader.LoadMat("World/Sun", -1);

		// Token: 0x0400103E RID: 4158
		public static readonly Material PlanetGlow = MatLoader.LoadMat("World/PlanetGlow", -1);

		// Token: 0x0400103F RID: 4159
		public static readonly Material SmallHills = MaterialPool.MatFrom("World/Hills/SmallHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x04001040 RID: 4160
		public static readonly Material LargeHills = MaterialPool.MatFrom("World/Hills/LargeHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x04001041 RID: 4161
		public static readonly Material Mountains = MaterialPool.MatFrom("World/Hills/Mountains", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x04001042 RID: 4162
		public static readonly Material ImpassableMountains = MaterialPool.MatFrom("World/Hills/Impassable", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x04001043 RID: 4163
		public static readonly Material VertexColor = MatLoader.LoadMat("World/WorldVertexColor", -1);

		// Token: 0x04001044 RID: 4164
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent, 3560);

		// Token: 0x04001045 RID: 4165
		private static int NumMatsPerMode = 50;

		// Token: 0x04001046 RID: 4166
		public static Material OverlayModeMatOcean = SolidColorMaterials.NewSolidColorMaterial(new Color(0.09f, 0.18f, 0.2f), ShaderDatabase.Transparent);

		// Token: 0x04001047 RID: 4167
		private static Material[] matsFertility;

		// Token: 0x04001048 RID: 4168
		private static readonly Color[] FertilitySpectrum = new Color[]
		{
			new Color(0f, 1f, 0f, 0f),
			new Color(0f, 1f, 0f, 0.5f)
		};

		// Token: 0x04001049 RID: 4169
		private const float TempRange = 50f;

		// Token: 0x0400104A RID: 4170
		private static Material[] matsTemperature;

		// Token: 0x0400104B RID: 4171
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

		// Token: 0x0400104C RID: 4172
		private const float ElevationMax = 5000f;

		// Token: 0x0400104D RID: 4173
		private static Material[] matsElevation;

		// Token: 0x0400104E RID: 4174
		private static readonly Color[] ElevationSpectrum = new Color[]
		{
			new Color(0.224f, 0.18f, 0.15f),
			new Color(0.447f, 0.369f, 0.298f),
			new Color(0.6f, 0.6f, 0.6f),
			new Color(1f, 1f, 1f)
		};

		// Token: 0x0400104F RID: 4175
		private const float RainfallMax = 5000f;

		// Token: 0x04001050 RID: 4176
		private static Material[] matsRainfall;

		// Token: 0x04001051 RID: 4177
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
	}
}
