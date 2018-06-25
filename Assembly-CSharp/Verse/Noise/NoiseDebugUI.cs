using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F71 RID: 3953
	public static class NoiseDebugUI
	{
		// Token: 0x04003EC8 RID: 16072
		private static List<NoiseDebugUI.Noise2D> noises2D = new List<NoiseDebugUI.Noise2D>();

		// Token: 0x04003EC9 RID: 16073
		private static List<NoiseDebugUI.NoisePlanet> planetNoises = new List<NoiseDebugUI.NoisePlanet>();

		// Token: 0x04003ECA RID: 16074
		private static Mesh planetNoiseMesh;

		// Token: 0x04003ECB RID: 16075
		private static NoiseDebugUI.NoisePlanet currentPlanetNoise;

		// Token: 0x04003ECC RID: 16076
		private static NoiseDebugUI.NoisePlanet lastDrawnPlanetNoise;

		// Token: 0x04003ECD RID: 16077
		private static List<Color32> planetNoiseMeshColors = new List<Color32>();

		// Token: 0x04003ECE RID: 16078
		private static List<Vector3> planetNoiseMeshVerts;

		// Token: 0x17000F48 RID: 3912
		// (set) Token: 0x06005F71 RID: 24433 RVA: 0x0030ABB6 File Offset: 0x00308FB6
		public static IntVec2 RenderSize
		{
			set
			{
				NoiseRenderer.renderSize = value;
			}
		}

		// Token: 0x06005F72 RID: 24434 RVA: 0x0030ABBF File Offset: 0x00308FBF
		public static void StoreNoiseRender(ModuleBase noise, string name, IntVec2 renderSize)
		{
			NoiseDebugUI.RenderSize = renderSize;
			NoiseDebugUI.StoreNoiseRender(noise, name);
		}

		// Token: 0x06005F73 RID: 24435 RVA: 0x0030ABD0 File Offset: 0x00308FD0
		public static void StoreNoiseRender(ModuleBase noise, string name)
		{
			if (Prefs.DevMode && DebugViewSettings.drawRecordedNoise)
			{
				NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(noise, name);
				NoiseDebugUI.noises2D.Add(item);
			}
		}

		// Token: 0x06005F74 RID: 24436 RVA: 0x0030AC0C File Offset: 0x0030900C
		public static void StorePlanetNoise(ModuleBase noise, string name)
		{
			if (Prefs.DevMode && DebugViewSettings.drawRecordedNoise)
			{
				NoiseDebugUI.NoisePlanet item = new NoiseDebugUI.NoisePlanet(noise, name);
				NoiseDebugUI.planetNoises.Add(item);
			}
		}

		// Token: 0x06005F75 RID: 24437 RVA: 0x0030AC48 File Offset: 0x00309048
		public static void NoiseDebugOnGUI()
		{
			if (Prefs.DevMode && DebugViewSettings.drawRecordedNoise)
			{
				if (Widgets.ButtonText(new Rect(0f, 40f, 200f, 30f), "Clear noise renders", true, false, true))
				{
					NoiseDebugUI.Clear();
				}
				if (Widgets.ButtonText(new Rect(200f, 40f, 200f, 30f), "Hide noise renders", true, false, true))
				{
					DebugViewSettings.drawRecordedNoise = false;
				}
				if (WorldRendererUtility.WorldRenderedNow)
				{
					if (NoiseDebugUI.planetNoises.Any<NoiseDebugUI.NoisePlanet>())
					{
						if (Widgets.ButtonText(new Rect(400f, 40f, 200f, 30f), "Next planet noise", true, false, true))
						{
							if (Event.current.button == 1)
							{
								if (NoiseDebugUI.currentPlanetNoise == null || NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == -1)
								{
									NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.Count - 1];
								}
								else if (NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == 0)
								{
									NoiseDebugUI.currentPlanetNoise = null;
								}
								else
								{
									NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) - 1];
								}
							}
							else if (NoiseDebugUI.currentPlanetNoise == null || NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == -1)
							{
								NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[0];
							}
							else if (NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == NoiseDebugUI.planetNoises.Count - 1)
							{
								NoiseDebugUI.currentPlanetNoise = null;
							}
							else
							{
								NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) + 1];
							}
						}
					}
					if (NoiseDebugUI.currentPlanetNoise != null)
					{
						Rect rect = new Rect(605f, 40f, 300f, 30f);
						Text.Font = GameFont.Medium;
						Widgets.Label(rect, NoiseDebugUI.currentPlanetNoise.name);
						Text.Font = GameFont.Small;
					}
				}
				float num = 0f;
				float num2 = 90f;
				Text.Font = GameFont.Tiny;
				foreach (NoiseDebugUI.Noise2D noise2D in NoiseDebugUI.noises2D)
				{
					Texture2D texture = noise2D.Texture;
					if (num + (float)texture.width + 5f > (float)UI.screenWidth)
					{
						num = 0f;
						num2 += (float)(texture.height + 5 + 25);
					}
					Rect position = new Rect(num, num2, (float)texture.width, (float)texture.height);
					GUI.DrawTexture(position, texture);
					Rect rect2 = new Rect(num, num2 - 15f, (float)texture.width, (float)texture.height);
					GUI.color = Color.black;
					Widgets.Label(rect2, noise2D.name);
					GUI.color = Color.white;
					Widgets.Label(new Rect(rect2.x + 1f, rect2.y + 1f, rect2.width, rect2.height), noise2D.name);
					num += (float)(texture.width + 5);
				}
			}
		}

		// Token: 0x06005F76 RID: 24438 RVA: 0x0030AFC0 File Offset: 0x003093C0
		public static void RenderPlanetNoise()
		{
			if (Prefs.DevMode && DebugViewSettings.drawRecordedNoise)
			{
				if (NoiseDebugUI.currentPlanetNoise != null)
				{
					if (NoiseDebugUI.planetNoiseMesh == null)
					{
						List<int> triangles;
						SphereGenerator.Generate(6, 100.3f, Vector3.forward, 360f, out NoiseDebugUI.planetNoiseMeshVerts, out triangles);
						NoiseDebugUI.planetNoiseMesh = new Mesh();
						NoiseDebugUI.planetNoiseMesh.name = "NoiseDebugUI";
						NoiseDebugUI.planetNoiseMesh.SetVertices(NoiseDebugUI.planetNoiseMeshVerts);
						NoiseDebugUI.planetNoiseMesh.SetTriangles(triangles, 0);
						NoiseDebugUI.lastDrawnPlanetNoise = null;
					}
					if (NoiseDebugUI.lastDrawnPlanetNoise != NoiseDebugUI.currentPlanetNoise)
					{
						NoiseDebugUI.UpdatePlanetNoiseVertexColors();
						NoiseDebugUI.lastDrawnPlanetNoise = NoiseDebugUI.currentPlanetNoise;
					}
					Graphics.DrawMesh(NoiseDebugUI.planetNoiseMesh, Vector3.zero, Quaternion.identity, WorldMaterials.VertexColor, WorldCameraManager.WorldLayer);
				}
			}
		}

		// Token: 0x06005F77 RID: 24439 RVA: 0x0030B09C File Offset: 0x0030949C
		public static void Clear()
		{
			for (int i = 0; i < NoiseDebugUI.noises2D.Count; i++)
			{
				UnityEngine.Object.Destroy(NoiseDebugUI.noises2D[i].Texture);
			}
			NoiseDebugUI.noises2D.Clear();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x06005F78 RID: 24440 RVA: 0x0030B0EC File Offset: 0x003094EC
		public static void ClearPlanetNoises()
		{
			NoiseDebugUI.planetNoises.Clear();
			NoiseDebugUI.currentPlanetNoise = null;
			NoiseDebugUI.lastDrawnPlanetNoise = null;
			if (NoiseDebugUI.planetNoiseMesh != null)
			{
				Mesh localPlanetNoiseMesh = NoiseDebugUI.planetNoiseMesh;
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					UnityEngine.Object.Destroy(localPlanetNoiseMesh);
				});
				NoiseDebugUI.planetNoiseMesh = null;
			}
		}

		// Token: 0x06005F79 RID: 24441 RVA: 0x0030B14C File Offset: 0x0030954C
		private static void UpdatePlanetNoiseVertexColors()
		{
			NoiseDebugUI.planetNoiseMeshColors.Clear();
			for (int i = 0; i < NoiseDebugUI.planetNoiseMeshVerts.Count; i++)
			{
				float value = NoiseDebugUI.currentPlanetNoise.noise.GetValue(NoiseDebugUI.planetNoiseMeshVerts[i]);
				byte b = (byte)Mathf.Clamp((value * 0.5f + 0.5f) * 255f, 0f, 255f);
				NoiseDebugUI.planetNoiseMeshColors.Add(new Color32(b, b, b, byte.MaxValue));
			}
			NoiseDebugUI.planetNoiseMesh.SetColors(NoiseDebugUI.planetNoiseMeshColors);
		}

		// Token: 0x02000F72 RID: 3954
		private class Noise2D
		{
			// Token: 0x04003ECF RID: 16079
			private Texture2D tex;

			// Token: 0x04003ED0 RID: 16080
			public string name;

			// Token: 0x04003ED1 RID: 16081
			private ModuleBase noise;

			// Token: 0x06005F7B RID: 24443 RVA: 0x0030B206 File Offset: 0x00309606
			public Noise2D(ModuleBase noise, string name)
			{
				this.noise = noise;
				this.name = name;
			}

			// Token: 0x17000F49 RID: 3913
			// (get) Token: 0x06005F7C RID: 24444 RVA: 0x0030B220 File Offset: 0x00309620
			public Texture2D Texture
			{
				get
				{
					if (this.tex == null)
					{
						this.tex = NoiseRenderer.NoiseRendered(this.noise);
					}
					return this.tex;
				}
			}
		}

		// Token: 0x02000F73 RID: 3955
		private class NoisePlanet
		{
			// Token: 0x04003ED2 RID: 16082
			public string name;

			// Token: 0x04003ED3 RID: 16083
			public ModuleBase noise;

			// Token: 0x06005F7D RID: 24445 RVA: 0x0030B25D File Offset: 0x0030965D
			public NoisePlanet(ModuleBase noise, string name)
			{
				this.name = name;
				this.noise = noise;
			}
		}
	}
}
