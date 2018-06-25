using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F4 RID: 756
	public class GameRules : IExposable
	{
		// Token: 0x04000833 RID: 2099
		private List<Type> disallowedDesignatorTypes = new List<Type>();

		// Token: 0x04000834 RID: 2100
		private List<BuildableDef> disallowedBuildings = new List<BuildableDef>();

		// Token: 0x06000C82 RID: 3202 RVA: 0x0006F1A8 File Offset: 0x0006D5A8
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

		// Token: 0x06000C83 RID: 3203 RVA: 0x0006F208 File Offset: 0x0006D608
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

		// Token: 0x06000C84 RID: 3204 RVA: 0x0006F260 File Offset: 0x0006D660
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

		// Token: 0x06000C85 RID: 3205 RVA: 0x0006F2B0 File Offset: 0x0006D6B0
		public void ExposeData()
		{
			Scribe_Collections.Look<BuildableDef>(ref this.disallowedBuildings, "disallowedBuildings", LookMode.Undefined, new object[0]);
			Scribe_Collections.Look<Type>(ref this.disallowedDesignatorTypes, "disallowedDesignatorTypes", LookMode.Undefined, new object[0]);
		}
	}
}
