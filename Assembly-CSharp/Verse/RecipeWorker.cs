using RimWorld;
using System.Collections.Generic;

namespace Verse
{
	public class RecipeWorker
	{
		public RecipeDef recipe;

		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			yield break;
		}

		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients)
		{
		}

		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			if (pawn.Faction == billDoerFaction)
			{
				return false;
			}
			return this.recipe.isViolation;
		}

		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.LabelCap;
		}

		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}
	}
}
