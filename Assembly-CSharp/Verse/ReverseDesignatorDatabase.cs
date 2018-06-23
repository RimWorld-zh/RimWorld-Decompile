using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E14 RID: 3604
	public class ReverseDesignatorDatabase
	{
		// Token: 0x04003591 RID: 13713
		private List<Designator> desList;

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x060051DE RID: 20958 RVA: 0x0029F324 File Offset: 0x0029D724
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

		// Token: 0x060051DF RID: 20959 RVA: 0x0029F350 File Offset: 0x0029D750
		public void Reinit()
		{
			this.desList = null;
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x0029F35C File Offset: 0x0029D75C
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

		// Token: 0x060051E1 RID: 20961 RVA: 0x0029F3D0 File Offset: 0x0029D7D0
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
