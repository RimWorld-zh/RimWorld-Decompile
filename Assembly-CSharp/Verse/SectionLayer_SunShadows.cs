using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C4D RID: 3149
	internal class SectionLayer_SunShadows : SectionLayer
	{
		// Token: 0x06004552 RID: 17746 RVA: 0x0024909B File Offset: 0x0024749B
		public SectionLayer_SunShadows(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Buildings;
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06004553 RID: 17747 RVA: 0x002490AC File Offset: 0x002474AC
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawShadows;
			}
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x002490C8 File Offset: 0x002474C8
		public override void Regenerate()
		{
			if (MatBases.SunShadow.shader.isSupported)
			{
				Building[] innerArray = base.Map.edificeGrid.InnerArray;
				float y = AltitudeLayer.Shadows.AltitudeFor();
				CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
				cellRect.ClipInsideMap(base.Map);
				LayerSubMesh subMesh = base.GetSubMesh(MatBases.SunShadow);
				subMesh.Clear(MeshParts.All);
				subMesh.verts.Capacity = cellRect.Area * 2;
				subMesh.tris.Capacity = cellRect.Area * 4;
				subMesh.colors.Capacity = cellRect.Area * 2;
				CellIndices cellIndices = base.Map.cellIndices;
				for (int i = cellRect.minX; i <= cellRect.maxX; i++)
				{
					for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
					{
						Thing thing = innerArray[cellIndices.CellToIndex(i, j)];
						if (thing != null && thing.def.staticSunShadowHeight > 0f)
						{
							float staticSunShadowHeight = thing.def.staticSunShadowHeight;
							Color32 item = new Color32(0, 0, 0, (byte)(255f * staticSunShadowHeight));
							int count = subMesh.verts.Count;
							subMesh.verts.Add(new Vector3((float)i, y, (float)j));
							subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
							subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
							subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
							subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
							subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
							subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
							subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
							int count2 = subMesh.verts.Count;
							subMesh.tris.Add(count2 - 4);
							subMesh.tris.Add(count2 - 3);
							subMesh.tris.Add(count2 - 2);
							subMesh.tris.Add(count2 - 4);
							subMesh.tris.Add(count2 - 2);
							subMesh.tris.Add(count2 - 1);
							if (i > 0)
							{
								thing = innerArray[cellIndices.CellToIndex(i - 1, j)];
								if (thing == null || thing.def.staticSunShadowHeight < staticSunShadowHeight)
								{
									int count3 = subMesh.verts.Count;
									subMesh.verts.Add(new Vector3((float)i, y, (float)j));
									subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
									subMesh.colors.Add(item);
									subMesh.colors.Add(item);
									subMesh.tris.Add(count + 1);
									subMesh.tris.Add(count);
									subMesh.tris.Add(count3);
									subMesh.tris.Add(count3);
									subMesh.tris.Add(count3 + 1);
									subMesh.tris.Add(count + 1);
								}
							}
							if (i < base.Map.Size.x - 1)
							{
								thing = innerArray[cellIndices.CellToIndex(i + 1, j)];
								if (thing == null || thing.def.staticSunShadowHeight < staticSunShadowHeight)
								{
									int count4 = subMesh.verts.Count;
									subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
									subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
									subMesh.colors.Add(item);
									subMesh.colors.Add(item);
									subMesh.tris.Add(count + 2);
									subMesh.tris.Add(count4);
									subMesh.tris.Add(count4 + 1);
									subMesh.tris.Add(count4 + 1);
									subMesh.tris.Add(count + 3);
									subMesh.tris.Add(count + 2);
								}
							}
							if (j > 0)
							{
								thing = innerArray[cellIndices.CellToIndex(i, j - 1)];
								if (thing == null || thing.def.staticSunShadowHeight < staticSunShadowHeight)
								{
									int count5 = subMesh.verts.Count;
									subMesh.verts.Add(new Vector3((float)i, y, (float)j));
									subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
									subMesh.colors.Add(item);
									subMesh.colors.Add(item);
									subMesh.tris.Add(count);
									subMesh.tris.Add(count + 3);
									subMesh.tris.Add(count5);
									subMesh.tris.Add(count + 3);
									subMesh.tris.Add(count5 + 1);
									subMesh.tris.Add(count5);
								}
							}
						}
					}
				}
				if (subMesh.verts.Count > 0)
				{
					subMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
					float num = Mathf.Max(15f, 15f);
					Vector3 size = subMesh.mesh.bounds.size;
					size.x += 2f * num + 2f;
					size.z += 2f * num + 2f;
					subMesh.mesh.bounds = new Bounds(subMesh.mesh.bounds.center, size);
				}
			}
		}

		// Token: 0x04002F60 RID: 12128
		private static readonly Color32 LowVertexColor = new Color32(0, 0, 0, 0);
	}
}
