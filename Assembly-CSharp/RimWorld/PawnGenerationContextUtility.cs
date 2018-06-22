using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000635 RID: 1589
	public static class PawnGenerationContextUtility
	{
		// Token: 0x060020CE RID: 8398 RVA: 0x00118CC8 File Offset: 0x001170C8
		public static string ToStringHuman(this PawnGenerationContext context)
		{
			string result;
			if (context != PawnGenerationContext.All)
			{
				if (context != PawnGenerationContext.PlayerStarter)
				{
					if (context != PawnGenerationContext.NonPlayer)
					{
						throw new NotImplementedException();
					}
					result = "PawnGenerationContext_NonPlayer".Translate();
				}
				else
				{
					result = "PawnGenerationContext_PlayerStarter".Translate();
				}
			}
			else
			{
				result = "PawnGenerationContext_All".Translate();
			}
			return result;
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x00118D28 File Offset: 0x00117128
		public static bool Includes(this PawnGenerationContext includer, PawnGenerationContext other)
		{
			return includer == PawnGenerationContext.All || includer == other;
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00118D50 File Offset: 0x00117150
		public static PawnGenerationContext GetRandom()
		{
			Array values = Enum.GetValues(typeof(PawnGenerationContext));
			return (PawnGenerationContext)values.GetValue(Rand.Range(0, values.Length));
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00118D8C File Offset: 0x0011718C
		public static bool OverlapsWith(this PawnGenerationContext a, PawnGenerationContext b)
		{
			return a == PawnGenerationContext.All || b == PawnGenerationContext.All || a == b;
		}
	}
}
