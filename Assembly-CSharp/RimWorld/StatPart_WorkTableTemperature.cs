using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BB RID: 2491
	public class StatPart_WorkTableTemperature : StatPart
	{
		// Token: 0x040023C1 RID: 9153
		public const float WorkRateFactor = 0.7f;

		// Token: 0x040023C2 RID: 9154
		public const float MinTemp = 9f;

		// Token: 0x040023C3 RID: 9155
		public const float MaxTemp = 35f;

		// Token: 0x060037C7 RID: 14279 RVA: 0x001DAF0C File Offset: 0x001D930C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				val *= 0.7f;
			}
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x001DAF38 File Offset: 0x001D9338
		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				result = "BadTemperature".Translate().CapitalizeFirst() + ": x" + 0.7f.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x001DAF94 File Offset: 0x001D9394
		public static bool Applies(Thing t)
		{
			return t.Spawned && StatPart_WorkTableTemperature.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x001DAFD4 File Offset: 0x001D93D4
		public static bool Applies(ThingDef tDef, Map map, IntVec3 c)
		{
			bool result;
			if (map == null)
			{
				result = false;
			}
			else if (tDef.building == null || !tDef.building.workSpeedPenaltyTemperature)
			{
				result = false;
			}
			else
			{
				float temperatureForCell = GenTemperature.GetTemperatureForCell(c, map);
				result = (temperatureForCell < 9f || temperatureForCell > 35f);
			}
			return result;
		}
	}
}
