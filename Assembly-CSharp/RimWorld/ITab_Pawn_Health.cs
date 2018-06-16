using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000852 RID: 2130
	public class ITab_Pawn_Health : ITab
	{
		// Token: 0x0600303D RID: 12349 RVA: 0x001A3D74 File Offset: 0x001A2174
		public ITab_Pawn_Health()
		{
			this.size = new Vector2(630f, 430f);
			this.labelKey = "TabHealth";
			this.tutorTag = "Health";
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x0600303E RID: 12350 RVA: 0x001A3DA8 File Offset: 0x001A21A8
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

		// Token: 0x0600303F RID: 12351 RVA: 0x001A3DF4 File Offset: 0x001A21F4
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

		// Token: 0x06003040 RID: 12352 RVA: 0x001A3E84 File Offset: 0x001A2284
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

		// Token: 0x04001A1A RID: 6682
		private const int HideBloodLossTicksThreshold = 60000;

		// Token: 0x04001A1B RID: 6683
		public const float Width = 630f;
	}
}
