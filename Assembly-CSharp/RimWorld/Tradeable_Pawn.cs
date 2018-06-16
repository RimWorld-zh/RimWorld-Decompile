using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000785 RID: 1925
	public class Tradeable_Pawn : Tradeable
	{
		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06002AAE RID: 10926 RVA: 0x001692B4 File Offset: 0x001676B4
		public override Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.AnyPawn);
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06002AAF RID: 10927 RVA: 0x001692D4 File Offset: 0x001676D4
		public override string Label
		{
			get
			{
				string text = base.Label;
				if (this.AnyPawn.Name != null && !this.AnyPawn.Name.Numerical)
				{
					text = text + ", " + this.AnyPawn.def.label;
				}
				string text2 = text;
				return string.Concat(new string[]
				{
					text2,
					" (",
					this.AnyPawn.gender.GetLabel(),
					", ",
					this.AnyPawn.ageTracker.AgeBiologicalYearsFloat.ToString("F0"),
					")"
				});
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002AB0 RID: 10928 RVA: 0x00169390 File Offset: 0x00167790
		public override string TipDescription
		{
			get
			{
				string result;
				if (!this.HasAnyThing)
				{
					result = "";
				}
				else
				{
					string text = this.AnyPawn.MainDesc(true);
					text = text + "\n\n" + this.AnyPawn.def.description;
					result = text;
				}
				return result;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002AB1 RID: 10929 RVA: 0x001693E8 File Offset: 0x001677E8
		private Pawn AnyPawn
		{
			get
			{
				return (Pawn)this.AnyThing;
			}
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x00169408 File Offset: 0x00167808
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerSells)
			{
				List<Pawn> list = this.thingsColony.Take(base.CountToTransferToDestination).Cast<Pawn>().ToList<Pawn>();
				for (int i = 0; i < list.Count; i++)
				{
					TradeSession.trader.GiveSoldThingToTrader(list[i], 1, TradeSession.playerNegotiator);
				}
			}
			else if (base.ActionToDo == TradeAction.PlayerBuys)
			{
				List<Pawn> list2 = this.thingsTrader.Take(base.CountToTransferToSource).Cast<Pawn>().ToList<Pawn>();
				for (int j = 0; j < list2.Count; j++)
				{
					TradeSession.trader.GiveSoldThingToPlayer(list2[j], 1, TradeSession.playerNegotiator);
				}
			}
		}
	}
}
