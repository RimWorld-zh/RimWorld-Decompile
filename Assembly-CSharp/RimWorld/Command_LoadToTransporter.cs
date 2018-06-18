using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000748 RID: 1864
	[StaticConstructorOnStartup]
	public class Command_LoadToTransporter : Command
	{
		// Token: 0x0600293F RID: 10559 RVA: 0x0015EEF0 File Offset: 0x0015D2F0
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			if (this.transporters == null)
			{
				this.transporters = new List<CompTransporter>();
			}
			if (!this.transporters.Contains(this.transComp))
			{
				this.transporters.Add(this.transComp);
			}
			CompLaunchable launchable = this.transComp.Launchable;
			if (launchable != null)
			{
				Building fuelingPortSource = launchable.FuelingPortSource;
				if (fuelingPortSource != null)
				{
					Map map = this.transComp.Map;
					Command_LoadToTransporter.tmpFuelingPortGivers.Clear();
					map.floodFiller.FloodFill(fuelingPortSource.Position, (IntVec3 x) => FuelingPortUtility.AnyFuelingPortGiverAt(x, map), delegate(IntVec3 x)
					{
						Command_LoadToTransporter.tmpFuelingPortGivers.Add(FuelingPortUtility.FuelingPortGiverAt(x, map));
					}, int.MaxValue, false, null);
					for (int i = 0; i < this.transporters.Count; i++)
					{
						Building fuelingPortSource2 = this.transporters[i].Launchable.FuelingPortSource;
						if (fuelingPortSource2 != null && !Command_LoadToTransporter.tmpFuelingPortGivers.Contains(fuelingPortSource2))
						{
							Messages.Message("MessageTransportersNotAdjacent".Translate(), fuelingPortSource2, MessageTypeDefOf.RejectInput, false);
							return;
						}
					}
				}
			}
			for (int j = 0; j < this.transporters.Count; j++)
			{
				if (this.transporters[j] != this.transComp)
				{
					if (!this.transComp.Map.reachability.CanReach(this.transComp.parent.Position, this.transporters[j].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
					{
						Messages.Message("MessageTransporterUnreachable".Translate(), this.transporters[j].parent, MessageTypeDefOf.RejectInput, false);
						return;
					}
				}
			}
			Find.WindowStack.Add(new Dialog_LoadTransporters(this.transComp.Map, this.transporters));
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x0015F108 File Offset: 0x0015D508
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_LoadToTransporter command_LoadToTransporter = (Command_LoadToTransporter)other;
			bool result;
			if (command_LoadToTransporter.transComp.parent.def != this.transComp.parent.def)
			{
				result = false;
			}
			else
			{
				if (this.transporters == null)
				{
					this.transporters = new List<CompTransporter>();
				}
				this.transporters.Add(command_LoadToTransporter.transComp);
				result = false;
			}
			return result;
		}

		// Token: 0x0400167E RID: 5758
		public CompTransporter transComp;

		// Token: 0x0400167F RID: 5759
		private List<CompTransporter> transporters;

		// Token: 0x04001680 RID: 5760
		private static HashSet<Building> tmpFuelingPortGivers = new HashSet<Building>();
	}
}
