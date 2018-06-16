using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CE6 RID: 3302
	internal struct PawnStatusEffecters
	{
		// Token: 0x060048A6 RID: 18598 RVA: 0x002617B0 File Offset: 0x0025FBB0
		public PawnStatusEffecters(Pawn pawn)
		{
			this.pawn = pawn;
			this.pairs = new List<PawnStatusEffecters.LiveEffecter>();
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x002617C8 File Offset: 0x0025FBC8
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

		// Token: 0x060048A8 RID: 18600 RVA: 0x002618F4 File Offset: 0x0025FCF4
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

		// Token: 0x0400312F RID: 12591
		public Pawn pawn;

		// Token: 0x04003130 RID: 12592
		private List<PawnStatusEffecters.LiveEffecter> pairs;

		// Token: 0x02000CE7 RID: 3303
		private class LiveEffecter : IFullPoolable
		{
			// Token: 0x17000B7B RID: 2939
			// (get) Token: 0x060048AA RID: 18602 RVA: 0x00261978 File Offset: 0x0025FD78
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame > this.lastMaintainTick;
				}
			}

			// Token: 0x060048AB RID: 18603 RVA: 0x0026199F File Offset: 0x0025FD9F
			public void Cleanup()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
				}
				FullPool<PawnStatusEffecters.LiveEffecter>.Return(this);
			}

			// Token: 0x060048AC RID: 18604 RVA: 0x002619BE File Offset: 0x0025FDBE
			public void Reset()
			{
				this.def = null;
				this.effecter = null;
				this.lastMaintainTick = -1;
			}

			// Token: 0x060048AD RID: 18605 RVA: 0x002619D6 File Offset: 0x0025FDD6
			public void Maintain()
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}

			// Token: 0x060048AE RID: 18606 RVA: 0x002619E9 File Offset: 0x0025FDE9
			public void Tick(Pawn pawn)
			{
				if (this.effecter == null)
				{
					this.effecter = this.def.Spawn();
				}
				this.effecter.EffectTick(pawn, null);
			}

			// Token: 0x04003131 RID: 12593
			public EffecterDef def;

			// Token: 0x04003132 RID: 12594
			public Effecter effecter;

			// Token: 0x04003133 RID: 12595
			public int lastMaintainTick;
		}
	}
}
