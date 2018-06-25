using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F73 RID: 3955
	public static class NoiseRenderer
	{
		// Token: 0x04003ECC RID: 16076
		public static IntVec2 renderSize = new IntVec2(200, 200);

		// Token: 0x04003ECD RID: 16077
		private static Color[] spectrum = new Color[]
		{
			Color.black,
			Color.blue,
			Color.green,
			Color.white
		};

		// Token: 0x06005F7E RID: 24446 RVA: 0x0030B048 File Offset: 0x00309448
		public static Texture2D NoiseRendered(ModuleBase noise)
		{
			return NoiseRenderer.NoiseRendered(new CellRect(0, 0, NoiseRenderer.renderSize.x, NoiseRenderer.renderSize.z), noise);
		}

		// Token: 0x06005F7F RID: 24447 RVA: 0x0030B080 File Offset: 0x00309480
		public static Texture2D NoiseRendered(CellRect rect, ModuleBase noise)
		{
			Texture2D texture2D = new Texture2D(rect.Width, rect.Height);
			texture2D.name = "NoiseRender";
			foreach (IntVec2 coordinate in rect.Cells2D)
			{
				texture2D.SetPixel(coordinate.x, coordinate.z, NoiseRenderer.ColorForValue(noise.GetValue(coordinate)));
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06005F80 RID: 24448 RVA: 0x0030B124 File Offset: 0x00309524
		private static Color ColorForValue(float val)
		{
			val = val * 0.5f + 0.5f;
			return ColorsFromSpectrum.Get(NoiseRenderer.spectrum, val);
		}
	}
}
