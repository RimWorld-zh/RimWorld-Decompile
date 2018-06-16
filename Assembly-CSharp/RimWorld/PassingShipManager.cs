using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200076A RID: 1898
	public sealed class PassingShipManager : IExposable
	{
		// Token: 0x060029EF RID: 10735 RVA: 0x00163161 File Offset: 0x00161561
		public PassingShipManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x0016317C File Offset: 0x0016157C
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

		// Token: 0x060029F1 RID: 10737 RVA: 0x001631DE File Offset: 0x001615DE
		public void AddShip(PassingShip vis)
		{
			this.passingShips.Add(vis);
			vis.passingShipManager = this;
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x001631F4 File Offset: 0x001615F4
		public void RemoveShip(PassingShip vis)
		{
			this.passingShips.Remove(vis);
			vis.passingShipManager = null;
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x0016320C File Offset: 0x0016160C
		public void PassingShipManagerTick()
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				this.passingShips[i].PassingShipTick();
			}
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x0016324C File Offset: 0x0016164C
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

		// Token: 0x040016A4 RID: 5796
		public Map map;

		// Token: 0x040016A5 RID: 5797
		public List<PassingShip> passingShips = new List<PassingShip>();

		// Token: 0x040016A6 RID: 5798
		private static List<PassingShip> tmpPassingShips = new List<PassingShip>();
	}
}
