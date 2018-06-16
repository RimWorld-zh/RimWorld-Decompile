using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000EF3 RID: 3827
	public static class LookTargetsUtility
	{
		// Token: 0x06005B4D RID: 23373 RVA: 0x002E85D8 File Offset: 0x002E69D8
		public static bool IsValid(this LookTargets lookTargets)
		{
			return lookTargets != null && lookTargets.IsValid;
		}

		// Token: 0x06005B4E RID: 23374 RVA: 0x002E85FC File Offset: 0x002E69FC
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

		// Token: 0x06005B4F RID: 23375 RVA: 0x002E8628 File Offset: 0x002E6A28
		public static void TryHighlight(this LookTargets lookTargets, bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			if (lookTargets != null)
			{
				lookTargets.Highlight(arrow, colonistBar, circleOverlay);
			}
		}
	}
}
