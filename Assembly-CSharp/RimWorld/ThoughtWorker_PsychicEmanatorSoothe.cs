using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_PsychicEmanatorSoothe : ThoughtWorker
	{
		private const float Radius = 15f;

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (!p.Spawned)
			{
				result = false;
			}
			else
			{
				List<Thing> list = p.Map.listerThings.ThingsOfDef(ThingDefOf.PsychicEmanator);
				for (int i = 0; i < list.Count; i++)
				{
					CompPowerTrader compPowerTrader = list[i].TryGetComp<CompPowerTrader>();
					if ((compPowerTrader == null || compPowerTrader.PowerOn) && p.Position.InHorDistOf(list[i].Position, 15f))
						goto IL_007e;
				}
				result = false;
			}
			goto IL_00a7;
			IL_00a7:
			return result;
			IL_007e:
			result = true;
			goto IL_00a7;
		}
	}
}
