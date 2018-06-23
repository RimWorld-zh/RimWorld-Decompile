using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F2 RID: 754
	public class GameRules : IExposable
	{
		// Token: 0x04000830 RID: 2096
		private List<Type> disallowedDesignatorTypes = new List<Type>();

		// Token: 0x04000831 RID: 2097
		private List<BuildableDef> disallowedBuildings = new List<BuildableDef>();

		// Token: 0x06000C7F RID: 3199 RVA: 0x0006F050 File Offset: 0x0006D450
		public void SetAllowDesignator(Type type, bool allowed)
		{
			if (allowed && this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Remove(type);
			}
			if (!allowed && !this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Add(type);
			}
			Find.ReverseDesignatorDatabase.Reinit();
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0006F0B0 File Offset: 0x0006D4B0
		public void SetAllowBuilding(BuildableDef building, bool allowed)
		{
			if (allowed && this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Remove(building);
			}
			if (!allowed && !this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Add(building);
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0006F108 File Offset: 0x0006D508
		public bool DesignatorAllowed(Designator d)
		{
			Designator_Place designator_Place = d as Designator_Place;
			bool result;
			if (designator_Place != null)
			{
				result = !this.disallowedBuildings.Contains(designator_Place.PlacingDef);
			}
			else
			{
				result = !this.disallowedDesignatorTypes.Contains(d.GetType());
			}
			return result;
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0006F158 File Offset: 0x0006D558
		public void ExposeData()
		{
			Scribe_Collections.Look<BuildableDef>(ref this.disallowedBuildings, "disallowedBuildings", LookMode.Undefined, new object[0]);
			Scribe_Collections.Look<Type>(ref this.disallowedDesignatorTypes, "disallowedDesignatorTypes", LookMode.Undefined, new object[0]);
		}
	}
}
