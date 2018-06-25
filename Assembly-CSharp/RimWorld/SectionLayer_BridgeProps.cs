using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200038E RID: 910
	[StaticConstructorOnStartup]
	public class SectionLayer_BridgeProps : SectionLayer
	{
		// Token: 0x040009A8 RID: 2472
		private static readonly Material PropsLoopMat = MaterialPool.MatFrom("Terrain/Misc/BridgeProps_Loop", ShaderDatabase.Transparent);

		// Token: 0x040009A9 RID: 2473
		private static readonly Material PropsRightMat = MaterialPool.MatFrom("Terrain/Misc/BridgeProps_Right", ShaderDatabase.Transparent);

		// Token: 0x06000FDB RID: 4059 RVA: 0x00085038 File Offset: 0x00083438
		public SectionLayer_BridgeProps(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x0008504C File Offset: 0x0008344C
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00085068 File Offset: 0x00083468
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			Map map = base.Map;
			TerrainGrid terrainGrid = map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			float y = AltitudeLayer.TerrainScatter.AltitudeFor();
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			while (!iterator.Done())
			{
				IntVec3 intVec = iterator.Current;
				if (this.ShouldDrawPropsBelow(intVec, terrainGrid))
				{
					IntVec3 c = intVec;
					c.x++;
					Material material;
					if (c.InBounds(map) && this.ShouldDrawPropsBelow(c, terrainGrid))
					{
						material = SectionLayer_BridgeProps.PropsLoopMat;
					}
					else
					{
						material = SectionLayer_BridgeProps.PropsRightMat;
					}
					LayerSubMesh subMesh = base.GetSubMesh(material);
					int count = subMesh.verts.Count;
					subMesh.verts.Add(new Vector3((float)intVec.x, y, (float)(intVec.z - 1)));
					subMesh.verts.Add(new Vector3((float)intVec.x, y, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), y, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), y, (float)(intVec.z - 1)));
					subMesh.uvs.Add(new Vector2(0f, 0f));
					subMesh.uvs.Add(new Vector2(0f, 1f));
					subMesh.uvs.Add(new Vector2(1f, 1f));
					subMesh.uvs.Add(new Vector2(1f, 0f));
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 1);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count + 3);
				}
				iterator.MoveNext();
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x000852AC File Offset: 0x000836AC
		private bool ShouldDrawPropsBelow(IntVec3 c, TerrainGrid terrGrid)
		{
			TerrainDef terrainDef = terrGrid.TerrainAt(c);
			bool result;
			if (terrainDef == null || terrainDef != TerrainDefOf.Bridge)
			{
				result = false;
			}
			else
			{
				IntVec3 c2 = c;
				c2.z--;
				Map map = base.Map;
				if (!c2.InBounds(map))
				{
					result = false;
				}
				else
				{
					TerrainDef terrainDef2 = terrGrid.TerrainAt(c2);
					result = (terrainDef2 != TerrainDefOf.Bridge && (terrainDef2.passability == Traversability.Impassable || c2.SupportsStructureType(map, TerrainDefOf.Bridge.terrainAffordanceNeeded)));
				}
			}
			return result;
		}
	}
}
