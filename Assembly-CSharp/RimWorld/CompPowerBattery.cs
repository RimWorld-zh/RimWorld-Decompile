using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompPowerBattery : CompPower
	{
		private float storedEnergy;

		public float AmountCanAccept
		{
			get
			{
				if (base.parent.IsBrokenDown())
				{
					return 0f;
				}
				CompProperties_Battery props = this.Props;
				return (props.storedEnergyMax - this.storedEnergy) / props.efficiency;
			}
		}

		public float StoredEnergy
		{
			get
			{
				return this.storedEnergy;
			}
		}

		public float StoredEnergyPct
		{
			get
			{
				return this.storedEnergy / this.Props.storedEnergyMax;
			}
		}

		public new CompProperties_Battery Props
		{
			get
			{
				return (CompProperties_Battery)base.props;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.storedEnergy, "storedPower", 0f, false);
			CompProperties_Battery props = this.Props;
			if (this.storedEnergy > props.storedEnergyMax)
			{
				this.storedEnergy = props.storedEnergyMax;
			}
		}

		public void AddEnergy(float amount)
		{
			if (amount < 0.0)
			{
				Log.Error("Cannot add negative energy " + amount);
			}
			else
			{
				if (amount > this.AmountCanAccept)
				{
					amount = this.AmountCanAccept;
				}
				amount *= this.Props.efficiency;
				this.storedEnergy += amount;
			}
		}

		public void DrawPower(float amount)
		{
			this.storedEnergy -= amount;
			if (this.storedEnergy < 0.0)
			{
				Log.Error("Drawing power we don't have from " + base.parent);
				this.storedEnergy = 0f;
			}
		}

		public void SetStoredEnergyPct(float pct)
		{
			pct = Mathf.Clamp01(pct);
			this.storedEnergy = this.Props.storedEnergyMax * pct;
		}

		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "Breakdown")
			{
				this.DrawPower(this.StoredEnergy);
			}
		}

		public override string CompInspectStringExtra()
		{
			CompProperties_Battery props = this.Props;
			string text;
			string text2 = text = "PowerBatteryStored".Translate() + ": " + this.storedEnergy.ToString("F0") + " / " + props.storedEnergyMax.ToString("F0") + " Wd";
			text2 = text + "\n" + "PowerBatteryEfficiency".Translate() + ": " + ((float)(props.efficiency * 100.0)).ToString("F0") + "%";
			return text2 + "\n" + base.CompInspectStringExtra();
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo item in base.CompGetGizmosExtra())
			{
				yield return item;
			}
			if (Prefs.DevMode)
			{
				yield return (Gizmo)new Command_Action
				{
					defaultLabel = "DEBUG: Fill",
					action = (Action)delegate
					{
						((_003CCompGetGizmosExtra_003Ec__IteratorB3)/*Error near IL_00d9: stateMachine*/)._003C_003Ef__this.SetStoredEnergyPct(1f);
					}
				};
				yield return (Gizmo)new Command_Action
				{
					defaultLabel = "DEBUG: Empty",
					action = (Action)delegate
					{
						((_003CCompGetGizmosExtra_003Ec__IteratorB3)/*Error near IL_0123: stateMachine*/)._003C_003Ef__this.SetStoredEnergyPct(0f);
					}
				};
			}
		}
	}
}
