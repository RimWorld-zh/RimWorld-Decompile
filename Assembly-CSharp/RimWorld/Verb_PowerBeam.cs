using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D6 RID: 2518
	public class Verb_PowerBeam : Verb
	{
		// Token: 0x06003878 RID: 14456 RVA: 0x001E2D4C File Offset: 0x001E114C
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

		// Token: 0x06003879 RID: 14457 RVA: 0x001E2E28 File Offset: 0x001E1228
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 15f;
		}

		// Token: 0x0400240D RID: 9229
		private const int DurationTicks = 600;
	}
}
