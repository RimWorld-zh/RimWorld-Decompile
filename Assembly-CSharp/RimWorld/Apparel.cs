using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C6 RID: 1734
	public class Apparel : ThingWithComps
	{
		// Token: 0x040014E5 RID: 5349
		private bool wornByCorpseInt;

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x00141B40 File Offset: 0x0013FF40
		public Pawn Wearer
		{
			get
			{
				Pawn_ApparelTracker pawn_ApparelTracker = base.ParentHolder as Pawn_ApparelTracker;
				return (pawn_ApparelTracker == null) ? null : pawn_ApparelTracker.pawn;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x00141B74 File Offset: 0x0013FF74
		public bool WornByCorpse
		{
			get
			{
				return this.wornByCorpseInt;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x0600257C RID: 9596 RVA: 0x00141B90 File Offset: 0x0013FF90
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

		// Token: 0x0600257D RID: 9597 RVA: 0x00141BCE File Offset: 0x0013FFCE
		public void Notify_PawnKilled()
		{
			if (this.def.apparel.careIfWornByCorpse)
			{
				this.wornByCorpseInt = true;
			}
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00141BED File Offset: 0x0013FFED
		public void Notify_PawnResurrected()
		{
			this.wornByCorpseInt = false;
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00141BF7 File Offset: 0x0013FFF7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wornByCorpseInt, "wornByCorpse", false, false);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00141C12 File Offset: 0x00140012
		public virtual void DrawWornExtras()
		{
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x00141C18 File Offset: 0x00140018
		public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			return false;
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x00141C30 File Offset: 0x00140030
		public virtual bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return true;
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x00141C48 File Offset: 0x00140048
		public virtual IEnumerable<Gizmo> GetWornGizmos()
		{
			yield break;
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x00141C6C File Offset: 0x0014006C
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

		// Token: 0x06002585 RID: 9605 RVA: 0x00141CC0 File Offset: 0x001400C0
		public virtual float GetSpecialApparelScoreOffset()
		{
			return 0f;
		}
	}
}
