using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200055D RID: 1373
	public static class FactionRelationKindUtility
	{
		// Token: 0x060019EB RID: 6635 RVA: 0x000E15A8 File Offset: 0x000DF9A8
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

		// Token: 0x060019EC RID: 6636 RVA: 0x000E160C File Offset: 0x000DFA0C
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
