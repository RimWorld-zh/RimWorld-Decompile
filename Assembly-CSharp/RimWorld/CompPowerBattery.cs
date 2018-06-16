using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000415 RID: 1045
	public class CompPowerBattery : CompPower
	{
		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060011FB RID: 4603 RVA: 0x0009CB24 File Offset: 0x0009AF24
		public float AmountCanAccept
		{
			get
			{
				float result;
				if (this.parent.IsBrokenDown())
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

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x0009CB70 File Offset: 0x0009AF70
		public float StoredEnergy
		{
			get
			{
				return this.storedEnergy;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x0009CB8C File Offset: 0x0009AF8C
		public float StoredEnergyPct
		{
			get
			{
				return this.storedEnergy / this.Props.storedEnergyMax;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x0009CBB4 File Offset: 0x0009AFB4
		public new CompProperties_Battery Props
		{
			get
			{
				return (CompProperties_Battery)this.props;
			}
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0009CBD4 File Offset: 0x0009AFD4
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

		// Token: 0x06001200 RID: 4608 RVA: 0x0009CC22 File Offset: 0x0009B022
		public override void CompTick()
		{
			base.CompTick();
			this.DrawPower(Mathf.Min(5f * CompPower.WattsToWattDaysPerTick, this.storedEnergy));
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0009CC48 File Offset: 0x0009B048
		public void AddEnergy(float amount)
		{
			if (amount < 0f)
			{
				Log.Error("Cannot add negative energy " + amount, false);
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

		// Token: 0x06001202 RID: 4610 RVA: 0x0009CCB0 File Offset: 0x0009B0B0
		public void DrawPower(float amount)
		{
			this.storedEnergy -= amount;
			if (this.storedEnergy < 0f)
			{
				Log.Error("Drawing power we don't have from " + this.parent, false);
				this.storedEnergy = 0f;
			}
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0009CCFF File Offset: 0x0009B0FF
		public void SetStoredEnergyPct(float pct)
		{
			pct = Mathf.Clamp01(pct);
			this.storedEnergy = this.Props.storedEnergyMax * pct;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0009CD1D File Offset: 0x0009B11D
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "Breakdown")
			{
				this.DrawPower(this.StoredEnergy);
			}
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x0009CD3C File Offset: 0x0009B13C
		public override string CompInspectStringExtra()
		{
			CompProperties_Battery props = this.Props;
			string text = string.Concat(new string[]
			{
				"PowerBatteryStored".Translate(),
				": ",
				this.storedEnergy.ToString("F0"),
				" / ",
				props.storedEnergyMax.ToString("F0"),
				" Wd"
			});
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"\n",
				"PowerBatteryEfficiency".Translate(),
				": ",
				(props.efficiency * 100f).ToString("F0"),
				"%"
			});
			if (this.storedEnergy > 0f)
			{
				text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\n",
					"SelfDischarging".Translate(),
					": ",
					5f.ToString("F0"),
					" W"
				});
			}
			return text + "\n" + base.CompInspectStringExtra();
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0009CE74 File Offset: 0x0009B274
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo c in this.<CompGetGizmosExtra>__BaseCallProxy0())
			{
				yield return c;
			}
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Fill",
					action = delegate()
					{
						this.SetStoredEnergyPct(1f);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Empty",
					action = delegate()
					{
						this.SetStoredEnergyPct(0f);
					}
				};
			}
			yield break;
		}

		// Token: 0x04000AF5 RID: 2805
		private float storedEnergy = 0f;

		// Token: 0x04000AF6 RID: 2806
		private const float SelfDischargingWatts = 5f;
	}
}
