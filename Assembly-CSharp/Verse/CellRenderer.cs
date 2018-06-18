using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0C RID: 3084
	public static class CellRenderer
	{
		// Token: 0x06004354 RID: 17236 RVA: 0x0023862A File Offset: 0x00236A2A
		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x00238658 File Offset: 0x00236A58
		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			int num = Mathf.RoundToInt(colorPct * 100f);
			num = GenMath.PositiveMod(num, 100);
			return DebugMatsSpectrum.Mat(num, transparent);
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x0023868A File Offset: 0x00236A8A
		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x0023869C File Offset: 0x00236A9C
		public static void RenderCell(IntVec3 c, Material mat)
		{
			CellRenderer.InitFrame();
			if (CellRenderer.viewRect.Contains(c))
			{
				Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
				Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, mat, 0);
			}
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x002386E0 File Offset: 0x00236AE0
		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), 0.15f);
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x002386F8 File Offset: 0x00236AF8
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

		// Token: 0x04002E0B RID: 11787
		private static int lastCameraUpdateFrame = -1;

		// Token: 0x04002E0C RID: 11788
		private static CellRect viewRect;
	}
}
