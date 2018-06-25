using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000768 RID: 1896
	public sealed class PassingShipManager : IExposable
	{
		// Token: 0x040016A6 RID: 5798
		public Map map;

		// Token: 0x040016A7 RID: 5799
		public List<PassingShip> passingShips = new List<PassingShip>();

		// Token: 0x040016A8 RID: 5800
		private static List<PassingShip> tmpPassingShips = new List<PassingShip>();

		// Token: 0x060029ED RID: 10733 RVA: 0x0016377D File Offset: 0x00161B7D
		public PassingShipManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x00163798 File Offset: 0x00161B98
		public void ExposeData()
		{
			Scribe_Collections.Look<PassingShip>(ref this.passingShips, "passingShips", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.passingShips.Count; i++)
				{
					this.passingShips[i].passingShipManager = this;
				}
			}
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x001637FA File Offset: 0x00161BFA
		public void AddShip(PassingShip vis)
		{
			this.passingShips.Add(vis);
			vis.passingShipManager = this;
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x00163810 File Offset: 0x00161C10
		public void RemoveShip(PassingShip vis)
		{
			this.passingShips.Remove(vis);
			vis.passingShipManager = null;
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x00163828 File Offset: 0x00161C28
		public void PassingShipManagerTick()
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				this.passingShips[i].PassingShipTick();
			}
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x00163868 File Offset: 0x00161C68
		internal void DebugSendAllShipsAway()
		{
			PassingShipManager.tmpPassingShips.Clear();
			PassingShipManager.tmpPassingShips.AddRange(this.passingShips);
			for (int i = 0; i < PassingShipManager.tmpPassingShips.Count; i++)
			{
				PassingShipManager.tmpPassingShips[i].Depart();
			}
			Messages.Message("All passing ships sent away.", MessageTypeDefOf.TaskCompletion, false);
		}
	}
}
