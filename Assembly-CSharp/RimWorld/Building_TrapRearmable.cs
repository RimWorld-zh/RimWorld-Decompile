using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000688 RID: 1672
	public class Building_TrapRearmable : Building_Trap
	{
		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06002331 RID: 9009 RVA: 0x0012E72C File Offset: 0x0012CB2C
		public override bool Armed
		{
			get
			{
				return this.armedInt;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06002332 RID: 9010 RVA: 0x0012E748 File Offset: 0x0012CB48
		public override Graphic Graphic
		{
			get
			{
				Graphic graphic;
				if (this.armedInt)
				{
					graphic = base.Graphic;
				}
				else
				{
					if (this.graphicUnarmedInt == null)
					{
						this.graphicUnarmedInt = this.def.building.trapUnarmedGraphicData.GraphicColoredFor(this);
					}
					graphic = this.graphicUnarmedInt;
				}
				return graphic;
			}
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0012E7A2 File Offset: 0x0012CBA2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.armedInt, "armed", false, false);
			Scribe_Values.Look<bool>(ref this.autoRearm, "autoRearm", false, false);
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0012E7D0 File Offset: 0x0012CBD0
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

		// Token: 0x06002335 RID: 9013 RVA: 0x0012E81D File Offset: 0x0012CC1D
		public void Rearm()
		{
			this.armedInt = true;
			SoundDefOf.TrapArm.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0012E848 File Offset: 0x0012CC48
		private void DamagePawn(Pawn p)
		{
			BodyPartHeight height = (Rand.Value >= 0.666f) ? BodyPartHeight.Middle : BodyPartHeight.Top;
			int num = Mathf.RoundToInt(this.GetStatValue(StatDefOf.TrapMeleeDamage, true) * Building_TrapRearmable.TrapDamageFactor.RandomInRange);
			int randomInRange = Building_TrapRearmable.DamageCount.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				if (num <= 0)
				{
					break;
				}
				int num2 = Mathf.Max(1, Mathf.RoundToInt(Rand.Value * (float)num));
				num -= num2;
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Stab, (float)num2, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
				dinfo.SetBodyRegion(height, BodyPartDepth.Outside);
				p.TakeDamage(dinfo);
			}
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0012E908 File Offset: 0x0012CD08
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			yield return new Command_Toggle
			{
				defaultLabel = "CommandAutoRearm".Translate(),
				defaultDesc = "CommandAutoRearmDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc3,
				icon = TexCommand.RearmTrap,
				isActive = (() => this.autoRearm),
				toggleAction = delegate()
				{
					this.autoRearm = !this.autoRearm;
				}
			};
			yield break;
		}

		// Token: 0x040013C5 RID: 5061
		private bool autoRearm = false;

		// Token: 0x040013C6 RID: 5062
		private bool armedInt = true;

		// Token: 0x040013C7 RID: 5063
		private Graphic graphicUnarmedInt;

		// Token: 0x040013C8 RID: 5064
		private static readonly FloatRange TrapDamageFactor = new FloatRange(0.7f, 1.3f);

		// Token: 0x040013C9 RID: 5065
		private static readonly IntRange DamageCount = new IntRange(1, 2);
	}
}
