using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public abstract class CompPower : ThingComp
	{
		public PowerNet transNet = null;

		public CompPower connectParent = null;

		public List<CompPower> connectChildren = null;

		private static List<PowerNet> recentlyConnectedNets = new List<PowerNet>();

		private static CompPower lastManualReconnector = null;

		public static readonly float WattsToWattDaysPerTick = 1.66666669E-05f;

		public bool TransmitsPowerNow
		{
			get
			{
				return ((Building)base.parent).TransmitsPowerNow;
			}
		}

		public PowerNet PowerNet
		{
			get
			{
				return (this.transNet == null) ? ((this.connectParent == null) ? null : this.connectParent.transNet) : this.transNet;
			}
		}

		public CompProperties_Power Props
		{
			get
			{
				return (CompProperties_Power)base.props;
			}
		}

		public virtual void ResetPowerVars()
		{
			this.transNet = null;
			this.connectParent = null;
			this.connectChildren = null;
			CompPower.recentlyConnectedNets.Clear();
			CompPower.lastManualReconnector = null;
		}

		public virtual void SetUpPowerVars()
		{
		}

		public override void PostExposeData()
		{
			Thing thing = null;
			if (Scribe.mode == LoadSaveMode.Saving && this.connectParent != null)
			{
				thing = this.connectParent.parent;
			}
			Scribe_References.Look(ref thing, "parentThing", false);
			if (thing != null)
			{
				this.connectParent = ((ThingWithComps)thing).GetComp<CompPower>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.connectParent != null)
			{
				this.ConnectToTransmitter(this.connectParent, true);
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!this.Props.transmitsPower && !base.parent.def.ConnectToPower)
				return;
			base.parent.Map.mapDrawer.MapMeshDirty(base.parent.Position, MapMeshFlag.PowerGrid, true, false);
			if (this.Props.transmitsPower)
			{
				base.parent.Map.powerNetManager.Notify_TransmitterSpawned(this);
			}
			if (base.parent.def.ConnectToPower)
			{
				base.parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
			}
			this.SetUpPowerVars();
		}

		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (!this.Props.transmitsPower && !base.parent.def.ConnectToPower)
				return;
			if (this.Props.transmitsPower)
			{
				if (this.connectChildren != null)
				{
					for (int i = 0; i < this.connectChildren.Count; i++)
					{
						this.connectChildren[i].LostConnectParent();
					}
				}
				map.powerNetManager.Notify_TransmitterDespawned(this);
			}
			if (base.parent.def.ConnectToPower)
			{
				map.powerNetManager.Notify_ConnectorDespawned(this);
			}
			map.mapDrawer.MapMeshDirty(base.parent.Position, MapMeshFlag.PowerGrid, true, false);
		}

		public virtual void LostConnectParent()
		{
			this.connectParent = null;
			if (base.parent.Spawned)
			{
				base.parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
			}
		}

		public override void PostPrintOnto(SectionLayer layer)
		{
			base.PostPrintOnto(layer);
			if (this.connectParent != null)
			{
				PowerNetGraphics.PrintWirePieceConnecting(layer, base.parent, this.connectParent.parent, false);
			}
		}

		public override void CompPrintForPowerGrid(SectionLayer layer)
		{
			if (this.TransmitsPowerNow)
			{
				PowerOverlayMats.LinkedOverlayGraphic.Print(layer, base.parent);
			}
			if (base.parent.def.ConnectToPower)
			{
				PowerNetGraphics.PrintOverlayConnectorBaseFor(layer, base.parent);
			}
			if (this.connectParent != null)
			{
				PowerNetGraphics.PrintWirePieceConnecting(layer, base.parent, this.connectParent.parent, true);
			}
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
			if (this.connectParent == null)
				yield break;
			if (base.parent.Faction != Faction.OfPlayer)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				action = (Action)delegate
				{
					SoundDefOf.TickTiny.PlayOneShotOnCamera(null);
					((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_00f4: stateMachine*/)._0024this.TryManualReconnect();
				},
				hotKey = KeyBindingDefOf.Misc1,
				defaultDesc = "CommandTryReconnectDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TryReconnect", true),
				defaultLabel = "CommandTryReconnectLabel".Translate()
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_017f:
			/*Error near IL_0180: Unexpected return in MoveNext()*/;
		}

		private void TryManualReconnect()
		{
			if (CompPower.lastManualReconnector != this)
			{
				CompPower.recentlyConnectedNets.Clear();
				CompPower.lastManualReconnector = this;
			}
			if (this.PowerNet != null)
			{
				CompPower.recentlyConnectedNets.Add(this.PowerNet);
			}
			CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(base.parent.Position, base.parent.Map, CompPower.recentlyConnectedNets);
			if (compPower == null)
			{
				CompPower.recentlyConnectedNets.Clear();
				compPower = PowerConnectionMaker.BestTransmitterForConnector(base.parent.Position, base.parent.Map, null);
			}
			if (compPower != null)
			{
				PowerConnectionMaker.DisconnectFromPowerNet(this);
				this.ConnectToTransmitter(compPower, false);
				for (int i = 0; i < 5; i++)
				{
					MoteMaker.ThrowMetaPuff(compPower.parent.Position.ToVector3Shifted(), compPower.parent.Map);
				}
				base.parent.Map.mapDrawer.MapMeshDirty(base.parent.Position, MapMeshFlag.PowerGrid);
				base.parent.Map.mapDrawer.MapMeshDirty(base.parent.Position, MapMeshFlag.Things);
			}
		}

		public void ConnectToTransmitter(CompPower transmitter, bool reconnectingAfterLoading = false)
		{
			if (this.connectParent != null && (!reconnectingAfterLoading || this.connectParent != transmitter))
			{
				Log.Error("Tried to connect " + this + " to transmitter " + transmitter + " but it's already connected to " + this.connectParent + ".");
			}
			else
			{
				this.connectParent = transmitter;
				if (this.connectParent.connectChildren == null)
				{
					this.connectParent.connectChildren = new List<CompPower>();
				}
				transmitter.connectChildren.Add(this);
				PowerNet powerNet = this.PowerNet;
				if (powerNet != null)
				{
					powerNet.RegisterConnector(this);
				}
			}
		}

		public override string CompInspectStringExtra()
		{
			string result;
			if (this.PowerNet == null)
			{
				result = "PowerNotConnected".Translate();
			}
			else
			{
				string text = (this.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick).ToString("F0");
				string text2 = this.PowerNet.CurrentStoredEnergy().ToString("F0");
				string text3 = result = "PowerConnectedRateStored".Translate(text, text2);
			}
			return result;
		}
	}
}
