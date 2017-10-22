using Verse;

namespace RimWorld
{
	public class StatPart_UnfinishedThingIngredientsMass : StatPart
	{
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = default(float);
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			float mass = default(float);
			return (!this.TryGetValue(req, out mass)) ? null : ("StatsReport_IngredientsMass".Translate() + ": " + mass.ToStringMassOffset());
		}

		private bool TryGetValue(StatRequest req, out float value)
		{
			UnfinishedThing unfinishedThing = req.Thing as UnfinishedThing;
			bool result;
			if (unfinishedThing == null)
			{
				value = 0f;
				result = false;
			}
			else
			{
				float num = 0f;
				for (int i = 0; i < unfinishedThing.ingredients.Count; i++)
				{
					num += unfinishedThing.ingredients[i].GetStatValue(StatDefOf.Mass, true) * (float)unfinishedThing.ingredients[i].stackCount;
				}
				value = num;
				result = true;
			}
			return result;
		}
	}
}
