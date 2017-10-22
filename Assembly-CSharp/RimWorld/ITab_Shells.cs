namespace RimWorld
{
	public class ITab_Shells : ITab_Storage
	{
		protected override IStoreSettingsParent SelStoreSettingsParent
		{
			get
			{
				IStoreSettingsParent selStoreSettingsParent = base.SelStoreSettingsParent;
				IStoreSettingsParent result;
				if (selStoreSettingsParent != null)
				{
					result = selStoreSettingsParent;
				}
				else
				{
					Building_TurretGun building_TurretGun = base.SelObject as Building_TurretGun;
					result = ((building_TurretGun == null) ? null : base.GetThingOrThingCompStoreSettingsParent(building_TurretGun.gun));
				}
				return result;
			}
		}

		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}

		public ITab_Shells()
		{
			base.labelKey = "TabShells";
			base.tutorTag = "Shells";
		}
	}
}
