using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200072B RID: 1835
	[StaticConstructorOnStartup]
	public class Command_SetTargetFuelLevel : Command
	{
		// Token: 0x06002888 RID: 10376 RVA: 0x0015A7E4 File Offset: 0x00158BE4
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			if (this.refuelables == null)
			{
				this.refuelables = new List<CompRefuelable>();
			}
			if (!this.refuelables.Contains(this.refuelable))
			{
				this.refuelables.Add(this.refuelable);
			}
			int num = int.MaxValue;
			for (int i = 0; i < this.refuelables.Count; i++)
			{
				if ((int)this.refuelables[i].Props.fuelCapacity < num)
				{
					num = (int)this.refuelables[i].Props.fuelCapacity;
				}
			}
			int startingValue = num / 2;
			for (int j = 0; j < this.refuelables.Count; j++)
			{
				if ((int)this.refuelables[j].TargetFuelLevel <= num)
				{
					startingValue = (int)this.refuelables[j].TargetFuelLevel;
					break;
				}
			}
			Func<int, string> textGetter;
			if (this.refuelable.parent.def.building.hasFuelingPort)
			{
				textGetter = ((int x) => "SetPodLauncherTargetFuelLevel".Translate(new object[]
				{
					x,
					CompLaunchable.MaxLaunchDistanceAtFuelLevel((float)x)
				}));
			}
			else
			{
				textGetter = ((int x) => "SetTargetFuelLevel".Translate(new object[]
				{
					x
				}));
			}
			Dialog_Slider window = new Dialog_Slider(textGetter, 0, num, delegate(int value)
			{
				for (int k = 0; k < this.refuelables.Count; k++)
				{
					this.refuelables[k].TargetFuelLevel = (float)value;
				}
			}, startingValue);
			Find.WindowStack.Add(window);
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x0015A968 File Offset: 0x00158D68
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			if (this.refuelables == null)
			{
				this.refuelables = new List<CompRefuelable>();
			}
			this.refuelables.Add(((Command_SetTargetFuelLevel)other).refuelable);
			return false;
		}

		// Token: 0x04001639 RID: 5689
		public CompRefuelable refuelable;

		// Token: 0x0400163A RID: 5690
		private List<CompRefuelable> refuelables;
	}
}
