using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000766 RID: 1894
	public sealed class PassingShipManager : IExposable
	{
		// Token: 0x040016A2 RID: 5794
		public Map map;

		// Token: 0x040016A3 RID: 5795
		public List<PassingShip> passingShips = new List<PassingShip>();

		// Token: 0x040016A4 RID: 5796
		private static List<PassingShip> tmpPassingShips = new List<PassingShip>();

		// Token: 0x060029EA RID: 10730 RVA: 0x001633CD File Offset: 0x001617CD
		public PassingShipManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x001633E8 File Offset: 0x001617E8
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

		// Token: 0x060029EC RID: 10732 RVA: 0x0016344A File Offset: 0x0016184A
		public void AddShip(PassingShip vis)
		{
			this.passingShips.Add(vis);
			vis.passingShipManager = this;
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x00163460 File Offset: 0x00161860
		public void RemoveShip(PassingShip vis)
		{
			this.passingShips.Remove(vis);
			vis.passingShipManager = null;
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x00163478 File Offset: 0x00161878
		public void PassingShipManagerTick()
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				this.passingShips[i].PassingShipTick();
			}
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x001634B8 File Offset: 0x001618B8
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
