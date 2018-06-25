using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200055D RID: 1373
	public static class FactionRelationKindUtility
	{
		// Token: 0x060019EA RID: 6634 RVA: 0x000E1810 File Offset: 0x000DFC10
		public static string GetLabel(this FactionRelationKind kind)
		{
			string result;
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				result = "Hostile".Translate();
				break;
			case FactionRelationKind.Neutral:
				result = "Neutral".Translate();
				break;
			case FactionRelationKind.Ally:
				result = "Ally".Translate();
				break;
			default:
				result = "error";
				break;
			}
			return result;
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x000E1874 File Offset: 0x000DFC74
		public static Color GetColor(this FactionRelationKind kind)
		{
			Color result;
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				result = Color.red;
				break;
			case FactionRelationKind.Neutral:
				result = new Color(0f, 0.75f, 1f);
				break;
			case FactionRelationKind.Ally:
				result = Color.green;
				break;
			default:
				result = Color.white;
				break;
			}
			return result;
		}
	}
}
