using UnityEngine;

namespace Verse
{
	internal class SectionLayer_IndoorMask : SectionLayer
	{
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawShadows;
			}
		}

		public SectionLayer_IndoorMask(Section section) : base(section)
		{
			base.relevantChangeTypes = (MapMeshFlag.FogOfWar | MapMeshFlag.Roofs);
		}

		private bool HideRainPrimary(IntVec3 c)
		{
			bool result;
			if (base.Map.fogGrid.IsFogged(c))
			{
				result = false;
				goto IL_009d;
			}
			if (c.Roofed(base.Map))
			{
				Building edifice = c.GetEdifice(base.Map);
				if (edifice == null)
				{
					result = true;
				}
				else if (edifice.def.Fillage != FillCategory.Full)
				{
					result = true;
				}
				else
				{
					if (edifice.def.size.x <= 1 && edifice.def.size.z <= 1)
					{
						goto IL_0096;
					}
					result = true;
				}
				goto IL_009d;
			}
			goto IL_0096;
			IL_009d:
			return result;
			IL_0096:
			result = false;
			goto IL_009d;
		}

		public override void Regenerate()
		{
			if (MatBases.SunShadow.shader.isSupported)
			{
				LayerSubMesh subMesh = base.GetSubMesh(MatBases.IndoorMask);
				subMesh.Clear(MeshParts.All);
				Building[] innerArray = base.Map.edificeGrid.InnerArray;
				CellRect cellRect = new CellRect(base.section.botLeft.x, base.section.botLeft.z, 17, 17);
				cellRect.ClipInsideMap(base.Map);
				subMesh.verts.Capacity = cellRect.Area * 2;
				subMesh.tris.Capacity = cellRect.Area * 4;
				float y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
				CellIndices cellIndices = base.Map.cellIndices;
				for (int i = cellRect.minX; i <= cellRect.maxX; i++)
				{
					for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
					{
						IntVec3 intVec = new IntVec3(i, 0, j);
						if (!this.HideRainPrimary(intVec))
						{
							bool flag = intVec.Roofed(base.Map);
							bool flag2 = false;
							if (flag)
							{
								for (int k = 0; k < 8; k++)
								{
									IntVec3 c = intVec + GenAdj.AdjacentCells[k];
									if (c.InBounds(base.Map) && this.HideRainPrimary(c))
									{
										flag2 = true;
										break;
									}
								}
							}
							if (flag && flag2)
								goto IL_017c;
							continue;
						}
						goto IL_017c;
						IL_017c:
						Thing thing = innerArray[cellIndices.CellToIndex(i, j)];
						float num = (float)((thing == null || (thing.def.passability != Traversability.Impassable && !thing.def.IsDoor)) ? 0.15999999642372131 : 0.0);
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
