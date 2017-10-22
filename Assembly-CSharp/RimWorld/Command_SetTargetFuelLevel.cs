using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Command_SetTargetFuelLevel : Command
	{
		public CompRefuelable refuelable;

		private List<CompRefuelable> refuelables;

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
			int num = 2147483647;
			for (int i = 0; i < this.refuelables.Count; i++)
			{
				if ((int)this.refuelables[i].Props.fuelCapacity < num)
				{
					num = (int)this.refuelables[i].Props.fuelCapacity;
				}
			}
			int startingValue = num / 2;
			int num2 = 0;
			while (num2 < this.refuelables.Count)
			{
				if ((int)this.refuelables[num2].TargetFuelLevel > num)
				{
					num2++;
					continue;
				}
				startingValue = (int)this.refuelables[num2].TargetFuelLevel;
				break;
			}
			Func<int, string> textGetter = (!this.refuelable.parent.def.building.hasFuelingPort) ? ((Func<int, string>)((int x) => "SetTargetFuelLevel".Translate(x))) : ((Func<int, string>)((int x) => "SetPodLauncherTargetFuelLevel".Translate(x, CompLaunchable.MaxLaunchDistanceAtFuelLevel((float)x))));
			Dialog_Slider window = new Dialog_Slider(textGetter, 0, num, (Action<int>)delegate(int value)
			{
				for (int j = 0; j < this.refuelables.Count; j++)
				{
					this.refuelables[j].TargetFuelLevel = (float)value;
				}
			}, startingValue);
			Find.WindowStack.Add(window);
		}

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
