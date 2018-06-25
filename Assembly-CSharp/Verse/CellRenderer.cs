using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0C RID: 3084
	public static class CellRenderer
	{
		// Token: 0x04002E1C RID: 11804
		private static int lastCameraUpdateFrame = -1;

		// Token: 0x04002E1D RID: 11805
		private static CellRect viewRect;

		// Token: 0x06004360 RID: 17248 RVA: 0x00239DAE File Offset: 0x002381AE
		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x00239DDC File Offset: 0x002381DC
		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			int num = Mathf.RoundToInt(colorPct * 100f);
			num = GenMath.PositiveMod(num, 100);
			return DebugMatsSpectrum.Mat(num, transparent);
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x00239E0E File Offset: 0x0023820E
		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x00239E20 File Offset: 0x00238220
		public static void RenderCell(IntVec3 c, Material mat)
		{
			CellRenderer.InitFrame();
			if (CellRenderer.viewRect.Contains(c))
			{
				Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
				Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, mat, 0);
			}
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x00239E64 File Offset: 0x00238264
		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), 0.15f);
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x00239E7C File Offset: 0x0023827C
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
	}
}
