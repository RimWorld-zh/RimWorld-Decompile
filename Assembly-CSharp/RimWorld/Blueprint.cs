using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000674 RID: 1652
	public abstract class Blueprint : ThingWithComps, IConstructible
	{
		// Token: 0x04001393 RID: 5011
		private static List<CompSpawnerMechanoidsOnDamaged> tmpCrashedShipParts = new List<CompSpawnerMechanoidsOnDamaged>();

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x0012B124 File Offset: 0x00129524
		public override string Label
		{
			get
			{
				return this.def.entityDefToBuild.label + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060022AA RID: 8874
		protected abstract float WorkTotal { get; }

		// Token: 0x060022AB RID: 8875 RVA: 0x0012B158 File Offset: 0x00129558
		public override void Tick()
		{
			base.Tick();
			if (!GenConstruct.CanBuildOnTerrain(this.def.entityDefToBuild, base.Position, base.Map, base.Rotation, null))
			{
				this.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x0012B190 File Offset: 0x00129590
		public override void Draw()
		{
			if (this.def.drawerType == DrawerType.RealtimeOnly)
			{
				base.Draw();
			}
			else
			{
				base.Comps_PostDraw();
			}
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x0012B1B8 File Offset: 0x001295B8
		public virtual bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
		{
			jobEnded = false;
			bool result;
			if (GenConstruct.FirstBlockingThing(this, workerPawn) != null)
			{
				workerPawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				jobEnded = true;
				createdThing = null;
				result = false;
			}
			else
			{
				createdThing = this.MakeSolidThing();
				Map map = base.Map;
				CellRect cellRect = this.OccupiedRect();
				GenSpawn.WipeExistingThings(base.Position, base.Rotation, createdThing.def, map, DestroyMode.Deconstruct);
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
				createdThing.SetFactionDirect(workerPawn.Faction);
				GenSpawn.Spawn(createdThing, base.Position, map, base.Rotation, WipeMode.Vanish, false);
				Blueprint.tmpCrashedShipParts.Clear();
				CellRect.CellRectIterator iterator = cellRect.ExpandedBy(3).GetIterator();
				while (!iterator.Done())
				{
					if (iterator.Current.InBounds(map))
					{
						List<Thing> thingList = iterator.Current.GetThingList(map);
						for (int i = 0; i < thingList.Count; i++)
						{
							CompSpawnerMechanoidsOnDamaged compSpawnerMechanoidsOnDamaged = thingList[i].TryGetComp<CompSpawnerMechanoidsOnDamaged>();
							if (compSpawnerMechanoidsOnDamaged != null)
							{
								Blueprint.tmpCrashedShipParts.Add(compSpawnerMechanoidsOnDamaged);
							}
						}
					}
					iterator.MoveNext();
				}
				Blueprint.tmpCrashedShipParts.RemoveDuplicates<CompSpawnerMechanoidsOnDamaged>();
				for (int j = 0; j < Blueprint.tmpCrashedShipParts.Count; j++)
				{
					Blueprint.tmpCrashedShipParts[j].Notify_BlueprintReplacedWithSolidThingNearby(workerPawn);
				}
				Blueprint.tmpCrashedShipParts.Clear();
				result = true;
			}
			return result;
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x0012B342 File Offset: 0x00129742
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			map.blueprintGrid.Register(this);
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x0012B35C File Offset: 0x0012975C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			map.blueprintGrid.DeRegister(this);
		}

		// Token: 0x060022B0 RID: 8880
		protected abstract Thing MakeSolidThing();

		// Token: 0x060022B1 RID: 8881
		public abstract List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x060022B2 RID: 8882
		public abstract ThingDef UIStuff();

		// Token: 0x060022B3 RID: 8883 RVA: 0x0012B384 File Offset: 0x00129784
		public Thing BlockingHaulableOnTop()
		{
			Thing result;
			if (this.def.entityDefToBuild.passability == Traversability.Standable)
			{
				result = null;
			}
			else
			{
				CellRect.CellRectIterator iterator = this.OccupiedRect().GetIterator();
				while (!iterator.Done())
				{
					List<Thing> thingList = iterator.Current.GetThingList(base.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Thing thing = thingList[i];
						if (thing.def.EverHaulable)
						{
							return thing;
						}
					}
					iterator.MoveNext();
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x0012B434 File Offset: 0x00129834
		public override ushort PathFindCostFor(Pawn p)
		{
			ushort result;
			if (base.Faction == null)
			{
				result = 0;
			}
			else if (this.def.entityDefToBuild is TerrainDef)
			{
				result = 0;
			}
			else
			{
				if (p.Faction == base.Faction || p.HostFaction == base.Faction)
				{
					if (base.Map.reservationManager.IsReservedByAnyoneOf(this, p.Faction) || (p.HostFaction != null && base.Map.reservationManager.IsReservedByAnyoneOf(this, p.HostFaction)))
					{
						return Frame.AvoidUnderConstructionPathFindCost;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x0012B4F8 File Offset: 0x001298F8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.Append("WorkLeft".Translate() + ": " + this.WorkTotal.ToStringWorkAmount());
			return stringBuilder.ToString();
		}
	}
}
