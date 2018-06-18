using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C53 RID: 3155
	public static class Printer_Shadow
	{
		// Token: 0x06004568 RID: 17768 RVA: 0x0024A71C File Offset: 0x00248B1C
		public static void PrintShadow(SectionLayer layer, Vector3 center, ShadowData shadow, Rot4 rotation)
		{
			Printer_Shadow.PrintShadow(layer, center, shadow.volume, rotation);
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x0024A730 File Offset: 0x00248B30
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

		// Token: 0x04002F6D RID: 12141
		private static readonly Color32 LowVertexColor = new Color32(0, 0, 0, 0);

		// Token: 0x04002F6E RID: 12142
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowSizeOffsetX = 0f;

		// Token: 0x04002F6F RID: 12143
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowSizeOffsetY = 0f;

		// Token: 0x04002F70 RID: 12144
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowSizeOffsetZ = 0f;
	}
}
