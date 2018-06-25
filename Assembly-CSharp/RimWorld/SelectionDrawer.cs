using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000863 RID: 2147
	[StaticConstructorOnStartup]
	public static class SelectionDrawer
	{
		// Token: 0x04001A58 RID: 6744
		private static Dictionary<object, float> selectTimes = new Dictionary<object, float>();

		// Token: 0x04001A59 RID: 6745
		private static readonly Material SelectionBracketMat = MaterialPool.MatFrom("UI/Overlays/SelectionBracket", ShaderDatabase.MetaOverlay);

		// Token: 0x04001A5A RID: 6746
		private static Vector3[] bracketLocs = new Vector3[4];

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x001A6E3C File Offset: 0x001A523C
		public static Dictionary<object, float> SelectTimes
		{
			get
			{
				return SelectionDrawer.selectTimes;
			}
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x001A6E56 File Offset: 0x001A5256
		public static void Notify_Selected(object t)
		{
			SelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x001A6E69 File Offset: 0x001A5269
		public static void Clear()
		{
			SelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x060030B5 RID: 12469 RVA: 0x001A6E78 File Offset: 0x001A5278
		public static void DrawSelectionOverlays()
		{
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				SelectionDrawer.DrawSelectionBracketFor(obj);
				Thing thing = obj as Thing;
				if (thing != null)
				{
					thing.DrawExtraSelectionOverlays();
				}
			}
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x001A6EF0 File Offset: 0x001A52F0
		private static void DrawSelectionBracketFor(object obj)
		{
			Zone zone = obj as Zone;
			if (zone != null)
			{
				GenDraw.DrawFieldEdges(zone.Cells);
			}
			Thing thing = obj as Thing;
			if (thing != null)
			{
				SelectionDrawerUtility.CalculateSelectionBracketPositionsWorld<object>(SelectionDrawer.bracketLocs, thing, thing.DrawPos, thing.RotatedSize.ToVector2(), SelectionDrawer.selectTimes, Vector2.one, 1f);
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					Quaternion rotation = Quaternion.AngleAxis((float)num, Vector3.up);
					Graphics.DrawMesh(MeshPool.plane10, SelectionDrawer.bracketLocs[i], rotation, SelectionDrawer.SelectionBracketMat, 0);
					num -= 90;
				}
			}
		}
	}
}
