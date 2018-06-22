using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000418 RID: 1048
	public abstract class CompPower : ThingComp
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001220 RID: 4640 RVA: 0x0009C3C8 File Offset: 0x0009A7C8
		public bool TransmitsPowerNow
		{
			get
			{
				return ((Building)this.parent).TransmitsPowerNow;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x0009C3F0 File Offset: 0x0009A7F0
		public PowerNet PowerNet
		{
			get
			{
				PowerNet result;
				if (this.transNet != null)
				{
					result = this.transNet;
				}
				else if (this.connectParent != null)
				{
					result = this.connectParent.transNet;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001222 RID: 4642 RVA: 0x0009C43C File Offset: 0x0009A83C
		public CompProperties_Power Props
		{
			get
			{
				return (CompProperties_Power)this.props;
			}
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0009C45C File Offset: 0x0009A85C
		public virtual void ResetPowerVars()
		{
			this.transNet = null;
			this.connectParent = null;
			this.connectChildren = null;
			CompPower.recentlyConnectedNets.Clear();
			CompPower.lastManualReconnector = null;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0009C484 File Offset: 0x0009A884
		public virtual void SetUpPowerVars()
		{
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0009C488 File Offset: 0x0009A888
		public override void PostExposeData()
		{
			Thing thing = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.connectParent != null)
				{
					thing = this.connectParent.parent;
				}
			}
			Scribe_References.Look<Thing>(ref thing, "parentThing", false);
			if (thing != null)
			{
				this.connectParent = ((ThingWithComps)thing).GetComp<CompPower>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.connectParent != null)
				{
					this.ConnectToTransmitter(this.connectParent, true);
				}
			}
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0009C508 File Offset: 0x0009A908
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.Props.transmitsPower || this.parent.def.ConnectToPower)
			{
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.PowerGrid, true, false);
				if (this.Props.transmitsPower)
				{
					this.parent.Map.powerNetManager.Notify_TransmitterSpawned(this);
				}
				if (this.parent.def.ConnectToPower)
				{
					this.parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
				}
				this.SetUpPowerVars();
			}
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0009C5C4 File Offset: 0x0009A9C4
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.Props.transmitsPower || this.parent.def.ConnectToPower)
			{
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
				if (this.parent.def.ConnectToPower)
				{
					map.powerNetManager.Notify_ConnectorDespawned(this);
				}
				map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.PowerGrid, true, false);
			}
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0009C698 File Offset: 0x0009AA98
		public virtual void LostConnectParent()
		{
			this.connectParent = null;
			if (this.parent.Spawned)
			{
				this.parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
			}
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0009C6C8 File Offset: 0x0009AAC8
		public override void PostPrintOnto(SectionLayer layer)
		{
			base.PostPrintOnto(layer);
			if (this.connectParent != null)
			{
				PowerNetGraphics.PrintWirePieceConnecting(layer, this.parent, this.connectParent.parent, false);
			}
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0009C6F8 File Offset: 0x0009AAF8
		public override void CompPrintForPowerGrid(SectionLayer layer)
		{
			if (this.TransmitsPowerNow)
			{
				PowerOverlayMats.LinkedOverlayGraphic.Print(layer, this.parent);
			}
			if (this.parent.def.ConnectToPower)
			{
				PowerNetGraphics.PrintOverlayConnectorBaseFor(layer, this.parent);
			}
			if (this.connectParent != null)
			{
				PowerNetGraphics.PrintWirePieceConnecting(layer, this.parent, this.connectParent.parent, true);
			}
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0009C768 File Offset: 0x0009AB68
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo c in this.<CompGetGizmosExtra>__BaseCallProxy0())
			{
				yield return c;
			}
			if (this.connectParent != null && this.parent.Faction == Faction.OfPlayer)
			{
				yield return new Command_Action
				{
					action = delegate()
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						this.TryManualReconnect();
					},
					hotKey = KeyBindingDefOf.Misc1,
					defaultDesc = "CommandTryReconnectDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/TryReconnect", true),
					defaultLabel = "CommandTryReconnectLabel".Translate()
				};
			}
			yield break;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0009C794 File Offset: 0x0009AB94
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
			CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(this.parent.Position, this.parent.Map, CompPower.recentlyConnectedNets);
			if (compPower == null)
			{
				CompPower.recentlyConnectedNets.Clear();
				compPower = PowerConnectionMaker.BestTransmitterForConnector(this.parent.Position, this.parent.Map, null);
			}
			if (compPower != null)
			{
				PowerConnectionMaker.DisconnectFromPowerNet(this);
				this.ConnectToTransmitter(compPower, false);
				for (int i = 0; i < 5; i++)
				{
					MoteMaker.ThrowMetaPuff(compPower.parent.Position.ToVector3Shifted(), compPower.parent.Map);
				}
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.PowerGrid);
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0009C8C0 File Offset: 0x0009ACC0
		public void ConnectToTransmitter(CompPower transmitter, bool reconnectingAfterLoading = false)
		{
			if (this.connectParent != null && (!reconnectingAfterLoading || this.connectParent != transmitter))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to connect ",
					this,
					" to transmitter ",
					transmitter,
					" but it's already connected to ",
					this.connectParent,
					"."
				}), false);
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

		// Token: 0x0600122E RID: 4654 RVA: 0x0009C97C File Offset: 0x0009AD7C
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
				string text3 = "PowerConnectedRateStored".Translate(new object[]
				{
					text,
					text2
				});
				result = text3;
			}
			return result;
		}

		// Token: 0x04000B01 RID: 2817
		public PowerNet transNet = null;

		// Token: 0x04000B02 RID: 2818
		public CompPower connectParent = null;

		// Token: 0x04000B03 RID: 2819
		public List<CompPower> connectChildren = null;

		// Token: 0x04000B04 RID: 2820
		private static List<PowerNet> recentlyConnectedNets = new List<PowerNet>();

		// Token: 0x04000B05 RID: 2821
		private static CompPower lastManualReconnector = null;

		// Token: 0x04000B06 RID: 2822
		public static readonly float WattsToWattDaysPerTick = 1.66666669E-05f;
	}
}
