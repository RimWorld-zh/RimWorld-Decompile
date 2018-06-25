using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C57 RID: 3159
	internal class SectionLayer_Zones : SectionLayer
	{
		// Token: 0x06004580 RID: 17792 RVA: 0x0024C474 File Offset: 0x0024A874
		public SectionLayer_Zones(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Zone;
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06004581 RID: 17793 RVA: 0x0024C48C File Offset: 0x0024A88C
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawWorldOverlays;
			}
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x0024C4A6 File Offset: 0x0024A8A6
		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawZones)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x0024C4BC File Offset: 0x0024A8BC
		public override void Regenerate()
		{
			float y = AltitudeLayer.Zone.AltitudeFor();
			ZoneManager zoneManager = base.Map.zoneManager;
			CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(base.Map);
			base.ClearSubMeshes(MeshParts.All);
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					Zone zone = zoneManager.ZoneAt(new IntVec3(i, 0, j));
					if (zone != null && !zone.hidden)
					{
						LayerSubMesh subMesh = base.GetSubMesh(zone.Material);
						int count = subMesh.verts.Count;
						subMesh.verts.Add(new Vector3((float)i, y, (float)j));
						subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
						subMesh.tris.Add(count);
						subMesh.tris.Add(count + 1);
						subMesh.tris.Add(count + 2);
						subMesh.tris.Add(count);
						subMesh.tris.Add(count + 2);
						subMesh.tris.Add(count + 3);
					}
				}
			}
			base.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
		}
	}
}
