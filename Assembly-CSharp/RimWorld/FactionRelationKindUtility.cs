using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200055F RID: 1375
	public static class FactionRelationKindUtility
	{
		// Token: 0x060019F0 RID: 6640 RVA: 0x000E1404 File Offset: 0x000DF804
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

		// Token: 0x060019F1 RID: 6641 RVA: 0x000E1468 File Offset: 0x000DF868
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
