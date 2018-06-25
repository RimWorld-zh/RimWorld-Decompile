using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D8 RID: 2520
	public class Verb_PowerBeam : Verb
	{
		// Token: 0x04002415 RID: 9237
		private const int DurationTicks = 600;

		// Token: 0x0600387C RID: 14460 RVA: 0x001E3144 File Offset: 0x001E1544
		protected override bool TryCastShot()
		{
			bool result;
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				result = false;
			}
			else
			{
				PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
				powerBeam.duration = 600;
				powerBeam.instigator = this.caster;
				powerBeam.weaponDef = ((this.ownerEquipment == null) ? null : this.ownerEquipment.def);
				powerBeam.StartStrike();
				if (this.ownerEquipment != null && !this.ownerEquipment.Destroyed)
				{
					this.ownerEquipment.Destroy(DestroyMode.Vanish);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x001E3220 File Offset: 0x001E1620
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 15f;
		}
	}
}
