using System;

namespace RimWorld.Planet
{
	// Token: 0x0200061F RID: 1567
	public static class EnterCooldownCompUtility
	{
		// Token: 0x06001FD8 RID: 8152 RVA: 0x00112974 File Offset: 0x00110D74
		public static bool EnterCooldownBlocksEntering(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return component != null && component.BlocksEntering;
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x001129A0 File Offset: 0x00110DA0
		public static float EnterCooldownDaysLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return (component == null) ? 0f : component.DaysLeft;
		}
	}
}
