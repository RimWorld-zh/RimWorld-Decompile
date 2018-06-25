using System;
using UnityEngine;

namespace Verse
{
	internal class SectionLayer_IndoorMask : SectionLayer
	{
		public SectionLayer_IndoorMask(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlag.FogOfWar | MapMeshFlag.Roofs);
		}

		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawShadows;
			}
		}

		private bool HideRainPrimary(IntVec3 c)
		{
			bool result;
			if (base.Map.fogGrid.IsFogged(c))
			{
				result = false;
			}
			else
			{
				if (c.Roofed(base.Map))
				{
					Building edifice = c.GetEdifice(base.Map);
					if (edifice == null)
					{
						return true;
					}
					if (edifice.def.Fillage != FillCategory.Full)
					{
						return true;
					}
					if (edifice.def.size.x > 1 || edifice.def.size.z > 1)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		public override void Regenerate()
		{
			if (MatBases.SunShadow.shader.isSupported)
			{
				LayerSubMesh subMesh = base.GetSubMesh(MatBases.IndoorMask);
				subMesh.Clear(MeshParts.All);
				Building[] innerArray = base.Map.edificeGrid.InnerArray;
				CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
				cellRect.ClipInsideMap(base.Map);
				subMesh.verts.Capacity = cellRect.Area * 2;
				subMesh.tris.Capacity = cellRect.Area * 4;
				float y = AltitudeLayer.MetaOverlays.AltitudeFor();
				CellIndices cellIndices = base.Map.cellIndices;
				for (int i = cellRect.minX; i <= cellRect.maxX; i++)
				{
					int j = cellRect.minZ;
					while (j <= cellRect.maxZ)
					{
						IntVec3 intVec = new IntVec3(i, 0, j);
						if (this.HideRainPrimary(intVec))
						{
							goto IL_17C;
						}
						bool flag = intVec.Roofed(base.Map);
						bool flag2 = false;
						if (flag)
						{
							for (int k = 0; k < 8; k++)
							{
								IntVec3 c = intVec + GenAdj.AdjacentCells[k];
								if (c.InBounds(base.Map))
								{
									if (this.HideRainPrimary(c))
									{
										flag2 = true;
										break;
									}
								}
							}
						}
						if (flag && flag2)
						{
							goto IL_17C;
						}
						IL_2AC:
						j++;
						continue;
						IL_17C:
						Thing thing = innerArray[cellIndices.CellToIndex(i, j)];
						float num;
						if (thing != null && (thing.def.passability == Traversability.Impassable || thing.def.IsDoor))
						{
							num = 0f;
						}
						else
						{
							num = 0.16f;
						}
						subMesh.verts.Add(new Vector3((float)i - num, y, (float)j - num));
						subMesh.verts.Add(new Vector3((float)i - num, y, (float)(j + 1) + num));
						subMesh.verts.Add(new Vector3((float)(i + 1) + num, y, (float)(j + 1) + num));
						subMesh.verts.Add(new Vector3((float)(i + 1) + num, y, (float)j - num));
						int count = subMesh.verts.Count;
						subMesh.tris.Add(count - 4);
						subMesh.tris.Add(count - 3);
						subMesh.tris.Add(count - 2);
						subMesh.tris.Add(count - 4);
						subMesh.tris.Add(count - 2);
						subMesh.tris.Add(count - 1);
						goto IL_2AC;
					}
				}
				if (subMesh.verts.Count > 0)
				{
					subMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
				}
			}
		}
	}
}
