using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000415 RID: 1045
	public class CompPowerBattery : CompPower
	{
		// Token: 0x04000AF6 RID: 2806
		private float storedEnergy = 0f;

		// Token: 0x04000AF7 RID: 2807
		private const float SelfDischargingWatts = 5f;

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060011FB RID: 4603 RVA: 0x0009CD08 File Offset: 0x0009B108
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
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x0009CD54 File Offset: 0x0009B154
		public float StoredEnergy
		{
			get
			{
				return this.storedEnergy;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x0009CD70 File Offset: 0x0009B170
		public float StoredEnergyPct
		{
			get
			{
				return this.storedEnergy / this.Props.storedEnergyMax;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x0009CD98 File Offset: 0x0009B198
		public new CompProperties_Battery Props
		{
			get
			{
				return (CompProperties_Battery)this.props;
			}
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0009CDB8 File Offset: 0x0009B1B8
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

		// Token: 0x06001200 RID: 4608 RVA: 0x0009CE06 File Offset: 0x0009B206
		public override void CompTick()
		{
			base.CompTick();
			this.DrawPower(Mathf.Min(5f * CompPower.WattsToWattDaysPerTick, this.storedEnergy));
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0009CE2C File Offset: 0x0009B22C
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

		// Token: 0x06001202 RID: 4610 RVA: 0x0009CE94 File Offset: 0x0009B294
		public void DrawPower(float amount)
		{
			this.storedEnergy -= amount;
			if (this.storedEnergy < 0f)
			{
				Log.Error("Drawing power we don't have from " + this.parent, false);
				this.storedEnergy = 0f;
			}
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0009CEE3 File Offset: 0x0009B2E3
		public void SetStoredEnergyPct(float pct)
		{
			pct = Mathf.Clamp01(pct);
			this.storedEnergy = this.Props.storedEnergyMax * pct;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0009CF01 File Offset: 0x0009B301
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "Breakdown")
			{
				this.DrawPower(this.StoredEnergy);
			}
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x0009CF20 File Offset: 0x0009B320
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

		// Token: 0x06001206 RID: 4614 RVA: 0x0009D058 File Offset: 0x0009B458
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
	}
}
