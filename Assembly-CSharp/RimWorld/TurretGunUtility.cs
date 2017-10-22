using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class TurretGunUtility
	{
		public static bool NeedsShells(ThingDef turret)
		{
			return turret.category == ThingCategory.Building && turret.building.IsTurret && turret.building.turretGunDef.HasComp(typeof(CompChangeableProjectile));
		}

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
				ThingDef thingDef = default(ThingDef);
				result = ((!(from x in DefDatabase<ThingDef>.AllDefsListForReading
				where fixedFilter.Allows(x) && (allowEMP || x.projectileWhenLoaded.projectile.damageDef != DamageDefOf.EMP) && (!mustHarmHealth || x.projectileWhenLoaded.projectile.damageDef.harmsHealth) && (techLevel == TechLevel.Undefined || (int)x.techLevel <= (int)techLevel) && (allowAntigrainWarhead || x != ThingDefOf.Shell_AntigrainWarhead) && (maxMarketValue < 0.0 || x.BaseMarketValue <= maxMarketValue)
				select x).TryRandomElement<ThingDef>(out thingDef)) ? null : thingDef);
			}
			return result;
		}
	}
}
