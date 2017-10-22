using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class PowerNetManager
	{
		private enum DelayedActionType
		{
			RegisterTransmitter = 0,
			DeregisterTransmitter = 1,
			RegisterConnector = 2,
			DeregisterConnector = 3
		}

		private struct DelayedAction
		{
			public DelayedActionType type;

			public CompPower compPower;

			public DelayedAction(DelayedActionType type, CompPower compPower)
			{
				this.type = type;
				this.compPower = compPower;
			}
		}

		public Map map;

		private List<PowerNet> allNets = new List<PowerNet>();

		private List<DelayedAction> delayedActions = new List<DelayedAction>();

		public List<PowerNet> AllNetsListForReading
		{
			get
			{
				return this.allNets;
			}
		}

		public PowerNetManager(Map map)
		{
			this.map = map;
		}

		public void Notify_TransmitterSpawned(CompPower newTransmitter)
		{
			this.delayedActions.Add(new DelayedAction(DelayedActionType.RegisterTransmitter, newTransmitter));
			this.NotifyDrawersForWireUpdate(newTransmitter.parent.Position);
		}

		public void Notify_TransmitterDespawned(CompPower oldTransmitter)
		{
			this.delayedActions.Add(new DelayedAction(DelayedActionType.DeregisterTransmitter, oldTransmitter));
			this.NotifyDrawersForWireUpdate(oldTransmitter.parent.Position);
		}

		public void Notfiy_TransmitterTransmitsPowerNowChanged(CompPower transmitter)
		{
			if (transmitter.parent.Spawned)
			{
				this.delayedActions.Add(new DelayedAction(DelayedActionType.DeregisterTransmitter, transmitter));
				this.delayedActions.Add(new DelayedAction(DelayedActionType.RegisterTransmitter, transmitter));
				this.NotifyDrawersForWireUpdate(transmitter.parent.Position);
			}
		}

		public void Notify_ConnectorWantsConnect(CompPower wantingCon)
		{
			if (Scribe.mode == LoadSaveMode.Inactive && !this.HasRegisterConnectorDuplicate(wantingCon))
			{
				this.delayedActions.Add(new DelayedAction(DelayedActionType.RegisterConnector, wantingCon));
			}
			this.NotifyDrawersForWireUpdate(wantingCon.parent.Position);
		}

		public void Notify_ConnectorDespawned(CompPower oldCon)
		{
			this.delayedActions.Add(new DelayedAction(DelayedActionType.DeregisterConnector, oldCon));
			this.NotifyDrawersForWireUpdate(oldCon.parent.Position);
		}

		public void NotifyDrawersForWireUpdate(IntVec3 root)
		{
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlag.Things, true, false);
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlag.PowerGrid, true, false);
		}

		public void RegisterPowerNet(PowerNet newNet)
		{
			this.allNets.Add(newNet);
			newNet.powerNetManager = this;
			this.map.powerNetGrid.Notify_PowerNetCreated(newNet);
			PowerNetMaker.UpdateVisualLinkagesFor(newNet);
		}

		public void DeletePowerNet(PowerNet oldNet)
		{
			this.allNets.Remove(oldNet);
			this.map.powerNetGrid.Notify_PowerNetDeleted(oldNet);
		}

		public void PowerNetsTick()
		{
			for (int i = 0; i < this.allNets.Count; i++)
			{
				this.allNets[i].PowerNetTick();
			}
		}

		public void UpdatePowerNetsAndConnections_First()
		{
			int count = this.delayedActions.Count;
			for (int num = 0; num < count; num++)
			{
				DelayedAction delayedAction = this.delayedActions[num];
				DelayedAction delayedAction2 = this.delayedActions[num];
				switch (delayedAction2.type)
				{
				case DelayedActionType.RegisterTransmitter:
				{
					ThingWithComps parent = delayedAction.compPower.parent;
					if (this.map.powerNetGrid.TransmittedPowerNetAt(parent.Position) != null)
					{
						Log.Warning("Tried to register trasmitter " + parent + " at " + parent.Position + ", but there is already a power net here. There can't be two transmitters on the same cell.");
					}
					delayedAction.compPower.SetUpPowerVars();
					foreach (IntVec3 item in GenAdj.CellsAdjacentCardinal(parent))
					{
						this.TryDestroyNetAt(item);
					}
					break;
				}
				case DelayedActionType.DeregisterTransmitter:
				{
					this.TryDestroyNetAt(delayedAction.compPower.parent.Position);
					PowerConnectionMaker.DisconnectAllFromTransmitterAndSetWantConnect(delayedAction.compPower, this.map);
					delayedAction.compPower.ResetPowerVars();
					break;
				}
				}
			}
			for (int num2 = 0; num2 < count; num2++)
			{
				DelayedAction delayedAction3 = this.delayedActions[num2];
				if (delayedAction3.type == DelayedActionType.RegisterTransmitter || delayedAction3.type == DelayedActionType.DeregisterTransmitter)
				{
					this.TryCreateNetAt(delayedAction3.compPower.parent.Position);
					foreach (IntVec3 item2 in GenAdj.CellsAdjacentCardinal(delayedAction3.compPower.parent))
					{
						this.TryCreateNetAt(item2);
					}
				}
			}
			for (int num3 = 0; num3 < count; num3++)
			{
				DelayedAction delayedAction4 = this.delayedActions[num3];
				DelayedAction delayedAction5 = this.delayedActions[num3];
				switch (delayedAction5.type)
				{
				case DelayedActionType.RegisterConnector:
				{
					delayedAction4.compPower.SetUpPowerVars();
					PowerConnectionMaker.TryConnectToAnyPowerNet(delayedAction4.compPower, null);
					break;
				}
				case DelayedActionType.DeregisterConnector:
				{
					PowerConnectionMaker.DisconnectFromPowerNet(delayedAction4.compPower);
					delayedAction4.compPower.ResetPowerVars();
					break;
				}
				}
			}
			this.delayedActions.RemoveRange(0, count);
			if (DebugViewSettings.drawPower)
			{
				this.DrawDebugPowerNets();
			}
		}

		private bool HasRegisterConnectorDuplicate(CompPower compPower)
		{
			int num = this.delayedActions.Count - 1;
			bool result;
			while (true)
			{
				if (num >= 0)
				{
					DelayedAction delayedAction = this.delayedActions[num];
					if (delayedAction.compPower == compPower)
					{
						DelayedAction delayedAction2 = this.delayedActions[num];
						if (delayedAction2.type == DelayedActionType.DeregisterConnector)
						{
							result = false;
							break;
						}
						DelayedAction delayedAction3 = this.delayedActions[num];
						if (delayedAction3.type == DelayedActionType.RegisterConnector)
						{
							result = true;
							break;
						}
					}
					num--;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		private void TryCreateNetAt(IntVec3 cell)
		{
			if (cell.InBounds(this.map) && this.map.powerNetGrid.TransmittedPowerNetAt(cell) == null)
			{
				Building transmitter = cell.GetTransmitter(this.map);
				if (transmitter != null && transmitter.TransmitsPowerNow)
				{
					PowerNet powerNet = PowerNetMaker.NewPowerNetStartingFrom(transmitter);
					this.RegisterPowerNet(powerNet);
					for (int i = 0; i < powerNet.transmitters.Count; i++)
					{
						PowerConnectionMaker.ConnectAllConnectorsToTransmitter(powerNet.transmitters[i]);
					}
				}
			}
		}

		private void TryDestroyNetAt(IntVec3 cell)
		{
			if (cell.InBounds(this.map))
			{
				PowerNet powerNet = this.map.powerNetGrid.TransmittedPowerNetAt(cell);
				if (powerNet != null)
				{
					this.DeletePowerNet(powerNet);
				}
			}
		}

		private void DrawDebugPowerNets()
		{
			if (Current.ProgramState == ProgramState.Playing && Find.VisibleMap == this.map)
			{
				int num = 0;
				foreach (PowerNet allNet in this.allNets)
				{
					foreach (CompPower item in allNet.transmitters.Concat(allNet.connectors))
					{
						foreach (IntVec3 item2 in GenAdj.CellsOccupiedBy(item.parent))
						{
							CellRenderer.RenderCell(item2, (float)((float)num * 0.43999999761581421));
						}
					}
					num++;
				}
			}
		}
	}
}
