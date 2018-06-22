using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000861 RID: 2145
	[StaticConstructorOnStartup]
	public static class SelectionDrawer
	{
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060030AE RID: 12462 RVA: 0x001A6CEC File Offset: 0x001A50EC
		public static Dictionary<object, float> SelectTimes
		{
			get
			{
				return SelectionDrawer.selectTimes;
			}
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x001A6D06 File Offset: 0x001A5106
		public static void Notify_Selected(object t)
		{
			SelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x001A6D19 File Offset: 0x001A5119
		public static void Clear()
		{
			SelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x001A6D28 File Offset: 0x001A5128
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

		// Token: 0x060030B2 RID: 12466 RVA: 0x001A6DA0 File Offset: 0x001A51A0
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

		// Token: 0x04001A58 RID: 6744
		private static Dictionary<object, float> selectTimes = new Dictionary<object, float>();

		// Token: 0x04001A59 RID: 6745
		private static readonly Material SelectionBracketMat = MaterialPool.MatFrom("UI/Overlays/SelectionBracket", ShaderDatabase.MetaOverlay);

		// Token: 0x04001A5A RID: 6746
		private static Vector3[] bracketLocs = new Vector3[4];
	}
}
