using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000428 RID: 1064
	public class PowerNetManager
	{
		// Token: 0x06001294 RID: 4756 RVA: 0x000A1416 File Offset: 0x0009F816
		public PowerNetManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x000A143C File Offset: 0x0009F83C
		public List<PowerNet> AllNetsListForReading
		{
			get
			{
				return this.allNets;
			}
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x000A1457 File Offset: 0x0009F857
		public void Notify_TransmitterSpawned(CompPower newTransmitter)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterTransmitter, newTransmitter));
			this.NotifyDrawersForWireUpdate(newTransmitter.parent.Position);
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x000A147D File Offset: 0x0009F87D
		public void Notify_TransmitterDespawned(CompPower oldTransmitter)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterTransmitter, oldTransmitter));
			this.NotifyDrawersForWireUpdate(oldTransmitter.parent.Position);
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x000A14A4 File Offset: 0x0009F8A4
		public void Notfiy_TransmitterTransmitsPowerNowChanged(CompPower transmitter)
		{
			if (transmitter.parent.Spawned)
			{
				this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterTransmitter, transmitter));
				this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterTransmitter, transmitter));
				this.NotifyDrawersForWireUpdate(transmitter.parent.Position);
			}
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x000A14FC File Offset: 0x0009F8FC
		public void Notify_ConnectorWantsConnect(CompPower wantingCon)
		{
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				if (!this.HasRegisterConnectorDuplicate(wantingCon))
				{
					this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterConnector, wantingCon));
				}
			}
			this.NotifyDrawersForWireUpdate(wantingCon.parent.Position);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x000A153A File Offset: 0x0009F93A
		public void Notify_ConnectorDespawned(CompPower oldCon)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterConnector, oldCon));
			this.NotifyDrawersForWireUpdate(oldCon.parent.Position);
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x000A1560 File Offset: 0x0009F960
		public void NotifyDrawersForWireUpdate(IntVec3 root)
		{
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlag.Things, true, false);
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlag.PowerGrid, true, false);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x000A158F File Offset: 0x0009F98F
		public void RegisterPowerNet(PowerNet newNet)
		{
			this.allNets.Add(newNet);
			newNet.powerNetManager = this;
			this.map.powerNetGrid.Notify_PowerNetCreated(newNet);
			PowerNetMaker.UpdateVisualLinkagesFor(newNet);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x000A15BC File Offset: 0x0009F9BC
		public void DeletePowerNet(PowerNet oldNet)
		{
			this.allNets.Remove(oldNet);
			this.map.powerNetGrid.Notify_PowerNetDeleted(oldNet);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x000A15E0 File Offset: 0x0009F9E0
		public void PowerNetsTick()
		{
			for (int i = 0; i < this.allNets.Count; i++)
			{
				this.allNets[i].PowerNetTick();
			}
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x000A1620 File Offset: 0x0009FA20
		public void UpdatePowerNetsAndConnections_First()
		{
			int count = this.delayedActions.Count;
			for (int i = 0; i < count; i++)
			{
				PowerNetManager.DelayedAction delayedAction = this.delayedActions[i];
				PowerNetManager.DelayedActionType type = this.delayedActions[i].type;
				if (type != PowerNetManager.DelayedActionType.RegisterTransmitter)
				{
					if (type == PowerNetManager.DelayedActionType.DeregisterTransmitter)
					{
						this.TryDestroyNetAt(delayedAction.position);
						PowerConnectionMaker.DisconnectAllFromTransmitterAndSetWantConnect(delayedAction.compPower, this.map);
						delayedAction.compPower.ResetPowerVars();
					}
				}
				else if (delayedAction.position == delayedAction.compPower.parent.Position)
				{
					ThingWithComps parent = delayedAction.compPower.parent;
					if (this.map.powerNetGrid.TransmittedPowerNetAt(parent.Position) != null)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Tried to register trasmitter ",
							parent,
							" at ",
							parent.Position,
							", but there is already a power net here. There can't be two transmitters on the same cell."
						}), false);
					}
					delayedAction.compPower.SetUpPowerVars();
					foreach (IntVec3 cell in GenAdj.CellsAdjacentCardinal(parent))
					{
						this.TryDestroyNetAt(cell);
					}
				}
			}
			for (int j = 0; j < count; j++)
			{
				PowerNetManager.DelayedAction delayedAction2 = this.delayedActions[j];
				if ((delayedAction2.type == PowerNetManager.DelayedActionType.RegisterTransmitter && delayedAction2.position == delayedAction2.compPower.parent.Position) || delayedAction2.type == PowerNetManager.DelayedActionType.DeregisterTransmitter)
				{
					this.TryCreateNetAt(delayedAction2.position);
					foreach (IntVec3 cell2 in GenAdj.CellsAdjacentCardinal(delayedAction2.position, delayedAction2.rotation, delayedAction2.compPower.parent.def.size))
					{
						this.TryCreateNetAt(cell2);
					}
				}
			}
			for (int k = 0; k < count; k++)
			{
				PowerNetManager.DelayedAction delayedAction3 = this.delayedActions[k];
				PowerNetManager.DelayedActionType type2 = this.delayedActions[k].type;
				if (type2 != PowerNetManager.DelayedActionType.RegisterConnector)
				{
					if (type2 == PowerNetManager.DelayedActionType.DeregisterConnector)
					{
						PowerConnectionMaker.DisconnectFromPowerNet(delayedAction3.compPower);
						delayedAction3.compPower.ResetPowerVars();
					}
				}
				else if (delayedAction3.position == delayedAction3.compPower.parent.Position)
				{
					delayedAction3.compPower.SetUpPowerVars();
					PowerConnectionMaker.TryConnectToAnyPowerNet(delayedAction3.compPower, null);
				}
			}
			this.delayedActions.RemoveRange(0, count);
			if (DebugViewSettings.drawPower)
			{
				this.DrawDebugPowerNets();
			}
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x000A1960 File Offset: 0x0009FD60
		private bool HasRegisterConnectorDuplicate(CompPower compPower)
		{
			for (int i = this.delayedActions.Count - 1; i >= 0; i--)
			{
				if (this.delayedActions[i].compPower == compPower)
				{
					bool result;
					if (this.delayedActions[i].type == PowerNetManager.DelayedActionType.DeregisterConnector)
					{
						result = false;
					}
					else
					{
						if (this.delayedActions[i].type != PowerNetManager.DelayedActionType.RegisterConnector)
						{
							goto IL_78;
						}
						result = true;
					}
					return result;
				}
				IL_78:;
			}
			return false;
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x000A19F8 File Offset: 0x0009FDF8
		private void TryCreateNetAt(IntVec3 cell)
		{
			if (cell.InBounds(this.map))
			{
				if (this.map.powerNetGrid.TransmittedPowerNetAt(cell) == null)
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
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x000A1A94 File Offset: 0x0009FE94
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

		// Token: 0x060012A3 RID: 4771 RVA: 0x000A1AD8 File Offset: 0x0009FED8
		private void DrawDebugPowerNets()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Find.CurrentMap == this.map)
				{
					int num = 0;
					foreach (PowerNet powerNet in this.allNets)
					{
						foreach (CompPower compPower in powerNet.transmitters.Concat(powerNet.connectors))
						{
							foreach (IntVec3 c in GenAdj.CellsOccupiedBy(compPower.parent))
							{
								CellRenderer.RenderCell(c, (float)num * 0.44f);
							}
						}
						num++;
					}
				}
			}
		}

		// Token: 0x04000B58 RID: 2904
		public Map map;

		// Token: 0x04000B59 RID: 2905
		private List<PowerNet> allNets = new List<PowerNet>();

		// Token: 0x04000B5A RID: 2906
		private List<PowerNetManager.DelayedAction> delayedActions = new List<PowerNetManager.DelayedAction>();

		// Token: 0x02000429 RID: 1065
		private enum DelayedActionType
		{
			// Token: 0x04000B5C RID: 2908
			RegisterTransmitter,
			// Token: 0x04000B5D RID: 2909
			DeregisterTransmitter,
			// Token: 0x04000B5E RID: 2910
			RegisterConnector,
			// Token: 0x04000B5F RID: 2911
			DeregisterConnector
		}

		// Token: 0x0200042A RID: 1066
		private struct DelayedAction
		{
			// Token: 0x060012A4 RID: 4772 RVA: 0x000A1C10 File Offset: 0x000A0010
			public DelayedAction(PowerNetManager.DelayedActionType type, CompPower compPower)
			{
				this.type = type;
				this.compPower = compPower;
				this.position = compPower.parent.Position;
				this.rotation = compPower.parent.Rotation;
			}

			// Token: 0x04000B60 RID: 2912
			public PowerNetManager.DelayedActionType type;

			// Token: 0x04000B61 RID: 2913
			public CompPower compPower;

			// Token: 0x04000B62 RID: 2914
			public IntVec3 position;

			// Token: 0x04000B63 RID: 2915
			public Rot4 rotation;
		}
	}
}
