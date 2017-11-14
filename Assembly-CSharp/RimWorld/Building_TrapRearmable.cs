using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Building_TrapRearmable : Building_Trap
	{
		private bool autoRearm;

		private bool armedInt = true;

		private Graphic graphicUnarmedInt;

		private static readonly FloatRange TrapDamageFactor = new FloatRange(0.7f, 1.3f);

		private static readonly IntRange DamageCount = new IntRange(1, 2);

		public override bool Armed
		{
			get
			{
				return this.armedInt;
			}
		}

		public override Graphic Graphic
		{
			get
			{
				if (this.armedInt)
				{
					return base.Graphic;
				}
				if (this.graphicUnarmedInt == null)
				{
					this.graphicUnarmedInt = base.def.building.trapUnarmedGraphicData.GraphicColoredFor(this);
				}
				return this.graphicUnarmedInt;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.armedInt, "armed", false, false);
			Scribe_Values.Look<bool>(ref this.autoRearm, "autoRearm", false, false);
		}

		protected override void SpringSub(Pawn p)
		{
			this.armedInt = false;
			if (p != null)
			{
				this.DamagePawn(p);
			}
			if (this.autoRearm)
			{
				base.Map.designationManager.AddDesignation(new Designation(this, DesignationDefOf.RearmTrap));
			}
		}

		public void Rearm()
		{
			this.armedInt = true;
			SoundDef.Named("TrapArm").PlayOneShot(new TargetInfo(base.Position, base.Map, false));
		}

		private void DamagePawn(Pawn p)
		{
			BodyPartHeight height = (BodyPartHeight)((!(Rand.Value < 0.66600000858306885)) ? 2 : 3);
			int num = Mathf.RoundToInt(this.GetStatValue(StatDefOf.TrapMeleeDamage, true) * Building_TrapRearmable.TrapDamageFactor.RandomInRange);
			int randomInRange = Building_TrapRearmable.DamageCount.RandomInRange;
			int num2 = 0;
			while (num2 < randomInRange && num > 0)
			{
				int num3 = Mathf.Max(1, Mathf.RoundToInt(Rand.Value * (float)num));
				num -= num3;
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Stab, num3, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
				dinfo.SetBodyRegion(height, BodyPartDepth.Outside);
				p.TakeDamage(dinfo);
				num2++;
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = base.GetGizmos().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield return (Gizmo)new Command_Toggle
			{
				defaultLabel = "CommandAutoRearm".Translate(),
				defaultDesc = "CommandAutoRearmDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc3,
				icon = TexCommand.RearmTrap,
				isActive = (() => ((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_010f: stateMachine*/)._0024this.autoRearm),
				toggleAction = delegate
				{
					((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_0126: stateMachine*/)._0024this.autoRearm = !((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_0126: stateMachine*/)._0024this.autoRearm;
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0160:
			/*Error near IL_0161: Unexpected return in MoveNext()*/;
		}
	}
}
