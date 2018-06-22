using System;

namespace RimWorld.Planet
{
	// Token: 0x0200061D RID: 1565
	public static class EnterCooldownCompUtility
	{
		// Token: 0x06001FD5 RID: 8149 RVA: 0x001125BC File Offset: 0x001109BC
		public static bool EnterCooldownBlocksEntering(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return component != null && component.BlocksEntering;
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x001125E8 File Offset: 0x001109E8
		public static float EnterCooldownDaysLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return (component == null) ? 0f : component.DaysLeft;
		}
	}
}
