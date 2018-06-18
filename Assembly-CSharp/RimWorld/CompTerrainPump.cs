using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000742 RID: 1858
	public abstract class CompTerrainPump : ThingComp
	{
		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x0600290A RID: 10506 RVA: 0x0015DB34 File Offset: 0x0015BF34
		private CompProperties_TerrainPump Props
		{
			get
			{
				return (CompProperties_TerrainPump)this.props;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x0600290B RID: 10507 RVA: 0x0015DB54 File Offset: 0x0015BF54
		private float ProgressDays
		{
			get
			{
				return (float)this.progressTicks / 60000f;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600290C RID: 10508 RVA: 0x0015DB78 File Offset: 0x0015BF78
		private float CurrentRadius
		{
			get
			{
				return Mathf.Min(this.Props.radius, this.ProgressDays / this.Props.daysToRadius * this.Props.radius);
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x0600290D RID: 10509 RVA: 0x0015DBBC File Offset: 0x0015BFBC
		private bool Working
		{
			get
			{
				return this.powerComp == null || this.powerComp.PowerOn;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600290E RID: 10510 RVA: 0x0015DBEC File Offset: 0x0015BFEC
		private int TicksUntilRadiusInteger
		{
			get
			{
				float num = Mathf.Ceil(this.CurrentRadius) - this.CurrentRadius;
				if (num < 1E-05f)
				{
					num = 1f;
				}
				float num2 = this.Props.radius / this.Props.daysToRadius;
				float num3 = num / num2;
				return (int)(num3 * 60000f);
			}
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x0015DC49 File Offset: 0x0015C049
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x0015DC5D File Offset: 0x0015C05D
		public override void PostDeSpawn(Map map)
		{
			this.progressTicks = 0;
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x0015DC68 File Offset: 0x0015C068
		public override void CompTickRare()
		{
			if (this.Working)
			{
				this.progressTicks += 250;
				int num = GenRadial.NumCellsInRadius(this.CurrentRadius);
				for (int i = 0; i < num; i++)
				{
					this.AffectCell(this.parent.Position + GenRadial.RadialPattern[i]);
				}
			}
		}

		// Token: 0x06002912 RID: 10514
		protected abstract void AffectCell(IntVec3 c);

		// Token: 0x06002913 RID: 10515 RVA: 0x0015DCDB File Offset: 0x0015C0DB
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.progressTicks, "progressTicks", 0, false);
		}

		// Token: 0x06002914 RID: 10516 RVA: 0x0015DCF0 File Offset: 0x0015C0F0
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.CurrentRadius < this.Props.radius - 0.0001f)
			{
				GenDraw.DrawRadiusRing(this.parent.Position, this.CurrentRadius);
			}
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x0015DD28 File Offset: 0x0015C128
		public override string CompInspectStringExtra()
		{
			string text = string.Concat(new string[]
			{
				"TimePassed".Translate().CapitalizeFirst(),
				": ",
				this.progressTicks.ToStringTicksToPeriod(),
				"\n",
				"CurrentRadius".Translate().CapitalizeFirst(),
				": ",
				this.CurrentRadius.ToString("F1")
			});
			if (this.ProgressDays < this.Props.daysToRadius && this.Working)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\n",
					"RadiusExpandsIn".Translate().CapitalizeFirst(),
					": ",
					this.TicksUntilRadiusInteger.ToStringTicksToPeriod()
				});
			}
			return text;
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x0015DE10 File Offset: 0x0015C210
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Progress 1 day",
					action = delegate()
					{
						this.progressTicks += 60000;
					}
				};
			}
			yield break;
		}

		// Token: 0x0400166F RID: 5743
		private CompPowerTrader powerComp;

		// Token: 0x04001670 RID: 5744
		private int progressTicks;
	}
}
