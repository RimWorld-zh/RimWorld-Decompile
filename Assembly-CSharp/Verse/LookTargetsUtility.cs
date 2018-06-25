using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000EF4 RID: 3828
	public static class LookTargetsUtility
	{
		// Token: 0x06005B76 RID: 23414 RVA: 0x002EA804 File Offset: 0x002E8C04
		public static bool IsValid(this LookTargets lookTargets)
		{
			return lookTargets != null && lookTargets.IsValid;
		}

		// Token: 0x06005B77 RID: 23415 RVA: 0x002EA828 File Offset: 0x002E8C28
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

		// Token: 0x06005B78 RID: 23416 RVA: 0x002EA854 File Offset: 0x002E8C54
		public static void TryHighlight(this LookTargets lookTargets, bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			if (lookTargets != null)
			{
				lookTargets.Highlight(arrow, colonistBar, circleOverlay);
			}
		}
	}
}
