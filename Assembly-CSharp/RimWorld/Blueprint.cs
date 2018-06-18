using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000676 RID: 1654
	public abstract class Blueprint : ThingWithComps, IConstructible
	{
		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060022AE RID: 8878 RVA: 0x0012AC24 File Offset: 0x00129024
		public override string Label
		{
			get
			{
				return this.def.entityDefToBuild.label + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060022AF RID: 8879
		protected abstract float WorkTotal { get; }

		// Token: 0x060022B0 RID: 8880 RVA: 0x0012AC58 File Offset: 0x00129058
		public override void Tick()
		{
			base.Tick();
			if (!GenConstruct.CanBuildOnTerrain(this.def.entityDefToBuild, base.Position, base.Map, base.Rotation, null))
			{
				this.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x0012AC90 File Offset: 0x00129090
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

		// Token: 0x060022B2 RID: 8882 RVA: 0x0012ACB8 File Offset: 0x001290B8
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

		// Token: 0x060022B3 RID: 8883 RVA: 0x0012AE42 File Offset: 0x00129242
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			map.blueprintGrid.Register(this);
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x0012AE5C File Offset: 0x0012925C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			map.blueprintGrid.DeRegister(this);
		}

		// Token: 0x060022B5 RID: 8885
		protected abstract Thing MakeSolidThing();

		// Token: 0x060022B6 RID: 8886
		public abstract List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x060022B7 RID: 8887
		public abstract ThingDef UIStuff();

		// Token: 0x060022B8 RID: 8888 RVA: 0x0012AE84 File Offset: 0x00129284
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

		// Token: 0x060022B9 RID: 8889 RVA: 0x0012AF34 File Offset: 0x00129334
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

		// Token: 0x060022BA RID: 8890 RVA: 0x0012AFF8 File Offset: 0x001293F8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.Append("WorkLeft".Translate() + ": " + this.WorkTotal.ToStringWorkAmount());
			return stringBuilder.ToString();
		}

		// Token: 0x04001391 RID: 5009
		private static List<CompSpawnerMechanoidsOnDamaged> tmpCrashedShipParts = new List<CompSpawnerMechanoidsOnDamaged>();
	}
}
