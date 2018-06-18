using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E17 RID: 3607
	public class ReverseDesignatorDatabase
	{
		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x060051CA RID: 20938 RVA: 0x0029DD44 File Offset: 0x0029C144
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

		// Token: 0x060051CB RID: 20939 RVA: 0x0029DD70 File Offset: 0x0029C170
		public void Reinit()
		{
			this.desList = null;
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x0029DD7C File Offset: 0x0029C17C
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

		// Token: 0x060051CD RID: 20941 RVA: 0x0029DDF0 File Offset: 0x0029C1F0
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

		// Token: 0x0400358A RID: 13706
		private List<Designator> desList;
	}
}
