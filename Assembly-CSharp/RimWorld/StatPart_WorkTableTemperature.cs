using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BF RID: 2495
	public class StatPart_WorkTableTemperature : StatPart
	{
		// Token: 0x060037CB RID: 14283 RVA: 0x001DAC60 File Offset: 0x001D9060
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				val *= 0.6f;
			}
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x001DAC8C File Offset: 0x001D908C
		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				result = "BadTemperature".Translate().CapitalizeFirst() + ": x" + 0.6f.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x001DACE8 File Offset: 0x001D90E8
		public static bool Applies(Thing t)
		{
			return t.Spawned && StatPart_WorkTableTemperature.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x001DAD28 File Offset: 0x001D9128
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
				result = (temperatureForCell < 5f || temperatureForCell > 35f);
			}
			return result;
		}

		// Token: 0x040023C6 RID: 9158
		public const float WorkRateFactor = 0.6f;

		// Token: 0x040023C7 RID: 9159
		public const float MinTemp = 5f;

		// Token: 0x040023C8 RID: 9160
		public const float MaxTemp = 35f;
	}
}
