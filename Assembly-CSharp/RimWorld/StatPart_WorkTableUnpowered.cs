using System;
using Verse;

namespace RimWorld
{
	public class StatPart_WorkTableUnpowered : StatPart
	{
		public StatPart_WorkTableUnpowered()
		{
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				val *= req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
			}
		}

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
