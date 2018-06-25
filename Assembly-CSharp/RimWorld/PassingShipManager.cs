using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000768 RID: 1896
	public sealed class PassingShipManager : IExposable
	{
		// Token: 0x040016A2 RID: 5794
		public Map map;

		// Token: 0x040016A3 RID: 5795
		public List<PassingShip> passingShips = new List<PassingShip>();

		// Token: 0x040016A4 RID: 5796
		private static List<PassingShip> tmpPassingShips = new List<PassingShip>();

		// Token: 0x060029EE RID: 10734 RVA: 0x0016351D File Offset: 0x0016191D
		public PassingShipManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x00163538 File Offset: 0x00161938
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

		// Token: 0x060029F0 RID: 10736 RVA: 0x0016359A File Offset: 0x0016199A
		public void AddShip(PassingShip vis)
		{
			this.passingShips.Add(vis);
			vis.passingShipManager = this;
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x001635B0 File Offset: 0x001619B0
		public void RemoveShip(PassingShip vis)
		{
			this.passingShips.Remove(vis);
			vis.passingShipManager = null;
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x001635C8 File Offset: 0x001619C8
		public void PassingShipManagerTick()
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				this.passingShips[i].PassingShipTick();
			}
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x00163608 File Offset: 0x00161A08
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
