using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CE2 RID: 3298
	internal struct PawnStatusEffecters
	{
		// Token: 0x04003138 RID: 12600
		public Pawn pawn;

		// Token: 0x04003139 RID: 12601
		private List<PawnStatusEffecters.LiveEffecter> pairs;

		// Token: 0x060048B5 RID: 18613 RVA: 0x00262BA0 File Offset: 0x00260FA0
		public PawnStatusEffecters(Pawn pawn)
		{
			this.pawn = pawn;
			this.pairs = new List<PawnStatusEffecters.LiveEffecter>();
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x00262BB8 File Offset: 0x00260FB8
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
			for (int j = this.pairs.Count - 1; j >= 0; j--)
			{
				if (this.pairs[j].Expired)
				{
					this.pairs[j].Cleanup();
					this.pairs.RemoveAt(j);
				}
				else
				{
					this.pairs[j].Tick(this.pawn);
				}
			}
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x00262CE4 File Offset: 0x002610E4
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
			PawnStatusEffecters.LiveEffecter liveEffecter = FullPool<PawnStatusEffecters.LiveEffecter>.Get();
			liveEffecter.def = def;
			liveEffecter.Maintain();
			this.pairs.Add(liveEffecter);
		}

		// Token: 0x02000CE3 RID: 3299
		private class LiveEffecter : IFullPoolable
		{
			// Token: 0x0400313A RID: 12602
			public EffecterDef def;

			// Token: 0x0400313B RID: 12603
			public Effecter effecter;

			// Token: 0x0400313C RID: 12604
			public int lastMaintainTick;

			// Token: 0x17000B7C RID: 2940
			// (get) Token: 0x060048B9 RID: 18617 RVA: 0x00262D68 File Offset: 0x00261168
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame > this.lastMaintainTick;
				}
			}

			// Token: 0x060048BA RID: 18618 RVA: 0x00262D8F File Offset: 0x0026118F
			public void Cleanup()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
				}
				FullPool<PawnStatusEffecters.LiveEffecter>.Return(this);
			}

			// Token: 0x060048BB RID: 18619 RVA: 0x00262DAE File Offset: 0x002611AE
			public void Reset()
			{
				this.def = null;
				this.effecter = null;
				this.lastMaintainTick = -1;
			}

			// Token: 0x060048BC RID: 18620 RVA: 0x00262DC6 File Offset: 0x002611C6
			public void Maintain()
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}

			// Token: 0x060048BD RID: 18621 RVA: 0x00262DD9 File Offset: 0x002611D9
			public void Tick(Pawn pawn)
			{
				if (this.effecter == null)
				{
					this.effecter = this.def.Spawn();
				}
				this.effecter.EffectTick(pawn, null);
			}
		}
	}
}
