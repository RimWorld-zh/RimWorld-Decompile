using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000EF2 RID: 3826
	public static class LookTargetsUtility
	{
		// Token: 0x06005B73 RID: 23411 RVA: 0x002EA6E4 File Offset: 0x002E8AE4
		public static bool IsValid(this LookTargets lookTargets)
		{
			return lookTargets != null && lookTargets.IsValid;
		}

		// Token: 0x06005B74 RID: 23412 RVA: 0x002EA708 File Offset: 0x002E8B08
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

		// Token: 0x06005B75 RID: 23413 RVA: 0x002EA734 File Offset: 0x002E8B34
		public static void TryHighlight(this LookTargets lookTargets, bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			if (lookTargets != null)
			{
				lookTargets.Highlight(arrow, colonistBar, circleOverlay);
			}
		}
	}
}
