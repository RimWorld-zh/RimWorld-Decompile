using System;

namespace RimWorld.Planet
{
	// Token: 0x02000621 RID: 1569
	public static class EnterCooldownCompUtility
	{
		// Token: 0x06001FDE RID: 8158 RVA: 0x00112568 File Offset: 0x00110968
		public static bool EnterCooldownBlocksEntering(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return component != null && component.BlocksEntering;
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x00112594 File Offset: 0x00110994
		public static float EnterCooldownDaysLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return (component == null) ? 0f : component.DaysLeft;
		}
	}
}
