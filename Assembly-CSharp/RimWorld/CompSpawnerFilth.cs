using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073A RID: 1850
	public class CompSpawnerFilth : ThingComp
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060028CF RID: 10447 RVA: 0x0015BEB8 File Offset: 0x0015A2B8
		private CompProperties_SpawnerFilth Props
		{
			get
			{
				return (CompProperties_SpawnerFilth)this.props;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060028D0 RID: 10448 RVA: 0x0015BED8 File Offset: 0x0015A2D8
		private bool CanSpawnFilth
		{
			get
			{
				Hive hive = this.parent as Hive;
				bool result;
				if (hive != null && !hive.active)
				{
					result = false;
				}
				else
				{
					RotStage? requiredRotStage = this.Props.requiredRotStage;
					result = (requiredRotStage == null || !(this.parent.GetRotStage() != this.Props.requiredRotStage));
				}
				return result;
			}
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x0015BF63 File Offset: 0x0015A363
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpawnTimestamp, "nextSpawnTimestamp", -1, false);
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x0015BF80 File Offset: 0x0015A380
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				for (int i = 0; i < this.Props.spawnCountOnSpawn; i++)
				{
					this.TrySpawnFilth();
				}
			}
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x0015BFBC File Offset: 0x0015A3BC
		public override void CompTick()
		{
			base.CompTick();
			if (this.CanSpawnFilth)
			{
				if (this.Props.spawnMtbHours > 0f && Rand.MTBEventOccurs(this.Props.spawnMtbHours, 2500f, 1f))
				{
					this.TrySpawnFilth();
				}
				if (this.Props.spawnEveryDays >= 0f && Find.TickManager.TicksGame >= this.nextSpawnTimestamp)
				{
					if (this.nextSpawnTimestamp != -1)
					{
						this.TrySpawnFilth();
					}
					this.nextSpawnTimestamp = Find.TickManager.TicksGame + (int)(this.Props.spawnEveryDays * 60000f);
				}
			}
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x0015C078 File Offset: 0x0015A478
		public void TrySpawnFilth()
		{
			if (this.parent.Map != null)
			{
				IntVec3 c;
				if (CellFinder.TryFindRandomReachableCellNear(this.parent.Position, this.parent.Map, this.Props.spawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 x) => x.Standable(this.parent.Map), (Region x) => true, out c, 999999))
				{
					FilthMaker.MakeFilth(c, this.parent.Map, this.Props.filthDef, 1);
				}
			}
		}

		// Token: 0x04001654 RID: 5716
		private int nextSpawnTimestamp = -1;
	}
}
