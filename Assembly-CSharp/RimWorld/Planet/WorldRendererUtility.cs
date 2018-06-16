using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A4 RID: 1444
	public static class WorldRendererUtility
	{
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x000ED9E0 File Offset: 0x000EBDE0
		public static WorldRenderMode CurrentWorldRenderMode
		{
			get
			{
				WorldRenderMode result;
				if (Find.World == null)
				{
					result = WorldRenderMode.None;
				}
				else if (Current.ProgramState == ProgramState.Playing && Find.CurrentMap == null)
				{
					result = WorldRenderMode.Planet;
				}
				else
				{
					result = Find.World.renderer.wantedMode;
				}
				return result;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001B8A RID: 7050 RVA: 0x000EDA34 File Offset: 0x000EBE34
		public static bool WorldRenderedNow
		{
			get
			{
				return WorldRendererUtility.CurrentWorldRenderMode != WorldRenderMode.None;
			}
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x000EDA54 File Offset: 0x000EBE54
		public static void UpdateWorldShadersParams()
		{
			Vector3 v = -GenCelestial.CurSunPositionInWorldSpace();
			bool usePlanetDayNightSystem = Find.PlaySettings.usePlanetDayNightSystem;
			float value = (!usePlanetDayNightSystem) ? 0f : 1f;
			Shader.SetGlobalVector(ShaderPropertyIDs.PlanetSunLightDirection, v);
			Shader.SetGlobalFloat(ShaderPropertyIDs.PlanetSunLightEnabled, value);
			WorldMaterials.PlanetGlow.SetFloat(ShaderPropertyIDs.PlanetRadius, 100f);
			WorldMaterials.PlanetGlow.SetFloat(ShaderPropertyIDs.GlowRadius, 8f);
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x000EDAD1 File Offset: 0x000EBED1
		public static void PrintQuadTangentialToPlanet(Vector3 pos, float size, float altOffset, LayerSubMesh subMesh, bool counterClockwise = false, bool randomizeRotation = false, bool printUVs = true)
		{
			WorldRendererUtility.PrintQuadTangentialToPlanet(pos, pos, size, altOffset, subMesh, counterClockwise, randomizeRotation, printUVs);
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x000EDAE4 File Offset: 0x000EBEE4
		public static void PrintQuadTangentialToPlanet(Vector3 pos, Vector3 posForTangents, float size, float altOffset, LayerSubMesh subMesh, bool counterClockwise = false, bool randomizeRotation = false, bool printUVs = true)
		{
			Vector3 a;
			Vector3 a2;
			WorldRendererUtility.GetTangentsToPlanet(posForTangents, out a, out a2, randomizeRotation);
			Vector3 normalized = posForTangents.normalized;
			float d = size * 0.5f;
			Vector3 item = pos - a * d - a2 * d + normalized * altOffset;
			Vector3 item2 = pos - a * d + a2 * d + normalized * altOffset;
			Vector3 item3 = pos + a * d + a2 * d + normalized * altOffset;
			Vector3 item4 = pos + a * d - a2 * d + normalized * altOffset;
			int count = subMesh.verts.Count;
			subMesh.verts.Add(item);
			subMesh.verts.Add(item2);
			subMesh.verts.Add(item3);
			subMesh.verts.Add(item4);
			if (printUVs)
			{
				subMesh.uvs.Add(new Vector2(0f, 0f));
				subMesh.uvs.Add(new Vector2(0f, 1f));
				subMesh.uvs.Add(new Vector2(1f, 1f));
				subMesh.uvs.Add(new Vector2(1f, 0f));
			}
			if (counterClockwise)
			{
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count);
			}
			else
			{
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count + 3);
			}
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000EDD44 File Offset: 0x000EC144
		public static void DrawQuadTangentialToPlanet(Vector3 pos, float size, float altOffset, Material material, bool counterClockwise = false, bool useSkyboxLayer = false, MaterialPropertyBlock propertyBlock = null)
		{
			if (material == null)
			{
				Log.Warning("Tried to draw quad with null material.", false);
			}
			else
			{
				Vector3 normalized = pos.normalized;
				Vector3 vector;
				if (counterClockwise)
				{
					vector = -normalized;
				}
				else
				{
					vector = normalized;
				}
				Quaternion q = Quaternion.LookRotation(Vector3.Cross(vector, Vector3.up), vector);
				Vector3 s = new Vector3(size, 1f, size);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(pos + normalized * altOffset, q, s);
				int layer = (!useSkyboxLayer) ? WorldCameraManager.WorldLayer : WorldCameraManager.WorldSkyboxLayer;
				if (propertyBlock != null)
				{
					Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0, propertyBlock);
				}
				else
				{
					Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer);
				}
			}
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x000EDE10 File Offset: 0x000EC210
		public static void GetTangentsToPlanet(Vector3 pos, out Vector3 first, out Vector3 second, bool randomizeRotation = false)
		{
			Vector3 upwards;
			if (randomizeRotation)
			{
				upwards = Rand.UnitVector3;
			}
			else
			{
				upwards = Vector3.up;
			}
			Quaternion rotation = Quaternion.LookRotation(pos.normalized, upwards);
			first = rotation * Vector3.up;
			second = rotation * Vector3.right;
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x000EDE68 File Offset: 0x000EC268
		public static Vector3 ProjectOnQuadTangentialToPlanet(Vector3 center, Vector2 point)
		{
			Vector3 a;
			Vector3 a2;
			WorldRendererUtility.GetTangentsToPlanet(center, out a, out a2, false);
			return point.x * a + point.y * a2;
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000EDEA8 File Offset: 0x000EC2A8
		public static void GetTangentialVectorFacing(Vector3 root, Vector3 pointToFace, out Vector3 forward, out Vector3 right)
		{
			Quaternion rotation = Quaternion.LookRotation(root, pointToFace);
			forward = rotation * Vector3.up;
			right = rotation * Vector3.left;
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x000EDEE0 File Offset: 0x000EC2E0
		public static void PrintTextureAtlasUVs(int indexX, int indexY, int numX, int numY, LayerSubMesh subMesh)
		{
			float num = 1f / (float)numX;
			float num2 = 1f / (float)numY;
			float num3 = (float)indexX * num;
			float num4 = (float)indexY * num2;
			subMesh.uvs.Add(new Vector2(num3, num4));
			subMesh.uvs.Add(new Vector2(num3, num4 + num2));
			subMesh.uvs.Add(new Vector2(num3 + num, num4 + num2));
			subMesh.uvs.Add(new Vector2(num3 + num, num4));
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000EDF74 File Offset: 0x000EC374
		public static bool HiddenBehindTerrainNow(Vector3 pos)
		{
			Vector3 normalized = pos.normalized;
			Vector3 currentlyLookingAtPointOnSphere = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
			return Vector3.Angle(normalized, currentlyLookingAtPointOnSphere) > 73f;
		}
	}
}
