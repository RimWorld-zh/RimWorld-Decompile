using UnityEngine;

namespace Verse
{
	public static class CellRenderer
	{
		private static int lastCameraUpdateFrame = -1;

		private static CellRect viewRect;

		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			int x = Mathf.RoundToInt((float)(colorPct * 100.0));
			x = GenMath.PositiveMod(x, 100);
			return DebugMatsSpectrum.Mat(x, transparent);
		}

		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		public static void RenderCell(IntVec3 c, Material mat)
		{
			CellRenderer.InitFrame();
			if (CellRenderer.viewRect.Contains(c))
			{
				Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
				Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, mat, 0);
			}
		}

		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), 0.15f);
		}

		public static void RenderSpot(Vector3 loc, Material mat, float scale = 0.15f)
		{
			CellRenderer.InitFrame();
			if (CellRenderer.viewRect.Contains(loc.ToIntVec3()))
			{
				loc.y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
				Vector3 s = new Vector3(scale, 1f, scale);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(loc, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.circle, matrix, mat, 0);
			}
		}
	}
}
