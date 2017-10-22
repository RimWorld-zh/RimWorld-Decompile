using System;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class CompPowerTrader : CompPower
	{
		public Action powerStartedAction;

		public Action powerStoppedAction;

		private bool powerOnInt = false;

		public float powerOutputInt = 0f;

		private bool powerLastOutputted = false;

		private Sustainer sustainerPowered = null;

		protected CompFlickable flickableComp;

		public const string PowerTurnedOnSignal = "PowerTurnedOn";

		public const string PowerTurnedOffSignal = "PowerTurnedOff";

		public float PowerOutput
		{
			get
			{
				return this.powerOutputInt;
			}
			set
			{
				this.powerOutputInt = value;
				if (this.powerOutputInt > 0.0)
				{
					this.powerLastOutputted = true;
				}
				if (this.powerOutputInt < 0.0)
				{
					this.powerLastOutputted = false;
				}
			}
		}

		public float EnergyOutputPerTick
		{
			get
			{
				return this.PowerOutput * CompPower.WattsToWattDaysPerTick;
			}
		}

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
						if (!FlickUtility.WantsToBeOn(base.parent))
						{
							Log.Warning("Tried to power on " + base.parent + " which did not desire it.");
						}
						else if (base.parent.IsBrokenDown())
						{
							Log.Warning("Tried to power on " + base.parent + " which is broken down.");
						}
						else
						{
							if ((object)this.powerStartedAction != null)
							{
								this.powerStartedAction();
							}
							base.parent.BroadcastCompSignal("PowerTurnedOn");
							SoundDef soundDef = ((CompProperties_Power)base.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOn;
							if (soundDef.NullOrUndefined())
							{
								soundDef = SoundDefOf.PowerOnSmall;
							}
							soundDef.PlayOneShot(new TargetInfo(base.parent.Position, base.parent.Map, false));
							this.StartSustainerPoweredIfInactive();
						}
					}
					else
					{
						if ((object)this.powerStoppedAction != null)
						{
							this.powerStoppedAction();
						}
						base.parent.BroadcastCompSignal("PowerTurnedOff");
						SoundDef soundDef2 = ((CompProperties_Power)base.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOff;
						if (soundDef2.NullOrUndefined())
						{
							soundDef2 = SoundDefOf.PowerOffSmall;
						}
						if (base.parent.Spawned)
						{
							soundDef2.PlayOneShot(new TargetInfo(base.parent.Position, base.parent.Map, false));
						}
						this.EndSustainerPoweredIfActive();
					}
				}
			}
		}

		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(base.parent.LabelCap + " CompPower:");
				stringBuilder.AppendLine("   PowerOn: " + this.PowerOn);
				stringBuilder.AppendLine("   energyProduction: " + this.PowerOutput);
				return stringBuilder.ToString();
			}
		}

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

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.flickableComp = base.parent.GetComp<CompFlickable>();
		}

		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.EndSustainerPoweredIfActive();
			this.powerOutputInt = 0f;
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.powerOnInt, "powerOn", true, false);
		}

		public override void PostDraw()
		{
			base.PostDraw();
			if (!base.parent.IsBrokenDown())
			{
				if (this.flickableComp != null && !this.flickableComp.SwitchIsOn)
				{
					base.parent.Map.overlayDrawer.DrawOverlay(base.parent, OverlayTypes.PowerOff);
				}
				else if (FlickUtility.WantsToBeOn(base.parent) && !this.PowerOn)
				{
					base.parent.Map.overlayDrawer.DrawOverlay(base.parent, OverlayTypes.NeedsPower);
				}
			}
		}

		public override void SetUpPowerVars()
		{
			base.SetUpPowerVars();
			CompProperties_Power props = base.Props;
			this.PowerOutput = (float)(-1.0 * props.basePowerConsumption);
			this.powerLastOutputted = (props.basePowerConsumption <= 0.0);
		}

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

		public override void LostConnectParent()
		{
			base.LostConnectParent();
			this.PowerOn = false;
		}

		public override string CompInspectStringExtra()
		{
			string str = (!this.powerLastOutputted) ? ("PowerNeeded".Translate() + ": " + ((float)(0.0 - this.PowerOutput)).ToString("#####0") + " W") : ("PowerOutput".Translate() + ": " + this.PowerOutput.ToString("#####0") + " W");
			return str + "\n" + base.CompInspectStringExtra();
		}

		private void StartSustainerPoweredIfInactive()
		{
			CompProperties_Power props = base.Props;
			if (!props.soundAmbientPowered.NullOrUndefined() && this.sustainerPowered == null)
			{
				SoundInfo info = SoundInfo.InMap((Thing)base.parent, MaintenanceType.None);
				this.sustainerPowered = props.soundAmbientPowered.TrySpawnSustainer(info);
			}
		}

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
