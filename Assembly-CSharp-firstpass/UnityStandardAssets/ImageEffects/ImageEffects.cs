using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200019C RID: 412
	[AddComponentMenu("")]
	public class ImageEffects
	{
		// Token: 0x0600092A RID: 2346 RVA: 0x00016B00 File Offset: 0x00014D00
		public static void RenderDistortion(Material material, RenderTexture source, RenderTexture destination, float angle, Vector2 center, Vector2 radius)
		{
			bool flag = source.texelSize.y < 0f;
			if (flag)
			{
				center.y = 1f - center.y;
				angle = -angle;
			}
			Matrix4x4 value = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, angle), Vector3.one);
			material.SetMatrix("_RotationMatrix", value);
			material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
			material.SetFloat("_Angle", angle * 0.0174532924f);
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00016BB6 File Offset: 0x00014DB6
		[Obsolete("Use Graphics.Blit(source,dest) instead")]
		public static void Blit(RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00016BC0 File Offset: 0x00014DC0
		[Obsolete("Use Graphics.Blit(source, destination, material) instead")]
		public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest, material);
		}
	}
}
