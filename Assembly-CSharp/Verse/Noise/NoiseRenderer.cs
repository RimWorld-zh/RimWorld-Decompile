using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F6F RID: 3951
	public static class NoiseRenderer
	{
		// Token: 0x04003EC9 RID: 16073
		public static IntVec2 renderSize = new IntVec2(200, 200);

		// Token: 0x04003ECA RID: 16074
		private static Color[] spectrum = new Color[]
		{
			Color.black,
			Color.blue,
			Color.green,
			Color.white
		};

		// Token: 0x06005F74 RID: 24436 RVA: 0x0030A9C8 File Offset: 0x00308DC8
		public static Texture2D NoiseRendered(ModuleBase noise)
		{
			return NoiseRenderer.NoiseRendered(new CellRect(0, 0, NoiseRenderer.renderSize.x, NoiseRenderer.renderSize.z), noise);
		}

		// Token: 0x06005F75 RID: 24437 RVA: 0x0030AA00 File Offset: 0x00308E00
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

		// Token: 0x06005F76 RID: 24438 RVA: 0x0030AAA4 File Offset: 0x00308EA4
		private static Color ColorForValue(float val)
		{
			val = val * 0.5f + 0.5f;
			return ColorsFromSpectrum.Get(NoiseRenderer.spectrum, val);
		}
	}
}
