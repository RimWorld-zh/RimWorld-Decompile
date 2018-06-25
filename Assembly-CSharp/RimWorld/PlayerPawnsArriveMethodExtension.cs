using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000645 RID: 1605
	public static class PlayerPawnsArriveMethodExtension
	{
		// Token: 0x06002148 RID: 8520 RVA: 0x0011AADC File Offset: 0x00118EDC
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
