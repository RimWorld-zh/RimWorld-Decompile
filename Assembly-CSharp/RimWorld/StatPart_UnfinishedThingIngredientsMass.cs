using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BB RID: 2491
	public class StatPart_UnfinishedThingIngredientsMass : StatPart
	{
		// Token: 0x060037BD RID: 14269 RVA: 0x001DA9D0 File Offset: 0x001D8DD0
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x001DA9F4 File Offset: 0x001D8DF4
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

		// Token: 0x060037BF RID: 14271 RVA: 0x001DAA38 File Offset: 0x001D8E38
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
