using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C0 RID: 2496
	public class StatPart_WorkTableUnpowered : StatPart
	{
		// Token: 0x060037D0 RID: 14288 RVA: 0x001DAD92 File Offset: 0x001D9192
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				val *= req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
			}
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x001DADD0 File Offset: 0x001D91D0
		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				float unpoweredWorkTableWorkSpeedFactor = req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
				result = "NoPower".Translate() + ": x" + unpoweredWorkTableWorkSpeedFactor.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x001DAE3C File Offset: 0x001D923C
		public static bool Applies(Thing th)
		{
			bool result;
			if (th.def.building.unpoweredWorkTableWorkSpeedFactor == 0f)
			{
				result = false;
			}
			else
			{
				CompPowerTrader compPowerTrader = th.TryGetComp<CompPowerTrader>();
				result = (compPowerTrader != null && !compPowerTrader.PowerOn);
			}
			return result;
		}
	}
}
