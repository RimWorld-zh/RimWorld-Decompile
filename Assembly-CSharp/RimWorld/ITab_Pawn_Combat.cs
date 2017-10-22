using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ITab_Pawn_Combat : ITab
	{
		public const float Width = 630f;

		private Pawn SelPawnForCombatInfo
		{
			get
			{
				Pawn result;
				if (base.SelPawn != null)
				{
					result = base.SelPawn;
					goto IL_004e;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					result = corpse.InnerPawn;
					goto IL_004e;
				}
				throw new InvalidOperationException("Social tab on non-pawn non-corpse " + base.SelThing);
				IL_004e:
				return result;
			}
		}

		public ITab_Pawn_Combat()
		{
			base.size = new Vector2(630f, 510f);
			base.labelKey = "TabCombat";
		}

		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, base.size.x, base.size.y);
			rect = rect.ContractedBy(10f);
			rect.yMin += 17f;
			InteractionCardUtility.DrawInteractionsLog(rect, this.SelPawnForCombatInfo, Find.BattleLog.RawEntries, 50);
		}
	}
}
