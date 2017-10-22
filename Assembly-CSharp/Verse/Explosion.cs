using RimWorld;
using System;
using System.Collections.Generic;
using Verse.Sound;

namespace Verse
{
	public class Explosion : IExposable
	{
		public ExplosionManager explosionManager;

		public IntVec3 position;

		public float radius;

		public DamageDef damType;

		public int damAmount;

		public Thing instigator;

		public ThingDef weaponGear;

		public bool applyDamageToExplosionCellsNeighbors;

		public ThingDef preExplosionSpawnThingDef;

		public float preExplosionSpawnChance;

		public int preExplosionSpawnThingCount = 1;

		public ThingDef postExplosionSpawnThingDef;

		public float postExplosionSpawnChance;

		public int postExplosionSpawnThingCount = 1;

		public bool finished;

		private int startTick;

		private List<IntVec3> cellsToAffect;

		private List<Thing> damagedThings;

		private HashSet<IntVec3> addedCellsAffectedOnlyByDamage;

		private static HashSet<IntVec3> tmpCells = new HashSet<IntVec3>();

		public Map Map
		{
			get
			{
				return this.explosionManager.map;
			}
		}

		public void StartExplosion(SoundDef explosionSound)
		{
			this.startTick = Find.TickManager.TicksGame;
			this.cellsToAffect = SimplePool<List<IntVec3>>.Get();
			this.cellsToAffect.Clear();
			this.damagedThings = SimplePool<List<Thing>>.Get();
			this.damagedThings.Clear();
			this.addedCellsAffectedOnlyByDamage = SimplePool<HashSet<IntVec3>>.Get();
			this.addedCellsAffectedOnlyByDamage.Clear();
			this.cellsToAffect.AddRange(this.damType.Worker.ExplosionCellsToHit(this));
			if (this.applyDamageToExplosionCellsNeighbors)
			{
				this.AddCellsNeighbors(this.cellsToAffect);
			}
			this.damType.Worker.ExplosionStart(this, this.cellsToAffect);
			this.PlayExplosionSound(explosionSound);
			MoteMaker.MakeWaterSplash(this.position.ToVector3Shifted(), this.Map, (float)(this.radius * 6.0), 20f);
			this.cellsToAffect.Sort((Comparison<IntVec3>)((IntVec3 a, IntVec3 b) => this.GetCellAffectTick(b).CompareTo(this.GetCellAffectTick(a))));
		}

		public void Tick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			int count = this.cellsToAffect.Count;
			int num = count - 1;
			while (num >= 0 && ticksGame >= this.GetCellAffectTick(this.cellsToAffect[num]))
			{
				try
				{
					this.AffectCell(this.cellsToAffect[num]);
				}
				catch (Exception ex)
				{
					Log.Error("Explosion could not affect cell " + this.cellsToAffect[num] + ": " + ex);
				}
				this.cellsToAffect.RemoveAt(num);
				num--;
			}
			if (!this.cellsToAffect.Any())
			{
				this.Finished();
			}
		}

		public void Finished()
		{
			if (!this.finished)
			{
				this.cellsToAffect.Clear();
				SimplePool<List<IntVec3>>.Return(this.cellsToAffect);
				this.cellsToAffect = null;
				this.damagedThings.Clear();
				SimplePool<List<Thing>>.Return(this.damagedThings);
				this.damagedThings = null;
				this.addedCellsAffectedOnlyByDamage.Clear();
				SimplePool<HashSet<IntVec3>>.Return(this.addedCellsAffectedOnlyByDamage);
				this.addedCellsAffectedOnlyByDamage = null;
				this.finished = true;
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damType, "damType");
			Scribe_Values.Look<int>(ref this.damAmount, "damAmount", 0, false);
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponGear, "weaponGear");
			Scribe_Values.Look<bool>(ref this.applyDamageToExplosionCellsNeighbors, "applyDamageToExplosionCellsNeighbors", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.preExplosionSpawnThingDef, "preExplosionSpawnThingDef");
			Scribe_Values.Look<float>(ref this.preExplosionSpawnChance, "preExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.preExplosionSpawnThingCount, "preExplosionSpawnThingCount", 1, false);
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnThingDef, "postExplosionSpawnThingDef");
			Scribe_Values.Look<float>(ref this.postExplosionSpawnChance, "postExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.postExplosionSpawnThingCount, "postExplosionSpawnThingCount", 1, false);
			Scribe_Values.Look<bool>(ref this.finished, "finished", false, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Collections.Look<IntVec3>(ref this.cellsToAffect, "cellsToAffect", LookMode.Value, new object[0]);
			Scribe_Collections.Look<Thing>(ref this.damagedThings, "damagedThings", LookMode.Reference, new object[0]);
			Scribe_Collections.Look<IntVec3>(ref this.addedCellsAffectedOnlyByDamage, "addedCellsAffectedOnlyByDamage", LookMode.Value);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.damagedThings.RemoveAll((Predicate<Thing>)((Thing x) => x == null));
			}
		}

		private int GetCellAffectTick(IntVec3 cell)
		{
			return this.startTick + (int)((cell - this.position).LengthHorizontal * 1.5);
		}

		private void AffectCell(IntVec3 c)
		{
			bool flag = this.ShouldCellBeAffectedOnlyByDamage(c);
			if (!flag && c.Walkable(this.Map) && Rand.Value < this.preExplosionSpawnChance)
			{
				this.TrySpawnExplosionThing(this.preExplosionSpawnThingDef, c, this.preExplosionSpawnThingCount);
			}
			this.damType.Worker.ExplosionAffectCell(this, c, this.damagedThings, !flag);
			if (!flag && c.Walkable(this.Map) && Rand.Value < this.postExplosionSpawnChance)
			{
				this.TrySpawnExplosionThing(this.postExplosionSpawnThingDef, c, this.postExplosionSpawnThingCount);
			}
		}

		private void TrySpawnExplosionThing(ThingDef thingDef, IntVec3 c, int count)
		{
			if (thingDef != null)
			{
				if (thingDef.IsFilth)
				{
					FilthMaker.MakeFilth(c, this.Map, thingDef, count);
				}
				else
				{
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = count;
					GenSpawn.Spawn(thing, c, this.Map);
				}
			}
		}

		private void PlayExplosionSound(SoundDef explosionSound)
		{
			if ((!Prefs.DevMode) ? (!explosionSound.NullOrUndefined()) : (explosionSound != null))
			{
				explosionSound.PlayOneShot(new TargetInfo(this.position, this.Map, false));
			}
			else
			{
				this.damType.soundExplosion.PlayOneShot(new TargetInfo(this.position, this.Map, false));
			}
		}

		private void AddCellsNeighbors(List<IntVec3> cells)
		{
			Explosion.tmpCells.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				Explosion.tmpCells.Add(cells[i]);
			}
			for (int j = 0; j < cells.Count; j++)
			{
				if (cells[j].Walkable(this.Map))
				{
					for (int k = 0; k < GenAdj.AdjacentCells.Length; k++)
					{
						IntVec3 intVec = cells[j] + GenAdj.AdjacentCells[k];
						if (intVec.InBounds(this.Map) && Explosion.tmpCells.Add(intVec))
						{
							this.addedCellsAffectedOnlyByDamage.Add(intVec);
						}
					}
				}
			}
			cells.Clear();
			HashSet<IntVec3>.Enumerator enumerator = Explosion.tmpCells.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IntVec3 current = enumerator.Current;
					cells.Add(current);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			Explosion.tmpCells.Clear();
		}

		private bool ShouldCellBeAffectedOnlyByDamage(IntVec3 c)
		{
			if (!this.applyDamageToExplosionCellsNeighbors)
			{
				return false;
			}
			return this.addedCellsAffectedOnlyByDamage.Contains(c);
		}
	}
}
