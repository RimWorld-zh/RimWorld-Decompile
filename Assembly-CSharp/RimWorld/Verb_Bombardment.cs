using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D1 RID: 2513
	public class Verb_Bombardment : Verb
	{
		// Token: 0x06003860 RID: 14432 RVA: 0x001E1B68 File Offset: 0x001DFF68
		protected override bool TryCastShot()
		{
			bool result;
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				result = false;
			}
			else
			{
				Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
				bombardment.duration = 450;
				bombardment.instigator = this.caster;
				bombardment.weaponDef = ((this.ownerEquipment == null) ? null : this.ownerEquipment.def);
				bombardment.StartStrike();
				if (this.ownerEquipment != null && !this.ownerEquipment.Destroyed)
				{
					this.ownerEquipment.Destroy(DestroyMode.Vanish);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x001E1C44 File Offset: 0x001E0044
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		// Token: 0x04002408 RID: 9224
		private const int DurationTicks = 450;
	}
}
