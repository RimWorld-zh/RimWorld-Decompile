using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088C RID: 2188
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Predator : PawnColumnWorker_Icon
	{
		// Token: 0x060031E5 RID: 12773 RVA: 0x001AEC68 File Offset: 0x001AD068
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			Texture2D result;
			if (pawn.RaceProps.predator)
			{
				result = PawnColumnWorker_Predator.Icon;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x001AEC9C File Offset: 0x001AD09C
		protected override string GetIconTip(Pawn pawn)
		{
			return "IsPredator".Translate();
		}

		// Token: 0x04001AD2 RID: 6866
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Predator", true);
	}
}
