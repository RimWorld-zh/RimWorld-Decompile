using System.Text;
using UnityEngine;

namespace Verse
{
	public class SectionLayer_LightingOverlay : SectionLayer
	{
		private Color32[] glowGrid;

		private int firstCenterInd;

		private CellRect sectRect;

		private const byte RoofedAreaMinSkyCover = 100;

		private static readonly IntVec3[] CheckSquareOffsets = new IntVec3[4]
		{
			new IntVec3(0, 0, -1),
			new IntVec3(-1, 0, -1),
			new IntVec3(-1, 0, 0),
			new IntVec3(0, 0, 0)
		};

		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawLightingOverlay;
			}
		}

		public SectionLayer_LightingOverlay(Section section)
			: base(section)
		{
			base.relevantChangeTypes = MapMeshFlag.GroundGlow;
		}

		public string GlowReportAt(IntVec3 c)
		{
			Color32[] colors = base.GetSubMesh(MatBases.LightOverlay).mesh.colors32;
			int num = default(int);
			int num2 = default(int);
			int num3 = default(int);
			int num4 = default(int);
			int num5 = default(int);
			this.CalculateVertexIndices(c.x, c.z, out num, out num2, out num3, out num4, out num5);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("BL=" + colors[num]);
			stringBuilder.Append("\nTL=" + colors[num2]);
			stringBuilder.Append("\nTR=" + colors[num3]);
			stringBuilder.Append("\nBR=" + colors[num4]);
			stringBuilder.Append("\nCenter=" + colors[num5]);
			return stringBuilder.ToString();
		}

		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.LightOverlay);
			if (subMesh.verts.Count == 0)
			{
				this.MakeBaseGeometry(subMesh);
			}
			Color32[] array = new Color32[subMesh.verts.Count];
			int maxX = this.sectRect.maxX;
			int maxZ = this.sectRect.maxZ;
			bool[] array2 = new bool[4];
			Thing[] innerArray = base.Map.edificeGrid.InnerArray;
			RoofGrid roofGrid = base.Map.roofGrid;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = this.sectRect.minX; i <= maxX + 1; i++)
			{
				for (int j = this.sectRect.minZ; j <= maxZ + 1; j++)
				{
					int num = default(int);
					int num2 = default(int);
					int num3 = default(int);
					int num4 = default(int);
					int num5 = default(int);
					this.CalculateVertexIndices(i, j, out num, out num2, out num3, out num4, out num5);
					IntVec3 a = new IntVec3(i, 0, j);
					bool flag = false;
					for (int k = 0; k < 4; k++)
					{
						IntVec3 c = a + SectionLayer_LightingOverlay.CheckSquareOffsets[k];
						if (!c.InBounds(base.Map))
						{
							array2[k] = true;
						}
						else
						{
							Thing thing = innerArray[cellIndices.CellToIndex(c)];
							RoofDef roofDef = roofGrid.RoofAt(c.x, c.z);
							if (roofDef != null && (roofDef.isThickRoof || thing == null || !thing.def.holdsRoof || thing.def.altitudeLayer == AltitudeLayer.DoorMoveable))
							{
								flag = true;
							}
							if (thing != null && thing.def.blockLight)
							{
								array2[k] = true;
							}
							else
							{
								array2[k] = false;
							}
						}
					}
					ColorInt colorInt = new ColorInt(0, 0, 0, 0);
					int num6 = 0;
					if (!array2[0])
					{
						colorInt += this.glowGrid[cellIndices.CellToIndex(i, j - 1)].AsColorInt();
						num6++;
					}
					if (!array2[1])
					{
						colorInt += this.glowGrid[cellIndices.CellToIndex(i - 1, j - 1)].AsColorInt();
						num6++;
					}
					if (!array2[2])
					{
						colorInt += this.glowGrid[cellIndices.CellToIndex(i - 1, j)].AsColorInt();
						num6++;
					}
					if (!array2[3])
					{
						colorInt += this.glowGrid[cellIndices.CellToIndex(i, j)].AsColorInt();
						num6++;
					}
					if (num6 > 0)
					{
						colorInt /= (float)num6;
						array[num] = colorInt.ToColor32;
					}
					else
					{
						array[num] = new Color32(0, 0, 0, 0);
					}
					if (flag && array[num].a < 100)
					{
						array[num].a = 100;
					}
				}
			}
			for (int l = this.sectRect.minX; l <= maxX; l++)
			{
				for (int m = this.sectRect.minZ; m <= maxZ; m++)
				{
					int num7 = default(int);
					int num8 = default(int);
					int num9 = default(int);
					int num10 = default(int);
					int num11 = default(int);
					this.CalculateVertexIndices(l, m, out num7, out num8, out num9, out num10, out num11);
					ColorInt colA = default(ColorInt) + array[num7];
					colA += array[num8];
					colA += array[num9];
					colA += array[num10];
					array[num11] = (colA / 4f).ToColor32;
					Thing thing = innerArray[cellIndices.CellToIndex(l, m)];
					if (roofGrid.Roofed(l, m) && (thing == null || !thing.def.holdsRoof) && array[num11].a < 100)
					{
						array[num11].a = 100;
					}
				}
			}
			subMesh.mesh.colors32 = array;
		}

		private void MakeBaseGeometry(LayerSubMesh sm)
		{
			this.glowGrid = base.Map.glowGrid.glowGrid;
			this.sectRect = new CellRect(base.section.botLeft.x, base.section.botLeft.z, 17, 17);
			this.sectRect.ClipInsideMap(base.Map);
			int capacity = (this.sectRect.Width + 1) * (this.sectRect.Height + 1) + this.sectRect.Area;
			float y = Altitudes.AltitudeFor(AltitudeLayer.LightingOverlay);
			sm.verts.Capacity = capacity;
			for (int i = this.sectRect.minZ; i <= this.sectRect.maxZ + 1; i++)
			{
				for (int j = this.sectRect.minX; j <= this.sectRect.maxX + 1; j++)
				{
					sm.verts.Add(new Vector3((float)j, y, (float)i));
				}
			}
			this.firstCenterInd = sm.verts.Count;
			for (int k = this.sectRect.minZ; k <= this.sectRect.maxZ; k++)
			{
				for (int l = this.sectRect.minX; l <= this.sectRect.maxX; l++)
				{
					sm.verts.Add(new Vector3((float)((float)l + 0.5), y, (float)((float)k + 0.5)));
				}
			}
			sm.tris.Capacity = this.sectRect.Area * 4 * 3;
			for (int m = this.sectRect.minZ; m <= this.sectRect.maxZ; m++)
			{
				for (int n = this.sectRect.minX; n <= this.sectRect.maxX; n++)
				{
					int item = default(int);
					int item2 = default(int);
					int item3 = default(int);
					int item4 = default(int);
					int item5 = default(int);
					this.CalculateVertexIndices(n, m, out item, out item2, out item3, out item4, out item5);
					sm.tris.Add(item);
					sm.tris.Add(item5);
					sm.tris.Add(item4);
					sm.tris.Add(item);
					sm.tris.Add(item2);
					sm.tris.Add(item5);
					sm.tris.Add(item2);
					sm.tris.Add(item3);
					sm.tris.Add(item5);
					sm.tris.Add(item3);
					sm.tris.Add(item4);
					sm.tris.Add(item5);
				}
			}
			sm.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
		}

		private void CalculateVertexIndices(int worldX, int worldZ, out int botLeft, out int topLeft, out int topRight, out int botRight, out int center)
		{
			int num = worldX - this.sectRect.minX;
			int num2 = worldZ - this.sectRect.minZ;
			botLeft = num2 * (this.sectRect.Width + 1) + num;
			topLeft = (num2 + 1) * (this.sectRect.Width + 1) + num;
			topRight = (num2 + 1) * (this.sectRect.Width + 1) + (num + 1);
			botRight = num2 * (this.sectRect.Width + 1) + (num + 1);
			center = this.firstCenterInd + (num2 * this.sectRect.Width + num);
		}
	}
}
