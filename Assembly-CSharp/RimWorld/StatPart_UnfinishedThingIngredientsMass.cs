using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B7 RID: 2487
	public class StatPart_UnfinishedThingIngredientsMass : StatPart
	{
		// Token: 0x060037B7 RID: 14263 RVA: 0x001DABA8 File Offset: 0x001D8FA8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x001DABCC File Offset: 0x001D8FCC
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

		// Token: 0x060037B9 RID: 14265 RVA: 0x001DAC10 File Offset: 0x001D9010
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
