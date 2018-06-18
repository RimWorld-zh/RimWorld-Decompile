using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000EF2 RID: 3826
	public static class LookTargetsUtility
	{
		// Token: 0x06005B4B RID: 23371 RVA: 0x002E86B0 File Offset: 0x002E6AB0
		public static bool IsValid(this LookTargets lookTargets)
		{
			return lookTargets != null && lookTargets.IsValid;
		}

		// Token: 0x06005B4C RID: 23372 RVA: 0x002E86D4 File Offset: 0x002E6AD4
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

		// Token: 0x06005B4D RID: 23373 RVA: 0x002E8700 File Offset: 0x002E6B00
		public static void TryHighlight(this LookTargets lookTargets, bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			if (lookTargets != null)
			{
				lookTargets.Highlight(arrow, colonistBar, circleOverlay);
			}
		}
	}
}
