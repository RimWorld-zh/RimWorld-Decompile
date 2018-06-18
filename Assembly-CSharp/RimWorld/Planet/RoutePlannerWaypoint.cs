using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000605 RID: 1541
	public class RoutePlannerWaypoint : WorldObject
	{
		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x0010C5C8 File Offset: 0x0010A9C8
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

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0010C62C File Offset: 0x0010AA2C
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

		// Token: 0x06001EC8 RID: 7880 RVA: 0x0010C708 File Offset: 0x0010AB08
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
