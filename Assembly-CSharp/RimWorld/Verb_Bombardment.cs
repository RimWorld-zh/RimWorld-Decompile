using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D3 RID: 2515
	public class Verb_Bombardment : Verb
	{
		// Token: 0x04002409 RID: 9225
		private const int DurationTicks = 450;

		// Token: 0x06003864 RID: 14436 RVA: 0x001E1C8C File Offset: 0x001E008C
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

		// Token: 0x06003865 RID: 14437 RVA: 0x001E1D68 File Offset: 0x001E0168
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}
	}
}
