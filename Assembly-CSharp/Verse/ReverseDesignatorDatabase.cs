using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E16 RID: 3606
	public class ReverseDesignatorDatabase
	{
		// Token: 0x04003591 RID: 13713
		private List<Designator> desList;

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x060051E2 RID: 20962 RVA: 0x0029F450 File Offset: 0x0029D850
		public List<Designator> AllDesignators
		{
			get
			{
				if (this.desList == null)
				{
					this.InitDesignators();
				}
				return this.desList;
			}
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x0029F47C File Offset: 0x0029D87C
		public void Reinit()
		{
			this.desList = null;
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x0029F488 File Offset: 0x0029D888
		public T Get<T>() where T : Designator
		{
			if (this.desList == null)
			{
				this.InitDesignators();
			}
			for (int i = 0; i < this.desList.Count; i++)
			{
				T t = this.desList[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return (T)((object)null);
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x0029F4FC File Offset: 0x0029D8FC
		private void InitDesignators()
		{
			this.desList = new List<Designator>();
			this.desList.Add(new Designator_Cancel());
			this.desList.Add(new Designator_Claim());
			this.desList.Add(new Designator_Deconstruct());
			this.desList.Add(new Designator_Uninstall());
			this.desList.Add(new Designator_Haul());
			this.desList.Add(new Designator_Hunt());
			this.desList.Add(new Designator_Slaughter());
			this.desList.Add(new Designator_Tame());
			this.desList.Add(new Designator_PlantsCut());
			this.desList.Add(new Designator_PlantsHarvest());
			this.desList.Add(new Designator_Mine());
			this.desList.Add(new Designator_Strip());
			this.desList.Add(new Designator_RearmTrap());
			this.desList.Add(new Designator_Open());
			this.desList.RemoveAll((Designator des) => !Current.Game.Rules.DesignatorAllowed(des));
		}
	}
}
