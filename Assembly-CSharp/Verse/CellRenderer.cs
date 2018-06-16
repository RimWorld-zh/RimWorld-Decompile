using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0D RID: 3085
	public static class CellRenderer
	{
		// Token: 0x06004356 RID: 17238 RVA: 0x00238652 File Offset: 0x00236A52
		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x00238680 File Offset: 0x00236A80
		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			int num = Mathf.RoundToInt(colorPct * 100f);
			num = GenMath.PositiveMod(num, 100);
			return DebugMatsSpectrum.Mat(num, transparent);
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x002386B2 File Offset: 0x00236AB2
		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x002386C4 File Offset: 0x00236AC4
		public static void RenderCell(IntVec3 c, Material mat)
		{
			CellRenderer.InitFrame();
			if (CellRenderer.viewRect.Contains(c))
			{
				Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
				Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, mat, 0);
			}
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x00238708 File Offset: 0x00236B08
		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), 0.15f);
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x00238720 File Offset: 0x00236B20
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

		// Token: 0x04002E0D RID: 11789
		private static int lastCameraUpdateFrame = -1;

		// Token: 0x04002E0E RID: 11790
		private static CellRect viewRect;
	}
}
