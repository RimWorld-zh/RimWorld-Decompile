using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200072D RID: 1837
	[StaticConstructorOnStartup]
	public class Command_SetTargetFuelLevel : Command
	{
		// Token: 0x04001639 RID: 5689
		public CompRefuelable refuelable;

		// Token: 0x0400163A RID: 5690
		private List<CompRefuelable> refuelables;

		// Token: 0x0600288C RID: 10380 RVA: 0x0015A934 File Offset: 0x00158D34
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

		// Token: 0x0600288D RID: 10381 RVA: 0x0015AAB8 File Offset: 0x00158EB8
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			if (this.refuelables == null)
			{
				this.refuelables = new List<CompRefuelable>();
			}
			this.refuelables.Add(((Command_SetTargetFuelLevel)other).refuelable);
			return false;
		}
	}
}
