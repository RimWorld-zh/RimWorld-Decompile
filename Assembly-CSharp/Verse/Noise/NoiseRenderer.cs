using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F70 RID: 3952
	public static class NoiseRenderer
	{
		// Token: 0x06005F4D RID: 24397 RVA: 0x00308848 File Offset: 0x00306C48
		public static Texture2D NoiseRendered(ModuleBase noise)
		{
			return NoiseRenderer.NoiseRendered(new CellRect(0, 0, NoiseRenderer.renderSize.x, NoiseRenderer.renderSize.z), noise);
		}

		// Token: 0x06005F4E RID: 24398 RVA: 0x00308880 File Offset: 0x00306C80
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

		// Token: 0x06005F4F RID: 24399 RVA: 0x00308924 File Offset: 0x00306D24
		private static Color ColorForValue(float val)
		{
			val = val * 0.5f + 0.5f;
			return ColorsFromSpectrum.Get(NoiseRenderer.spectrum, val);
		}

		// Token: 0x04003EB8 RID: 16056
		public static IntVec2 renderSize = new IntVec2(200, 200);

		// Token: 0x04003EB9 RID: 16057
		private static Color[] spectrum = new Color[]
		{
			Color.black,
			Color.blue,
			Color.green,
			Color.white
		};
	}
}
