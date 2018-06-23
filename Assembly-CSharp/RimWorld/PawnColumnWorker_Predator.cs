using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000888 RID: 2184
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Predator : PawnColumnWorker_Icon
	{
		// Token: 0x04001AD0 RID: 6864
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Predator", true);

		// Token: 0x060031E0 RID: 12768 RVA: 0x001AEF18 File Offset: 0x001AD318
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

		// Token: 0x060031E1 RID: 12769 RVA: 0x001AEF4C File Offset: 0x001AD34C
		protected override string GetIconTip(Pawn pawn)
		{
			return "IsPredator".Translate();
		}
	}
}
