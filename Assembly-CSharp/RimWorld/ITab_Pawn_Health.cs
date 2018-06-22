using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084E RID: 2126
	public class ITab_Pawn_Health : ITab
	{
		// Token: 0x06003038 RID: 12344 RVA: 0x001A401C File Offset: 0x001A241C
		public ITab_Pawn_Health()
		{
			this.size = new Vector2(630f, 430f);
			this.labelKey = "TabHealth";
			this.tutorTag = "Health";
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06003039 RID: 12345 RVA: 0x001A4050 File Offset: 0x001A2450
		private Pawn PawnForHealth
		{
			get
			{
				Pawn result;
				if (base.SelPawn != null)
				{
					result = base.SelPawn;
				}
				else
				{
					Corpse corpse = base.SelThing as Corpse;
					if (corpse != null)
					{
						result = corpse.InnerPawn;
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x001A409C File Offset: 0x001A249C
		protected override void FillTab()
		{
			Pawn pawnForHealth = this.PawnForHealth;
			if (pawnForHealth == null)
			{
				Log.Error("Health tab found no selected pawn to display.", false);
			}
			else
			{
				Corpse corpse = base.SelThing as Corpse;
				bool showBloodLoss = corpse == null || corpse.Age < 60000;
				Rect outRect = new Rect(0f, 20f, this.size.x, this.size.y - 20f);
				HealthCardUtility.DrawPawnHealthCard(outRect, pawnForHealth, this.ShouldAllowOperations(), showBloodLoss, base.SelThing);
			}
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x001A412C File Offset: 0x001A252C
		private bool ShouldAllowOperations()
		{
			Pawn pawnForHealth = this.PawnForHealth;
			bool result;
			if (pawnForHealth.Dead)
			{
				result = false;
			}
			else
			{
				result = (base.SelThing.def.AllRecipes.Any((RecipeDef x) => x.AvailableNow) && (pawnForHealth.Faction == Faction.OfPlayer || (pawnForHealth.IsPrisonerOfColony || (pawnForHealth.HostFaction == Faction.OfPlayer && !pawnForHealth.health.capacities.CapableOf(PawnCapacityDefOf.Moving))) || ((!pawnForHealth.RaceProps.IsFlesh || pawnForHealth.Faction == null || !pawnForHealth.Faction.HostileTo(Faction.OfPlayer)) && (!pawnForHealth.RaceProps.Humanlike && pawnForHealth.Downed))));
			}
			return result;
		}

		// Token: 0x04001A18 RID: 6680
		private const int HideBloodLossTicksThreshold = 60000;

		// Token: 0x04001A19 RID: 6681
		public const float Width = 630f;
	}
}
