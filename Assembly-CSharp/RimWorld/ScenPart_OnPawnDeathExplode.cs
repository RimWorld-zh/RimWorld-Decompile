using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000635 RID: 1589
	public class ScenPart_OnPawnDeathExplode : ScenPart
	{
		// Token: 0x040012C4 RID: 4804
		private float radius = 5.9f;

		// Token: 0x040012C5 RID: 4805
		private DamageDef damage;

		// Token: 0x040012C6 RID: 4806
		private string radiusBuf;

		// Token: 0x060020CA RID: 8394 RVA: 0x00118AEB File Offset: 0x00116EEB
		public override void Randomize()
		{
			this.radius = (float)Rand.RangeInclusive(3, 8) - 0.1f;
			this.damage = this.PossibleDamageDefs().RandomElement<DamageDef>();
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00118B13 File Offset: 0x00116F13
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damage, "damage");
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x00118B44 File Offset: 0x00116F44
		public override string Summary(Scenario scen)
		{
			return "ScenPart_OnPawnDeathExplode".Translate(new object[]
			{
				this.damage.label,
				this.radius.ToString()
			});
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x00118B8C File Offset: 0x00116F8C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect.TopHalf(), "radius".Translate(), ref this.radius, ref this.radiusBuf, 0f, 1E+09f);
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.damage.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu<DamageDef>(this.PossibleDamageDefs(), (DamageDef d) => d.LabelCap, (DamageDef d) => delegate()
				{
					this.damage = d;
				});
			}
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x00118C2C File Offset: 0x0011702C
		public override void Notify_PawnDied(Corpse corpse)
		{
			if (corpse.Spawned)
			{
				GenExplosion.DoExplosion(corpse.Position, corpse.Map, this.radius, this.damage, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			}
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x00118C80 File Offset: 0x00117080
		private IEnumerable<DamageDef> PossibleDamageDefs()
		{
			yield return DamageDefOf.Bomb;
			yield return DamageDefOf.Flame;
			yield break;
		}
	}
}
