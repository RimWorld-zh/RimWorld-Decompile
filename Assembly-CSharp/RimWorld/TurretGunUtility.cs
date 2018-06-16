using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068B RID: 1675
	public static class TurretGunUtility
	{
		// Token: 0x0600236E RID: 9070 RVA: 0x0013041C File Offset: 0x0012E81C
		public static bool NeedsShells(ThingDef turret)
		{
			return turret.category == ThingCategory.Building && turret.building.IsTurret && turret.building.turretGunDef.HasComp(typeof(CompChangeableProjectile));
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x0013046C File Offset: 0x0012E86C
		public static ThingDef TryFindRandomShellDef(ThingDef turret, bool allowEMP = true, bool mustHarmHealth = true, TechLevel techLevel = TechLevel.Undefined, bool allowAntigrainWarhead = false, float maxMarketValue = -1f)
		{
			ThingDef result;
			if (!TurretGunUtility.NeedsShells(turret))
			{
				result = null;
			}
			else
			{
				ThingFilter fixedFilter = turret.building.turretGunDef.building.fixedStorageSettings.filter;
				ThingDef thingDef;
				if ((from x in DefDatabase<ThingDef>.AllDefsListForReading
				where fixedFilter.Allows(x) && (allowEMP || x.projectileWhenLoaded.projectile.damageDef != DamageDefOf.EMP) && (!mustHarmHealth || x.projectileWhenLoaded.projectile.damageDef.harmsHealth) && (techLevel == TechLevel.Undefined || x.techLevel <= techLevel) && (allowAntigrainWarhead || x != ThingDefOf.Shell_AntigrainWarhead) && (maxMarketValue < 0f || x.BaseMarketValue <= maxMarketValue)
				select x).TryRandomElement(out thingDef))
				{
					result = thingDef;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
	}
}
