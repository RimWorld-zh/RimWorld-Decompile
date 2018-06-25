using System;
using RimWorld.Planet;

namespace Verse
{
	public static class LookTargetsUtility
	{
		public static bool IsValid(this LookTargets lookTargets)
		{
			return lookTargets != null && lookTargets.IsValid;
		}

		public static GlobalTargetInfo TryGetPrimaryTarget(this LookTargets lookTargets)
		{
			GlobalTargetInfo result;
			if (lookTargets == null)
			{
				result = GlobalTargetInfo.Invalid;
			}
			else
			{
				result = lookTargets.PrimaryTarget;
			}
			return result;
		}

		public static void TryHighlight(this LookTargets lookTargets, bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			if (lookTargets != null)
			{
				lookTargets.Highlight(arrow, colonistBar, circleOverlay);
			}
		}
	}
}
