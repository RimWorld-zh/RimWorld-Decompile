using System;

namespace RimWorld.Planet
{
	// Token: 0x02000621 RID: 1569
	public static class EnterCooldownCompUtility
	{
		// Token: 0x06001FDC RID: 8156 RVA: 0x001124F0 File Offset: 0x001108F0
		public static bool EnterCooldownBlocksEntering(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return component != null && component.BlocksEntering;
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x0011251C File Offset: 0x0011091C
		public static float EnterCooldownDaysLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return (component == null) ? 0f : component.DaysLeft;
		}
	}
}
