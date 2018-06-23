using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F6C RID: 3948
	public static class NoiseDebugUI
	{
		// Token: 0x04003EBD RID: 16061
		private static List<NoiseDebugUI.Noise2D> noises2D = new List<NoiseDebugUI.Noise2D>();

		// Token: 0x04003EBE RID: 16062
		private static List<NoiseDebugUI.NoisePlanet> planetNoises = new List<NoiseDebugUI.NoisePlanet>();

		// Token: 0x04003EBF RID: 16063
		private static Mesh planetNoiseMesh;

		// Token: 0x04003EC0 RID: 16064
		private static NoiseDebugUI.NoisePlanet currentPlanetNoise;

		// Token: 0x04003EC1 RID: 16065
		private static NoiseDebugUI.NoisePlanet lastDrawnPlanetNoise;

		// Token: 0x04003EC2 RID: 16066
		private static List<Color32> planetNoiseMeshColors = new List<Color32>();

		// Token: 0x04003EC3 RID: 16067
		private static List<Vector3> planetNoiseMeshVerts;

		// Token: 0x17000F49 RID: 3913
		// (set) Token: 0x06005F67 RID: 24423 RVA: 0x0030A2F2 File Offset: 0x003086F2
		public static IntVec2 RenderSize
		{
			set
			{
				NoiseRenderer.renderSize = value;
			}
		}

		// Token: 0x06005F68 RID: 24424 RVA: 0x0030A2FB File Offset: 0x003086FB
		public static void StoreNoiseRender(ModuleBase noise, string name, IntVec2 renderSize)
		{
			NoiseDebugUI.RenderSize = renderSize;
			NoiseDebugUI.StoreNoiseRender(noise, name);
		}

		// Token: 0x06005F69 RID: 24425 RVA: 0x0030A30C File Offset: 0x0030870C
		public static void StoreNoiseRender(ModuleBase noise, string name)
		{
			if (Prefs.DevMode && DebugViewSettings.drawRecordedNoise)
			{
				NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(noise, name);
				NoiseDebugUI.noises2D.Add(item);
			}
		}

		// Token: 0x06005F6A RID: 24426 RVA: 0x0030A348 File Offset: 0x00308748
		public static void StorePlanetNoise(ModuleBase noise, string name)
		{
			if (Prefs.DevMode && DebugViewSettings.drawRecordedNoise)
			{
				NoiseDebugUI.NoisePlanet item = new NoiseDebugUI.NoisePlanet(noise, name);
				NoiseDebugUI.planetNoises.Add(item);
			}
		}

		// Token: 0x06005F6B RID: 24427 RVA: 0x0030A384 File Offset: 0x00308784
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

		// Token: 0x06005F6C RID: 24428 RVA: 0x0030A6FC File Offset: 0x00308AFC
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

		// Token: 0x06005F6D RID: 24429 RVA: 0x0030A7D8 File Offset: 0x00308BD8
		public static void Clear()
		{
			for (int i = 0; i < NoiseDebugUI.noises2D.Count; i++)
			{
				UnityEngine.Object.Destroy(NoiseDebugUI.noises2D[i].Texture);
			}
			NoiseDebugUI.noises2D.Clear();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x06005F6E RID: 24430 RVA: 0x0030A828 File Offset: 0x00308C28
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

		// Token: 0x06005F6F RID: 24431 RVA: 0x0030A888 File Offset: 0x00308C88
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

		// Token: 0x02000F6D RID: 3949
		private class Noise2D
		{
			// Token: 0x04003EC4 RID: 16068
			private Texture2D tex;

			// Token: 0x04003EC5 RID: 16069
			public string name;

			// Token: 0x04003EC6 RID: 16070
			private ModuleBase noise;

			// Token: 0x06005F71 RID: 24433 RVA: 0x0030A942 File Offset: 0x00308D42
			public Noise2D(ModuleBase noise, string name)
			{
				this.noise = noise;
				this.name = name;
			}

			// Token: 0x17000F4A RID: 3914
			// (get) Token: 0x06005F72 RID: 24434 RVA: 0x0030A95C File Offset: 0x00308D5C
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

		// Token: 0x02000F6E RID: 3950
		private class NoisePlanet
		{
			// Token: 0x04003EC7 RID: 16071
			public string name;

			// Token: 0x04003EC8 RID: 16072
			public ModuleBase noise;

			// Token: 0x06005F73 RID: 24435 RVA: 0x0030A999 File Offset: 0x00308D99
			public NoisePlanet(ModuleBase noise, string name)
			{
				this.name = name;
				this.noise = noise;
			}
		}
	}
}
