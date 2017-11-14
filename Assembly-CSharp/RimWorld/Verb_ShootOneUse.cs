using Verse;

namespace RimWorld
{
	public class Verb_ShootOneUse : Verb_Shoot
	{
		protected override bool TryCastShot()
		{
			if (base.TryCastShot())
			{
				if (base.burstShotsLeft <= 1)
				{
					this.SelfConsume();
				}
				return true;
			}
			if (base.burstShotsLeft < base.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
			return false;
		}

		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (base.state == VerbState.Bursting && base.burstShotsLeft < base.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
		}

		private void SelfConsume()
		{
			if (base.ownerEquipment != null && !base.ownerEquipment.Destroyed)
			{
				base.ownerEquipment.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
