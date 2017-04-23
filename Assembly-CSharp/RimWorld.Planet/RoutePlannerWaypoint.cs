using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				if (worldRoutePlanner.Active)
				{
					int num = worldRoutePlanner.waypoints.IndexOf(this);
					if (num >= 0)
					{
						return base.Label + ' ' + (num + 1);
					}
				}
				return base.Label;
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
					stringBuilder.Append("EstimatedTimeToThisWaypoint".Translate(new object[]
					{
						ticksToWaypoint.ToStringTicksToDays("0.#")
					}));
					if (num >= 2)
					{
						int ticksToWaypoint2 = worldRoutePlanner.GetTicksToWaypoint(num - 1);
						stringBuilder.AppendLine();
						stringBuilder.Append("EstimatedTimeToThisWaypointFromPrevious".Translate(new object[]
						{
							(ticksToWaypoint - ticksToWaypoint2).ToStringTicksToDays("0.#")
						}));
					}
				}
			}
			return stringBuilder.ToString();
		}

		[DebuggerHidden]
		public override IEnumerable<Gizmo> GetGizmos()
		{
			RoutePlannerWaypoint.<GetGizmos>c__Iterator107 <GetGizmos>c__Iterator = new RoutePlannerWaypoint.<GetGizmos>c__Iterator107();
			<GetGizmos>c__Iterator.<>f__this = this;
			RoutePlannerWaypoint.<GetGizmos>c__Iterator107 expr_0E = <GetGizmos>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
