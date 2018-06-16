using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000639 RID: 1593
	public static class PawnGenerationContextUtility
	{
		// Token: 0x060020D4 RID: 8404 RVA: 0x00118BA4 File Offset: 0x00116FA4
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

		// Token: 0x060020D5 RID: 8405 RVA: 0x00118C04 File Offset: 0x00117004
		public static bool Includes(this PawnGenerationContext includer, PawnGenerationContext other)
		{
			return includer == PawnGenerationContext.All || includer == other;
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x00118C2C File Offset: 0x0011702C
		public static PawnGenerationContext GetRandom()
		{
			Array values = Enum.GetValues(typeof(PawnGenerationContext));
			return (PawnGenerationContext)values.GetValue(Rand.Range(0, values.Length));
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x00118C68 File Offset: 0x00117068
		public static bool OverlapsWith(this PawnGenerationContext a, PawnGenerationContext b)
		{
			return a == PawnGenerationContext.All || b == PawnGenerationContext.All || a == b;
		}
	}
}
