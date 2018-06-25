using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E17 RID: 3607
	public class ReverseDesignatorDatabase
	{
		// Token: 0x04003598 RID: 13720
		private List<Designator> desList;

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x060051E2 RID: 20962 RVA: 0x0029F730 File Offset: 0x0029DB30
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

		// Token: 0x060051E3 RID: 20963 RVA: 0x0029F75C File Offset: 0x0029DB5C
		public void Reinit()
		{
			this.desList = null;
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x0029F768 File Offset: 0x0029DB68
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

		// Token: 0x060051E5 RID: 20965 RVA: 0x0029F7DC File Offset: 0x0029DBDC
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
