using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BD RID: 2493
	public class StatPart_WorkTableTemperature : StatPart
	{
		// Token: 0x040023C9 RID: 9161
		public const float WorkRateFactor = 0.7f;

		// Token: 0x040023CA RID: 9162
		public const float MinTemp = 9f;

		// Token: 0x040023CB RID: 9163
		public const float MaxTemp = 35f;

		// Token: 0x060037CB RID: 14283 RVA: 0x001DB320 File Offset: 0x001D9720
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				val *= 0.7f;
			}
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x001DB34C File Offset: 0x001D974C
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

		// Token: 0x060037CD RID: 14285 RVA: 0x001DB3A8 File Offset: 0x001D97A8
		public static bool Applies(Thing t)
		{
			return t.Spawned && StatPart_WorkTableTemperature.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x001DB3E8 File Offset: 0x001D97E8
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
