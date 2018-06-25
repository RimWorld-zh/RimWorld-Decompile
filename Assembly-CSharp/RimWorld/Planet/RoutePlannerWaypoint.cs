using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000603 RID: 1539
	public class RoutePlannerWaypoint : WorldObject
	{
		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001EC1 RID: 7873 RVA: 0x0010C760 File Offset: 0x0010AB60
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

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0010C7C4 File Offset: 0x0010ABC4
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
					stringBuilder.Append("EstimatedTimeToWaypoint".Translate(new object[]
					{
						ticksToWaypoint.ToStringTicksToDays("0.#")
					}));
					if (num >= 2)
					{
						int ticksToWaypoint2 = worldRoutePlanner.GetTicksToWaypoint(num - 1);
						stringBuilder.AppendLine();
						stringBuilder.Append("EstimatedTimeToWaypointFromPrevious".Translate(new object[]
						{
							(ticksToWaypoint - ticksToWaypoint2).ToStringTicksToDays("0.#")
						}));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0010C8A0 File Offset: 0x0010ACA0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			yield return new Command_Action
			{
				defaultLabel = "CommandRemoveWaypointLabel".Translate(),
				defaultDesc = "CommandRemoveWaypointDesc".Translate(),
				icon = TexCommand.RemoveRoutePlannerWaypoint,
				action = delegate()
				{
					Find.WorldRoutePlanner.TryRemoveWaypoint(this, true);
				}
			};
			yield break;
		}
	}
}
