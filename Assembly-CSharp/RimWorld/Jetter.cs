using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006CB RID: 1739
	public class Jetter : Thing
	{
		// Token: 0x060025A3 RID: 9635 RVA: 0x00142408 File Offset: 0x00140808
		public override void Tick()
		{
			if (this.JState == Jetter.JetterState.WickBurning)
			{
				base.Map.overlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
				this.WickTicksLeft--;
				if (this.WickTicksLeft == 0)
				{
					this.StartJetting();
				}
			}
			else if (this.JState == Jetter.JetterState.Jetting)
			{
				this.TicksUntilMove--;
				if (this.TicksUntilMove <= 0)
				{
					this.MoveJetter();
					this.TicksUntilMove = 3;
				}
			}
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x00142491 File Offset: 0x00140891
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && dinfo.Def.harmsHealth && this.JState == Jetter.JetterState.Resting)
			{
				this.StartWick();
			}
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x001424CB File Offset: 0x001408CB
		protected void StartWick()
		{
			this.JState = Jetter.JetterState.WickBurning;
			this.WickTicksLeft = 25;
			SoundDefOf.MetalHitImportant.PlayOneShot(this);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(this);
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x00142503 File Offset: 0x00140903
		protected void StartJetting()
		{
			this.JState = Jetter.JetterState.Jetting;
			this.TicksUntilMove = 3;
			this.wickSoundSustainer.End();
			this.wickSoundSustainer = null;
			this.wickSoundSustainer = SoundDefOf.HissJet.TrySpawnSustainer(this);
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x0014253C File Offset: 0x0014093C
		protected void MoveJetter()
		{
			IntVec3 intVec = base.Position + base.Rotation.FacingCell;
			if (!intVec.Walkable(base.Map) || base.Map.thingGrid.CellContains(intVec, ThingCategory.Pawn) || intVec.GetEdifice(base.Map) != null)
			{
				this.Destroy(DestroyMode.Vanish);
				GenExplosion.DoExplosion(base.Position, base.Map, 2.9f, DamageDefOf.Bomb, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			}
			else
			{
				base.Position = intVec;
			}
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x001425EC File Offset: 0x001409EC
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
			if (this.jetSoundSustainer != null)
			{
				this.jetSoundSustainer.End();
				this.jetSoundSustainer = null;
			}
		}

		// Token: 0x040014F7 RID: 5367
		private Jetter.JetterState JState = Jetter.JetterState.Resting;

		// Token: 0x040014F8 RID: 5368
		private int WickTicksLeft = 0;

		// Token: 0x040014F9 RID: 5369
		private int TicksUntilMove = 0;

		// Token: 0x040014FA RID: 5370
		protected Sustainer wickSoundSustainer = null;

		// Token: 0x040014FB RID: 5371
		protected Sustainer jetSoundSustainer = null;

		// Token: 0x040014FC RID: 5372
		private const int TicksBeforeBeginAccelerate = 25;

		// Token: 0x040014FD RID: 5373
		private const int TicksBetweenMoves = 3;

		// Token: 0x020006CC RID: 1740
		private enum JetterState
		{
			// Token: 0x040014FF RID: 5375
			Resting,
			// Token: 0x04001500 RID: 5376
			WickBurning,
			// Token: 0x04001501 RID: 5377
			Jetting
		}
	}
}
