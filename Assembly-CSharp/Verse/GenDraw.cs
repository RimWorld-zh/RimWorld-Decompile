using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F39 RID: 3897
	[StaticConstructorOnStartup]
	public static class GenDraw
	{
		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06005DC3 RID: 24003 RVA: 0x002FA3C0 File Offset: 0x002F87C0
		public static Material CurTargetingMat
		{
			get
			{
				GenDraw.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return GenDraw.TargetSquareMatSingle;
			}
		}

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06005DC4 RID: 24004 RVA: 0x002FA3EC File Offset: 0x002F87EC
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

		// Token: 0x06005DC5 RID: 24005 RVA: 0x002FA430 File Offset: 0x002F8830
		public static void DrawNoBuildEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(10);
		}

		// Token: 0x06005DC6 RID: 24006 RVA: 0x002FA43A File Offset: 0x002F883A
		public static void DrawNoZoneEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(5);
		}

		// Token: 0x06005DC7 RID: 24007 RVA: 0x002FA444 File Offset: 0x002F8844
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

		// Token: 0x06005DC8 RID: 24008 RVA: 0x002FA4E9 File Offset: 0x002F88E9
		public static void DrawLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.LineMatWhite);
		}

		// Token: 0x06005DC9 RID: 24009 RVA: 0x002FA4F8 File Offset: 0x002F88F8
		public static void DrawLineBetween(Vector3 A, Vector3 B, float layer)
		{
			GenDraw.DrawLineBetween(A + Vector3.up * layer, B + Vector3.up * layer, GenDraw.LineMatWhite);
		}

		// Token: 0x06005DCA RID: 24010 RVA: 0x002FA527 File Offset: 0x002F8927
		public static void DrawLineBetween(Vector3 A, Vector3 B, SimpleColor color)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.GetLineMat(color));
		}

		// Token: 0x06005DCB RID: 24011 RVA: 0x002FA538 File Offset: 0x002F8938
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

		// Token: 0x06005DCC RID: 24012 RVA: 0x002FA604 File Offset: 0x002F8A04
		public static void DrawCircleOutline(Vector3 center, float radius)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.LineMatWhite);
		}

		// Token: 0x06005DCD RID: 24013 RVA: 0x002FA613 File Offset: 0x002F8A13
		public static void DrawCircleOutline(Vector3 center, float radius, SimpleColor color)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.GetLineMat(color));
		}

		// Token: 0x06005DCE RID: 24014 RVA: 0x002FA624 File Offset: 0x002F8A24
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

		// Token: 0x06005DCF RID: 24015 RVA: 0x002FA6BC File Offset: 0x002F8ABC
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

		// Token: 0x06005DD0 RID: 24016 RVA: 0x002FA74A File Offset: 0x002F8B4A
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawWorldLineBetween(A, B, GenDraw.WorldLineMatWhite, 1f);
		}

		// Token: 0x06005DD1 RID: 24017 RVA: 0x002FA760 File Offset: 0x002F8B60
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

		// Token: 0x06005DD2 RID: 24018 RVA: 0x002FA848 File Offset: 0x002F8C48
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

		// Token: 0x06005DD3 RID: 24019 RVA: 0x002FA978 File Offset: 0x002F8D78
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

		// Token: 0x06005DD4 RID: 24020 RVA: 0x002FAA44 File Offset: 0x002F8E44
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

		// Token: 0x06005DD5 RID: 24021 RVA: 0x002FAA70 File Offset: 0x002F8E70
		private static void DrawTargetingHighlight_Cell(IntVec3 c)
		{
			Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.Building);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x06005DD6 RID: 24022 RVA: 0x002FAAA0 File Offset: 0x002F8EA0
		private static void DrawTargetingHighlight_Thing(Thing t)
		{
			Graphics.DrawMesh(MeshPool.plane10, t.TrueCenter() + Altitudes.AltIncVect, t.Rotation.AsQuat, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x06005DD7 RID: 24023 RVA: 0x002FAADC File Offset: 0x002F8EDC
		public static void DrawTargetingHightlight_Explosion(IntVec3 c, float Radius)
		{
			GenDraw.DrawRadiusRing(c, Radius);
		}

		// Token: 0x06005DD8 RID: 24024 RVA: 0x002FAAE8 File Offset: 0x002F8EE8
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

		// Token: 0x06005DD9 RID: 24025 RVA: 0x002FABEC File Offset: 0x002F8FEC
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

		// Token: 0x06005DDA RID: 24026 RVA: 0x002FAC87 File Offset: 0x002F9087
		public static void DrawFieldEdges(List<IntVec3> cells)
		{
			GenDraw.DrawFieldEdges(cells, Color.white);
		}

		// Token: 0x06005DDB RID: 24027 RVA: 0x002FAC98 File Offset: 0x002F9098
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

		// Token: 0x06005DDC RID: 24028 RVA: 0x002FAEE8 File Offset: 0x002F92E8
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

		// Token: 0x06005DDD RID: 24029 RVA: 0x002FAF88 File Offset: 0x002F9388
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

		// Token: 0x06005DDE RID: 24030 RVA: 0x002FB004 File Offset: 0x002F9404
		public static void DrawCooldownCircle(Vector3 center, float radius)
		{
			Vector3 s = new Vector3(radius, 1f, radius);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(center, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, GenDraw.AimPieMaterial, 0);
		}

		// Token: 0x06005DDF RID: 24031 RVA: 0x002FB048 File Offset: 0x002F9448
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

		// Token: 0x06005DE0 RID: 24032 RVA: 0x002FB27C File Offset: 0x002F967C
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

		// Token: 0x06005DE1 RID: 24033 RVA: 0x002FB2A8 File Offset: 0x002F96A8
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

		// Token: 0x04003DC7 RID: 15815
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent);

		// Token: 0x04003DC8 RID: 15816
		private const float TargetPulseFrequency = 8f;

		// Token: 0x04003DC9 RID: 15817
		public static readonly string LineTexPath = "UI/Overlays/ThingLine";

		// Token: 0x04003DCA RID: 15818
		public static readonly string OneSidedLineTexPath = "UI/Overlays/OneSidedLine";

		// Token: 0x04003DCB RID: 15819
		private static readonly Material LineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.white);

		// Token: 0x04003DCC RID: 15820
		private static readonly Material LineMatRed = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.red);

		// Token: 0x04003DCD RID: 15821
		private static readonly Material LineMatGreen = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.green);

		// Token: 0x04003DCE RID: 15822
		private static readonly Material LineMatBlue = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.blue);

		// Token: 0x04003DCF RID: 15823
		private static readonly Material LineMatMagenta = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.magenta);

		// Token: 0x04003DD0 RID: 15824
		private static readonly Material LineMatYellow = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.yellow);

		// Token: 0x04003DD1 RID: 15825
		private static readonly Material LineMatCyan = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.cyan);

		// Token: 0x04003DD2 RID: 15826
		private static readonly Material LineMatMetaOverlay = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.MetaOverlay);

		// Token: 0x04003DD3 RID: 15827
		private static readonly Material WorldLineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x04003DD4 RID: 15828
		private static readonly Material OneSidedWorldLineMatWhite = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x04003DD5 RID: 15829
		private const float LineWidth = 0.2f;

		// Token: 0x04003DD6 RID: 15830
		private const float BaseWorldLineWidth = 0.2f;

		// Token: 0x04003DD7 RID: 15831
		public static readonly Material InteractionCellMaterial = MaterialPool.MatFrom("UI/Overlays/InteractionCell", ShaderDatabase.Transparent);

		// Token: 0x04003DD8 RID: 15832
		private static readonly Color InteractionCellIntensity = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x04003DD9 RID: 15833
		private static List<int> cachedEdgeTiles = new List<int>();

		// Token: 0x04003DDA RID: 15834
		private static int cachedEdgeTilesForCenter = -1;

		// Token: 0x04003DDB RID: 15835
		private static int cachedEdgeTilesForRadius = -1;

		// Token: 0x04003DDC RID: 15836
		private static int cachedEdgeTilesForWorldSeed = -1;

		// Token: 0x04003DDD RID: 15837
		private static List<IntVec3> ringDrawCells = new List<IntVec3>();

		// Token: 0x04003DDE RID: 15838
		private static bool maxRadiusMessaged = false;

		// Token: 0x04003DDF RID: 15839
		private static BoolGrid fieldGrid;

		// Token: 0x04003DE0 RID: 15840
		private static bool[] rotNeeded = new bool[4];

		// Token: 0x04003DE1 RID: 15841
		private static readonly Material AimPieMaterial = SolidColorMaterials.SimpleSolidColorMaterial(new Color(1f, 1f, 1f, 0.3f), false);

		// Token: 0x04003DE2 RID: 15842
		private static readonly Material ArrowMatWhite = MaterialPool.MatFrom("UI/Overlays/Arrow", ShaderDatabase.CutoutFlying, Color.white);

		// Token: 0x02000F3A RID: 3898
		public struct FillableBarRequest
		{
			// Token: 0x04003DE4 RID: 15844
			public Vector3 center;

			// Token: 0x04003DE5 RID: 15845
			public Vector2 size;

			// Token: 0x04003DE6 RID: 15846
			public float fillPercent;

			// Token: 0x04003DE7 RID: 15847
			public Material filledMat;

			// Token: 0x04003DE8 RID: 15848
			public Material unfilledMat;

			// Token: 0x04003DE9 RID: 15849
			public float margin;

			// Token: 0x04003DEA RID: 15850
			public Rot4 rotation;

			// Token: 0x04003DEB RID: 15851
			public Vector2 preRotationOffset;
		}
	}
}
