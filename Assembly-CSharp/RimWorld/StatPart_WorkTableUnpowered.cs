using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BC RID: 2492
	public class StatPart_WorkTableUnpowered : StatPart
	{
		// Token: 0x060037CC RID: 14284 RVA: 0x001DB03E File Offset: 0x001D943E
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				val *= req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
			}
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x001DB07C File Offset: 0x001D947C
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

		// Token: 0x060037CE RID: 14286 RVA: 0x001DB0E8 File Offset: 0x001D94E8
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
