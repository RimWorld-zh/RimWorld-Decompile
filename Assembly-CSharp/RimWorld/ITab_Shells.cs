using System;

namespace RimWorld
{
	// Token: 0x02000859 RID: 2137
	public class ITab_Shells : ITab_Storage
	{
		// Token: 0x06003067 RID: 12391 RVA: 0x001A5A34 File Offset: 0x001A3E34
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06003068 RID: 12392 RVA: 0x001A5A54 File Offset: 0x001A3E54
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
					if (building_TurretGun != null)
					{
						result = base.GetThingOrThingCompStoreSettingsParent(building_TurretGun.gun);
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06003069 RID: 12393 RVA: 0x001A5AA4 File Offset: 0x001A3EA4
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}
	}
}
