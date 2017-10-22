using System.Collections.Generic;

namespace Verse
{
	internal struct PawnStatusEffecters
	{
		private class LiveEffecter : IFullPoolable
		{
			public EffecterDef def;

			public Effecter effecter;

			public int lastMaintainTick;

			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame > this.lastMaintainTick;
				}
			}

			public void Cleanup()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
				}
				FullPool<LiveEffecter>.Return(this);
			}

			public void Reset()
			{
				this.def = null;
				this.effecter = null;
				this.lastMaintainTick = -1;
			}

			public void Maintain()
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}

			public void Tick(Pawn pawn)
			{
				if (this.effecter == null)
				{
					this.effecter = this.def.Spawn();
				}
				this.effecter.EffectTick((Thing)pawn, (Thing)null);
			}
		}

		public Pawn pawn;

		private List<LiveEffecter> pairs;

		public PawnStatusEffecters(Pawn pawn)
		{
			this.pawn = pawn;
			this.pairs = new List<LiveEffecter>();
		}

		public void EffectersTick()
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffComp_Effecter hediffComp_Effecter = hediffs[i].TryGetComp<HediffComp_Effecter>();
				if (hediffComp_Effecter != null)
				{
					EffecterDef effecterDef = hediffComp_Effecter.CurrentStateEffecter();
					if (effecterDef != null)
					{
						this.AddOrMaintain(effecterDef);
					}
				}
			}
			if (this.pawn.mindState.mentalStateHandler.CurState != null)
			{
				EffecterDef effecterDef2 = this.pawn.mindState.mentalStateHandler.CurState.CurrentStateEffecter();
				if (effecterDef2 != null)
				{
					this.AddOrMaintain(effecterDef2);
				}
			}
			for (int num = this.pairs.Count - 1; num >= 0; num--)
			{
				if (this.pairs[num].Expired)
				{
					this.pairs[num].Cleanup();
					this.pairs.RemoveAt(num);
				}
				else
				{
					this.pairs[num].Tick(this.pawn);
				}
			}
		}

		private void AddOrMaintain(EffecterDef def)
		{
			for (int i = 0; i < this.pairs.Count; i++)
			{
				if (this.pairs[i].def == def)
				{
					this.pairs[i].Maintain();
					return;
				}
			}
			LiveEffecter liveEffecter = FullPool<LiveEffecter>.Get();
			liveEffecter.def = def;
			liveEffecter.Maintain();
			this.pairs.Add(liveEffecter);
		}
	}
}
