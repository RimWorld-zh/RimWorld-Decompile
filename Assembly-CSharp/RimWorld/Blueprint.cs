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
		// (get) Token: 0x060022AC RID: 8876 RVA: 0x0012ABAC File Offset: 0x00128FAC
		public override string Label
		{
			get
			{
				return this.def.entityDefToBuild.label + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060022AD RID: 8877
		protected abstract float WorkTotal { get; }

		// Token: 0x060022AE RID: 8878 RVA: 0x0012ABE0 File Offset: 0x00128FE0
		public override void Tick()
		{
			base.Tick();
			if (!GenConstruct.CanBuildOnTerrain(this.def.entityDefToBuild, base.Position, base.Map, base.Rotation, null))
			{
				this.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x0012AC18 File Offset: 0x00129018
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

		// Token: 0x060022B0 RID: 8880 RVA: 0x0012AC40 File Offset: 0x00129040
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

		// Token: 0x060022B1 RID: 8881 RVA: 0x0012ADCA File Offset: 0x001291CA
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			map.blueprintGrid.Register(this);
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x0012ADE4 File Offset: 0x001291E4
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			map.blueprintGrid.DeRegister(this);
		}

		// Token: 0x060022B3 RID: 8883
		protected abstract Thing MakeSolidThing();

		// Token: 0x060022B4 RID: 8884
		public abstract List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x060022B5 RID: 8885
		public abstract ThingDef UIStuff();

		// Token: 0x060022B6 RID: 8886 RVA: 0x0012AE0C File Offset: 0x0012920C
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

		// Token: 0x060022B7 RID: 8887 RVA: 0x0012AEBC File Offset: 0x001292BC
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

		// Token: 0x060022B8 RID: 8888 RVA: 0x0012AF80 File Offset: 0x00129380
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
