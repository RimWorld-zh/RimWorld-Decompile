using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompPowerBattery : CompPower
	{
		private float storedEnergy = 0f;

		private const float SelfDischargingWatts = 5f;

		public float AmountCanAccept
		{
			get
			{
				float result;
				if (base.parent.IsBrokenDown())
				{
					result = 0f;
				}
				else
				{
					CompProperties_Battery props = this.Props;
					result = (props.storedEnergyMax - this.storedEnergy) / props.efficiency;
				}
				return result;
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

		public override void CompTick()
		{
			base.CompTick();
			this.DrawPower(Mathf.Min((float)(5.0 * CompPower.WattsToWattDaysPerTick), this.storedEnergy));
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
			if (this.storedEnergy > 0.0)
			{
				text = text2;
				text2 = text + "\n" + "SelfDischarging".Translate() + ": " + 5f.ToString("F0") + " W";
			}
			return text2 + "\n" + base.CompInspectStringExtra();
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CCompGetGizmosExtra_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo c = enumerator.Current;
					yield return c;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!Prefs.DevMode)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "DEBUG: Fill",
				action = (Action)delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_00e8: stateMachine*/)._0024this.SetStoredEnergyPct(1f);
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0175:
			/*Error near IL_0176: Unexpected return in MoveNext()*/;
		}
	}
}
