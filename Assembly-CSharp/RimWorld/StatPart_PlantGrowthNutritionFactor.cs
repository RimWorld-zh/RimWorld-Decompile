using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B1 RID: 2481
	public class StatPart_PlantGrowthNutritionFactor : StatPart
	{
		// Token: 0x0600379F RID: 14239 RVA: 0x001DA378 File Offset: 0x001D8778
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x001DA39C File Offset: 0x001D879C
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

		// Token: 0x060037A1 RID: 14241 RVA: 0x001DA434 File Offset: 0x001D8834
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
