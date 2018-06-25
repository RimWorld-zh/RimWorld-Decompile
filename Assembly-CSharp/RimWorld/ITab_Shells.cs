using System;

namespace RimWorld
{
	// Token: 0x0200085B RID: 2139
	public class ITab_Shells : ITab_Storage
	{
		// Token: 0x0600306B RID: 12395 RVA: 0x001A5B84 File Offset: 0x001A3F84
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x0600306C RID: 12396 RVA: 0x001A5BA4 File Offset: 0x001A3FA4
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
		// (get) Token: 0x0600306D RID: 12397 RVA: 0x001A5BF4 File Offset: 0x001A3FF4
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}
	}
}
