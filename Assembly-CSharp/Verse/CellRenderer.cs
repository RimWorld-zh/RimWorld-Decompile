using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C09 RID: 3081
	public static class CellRenderer
	{
		// Token: 0x0600435D RID: 17245 RVA: 0x002399F2 File Offset: 0x00237DF2
		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x00239A20 File Offset: 0x00237E20
		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			int num = Mathf.RoundToInt(colorPct * 100f);
			num = GenMath.PositiveMod(num, 100);
			return DebugMatsSpectrum.Mat(num, transparent);
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x00239A52 File Offset: 0x00237E52
		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x00239A64 File Offset: 0x00237E64
		public static void RenderCell(IntVec3 c, Material mat)
		{
			CellRenderer.InitFrame();
			if (CellRenderer.viewRect.Contains(c))
			{
				Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
				Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, mat, 0);
			}
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x00239AA8 File Offset: 0x00237EA8
		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), 0.15f);
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x00239AC0 File Offset: 0x00237EC0
		public static void RenderSpot(Vector3 loc, Material mat, float scale = 0.15f)
		{
			CellRenderer.InitFrame();
			if (CellRenderer.viewRect.Contains(loc.ToIntVec3()))
			{
				loc.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				Vector3 s = new Vector3(scale, 1f, scale);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(loc, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.circle, matrix, mat, 0);
			}
		}

		// Token: 0x04002E15 RID: 11797
		private static int lastCameraUpdateFrame = -1;

		// Token: 0x04002E16 RID: 11798
		private static CellRect viewRect;
	}
}
