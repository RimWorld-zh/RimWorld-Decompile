using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D7 RID: 2519
	public class Verb_ShootOneUse : Verb_Shoot
	{
		// Token: 0x0600387B RID: 14459 RVA: 0x001E35A4 File Offset: 0x001E19A4
		protected override bool TryCastShot()
		{
			bool result;
			if (base.TryCastShot())
			{
				if (this.burstShotsLeft <= 1)
				{
					this.SelfConsume();
				}
				result = true;
			}
			else
			{
				if (this.burstShotsLeft < this.verbProps.burstShotCount)
				{
					this.SelfConsume();
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x001E35FC File Offset: 0x001E19FC
		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (this.state == VerbState.Bursting && this.burstShotsLeft < this.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x001E362D File Offset: 0x001E1A2D
		private void SelfConsume()
		{
			if (this.ownerEquipment != null && !this.ownerEquipment.Destroyed)
			{
				this.ownerEquipment.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
