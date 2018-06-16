using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009DB RID: 2523
	public class Verb_ShootOneUse : Verb_Shoot
	{
		// Token: 0x0600387F RID: 14463 RVA: 0x001E3290 File Offset: 0x001E1690
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

		// Token: 0x06003880 RID: 14464 RVA: 0x001E32E8 File Offset: 0x001E16E8
		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (this.state == VerbState.Bursting && this.burstShotsLeft < this.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x001E3319 File Offset: 0x001E1719
		private void SelfConsume()
		{
			if (this.ownerEquipment != null && !this.ownerEquipment.Destroyed)
			{
				this.ownerEquipment.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
