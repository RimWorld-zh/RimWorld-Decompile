using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F3E RID: 3902
	[StaticConstructorOnStartup]
	public static class GenDraw
	{
		// Token: 0x04003DE4 RID: 15844
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent);

		// Token: 0x04003DE5 RID: 15845
		private const float TargetPulseFrequency = 8f;

		// Token: 0x04003DE6 RID: 15846
		public static readonly string LineTexPath = "UI/Overlays/ThingLine";

		// Token: 0x04003DE7 RID: 15847
		public static readonly string OneSidedLineTexPath = "UI/Overlays/OneSidedLine";

		// Token: 0x04003DE8 RID: 15848
		private static readonly Material LineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.white);

		// Token: 0x04003DE9 RID: 15849
		private static readonly Material LineMatRed = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.red);

		// Token: 0x04003DEA RID: 15850
		private static readonly Material LineMatGreen = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.green);

		// Token: 0x04003DEB RID: 15851
		private static readonly Material LineMatBlue = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.blue);

		// Token: 0x04003DEC RID: 15852
		private static readonly Material LineMatMagenta = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.magenta);

		// Token: 0x04003DED RID: 15853
		private static readonly Material LineMatYellow = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.yellow);

		// Token: 0x04003DEE RID: 15854
		private static readonly Material LineMatCyan = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.cyan);

		// Token: 0x04003DEF RID: 15855
		private static readonly Material LineMatMetaOverlay = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.MetaOverlay);

		// Token: 0x04003DF0 RID: 15856
		private static readonly Material WorldLineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x04003DF1 RID: 15857
		private static readonly Material OneSidedWorldLineMatWhite = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x04003DF2 RID: 15858
		private const float LineWidth = 0.2f;

		// Token: 0x04003DF3 RID: 15859
		private const float BaseWorldLineWidth = 0.2f;

		// Token: 0x04003DF4 RID: 15860
		public static readonly Material InteractionCellMaterial = MaterialPool.MatFrom("UI/Overlays/InteractionCell", ShaderDatabase.Transparent);

		// Token: 0x04003DF5 RID: 15861
		private static readonly Color InteractionCellIntensity = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x04003DF6 RID: 15862
		private static List<int> cachedEdgeTiles = new List<int>();

		// Token: 0x04003DF7 RID: 15863
		private static int cachedEdgeTilesForCenter = -1;

		// Token: 0x04003DF8 RID: 15864
		private static int cachedEdgeTilesForRadius = -1;

		// Token: 0x04003DF9 RID: 15865
		private static int cachedEdgeTilesForWorldSeed = -1;

		// Token: 0x04003DFA RID: 15866
		private static List<IntVec3> ringDrawCells = new List<IntVec3>();

		// Token: 0x04003DFB RID: 15867
		private static bool maxRadiusMessaged = false;

		// Token: 0x04003DFC RID: 15868
		private static BoolGrid fieldGrid;

		// Token: 0x04003DFD RID: 15869
		private static bool[] rotNeeded = new bool[4];

		// Token: 0x04003DFE RID: 15870
		private static readonly Material AimPieMaterial = SolidColorMaterials.SimpleSolidColorMaterial(new Color(1f, 1f, 1f, 0.3f), false);

		// Token: 0x04003DFF RID: 15871
		private static readonly Material ArrowMatWhite = MaterialPool.MatFrom("UI/Overlays/Arrow", ShaderDatabase.CutoutFlying, Color.white);

		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06005DF5 RID: 24053 RVA: 0x002FCC9C File Offset: 0x002FB09C
		public static Material CurTargetingMat
		{
			get
			{
				GenDraw.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return GenDraw.TargetSquareMatSingle;
			}
		}

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06005DF6 RID: 24054 RVA: 0x002FCCC8 File Offset: 0x002FB0C8
		public static Color CurTargetingColor
		{
			get
			{
				float num = (float)Math.Sin((double)(Time.time * 8f));
				num *= 0.2f;
				num += 0.8f;
				return new Color(1f, num, num);
			}
		}

		// Token: 0x06005DF7 RID: 24055 RVA: 0x002FCD0C File Offset: 0x002FB10C
		public static void DrawNoBuildEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(10);
		}

		// Token: 0x06005DF8 RID: 24056 RVA: 0x002FCD16 File Offset: 0x002FB116
		public static void DrawNoZoneEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(5);
		}

		// Token: 0x06005DF9 RID: 24057 RVA: 0x002FCD20 File Offset: 0x002FB120
		private static void DrawMapEdgeLines(int edgeDist)
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			IntVec3 size = Find.CurrentMap.Size;
			Vector3 vector = new Vector3((float)edgeDist, y, (float)edgeDist);
			Vector3 vector2 = new Vector3((float)edgeDist, y, (float)(size.z - edgeDist));
			Vector3 vector3 = new Vector3((float)(size.x - edgeDist), y, (float)(size.z - edgeDist));
			Vector3 vector4 = new Vector3((float)(size.x - edgeDist), y, (float)edgeDist);
			GenDraw.DrawLineBetween(vector, vector2, GenDraw.LineMatMetaOverlay);
			GenDraw.DrawLineBetween(vector2, vector3, GenDraw.LineMatMetaOverlay);
			GenDraw.DrawLineBetween(vector3, vector4, GenDraw.LineMatMetaOverlay);
			GenDraw.DrawLineBetween(vector4, vector, GenDraw.LineMatMetaOverlay);
		}

		// Token: 0x06005DFA RID: 24058 RVA: 0x002FCDC5 File Offset: 0x002FB1C5
		public static void DrawLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.LineMatWhite);
		}

		// Token: 0x06005DFB RID: 24059 RVA: 0x002FCDD4 File Offset: 0x002FB1D4
		public static void DrawLineBetween(Vector3 A, Vector3 B, float layer)
		{
			GenDraw.DrawLineBetween(A + Vector3.up * layer, B + Vector3.up * layer, GenDraw.LineMatWhite);
		}

		// Token: 0x06005DFC RID: 24060 RVA: 0x002FCE03 File Offset: 0x002FB203
		public static void DrawLineBetween(Vector3 A, Vector3 B, SimpleColor color)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.GetLineMat(color));
		}

		// Token: 0x06005DFD RID: 24061 RVA: 0x002FCE14 File Offset: 0x002FB214
		public static void DrawLineBetween(Vector3 A, Vector3 B, Material mat)
		{
			if (Mathf.Abs(A.x - B.x) >= 0.01f || Mathf.Abs(A.z - B.z) >= 0.01f)
			{
				Vector3 pos = (A + B) / 2f;
				if (!(A == B))
				{
					A.y = B.y;
					float z = (A - B).MagnitudeHorizontal();
					Quaternion q = Quaternion.LookRotation(A - B);
					Vector3 s = new Vector3(0.2f, 1f, z);
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(pos, q, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0);
				}
			}
		}

		// Token: 0x06005DFE RID: 24062 RVA: 0x002FCEE0 File Offset: 0x002FB2E0
		public static void DrawCircleOutline(Vector3 center, float radius)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.LineMatWhite);
		}

		// Token: 0x06005DFF RID: 24063 RVA: 0x002FCEEF File Offset: 0x002FB2EF
		public static void DrawCircleOutline(Vector3 center, float radius, SimpleColor color)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.GetLineMat(color));
		}

		// Token: 0x06005E00 RID: 24064 RVA: 0x002FCF00 File Offset: 0x002FB300
		public static void DrawCircleOutline(Vector3 center, float radius, Material material)
		{
			int num = Mathf.Clamp(Mathf.RoundToInt(24f * radius), 12, 48);
			float num2 = 0f;
			float num3 = 6.28318548f / (float)num;
			Vector3 vector = center;
			Vector3 a = center;
			for (int i = 0; i < num + 2; i++)
			{
				if (i >= 2)
				{
					GenDraw.DrawLineBetween(a, vector, material);
				}
				a = vector;
				vector = center;
				vector.x += Mathf.Cos(num2) * radius;
				vector.z += Mathf.Sin(num2) * radius;
				num2 += num3;
			}
		}

		// Token: 0x06005E01 RID: 24065 RVA: 0x002FCF98 File Offset: 0x002FB398
		private static Material GetLineMat(SimpleColor color)
		{
			Material result;
			switch (color)
			{
			case SimpleColor.White:
				result = GenDraw.LineMatWhite;
				break;
			case SimpleColor.Red:
				result = GenDraw.LineMatRed;
				break;
			case SimpleColor.Green:
				result = GenDraw.LineMatGreen;
				break;
			case SimpleColor.Blue:
				result = GenDraw.LineMatBlue;
				break;
			case SimpleColor.Magenta:
				result = GenDraw.LineMatMagenta;
				break;
			case SimpleColor.Yellow:
				result = GenDraw.LineMatYellow;
				break;
			case SimpleColor.Cyan:
				result = GenDraw.LineMatCyan;
				break;
			default:
				result = GenDraw.LineMatWhite;
				break;
			}
			return result;
		}

		// Token: 0x06005E02 RID: 24066 RVA: 0x002FD026 File Offset: 0x002FB426
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawWorldLineBetween(A, B, GenDraw.WorldLineMatWhite, 1f);
		}

		// Token: 0x06005E03 RID: 24067 RVA: 0x002FD03C File Offset: 0x002FB43C
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B, Material material, float widthFactor = 1f)
		{
			if (Mathf.Abs(A.x - B.x) >= 0.005f || Mathf.Abs(A.y - B.y) >= 0.005f || Mathf.Abs(A.z - B.z) >= 0.005f)
			{
				Vector3 pos = (A + B) / 2f;
				float magnitude = (A - B).magnitude;
				Quaternion q = Quaternion.LookRotation(A - B, pos.normalized);
				Vector3 s = new Vector3(0.2f * Find.WorldGrid.averageTileSize * widthFactor, 1f, magnitude);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(pos, q, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, WorldCameraManager.WorldLayer);
			}
		}

		// Token: 0x06005E04 RID: 24068 RVA: 0x002FD124 File Offset: 0x002FB524
		public static void DrawWorldRadiusRing(int center, int radius)
		{
			if (radius >= 0)
			{
				if (GenDraw.cachedEdgeTilesForCenter != center || GenDraw.cachedEdgeTilesForRadius != radius || GenDraw.cachedEdgeTilesForWorldSeed != Find.World.info.Seed)
				{
					GenDraw.cachedEdgeTilesForCenter = center;
					GenDraw.cachedEdgeTilesForRadius = radius;
					GenDraw.cachedEdgeTilesForWorldSeed = Find.World.info.Seed;
					GenDraw.cachedEdgeTiles.Clear();
					Find.WorldFloodFiller.FloodFill(center, (int tile) => true, delegate(int tile, int dist)
					{
						bool result;
						if (dist > radius + 1)
						{
							result = true;
						}
						else
						{
							if (dist == radius + 1)
							{
								GenDraw.cachedEdgeTiles.Add(tile);
							}
							result = false;
						}
						return result;
					}, int.MaxValue, null);
					WorldGrid worldGrid = Find.WorldGrid;
					Vector3 c = worldGrid.GetTileCenter(center);
					Vector3 n = c.normalized;
					GenDraw.cachedEdgeTiles.Sort(delegate(int a, int b)
					{
						float num = Vector3.Dot(n, Vector3.Cross(worldGrid.GetTileCenter(a) - c, worldGrid.GetTileCenter(b) - c));
						int result;
						if (Mathf.Abs(num) < 0.0001f)
						{
							result = 0;
						}
						else if (num < 0f)
						{
							result = -1;
						}
						else
						{
							result = 1;
						}
						return result;
					});
				}
				GenDraw.DrawWorldLineStrip(GenDraw.cachedEdgeTiles, GenDraw.OneSidedWorldLineMatWhite, 5f);
			}
		}

		// Token: 0x06005E05 RID: 24069 RVA: 0x002FD254 File Offset: 0x002FB654
		public static void DrawWorldLineStrip(List<int> edgeTiles, Material material, float widthFactor)
		{
			if (edgeTiles.Count >= 3)
			{
				WorldGrid worldGrid = Find.WorldGrid;
				float d = 0.05f;
				for (int i = 0; i < edgeTiles.Count; i++)
				{
					int index = (i != 0) ? (i - 1) : (edgeTiles.Count - 1);
					int num = edgeTiles[index];
					int num2 = edgeTiles[i];
					if (worldGrid.IsNeighbor(num, num2))
					{
						Vector3 a = worldGrid.GetTileCenter(num);
						Vector3 vector = worldGrid.GetTileCenter(num2);
						a += a.normalized * d;
						vector += vector.normalized * d;
						GenDraw.DrawWorldLineBetween(a, vector, material, widthFactor);
					}
				}
			}
		}

		// Token: 0x06005E06 RID: 24070 RVA: 0x002FD320 File Offset: 0x002FB720
		public static void DrawTargetHighlight(LocalTargetInfo targ)
		{
			if (targ.Thing != null)
			{
				GenDraw.DrawTargetingHighlight_Thing(targ.Thing);
			}
			else
			{
				GenDraw.DrawTargetingHighlight_Cell(targ.Cell);
			}
		}

		// Token: 0x06005E07 RID: 24071 RVA: 0x002FD34C File Offset: 0x002FB74C
		private static void DrawTargetingHighlight_Cell(IntVec3 c)
		{
			Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.Building);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x06005E08 RID: 24072 RVA: 0x002FD37C File Offset: 0x002FB77C
		private static void DrawTargetingHighlight_Thing(Thing t)
		{
			Graphics.DrawMesh(MeshPool.plane10, t.TrueCenter() + Altitudes.AltIncVect, t.Rotation.AsQuat, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x06005E09 RID: 24073 RVA: 0x002FD3B8 File Offset: 0x002FB7B8
		public static void DrawTargetingHightlight_Explosion(IntVec3 c, float Radius)
		{
			GenDraw.DrawRadiusRing(c, Radius);
		}

		// Token: 0x06005E0A RID: 24074 RVA: 0x002FD3C4 File Offset: 0x002FB7C4
		public static void DrawInteractionCell(ThingDef tDef, IntVec3 center, Rot4 placingRot)
		{
			if (tDef.hasInteractionCell)
			{
				IntVec3 c = ThingUtility.InteractionCellWhenAt(tDef, center, placingRot, Find.CurrentMap);
				Vector3 vector = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
				Building edifice = c.GetEdifice(Find.CurrentMap);
				if (edifice == null || edifice.def.building == null || !edifice.def.building.isSittable)
				{
					if (tDef.interactionCellGraphic == null && tDef.interactionCellIcon != null)
					{
						ThingDef thingDef = tDef.interactionCellIcon;
						if (thingDef.blueprintDef != null)
						{
							thingDef = thingDef.blueprintDef;
						}
						tDef.interactionCellGraphic = thingDef.graphic.GetColoredVersion(ShaderTypeDefOf.EdgeDetect.Shader, GenDraw.InteractionCellIntensity, Color.white);
					}
					if (tDef.interactionCellGraphic != null)
					{
						tDef.interactionCellGraphic.DrawFromDef(vector, placingRot, tDef.interactionCellIcon, 0f);
					}
					else
					{
						Graphics.DrawMesh(MeshPool.plane10, vector, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
					}
				}
			}
		}

		// Token: 0x06005E0B RID: 24075 RVA: 0x002FD4C8 File Offset: 0x002FB8C8
		public static void DrawRadiusRing(IntVec3 center, float radius)
		{
			if (radius > GenRadial.MaxRadialPatternRadius)
			{
				if (!GenDraw.maxRadiusMessaged)
				{
					Log.Error("Cannot draw radius ring of radius " + radius + ": not enough squares in the precalculated list.", false);
					GenDraw.maxRadiusMessaged = true;
				}
			}
			else
			{
				GenDraw.ringDrawCells.Clear();
				int num = GenRadial.NumCellsInRadius(radius);
				for (int i = 0; i < num; i++)
				{
					GenDraw.ringDrawCells.Add(center + GenRadial.RadialPattern[i]);
				}
				GenDraw.DrawFieldEdges(GenDraw.ringDrawCells);
			}
		}

		// Token: 0x06005E0C RID: 24076 RVA: 0x002FD563 File Offset: 0x002FB963
		public static void DrawFieldEdges(List<IntVec3> cells)
		{
			GenDraw.DrawFieldEdges(cells, Color.white);
		}

		// Token: 0x06005E0D RID: 24077 RVA: 0x002FD574 File Offset: 0x002FB974
		public static void DrawFieldEdges(List<IntVec3> cells, Color color)
		{
			Map currentMap = Find.CurrentMap;
			Material material = MaterialPool.MatFrom(new MaterialRequest
			{
				shader = ShaderDatabase.Transparent,
				color = color,
				BaseTexPath = "UI/Overlays/TargetHighlight_Side"
			});
			material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
			if (GenDraw.fieldGrid == null)
			{
				GenDraw.fieldGrid = new BoolGrid(currentMap);
			}
			else
			{
				GenDraw.fieldGrid.ClearAndResizeTo(currentMap);
			}
			int x = currentMap.Size.x;
			int z = currentMap.Size.z;
			int count = cells.Count;
			for (int i = 0; i < count; i++)
			{
				if (cells[i].InBounds(currentMap))
				{
					GenDraw.fieldGrid[cells[i].x, cells[i].z] = true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				IntVec3 c = cells[j];
				if (c.InBounds(currentMap))
				{
					GenDraw.rotNeeded[0] = (c.z < z - 1 && !GenDraw.fieldGrid[c.x, c.z + 1]);
					GenDraw.rotNeeded[1] = (c.x < x - 1 && !GenDraw.fieldGrid[c.x + 1, c.z]);
					GenDraw.rotNeeded[2] = (c.z > 0 && !GenDraw.fieldGrid[c.x, c.z - 1]);
					GenDraw.rotNeeded[3] = (c.x > 0 && !GenDraw.fieldGrid[c.x - 1, c.z]);
					for (int k = 0; k < 4; k++)
					{
						if (GenDraw.rotNeeded[k])
						{
							Mesh plane = MeshPool.plane10;
							Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
							Rot4 rot = new Rot4(k);
							Graphics.DrawMesh(plane, position, rot.AsQuat, material, 0);
						}
					}
				}
			}
		}

		// Token: 0x06005E0E RID: 24078 RVA: 0x002FD7C4 File Offset: 0x002FBBC4
		public static void DrawAimPie(Thing shooter, LocalTargetInfo target, int degreesWide, float offsetDist)
		{
			float facing = 0f;
			if (target.Cell != shooter.Position)
			{
				if (target.Thing != null)
				{
					facing = (target.Thing.DrawPos - shooter.Position.ToVector3Shifted()).AngleFlat();
				}
				else
				{
					facing = (target.Cell - shooter.Position).AngleFlat;
				}
			}
			GenDraw.DrawAimPieRaw(shooter.DrawPos + new Vector3(0f, offsetDist, 0f), facing, degreesWide);
		}

		// Token: 0x06005E0F RID: 24079 RVA: 0x002FD864 File Offset: 0x002FBC64
		public static void DrawAimPieRaw(Vector3 center, float facing, int degreesWide)
		{
			if (degreesWide > 0)
			{
				if (degreesWide > 360)
				{
					degreesWide = 360;
				}
				center += Quaternion.AngleAxis(facing, Vector3.up) * Vector3.forward * 0.8f;
				Graphics.DrawMesh(MeshPool.pies[degreesWide], center, Quaternion.AngleAxis(facing + (float)(degreesWide / 2) - 90f, Vector3.up), GenDraw.AimPieMaterial, 0);
			}
		}

		// Token: 0x06005E10 RID: 24080 RVA: 0x002FD8E0 File Offset: 0x002FBCE0
		public static void DrawCooldownCircle(Vector3 center, float radius)
		{
			Vector3 s = new Vector3(radius, 1f, radius);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(center, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, GenDraw.AimPieMaterial, 0);
		}

		// Token: 0x06005E11 RID: 24081 RVA: 0x002FD924 File Offset: 0x002FBD24
		public static void DrawFillableBar(GenDraw.FillableBarRequest r)
		{
			Vector2 vector = r.preRotationOffset.RotatedBy(r.rotation.AsAngle);
			r.center += new Vector3(vector.x, 0f, vector.y);
			if (r.rotation == Rot4.South)
			{
				r.rotation = Rot4.North;
			}
			if (r.rotation == Rot4.West)
			{
				r.rotation = Rot4.East;
			}
			Vector3 s = new Vector3(r.size.x + r.margin, 1f, r.size.y + r.margin);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(r.center, r.rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, r.unfilledMat, 0);
			if (r.fillPercent > 0.001f)
			{
				s = new Vector3(r.size.x * r.fillPercent, 1f, r.size.y);
				matrix = default(Matrix4x4);
				Vector3 pos = r.center + Vector3.up * 0.01f;
				if (!r.rotation.IsHorizontal)
				{
					pos.x -= r.size.x * 0.5f;
					pos.x += 0.5f * r.size.x * r.fillPercent;
				}
				else
				{
					pos.z -= r.size.x * 0.5f;
					pos.z += 0.5f * r.size.x * r.fillPercent;
				}
				matrix.SetTRS(pos, r.rotation.AsQuat, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, r.filledMat, 0);
			}
		}

		// Token: 0x06005E12 RID: 24082 RVA: 0x002FDB58 File Offset: 0x002FBF58
		public static void DrawMeshNowOrLater(Mesh mesh, Vector3 loc, Quaternion quat, Material mat, bool drawNow)
		{
			if (drawNow)
			{
				mat.SetPass(0);
				Graphics.DrawMeshNow(mesh, loc, quat);
			}
			else
			{
				Graphics.DrawMesh(mesh, loc, quat, mat, 0);
			}
		}

		// Token: 0x06005E13 RID: 24083 RVA: 0x002FDB84 File Offset: 0x002FBF84
		public static void DrawArrowPointingAt(Vector3 mapTarget, bool offscreenOnly = false)
		{
			Vector3 vector = UI.UIToMapPosition((float)(UI.screenWidth / 2), (float)(UI.screenHeight / 2));
			if ((vector - mapTarget).MagnitudeHorizontalSquared() < 81f)
			{
				if (!offscreenOnly)
				{
					Vector3 position = mapTarget;
					position.y = AltitudeLayer.MetaOverlays.AltitudeFor();
					position.z -= 1.5f;
					Graphics.DrawMesh(MeshPool.plane20, position, Quaternion.identity, GenDraw.ArrowMatWhite, 0);
				}
			}
			else
			{
				Vector3 vector2 = (mapTarget - vector).normalized * 7f;
				Vector3 position2 = vector + vector2;
				position2.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				Quaternion rotation = Quaternion.LookRotation(vector2);
				Graphics.DrawMesh(MeshPool.plane20, position2, rotation, GenDraw.ArrowMatWhite, 0);
			}
		}

		// Token: 0x02000F3F RID: 3903
		public struct FillableBarRequest
		{
			// Token: 0x04003E01 RID: 15873
			public Vector3 center;

			// Token: 0x04003E02 RID: 15874
			public Vector2 size;

			// Token: 0x04003E03 RID: 15875
			public float fillPercent;

			// Token: 0x04003E04 RID: 15876
			public Material filledMat;

			// Token: 0x04003E05 RID: 15877
			public Material unfilledMat;

			// Token: 0x04003E06 RID: 15878
			public float margin;

			// Token: 0x04003E07 RID: 15879
			public Rot4 rotation;

			// Token: 0x04003E08 RID: 15880
			public Vector2 preRotationOffset;
		}
	}
}
