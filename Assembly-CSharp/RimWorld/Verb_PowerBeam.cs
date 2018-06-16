using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009DA RID: 2522
	public class Verb_PowerBeam : Verb
	{
		// Token: 0x0600387C RID: 14460 RVA: 0x001E2AA0 File Offset: 0x001E0EA0
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

		// Token: 0x0600387D RID: 14461 RVA: 0x001E2B7C File Offset: 0x001E0F7C
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 15f;
		}

		// Token: 0x04002412 RID: 9234
		private const int DurationTicks = 600;
	}
}
