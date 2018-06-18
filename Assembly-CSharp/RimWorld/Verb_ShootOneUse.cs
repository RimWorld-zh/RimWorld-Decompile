using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009DB RID: 2523
	public class Verb_ShootOneUse : Verb_Shoot
	{
		// Token: 0x06003881 RID: 14465 RVA: 0x001E3364 File Offset: 0x001E1764
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

		// Token: 0x06003882 RID: 14466 RVA: 0x001E33BC File Offset: 0x001E17BC
		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (this.state == VerbState.Bursting && this.burstShotsLeft < this.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x001E33ED File Offset: 0x001E17ED
		private void SelfConsume()
		{
			if (this.ownerEquipment != null && !this.ownerEquipment.Destroyed)
			{
				this.ownerEquipment.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
