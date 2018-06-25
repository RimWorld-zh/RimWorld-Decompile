using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CE5 RID: 3301
	internal struct PawnStatusEffecters
	{
		// Token: 0x0400313F RID: 12607
		public Pawn pawn;

		// Token: 0x04003140 RID: 12608
		private List<PawnStatusEffecters.LiveEffecter> pairs;

		// Token: 0x060048B8 RID: 18616 RVA: 0x00262F5C File Offset: 0x0026135C
		public PawnStatusEffecters(Pawn pawn)
		{
			this.pawn = pawn;
			this.pairs = new List<PawnStatusEffecters.LiveEffecter>();
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x00262F74 File Offset: 0x00261374
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

		// Token: 0x060048BA RID: 18618 RVA: 0x002630A0 File Offset: 0x002614A0
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

		// Token: 0x02000CE6 RID: 3302
		private class LiveEffecter : IFullPoolable
		{
			// Token: 0x04003141 RID: 12609
			public EffecterDef def;

			// Token: 0x04003142 RID: 12610
			public Effecter effecter;

			// Token: 0x04003143 RID: 12611
			public int lastMaintainTick;

			// Token: 0x17000B7B RID: 2939
			// (get) Token: 0x060048BC RID: 18620 RVA: 0x00263124 File Offset: 0x00261524
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame > this.lastMaintainTick;
				}
			}

			// Token: 0x060048BD RID: 18621 RVA: 0x0026314B File Offset: 0x0026154B
			public void Cleanup()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
				}
				FullPool<PawnStatusEffecters.LiveEffecter>.Return(this);
			}

			// Token: 0x060048BE RID: 18622 RVA: 0x0026316A File Offset: 0x0026156A
			public void Reset()
			{
				this.def = null;
				this.effecter = null;
				this.lastMaintainTick = -1;
			}

			// Token: 0x060048BF RID: 18623 RVA: 0x00263182 File Offset: 0x00261582
			public void Maintain()
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}

			// Token: 0x060048C0 RID: 18624 RVA: 0x00263195 File Offset: 0x00261595
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
