using System;
using Verse;

namespace RimWorld
{
	public class CompSpawnerFilth : ThingComp
	{
		private int nextSpawnTimestamp = -1;

		private CompProperties_SpawnerFilth Props
		{
			get
			{
				return (CompProperties_SpawnerFilth)base.props;
			}
		}

		private bool CanSpawnFilth
		{
			get
			{
				Hive hive = base.parent as Hive;
				bool result;
				if (hive != null && !hive.active)
				{
					result = false;
				}
				else
				{
					RotStage? requiredRotStage = this.Props.requiredRotStage;
					result = ((byte)((!requiredRotStage.HasValue || base.parent.GetRotStage() == this.Props.requiredRotStage) ? 1 : 0) != 0);
				}
				return result;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpawnTimestamp, "nextSpawnTimestamp", -1, false);
		}

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

		public override void CompTickRare()
		{
			if (this.CanSpawnFilth)
			{
				if (this.Props.spawnMtbHours > 0.0 && Rand.MTBEventOccurs(this.Props.spawnMtbHours, 2500f, 250f))
				{
					this.TrySpawnFilth();
				}
				if (this.Props.spawnEveryDays >= 0.0 && Find.TickManager.TicksGame >= this.nextSpawnTimestamp)
				{
					if (this.nextSpawnTimestamp != -1)
					{
						this.TrySpawnFilth();
					}
					this.nextSpawnTimestamp = Find.TickManager.TicksGame + (int)(this.Props.spawnEveryDays * 60000.0);
				}
			}
		}

		public void TrySpawnFilth()
		{
			IntVec3 c = default(IntVec3);
			if (base.parent.Map != null && CellFinder.TryFindRandomReachableCellNear(base.parent.Position, base.parent.Map, this.Props.spawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (Predicate<IntVec3>)((IntVec3 x) => x.Standable(base.parent.Map)), (Predicate<Region>)((Region x) => true), out c, 999999))
			{
				FilthMaker.MakeFilth(c, base.parent.Map, this.Props.filthDef, 1);
			}
		}
	}
}
