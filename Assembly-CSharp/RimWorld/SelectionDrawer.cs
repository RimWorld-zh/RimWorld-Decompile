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
		// Token: 0x04001A5C RID: 6748
		private static Dictionary<object, float> selectTimes = new Dictionary<object, float>();

		// Token: 0x04001A5D RID: 6749
		private static readonly Material SelectionBracketMat = MaterialPool.MatFrom("UI/Overlays/SelectionBracket", ShaderDatabase.MetaOverlay);

		// Token: 0x04001A5E RID: 6750
		private static Vector3[] bracketLocs = new Vector3[4];

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060030B1 RID: 12465 RVA: 0x001A70A4 File Offset: 0x001A54A4
		public static Dictionary<object, float> SelectTimes
		{
			get
			{
				return SelectionDrawer.selectTimes;
			}
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x001A70BE File Offset: 0x001A54BE
		public static void Notify_Selected(object t)
		{
			SelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x001A70D1 File Offset: 0x001A54D1
		public static void Clear()
		{
			SelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x001A70E0 File Offset: 0x001A54E0
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

		// Token: 0x060030B5 RID: 12469 RVA: 0x001A7158 File Offset: 0x001A5558
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
