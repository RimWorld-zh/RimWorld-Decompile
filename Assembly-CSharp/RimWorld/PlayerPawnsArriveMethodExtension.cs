using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000645 RID: 1605
	public static class PlayerPawnsArriveMethodExtension
	{
		// Token: 0x06002147 RID: 8519 RVA: 0x0011AD44 File Offset: 0x00119144
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
