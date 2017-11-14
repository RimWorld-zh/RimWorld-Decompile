using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class CompTerrainPump : ThingComp
	{
		private CompPowerTrader powerComp;

		private int progressTicks;

		private CompProperties_TerrainPump Props
		{
			get
			{
				return (CompProperties_TerrainPump)base.props;
			}
		}

		private float ProgressDays
		{
			get
			{
				return (float)((float)this.progressTicks / 60000.0);
			}
		}

		private float CurrentRadius
		{
			get
			{
				return Mathf.Min(this.Props.radius, this.ProgressDays / this.Props.daysToRadius * this.Props.radius);
			}
		}

		private bool Working
		{
			get
			{
				return this.powerComp == null || this.powerComp.PowerOn;
			}
		}

		private int TicksUntilRadiusInteger
		{
			get
			{
				float num = Mathf.Ceil(this.CurrentRadius) - this.CurrentRadius;
				if (num < 9.9999997473787516E-06)
				{
					num = 1f;
				}
				float num2 = this.Props.radius / this.Props.daysToRadius;
				float num3 = num / num2;
				return (int)(num3 * 60000.0);
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = base.parent.TryGetComp<CompPowerTrader>();
		}

		public override void PostDeSpawn(Map map)
		{
			this.progressTicks = 0;
		}

		public override void CompTickRare()
		{
			if (this.Working)
			{
				this.progressTicks += 250;
				int num = GenRadial.NumCellsInRadius(this.CurrentRadius);
				for (int i = 0; i < num; i++)
				{
					this.AffectCell(base.parent.Position + GenRadial.RadialPattern[i]);
				}
			}
		}

		protected abstract void AffectCell(IntVec3 c);

		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.progressTicks, "progressTicks", 0, false);
		}

		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.CurrentRadius < this.Props.radius - 9.9999997473787516E-05)
			{
				GenDraw.DrawRadiusRing(base.parent.Position, this.CurrentRadius);
			}
		}

		public override string CompInspectStringExtra()
		{
			string text = "TimePassed".Translate().CapitalizeFirst() + ": " + this.progressTicks.ToStringTicksToPeriod(true, false, true) + "\n" + "CurrentRadius".Translate().CapitalizeFirst() + ": " + this.CurrentRadius.ToString("F1");
			if (this.ProgressDays < this.Props.daysToRadius && this.Working)
			{
				string text2 = text;
				text = text2 + "\n" + "RadiusExpandsIn".Translate().CapitalizeFirst() + ": " + this.TicksUntilRadiusInteger.ToStringTicksToPeriod(true, false, true);
			}
			return text;
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "DEBUG: Progress 1 day",
				action = delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_004c: stateMachine*/)._0024this.progressTicks += 60000;
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
