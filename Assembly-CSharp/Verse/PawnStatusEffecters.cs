using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CE5 RID: 3301
	internal struct PawnStatusEffecters
	{
		// Token: 0x060048A4 RID: 18596 RVA: 0x00261788 File Offset: 0x0025FB88
		public PawnStatusEffecters(Pawn pawn)
		{
			this.pawn = pawn;
			this.pairs = new List<PawnStatusEffecters.LiveEffecter>();
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x002617A0 File Offset: 0x0025FBA0
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

		// Token: 0x060048A6 RID: 18598 RVA: 0x002618CC File Offset: 0x0025FCCC
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

		// Token: 0x0400312D RID: 12589
		public Pawn pawn;

		// Token: 0x0400312E RID: 12590
		private List<PawnStatusEffecters.LiveEffecter> pairs;

		// Token: 0x02000CE6 RID: 3302
		private class LiveEffecter : IFullPoolable
		{
			// Token: 0x17000B7A RID: 2938
			// (get) Token: 0x060048A8 RID: 18600 RVA: 0x00261950 File Offset: 0x0025FD50
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame > this.lastMaintainTick;
				}
			}

			// Token: 0x060048A9 RID: 18601 RVA: 0x00261977 File Offset: 0x0025FD77
			public void Cleanup()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
				}
				FullPool<PawnStatusEffecters.LiveEffecter>.Return(this);
			}

			// Token: 0x060048AA RID: 18602 RVA: 0x00261996 File Offset: 0x0025FD96
			public void Reset()
			{
				this.def = null;
				this.effecter = null;
				this.lastMaintainTick = -1;
			}

			// Token: 0x060048AB RID: 18603 RVA: 0x002619AE File Offset: 0x0025FDAE
			public void Maintain()
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}

			// Token: 0x060048AC RID: 18604 RVA: 0x002619C1 File Offset: 0x0025FDC1
			public void Tick(Pawn pawn)
			{
				if (this.effecter == null)
				{
					this.effecter = this.def.Spawn();
				}
				this.effecter.EffectTick(pawn, null);
			}

			// Token: 0x0400312F RID: 12591
			public EffecterDef def;

			// Token: 0x04003130 RID: 12592
			public Effecter effecter;

			// Token: 0x04003131 RID: 12593
			public int lastMaintainTick;
		}
	}
}
