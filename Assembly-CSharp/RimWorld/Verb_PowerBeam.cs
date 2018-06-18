using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009DA RID: 2522
	public class Verb_PowerBeam : Verb
	{
		// Token: 0x0600387E RID: 14462 RVA: 0x001E2B74 File Offset: 0x001E0F74
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

		// Token: 0x0600387F RID: 14463 RVA: 0x001E2C50 File Offset: 0x001E1050
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 15f;
		}

		// Token: 0x04002412 RID: 9234
		private const int DurationTicks = 600;
	}
}
