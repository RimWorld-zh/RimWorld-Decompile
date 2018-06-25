using System;

namespace RimWorld.Planet
{
	// Token: 0x0200061F RID: 1567
	public static class EnterCooldownCompUtility
	{
		// Token: 0x06001FD9 RID: 8153 RVA: 0x0011270C File Offset: 0x00110B0C
		public static bool EnterCooldownBlocksEntering(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return component != null && component.BlocksEntering;
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x00112738 File Offset: 0x00110B38
		public static float EnterCooldownDaysLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return (component == null) ? 0f : component.DaysLeft;
		}
	}
}
