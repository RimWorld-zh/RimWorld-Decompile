using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073A RID: 1850
	public class CompSpawnerFilth : ThingComp
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x0015BF4C File Offset: 0x0015A34C
		private CompProperties_SpawnerFilth Props
		{
			get
			{
				return (CompProperties_SpawnerFilth)this.props;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060028D2 RID: 10450 RVA: 0x0015BF6C File Offset: 0x0015A36C
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

		// Token: 0x060028D3 RID: 10451 RVA: 0x0015BFF7 File Offset: 0x0015A3F7
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpawnTimestamp, "nextSpawnTimestamp", -1, false);
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x0015C014 File Offset: 0x0015A414
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

		// Token: 0x060028D5 RID: 10453 RVA: 0x0015C050 File Offset: 0x0015A450
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

		// Token: 0x060028D6 RID: 10454 RVA: 0x0015C10C File Offset: 0x0015A50C
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
