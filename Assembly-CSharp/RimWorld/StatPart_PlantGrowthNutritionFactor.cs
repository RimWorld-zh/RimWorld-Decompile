using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B3 RID: 2483
	public class StatPart_PlantGrowthNutritionFactor : StatPart
	{
		// Token: 0x060037A3 RID: 14243 RVA: 0x001DA4B8 File Offset: 0x001D88B8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x001DA4DC File Offset: 0x001D88DC
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

		// Token: 0x060037A5 RID: 14245 RVA: 0x001DA574 File Offset: 0x001D8974
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
