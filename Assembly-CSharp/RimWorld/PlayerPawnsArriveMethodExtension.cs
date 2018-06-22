using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000643 RID: 1603
	public static class PlayerPawnsArriveMethodExtension
	{
		// Token: 0x06002144 RID: 8516 RVA: 0x0011A98C File Offset: 0x00118D8C
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
