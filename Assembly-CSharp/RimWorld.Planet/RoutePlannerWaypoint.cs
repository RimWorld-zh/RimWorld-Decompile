using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	public class RoutePlannerWaypoint : WorldObject
	{
		public override string Label
		{
			get
			{
				WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
				string result;
				if (worldRoutePlanner.Active)
				{
					int num = worldRoutePlanner.waypoints.IndexOf(this);
					if (num >= 0)
					{
						result = base.Label + ' ' + (num + 1);
						goto IL_0054;
					}
				}
				result = base.Label;
				goto IL_0054;
				IL_0054:
				return result;
			}
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
			if (worldRoutePlanner.Active)
			{
				int num = worldRoutePlanner.waypoints.IndexOf(this);
				if (num >= 1)
				{
					int ticksToWaypoint = worldRoutePlanner.GetTicksToWaypoint(num);
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("EstimatedTimeToThisWaypoint".Translate(ticksToWaypoint.ToStringTicksToDays("0.#")));
					if (num >= 2)
					{
						int ticksToWaypoint2 = worldRoutePlanner.GetTicksToWaypoint(num - 1);
						stringBuilder.AppendLine();
						stringBuilder.Append("EstimatedTimeToThisWaypointFromPrevious".Translate((ticksToWaypoint - ticksToWaypoint2).ToStringTicksToDays("0.#")));
					}
				}
			}
			return stringBuilder.ToString();
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "CommandRemoveWaypointLabel".Translate(),
				defaultDesc = "CommandRemoveWaypointDesc".Translate(),
				icon = TexCommand.RemoveRoutePlannerWaypoint,
				action = (Action)delegate
				{
					Find.WorldRoutePlanner.TryRemoveWaypoint(((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_0104: stateMachine*/)._0024this, true);
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_013f:
			/*Error near IL_0140: Unexpected return in MoveNext()*/;
		}
	}
}
