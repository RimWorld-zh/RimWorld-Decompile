using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class FactionRelationKindUtility
	{
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
