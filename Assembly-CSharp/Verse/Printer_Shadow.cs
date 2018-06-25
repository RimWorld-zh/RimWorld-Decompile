using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C53 RID: 3155
	public static class Printer_Shadow
	{
		// Token: 0x04002F7E RID: 12158
		private static readonly Color32 LowVertexColor = new Color32(0, 0, 0, 0);

		// Token: 0x04002F7F RID: 12159
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowSizeOffsetX = 0f;

		// Token: 0x04002F80 RID: 12160
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowSizeOffsetY = 0f;

		// Token: 0x04002F81 RID: 12161
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowSizeOffsetZ = 0f;

		// Token: 0x06004574 RID: 17780 RVA: 0x0024BEA8 File Offset: 0x0024A2A8
		public static void PrintShadow(SectionLayer layer, Vector3 center, ShadowData shadow, Rot4 rotation)
		{
			Printer_Shadow.PrintShadow(layer, center, shadow.volume, rotation);
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x0024BEBC File Offset: 0x0024A2BC
		public static void PrintShadow(SectionLayer layer, Vector3 center, Vector3 volume, Rot4 rotation)
		{
			if (DebugViewSettings.drawShadows)
			{
				LayerSubMesh subMesh = layer.GetSubMesh(MatBases.SunShadowFade);
				Color32 item = new Color32(byte.MaxValue, 0, 0, (byte)Mathf.Min(255f * (volume.y + Printer_Shadow.GlobalShadowSizeOffsetY), 255f));
				Vector3 vector = (volume + new Vector3(Printer_Shadow.GlobalShadowSizeOffsetX, 0f, Printer_Shadow.GlobalShadowSizeOffsetZ)).RotatedBy(rotation).Abs() / 2f;
				float x = center.x;
				float z = center.z;
				int count = subMesh.verts.Count;
				subMesh.verts.Add(new Vector3(x - vector.x, 0f, z - vector.z));
				subMesh.verts.Add(new Vector3(x - vector.x, 0f, z + vector.z));
				subMesh.verts.Add(new Vector3(x + vector.x, 0f, z + vector.z));
				subMesh.verts.Add(new Vector3(x + vector.x, 0f, z - vector.z));
				subMesh.colors.Add(Printer_Shadow.LowVertexColor);
				subMesh.colors.Add(Printer_Shadow.LowVertexColor);
				subMesh.colors.Add(Printer_Shadow.LowVertexColor);
				subMesh.colors.Add(Printer_Shadow.LowVertexColor);
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count + 3);
				int count2 = subMesh.verts.Count;
				subMesh.verts.Add(new Vector3(x - vector.x, 0f, z - vector.z));
				subMesh.verts.Add(new Vector3(x - vector.x, 0f, z + vector.z));
				subMesh.colors.Add(item);
				subMesh.colors.Add(item);
				subMesh.tris.Add(count);
				subMesh.tris.Add(count2);
				subMesh.tris.Add(count2 + 1);
				subMesh.tris.Add(count);
				subMesh.tris.Add(count2 + 1);
				subMesh.tris.Add(count + 1);
				int count3 = subMesh.verts.Count;
				subMesh.verts.Add(new Vector3(x + vector.x, 0f, z + vector.z));
				subMesh.verts.Add(new Vector3(x + vector.x, 0f, z - vector.z));
				subMesh.colors.Add(item);
				subMesh.colors.Add(item);
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count3);
				subMesh.tris.Add(count3 + 1);
				subMesh.tris.Add(count3 + 1);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 2);
				int count4 = subMesh.verts.Count;
				subMesh.verts.Add(new Vector3(x - vector.x, 0f, z - vector.z));
				subMesh.verts.Add(new Vector3(x + vector.x, 0f, z - vector.z));
				subMesh.colors.Add(item);
				subMesh.colors.Add(item);
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count4);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count4 + 1);
				subMesh.tris.Add(count4);
			}
		}
	}
}
