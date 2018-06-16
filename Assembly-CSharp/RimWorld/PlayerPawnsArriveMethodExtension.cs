using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000647 RID: 1607
	public static class PlayerPawnsArriveMethodExtension
	{
		// Token: 0x0600214A RID: 8522 RVA: 0x0011A814 File Offset: 0x00118C14
		public static string ToStringHuman(this PlayerPawnsArriveMethod method)
		{
			string result;
			if (method != PlayerPawnsArriveMethod.Standing)
			{
				if (method != PlayerPawnsArriveMethod.DropPods)
				{
					throw new NotImplementedException();
				}
				result = "PlayerPawnsArriveMethod_DropPods".Translate();
			}
			else
			{
				result = "PlayerPawnsArriveMethod_Standing".Translate();
			}
			return result;
		}
	}
}
