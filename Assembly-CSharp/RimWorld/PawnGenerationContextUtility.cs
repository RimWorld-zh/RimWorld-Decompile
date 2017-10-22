using System;
using Verse;

namespace RimWorld
{
	public static class PawnGenerationContextUtility
	{
		public static string ToStringHuman(this PawnGenerationContext context)
		{
			string result;
			switch (context)
			{
			case PawnGenerationContext.All:
			{
				result = "PawnGenerationContext_All".Translate();
				break;
			}
			case PawnGenerationContext.PlayerStarter:
			{
				result = "PawnGenerationContext_PlayerStarter".Translate();
				break;
			}
			case PawnGenerationContext.NonPlayer:
			{
				result = "PawnGenerationContext_NonPlayer".Translate();
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
			}
			return result;
		}

		public static bool Includes(this PawnGenerationContext includer, PawnGenerationContext other)
		{
			return includer == PawnGenerationContext.All || includer == other;
		}

		public static PawnGenerationContext GetRandom()
		{
			Array values = Enum.GetValues(typeof(PawnGenerationContext));
			return (PawnGenerationContext)values.GetValue(Rand.Range(0, values.Length));
		}

		public static bool OverlapsWith(this PawnGenerationContext a, PawnGenerationContext b)
		{
			return (byte)((a == PawnGenerationContext.All || b == PawnGenerationContext.All) ? 1 : ((a == b) ? 1 : 0)) != 0;
		}
	}
}
