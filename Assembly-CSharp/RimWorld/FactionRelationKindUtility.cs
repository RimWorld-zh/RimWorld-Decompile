using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200055B RID: 1371
	public static class FactionRelationKindUtility
	{
		// Token: 0x060019E7 RID: 6631 RVA: 0x000E1458 File Offset: 0x000DF858
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

		// Token: 0x060019E8 RID: 6632 RVA: 0x000E14BC File Offset: 0x000DF8BC
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
