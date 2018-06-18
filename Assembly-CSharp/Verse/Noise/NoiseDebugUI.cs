using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F6C RID: 3948
	public static class NoiseDebugUI
	{
		// Token: 0x17000F45 RID: 3909
		// (set) Token: 0x06005F3E RID: 24382 RVA: 0x0030824E File Offset: 0x0030664E
		public static IntVec2 RenderSize
		{
			set
			{
				NoiseRenderer.renderSize = value;
			}
		}

		// Token: 0x06005F3F RID: 24383 RVA: 0x00308257 File Offset: 0x00306657
		public static void StoreNoiseRender(ModuleBase noise, string name, IntVec2 renderSize)
		{
			NoiseDebugUI.RenderSize = renderSize;
			NoiseDebugUI.StoreNoiseRender(noise, name);
		}

		// Token: 0x06005F40 RID: 24384 RVA: 0x00308268 File Offset: 0x00306668
		public static void StoreNoiseRender(ModuleBase noise, string name)
		{
			if (Prefs.DevMode && DebugViewSettings.drawRecordedNoise)
			{
				NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(noise, name);
				NoiseDebugUI.noises2D.Add(item);
			}
		}

		// Token: 0x06005F41 RID: 24385 RVA: 0x003082A4 File Offset: 0x003066A4
		public static void StorePlanetNoise(ModuleBase noise, string name)
		{
			if (Prefs.DevMode && DebugViewSettings.drawRecordedNoise)
			{
				NoiseDebugUI.NoisePlanet item = new NoiseDebugUI.NoisePlanet(noise, name);
				NoiseDebugUI.planetNoises.Add(item);
			}
		}

		// Token: 0x06005F42 RID: 24386 RVA: 0x003082E0 File Offset: 0x003066E0
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

		// Token: 0x06005F43 RID: 24387 RVA: 0x00308658 File Offset: 0x00306A58
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

		// Token: 0x06005F44 RID: 24388 RVA: 0x00308734 File Offset: 0x00306B34
		public static void Clear()
		{
			for (int i = 0; i < NoiseDebugUI.noises2D.Count; i++)
			{
				UnityEngine.Object.Destroy(NoiseDebugUI.noises2D[i].Texture);
			}
			NoiseDebugUI.noises2D.Clear();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x06005F45 RID: 24389 RVA: 0x00308784 File Offset: 0x00306B84
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

		// Token: 0x06005F46 RID: 24390 RVA: 0x003087E4 File Offset: 0x00306BE4
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

		// Token: 0x04003EAB RID: 16043
		private static List<NoiseDebugUI.Noise2D> noises2D = new List<NoiseDebugUI.Noise2D>();

		// Token: 0x04003EAC RID: 16044
		private static List<NoiseDebugUI.NoisePlanet> planetNoises = new List<NoiseDebugUI.NoisePlanet>();

		// Token: 0x04003EAD RID: 16045
		private static Mesh planetNoiseMesh;

		// Token: 0x04003EAE RID: 16046
		private static NoiseDebugUI.NoisePlanet currentPlanetNoise;

		// Token: 0x04003EAF RID: 16047
		private static NoiseDebugUI.NoisePlanet lastDrawnPlanetNoise;

		// Token: 0x04003EB0 RID: 16048
		private static List<Color32> planetNoiseMeshColors = new List<Color32>();

		// Token: 0x04003EB1 RID: 16049
		private static List<Vector3> planetNoiseMeshVerts;

		// Token: 0x02000F6D RID: 3949
		private class Noise2D
		{
			// Token: 0x06005F48 RID: 24392 RVA: 0x0030889E File Offset: 0x00306C9E
			public Noise2D(ModuleBase noise, string name)
			{
				this.noise = noise;
				this.name = name;
			}

			// Token: 0x17000F46 RID: 3910
			// (get) Token: 0x06005F49 RID: 24393 RVA: 0x003088B8 File Offset: 0x00306CB8
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

			// Token: 0x04003EB2 RID: 16050
			private Texture2D tex;

			// Token: 0x04003EB3 RID: 16051
			public string name;

			// Token: 0x04003EB4 RID: 16052
			private ModuleBase noise;
		}

		// Token: 0x02000F6E RID: 3950
		private class NoisePlanet
		{
			// Token: 0x06005F4A RID: 24394 RVA: 0x003088F5 File Offset: 0x00306CF5
			public NoisePlanet(ModuleBase noise, string name)
			{
				this.name = name;
				this.noise = noise;
			}

			// Token: 0x04003EB5 RID: 16053
			public string name;

			// Token: 0x04003EB6 RID: 16054
			public ModuleBase noise;
		}
	}
}
