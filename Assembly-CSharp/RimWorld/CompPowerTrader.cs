using System;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000421 RID: 1057
	public class CompPowerTrader : CompPower
	{
		// Token: 0x04000B2C RID: 2860
		public Action powerStartedAction;

		// Token: 0x04000B2D RID: 2861
		public Action powerStoppedAction;

		// Token: 0x04000B2E RID: 2862
		private bool powerOnInt = false;

		// Token: 0x04000B2F RID: 2863
		public float powerOutputInt = 0f;

		// Token: 0x04000B30 RID: 2864
		private bool powerLastOutputted = false;

		// Token: 0x04000B31 RID: 2865
		private Sustainer sustainerPowered = null;

		// Token: 0x04000B32 RID: 2866
		protected CompFlickable flickableComp;

		// Token: 0x04000B33 RID: 2867
		public const string PowerTurnedOnSignal = "PowerTurnedOn";

		// Token: 0x04000B34 RID: 2868
		public const string PowerTurnedOffSignal = "PowerTurnedOff";

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001258 RID: 4696 RVA: 0x0009DD68 File Offset: 0x0009C168
		// (set) Token: 0x06001259 RID: 4697 RVA: 0x0009DD83 File Offset: 0x0009C183
		public float PowerOutput
		{
			get
			{
				return this.powerOutputInt;
			}
			set
			{
				this.powerOutputInt = value;
				if (this.powerOutputInt > 0f)
				{
					this.powerLastOutputted = true;
				}
				if (this.powerOutputInt < 0f)
				{
					this.powerLastOutputted = false;
				}
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600125A RID: 4698 RVA: 0x0009DDBC File Offset: 0x0009C1BC
		public float EnergyOutputPerTick
		{
			get
			{
				return this.PowerOutput * CompPower.WattsToWattDaysPerTick;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600125B RID: 4699 RVA: 0x0009DDE0 File Offset: 0x0009C1E0
		// (set) Token: 0x0600125C RID: 4700 RVA: 0x0009DDFC File Offset: 0x0009C1FC
		public bool PowerOn
		{
			get
			{
				return this.powerOnInt;
			}
			set
			{
				if (this.powerOnInt != value)
				{
					this.powerOnInt = value;
					if (this.powerOnInt)
					{
						if (!FlickUtility.WantsToBeOn(this.parent))
						{
							Log.Warning("Tried to power on " + this.parent + " which did not desire it.", false);
						}
						else if (this.parent.IsBrokenDown())
						{
							Log.Warning("Tried to power on " + this.parent + " which is broken down.", false);
						}
						else
						{
							if (this.powerStartedAction != null)
							{
								this.powerStartedAction();
							}
							this.parent.BroadcastCompSignal("PowerTurnedOn");
							SoundDef soundDef = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOn;
							if (soundDef.NullOrUndefined())
							{
								soundDef = SoundDefOf.Power_OnSmall;
							}
							soundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
							this.StartSustainerPoweredIfInactive();
						}
					}
					else
					{
						if (this.powerStoppedAction != null)
						{
							this.powerStoppedAction();
						}
						this.parent.BroadcastCompSignal("PowerTurnedOff");
						SoundDef soundDef2 = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOff;
						if (soundDef2.NullOrUndefined())
						{
							soundDef2 = SoundDefOf.Power_OffSmall;
						}
						if (this.parent.Spawned)
						{
							soundDef2.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
						}
						this.EndSustainerPoweredIfActive();
					}
				}
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600125D RID: 4701 RVA: 0x0009DFA8 File Offset: 0x0009C3A8
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.parent.LabelCap + " CompPower:");
				stringBuilder.AppendLine("   PowerOn: " + this.PowerOn);
				stringBuilder.AppendLine("   energyProduction: " + this.PowerOutput);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0009E020 File Offset: 0x0009C420
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "ScheduledOff" || signal == "Breakdown")
			{
				this.PowerOn = false;
			}
			if (signal == "RanOutOfFuel" && this.powerLastOutputted)
			{
				this.PowerOn = false;
			}
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0009E087 File Offset: 0x0009C487
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.flickableComp = this.parent.GetComp<CompFlickable>();
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0009E0A2 File Offset: 0x0009C4A2
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.EndSustainerPoweredIfActive();
			this.powerOutputInt = 0f;
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x0009E0BD File Offset: 0x0009C4BD
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.powerOnInt, "powerOn", true, false);
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x0009E0D8 File Offset: 0x0009C4D8
		public override void PostDraw()
		{
			base.PostDraw();
			if (!this.parent.IsBrokenDown())
			{
				if (this.flickableComp != null && !this.flickableComp.SwitchIsOn)
				{
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.PowerOff);
				}
				else if (FlickUtility.WantsToBeOn(this.parent))
				{
					if (!this.PowerOn)
					{
						this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.NeedsPower);
					}
				}
			}
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x0009E17C File Offset: 0x0009C57C
		public override void SetUpPowerVars()
		{
			base.SetUpPowerVars();
			CompProperties_Power props = base.Props;
			this.PowerOutput = -1f * props.basePowerConsumption;
			this.powerLastOutputted = (props.basePowerConsumption <= 0f);
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0009E1BF File Offset: 0x0009C5BF
		public override void ResetPowerVars()
		{
			base.ResetPowerVars();
			this.powerOnInt = false;
			this.powerOutputInt = 0f;
			this.powerLastOutputted = false;
			this.sustainerPowered = null;
			if (this.flickableComp != null)
			{
				this.flickableComp.ResetToOn();
			}
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0009E1FE File Offset: 0x0009C5FE
		public override void LostConnectParent()
		{
			base.LostConnectParent();
			this.PowerOn = false;
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0009E210 File Offset: 0x0009C610
		public override string CompInspectStringExtra()
		{
			string str;
			if (this.powerLastOutputted)
			{
				str = "PowerOutput".Translate() + ": " + this.PowerOutput.ToString("#####0") + " W";
			}
			else
			{
				str = "PowerNeeded".Translate() + ": " + (-this.PowerOutput).ToString("#####0") + " W";
			}
			return str + "\n" + base.CompInspectStringExtra();
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x0009E2A4 File Offset: 0x0009C6A4
		private void StartSustainerPoweredIfInactive()
		{
			CompProperties_Power props = base.Props;
			if (!props.soundAmbientPowered.NullOrUndefined() && this.sustainerPowered == null)
			{
				SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.None);
				this.sustainerPowered = props.soundAmbientPowered.TrySpawnSustainer(info);
			}
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0009E2FA File Offset: 0x0009C6FA
		private void EndSustainerPoweredIfActive()
		{
			if (this.sustainerPowered != null)
			{
				this.sustainerPowered.End();
				this.sustainerPowered = null;
			}
		}
	}
}
