using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ITab_Pawn_Health : ITab
	{
		private const int HideBloodLossTicksThreshold = 60000;

		public const float Width = 630f;

		private Pawn PawnForHealth
		{
			get
			{
				if (base.SelPawn != null)
				{
					return base.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return null;
			}
		}

		public ITab_Pawn_Health()
		{
			base.size = new Vector2(630f, 430f);
			base.labelKey = "TabHealth";
			base.tutorTag = "Health";
		}

		protected override void FillTab()
		{
			Pawn pawnForHealth = this.PawnForHealth;
			if (pawnForHealth == null)
			{
				Log.Error("Health tab found no selected pawn to display.");
			}
			else
			{
				Corpse corpse = base.SelThing as Corpse;
				bool showBloodLoss = corpse == null || corpse.Age < 60000;
				Rect outRect = new Rect(0f, 20f, base.size.x, (float)(base.size.y - 20.0));
				HealthCardUtility.DrawPawnHealthCard(outRect, pawnForHealth, this.ShouldAllowOperations(), showBloodLoss, base.SelThing);
			}
		}

		private bool ShouldAllowOperations()
		{
			Pawn pawnForHealth = this.PawnForHealth;
			if (pawnForHealth.Dead)
			{
				return false;
			}
			if (!base.SelThing.def.AllRecipes.Any((RecipeDef x) => x.AvailableNow))
			{
				return false;
			}
			if (pawnForHealth.Faction == Faction.OfPlayer)
			{
				return true;
			}
			if (!pawnForHealth.IsPrisonerOfColony && (pawnForHealth.HostFaction != Faction.OfPlayer || pawnForHealth.health.capacities.CapableOf(PawnCapacityDefOf.Moving)))
			{
				if (pawnForHealth.RaceProps.IsFlesh && pawnForHealth.Faction != null && pawnForHealth.Faction.HostileTo(Faction.OfPlayer))
				{
					return false;
				}
				if (!pawnForHealth.RaceProps.Humanlike && pawnForHealth.Downed)
				{
					return true;
				}
				return false;
			}
			return true;
		}
	}
}
