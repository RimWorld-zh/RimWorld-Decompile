using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000781 RID: 1921
	public class Tradeable_Pawn : Tradeable
	{
		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06002AA9 RID: 10921 RVA: 0x00169520 File Offset: 0x00167920
		public override Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.AnyPawn);
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002AAA RID: 10922 RVA: 0x00169540 File Offset: 0x00167940
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

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002AAB RID: 10923 RVA: 0x001695FC File Offset: 0x001679FC
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

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002AAC RID: 10924 RVA: 0x00169654 File Offset: 0x00167A54
		private Pawn AnyPawn
		{
			get
			{
				return (Pawn)this.AnyThing;
			}
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x00169674 File Offset: 0x00167A74
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
