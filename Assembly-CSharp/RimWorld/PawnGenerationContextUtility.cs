using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000637 RID: 1591
	public static class PawnGenerationContextUtility
	{
		// Token: 0x060020D1 RID: 8401 RVA: 0x00119080 File Offset: 0x00117480
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

		// Token: 0x060020D2 RID: 8402 RVA: 0x001190E0 File Offset: 0x001174E0
		public static bool Includes(this PawnGenerationContext includer, PawnGenerationContext other)
		{
			return includer == PawnGenerationContext.All || includer == other;
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00119108 File Offset: 0x00117508
		public static PawnGenerationContext GetRandom()
		{
			Array values = Enum.GetValues(typeof(PawnGenerationContext));
			return (PawnGenerationContext)values.GetValue(Rand.Range(0, values.Length));
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x00119144 File Offset: 0x00117544
		public static bool OverlapsWith(this PawnGenerationContext a, PawnGenerationContext b)
		{
			return a == PawnGenerationContext.All || b == PawnGenerationContext.All || a == b;
		}
	}
}
