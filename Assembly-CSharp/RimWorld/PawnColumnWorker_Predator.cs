using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088C RID: 2188
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Predator : PawnColumnWorker_Icon
	{
		// Token: 0x060031E7 RID: 12775 RVA: 0x001AED30 File Offset: 0x001AD130
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

		// Token: 0x060031E8 RID: 12776 RVA: 0x001AED64 File Offset: 0x001AD164
		protected override string GetIconTip(Pawn pawn)
		{
			return "IsPredator".Translate();
		}

		// Token: 0x04001AD2 RID: 6866
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Predator", true);
	}
}
