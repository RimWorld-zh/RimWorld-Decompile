using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000637 RID: 1591
	public static class PawnGenerationContextUtility
	{
		// Token: 0x060020D2 RID: 8402 RVA: 0x00118E18 File Offset: 0x00117218
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

		// Token: 0x060020D3 RID: 8403 RVA: 0x00118E78 File Offset: 0x00117278
		public static bool Includes(this PawnGenerationContext includer, PawnGenerationContext other)
		{
			return includer == PawnGenerationContext.All || includer == other;
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x00118EA0 File Offset: 0x001172A0
		public static PawnGenerationContext GetRandom()
		{
			Array values = Enum.GetValues(typeof(PawnGenerationContext));
			return (PawnGenerationContext)values.GetValue(Rand.Range(0, values.Length));
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x00118EDC File Offset: 0x001172DC
		public static bool OverlapsWith(this PawnGenerationContext a, PawnGenerationContext b)
		{
			return a == PawnGenerationContext.All || b == PawnGenerationContext.All || a == b;
		}
	}
}
