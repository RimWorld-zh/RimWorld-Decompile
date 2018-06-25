using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class StatPart_PlantGrowthNutritionFactor : StatPart
	{
		public StatPart_PlantGrowthNutritionFactor()
		{
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			float f;
			string result;
			if (this.TryGetFactor(req, out f))
			{
				Plant plant = (Plant)req.Thing;
				string text = "StatsReport_PlantGrowth".Translate(new object[]
				{
					plant.Growth.ToStringPercent()
				}) + ": x" + f.ToStringPercent();
				if (!plant.def.plant.Sowable)
				{
					text = text + " (" + "StatsReport_PlantGrowth_Wild".Translate() + ")";
				}
				result = text;
			}
			else
			{
				result = null;
			}
			return result;
		}

		private bool TryGetFactor(StatRequest req, out float factor)
		{
			bool result;
			if (!req.HasThing)
			{
				factor = 1f;
				result = false;
			}
			else
			{
				Plant plant = req.Thing as Plant;
				if (plant == null)
				{
					factor = 1f;
					result = false;
				}
				else if (plant.def.plant.Sowable)
				{
					factor = plant.Growth;
					result = true;
				}
				else
				{
					factor = Mathf.Lerp(0.5f, 1f, plant.Growth);
					result = true;
				}
			}
			return result;
		}
	}
}
