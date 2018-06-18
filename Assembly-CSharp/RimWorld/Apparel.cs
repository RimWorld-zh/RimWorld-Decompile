using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C8 RID: 1736
	public class Apparel : ThingWithComps
	{
		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x0600257F RID: 9599 RVA: 0x00141644 File Offset: 0x0013FA44
		public Pawn Wearer
		{
			get
			{
				Pawn_ApparelTracker pawn_ApparelTracker = base.ParentHolder as Pawn_ApparelTracker;
				return (pawn_ApparelTracker == null) ? null : pawn_ApparelTracker.pawn;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002580 RID: 9600 RVA: 0x00141678 File Offset: 0x0013FA78
		public bool WornByCorpse
		{
			get
			{
				return this.wornByCorpseInt;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002581 RID: 9601 RVA: 0x00141694 File Offset: 0x0013FA94
		public override string DescriptionDetailed
		{
			get
			{
				string text = base.DescriptionDetailed;
				if (this.WornByCorpse)
				{
					text = text + "\n" + "WasWornByCorpse".Translate();
				}
				return text;
			}
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x001416D2 File Offset: 0x0013FAD2
		public void Notify_PawnKilled()
		{
			if (this.def.apparel.careIfWornByCorpse)
			{
				this.wornByCorpseInt = true;
			}
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x001416F1 File Offset: 0x0013FAF1
		public void Notify_PawnResurrected()
		{
			this.wornByCorpseInt = false;
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x001416FB File Offset: 0x0013FAFB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wornByCorpseInt, "wornByCorpse", false, false);
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x00141716 File Offset: 0x0013FB16
		public virtual void DrawWornExtras()
		{
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x0014171C File Offset: 0x0013FB1C
		public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			return false;
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x00141734 File Offset: 0x0013FB34
		public virtual bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return true;
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x0014174C File Offset: 0x0013FB4C
		public virtual IEnumerable<Gizmo> GetWornGizmos()
		{
			yield break;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x00141770 File Offset: 0x0013FB70
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.WornByCorpse)
			{
				if (text.Length > 0)
				{
					text += "\n";
				}
				text += "WasWornByCorpse".Translate();
			}
			return text;
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x001417C4 File Offset: 0x0013FBC4
		public virtual float GetSpecialApparelScoreOffset()
		{
			return 0f;
		}

		// Token: 0x040014E3 RID: 5347
		private bool wornByCorpseInt;
	}
}
