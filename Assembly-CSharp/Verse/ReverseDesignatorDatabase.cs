using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E18 RID: 3608
	public class ReverseDesignatorDatabase
	{
		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x060051CC RID: 20940 RVA: 0x0029DD64 File Offset: 0x0029C164
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

		// Token: 0x060051CD RID: 20941 RVA: 0x0029DD90 File Offset: 0x0029C190
		public void Reinit()
		{
			this.desList = null;
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x0029DD9C File Offset: 0x0029C19C
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

		// Token: 0x060051CF RID: 20943 RVA: 0x0029DE10 File Offset: 0x0029C210
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

		// Token: 0x0400358C RID: 13708
		private List<Designator> desList;
	}
}
