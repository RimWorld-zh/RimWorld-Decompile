using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000740 RID: 1856
	public abstract class CompTerrainPump : ThingComp
	{
		// Token: 0x0400166D RID: 5741
		private CompPowerTrader powerComp;

		// Token: 0x0400166E RID: 5742
		private int progressTicks;

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002907 RID: 10503 RVA: 0x0015DE5C File Offset: 0x0015C25C
		private CompProperties_TerrainPump Props
		{
			get
			{
				return (CompProperties_TerrainPump)this.props;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06002908 RID: 10504 RVA: 0x0015DE7C File Offset: 0x0015C27C
		private float ProgressDays
		{
			get
			{
				return (float)this.progressTicks / 60000f;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06002909 RID: 10505 RVA: 0x0015DEA0 File Offset: 0x0015C2A0
		private float CurrentRadius
		{
			get
			{
				return Mathf.Min(this.Props.radius, this.ProgressDays / this.Props.daysToRadius * this.Props.radius);
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600290A RID: 10506 RVA: 0x0015DEE4 File Offset: 0x0015C2E4
		private bool Working
		{
			get
			{
				return this.powerComp == null || this.powerComp.PowerOn;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x0600290B RID: 10507 RVA: 0x0015DF14 File Offset: 0x0015C314
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

		// Token: 0x0600290C RID: 10508 RVA: 0x0015DF71 File Offset: 0x0015C371
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x0015DF85 File Offset: 0x0015C385
		public override void PostDeSpawn(Map map)
		{
			this.progressTicks = 0;
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x0015DF90 File Offset: 0x0015C390
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

		// Token: 0x0600290F RID: 10511
		protected abstract void AffectCell(IntVec3 c);

		// Token: 0x06002910 RID: 10512 RVA: 0x0015E003 File Offset: 0x0015C403
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.progressTicks, "progressTicks", 0, false);
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x0015E018 File Offset: 0x0015C418
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.CurrentRadius < this.Props.radius - 0.0001f)
			{
				GenDraw.DrawRadiusRing(this.parent.Position, this.CurrentRadius);
			}
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x0015E050 File Offset: 0x0015C450
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

		// Token: 0x06002913 RID: 10515 RVA: 0x0015E138 File Offset: 0x0015C538
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
	}
}
