using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BE RID: 2494
	public class StatPart_WorkTableUnpowered : StatPart
	{
		// Token: 0x060037D0 RID: 14288 RVA: 0x001DB452 File Offset: 0x001D9852
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				val *= req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
			}
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x001DB490 File Offset: 0x001D9890
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

		// Token: 0x060037D2 RID: 14290 RVA: 0x001DB4FC File Offset: 0x001D98FC
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
