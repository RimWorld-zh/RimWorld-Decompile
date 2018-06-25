using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B9 RID: 2489
	public class StatPart_UnfinishedThingIngredientsMass : StatPart
	{
		// Token: 0x060037BB RID: 14267 RVA: 0x001DACE8 File Offset: 0x001D90E8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x001DAD0C File Offset: 0x001D910C
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			string result;
			if (this.TryGetValue(req, out mass))
			{
				result = "StatsReport_IngredientsMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x001DAD50 File Offset: 0x001D9150
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
