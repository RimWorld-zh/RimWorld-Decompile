using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000738 RID: 1848
	public class CompSpawnerFilth : ThingComp
	{
		// Token: 0x04001656 RID: 5718
		private int nextSpawnTimestamp = -1;

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x0015C4D4 File Offset: 0x0015A8D4
		private CompProperties_SpawnerFilth Props
		{
			get
			{
				return (CompProperties_SpawnerFilth)this.props;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060028CE RID: 10446 RVA: 0x0015C4F4 File Offset: 0x0015A8F4
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

		// Token: 0x060028CF RID: 10447 RVA: 0x0015C57F File Offset: 0x0015A97F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpawnTimestamp, "nextSpawnTimestamp", -1, false);
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x0015C59C File Offset: 0x0015A99C
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

		// Token: 0x060028D1 RID: 10449 RVA: 0x0015C5D8 File Offset: 0x0015A9D8
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

		// Token: 0x060028D2 RID: 10450 RVA: 0x0015C694 File Offset: 0x0015AA94
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
	}
}
