using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class CompSpawnerFilth : ThingComp
	{
		private int nextSpawnTimestamp = -1;

		[CompilerGenerated]
		private static Predicate<Region> <>f__am$cache0;

		public CompSpawnerFilth()
		{
		}

		private CompProperties_SpawnerFilth Props
		{
			get
			{
				return (CompProperties_SpawnerFilth)this.props;
			}
		}

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

		[CompilerGenerated]
		private bool <TrySpawnFilth>m__0(IntVec3 x)
		{
			return x.Standable(this.parent.Map);
		}

		[CompilerGenerated]
		private static bool <TrySpawnFilth>m__1(Region x)
		{
			return true;
		}
	}
}
