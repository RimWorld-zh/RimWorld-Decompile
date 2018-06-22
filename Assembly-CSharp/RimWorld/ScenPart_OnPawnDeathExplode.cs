using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000633 RID: 1587
	public class ScenPart_OnPawnDeathExplode : ScenPart
	{
		// Token: 0x060020C6 RID: 8390 RVA: 0x0011899B File Offset: 0x00116D9B
		public override void Randomize()
		{
			this.radius = (float)Rand.RangeInclusive(3, 8) - 0.1f;
			this.damage = this.PossibleDamageDefs().RandomElement<DamageDef>();
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x001189C3 File Offset: 0x00116DC3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damage, "damage");
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x001189F4 File Offset: 0x00116DF4
		public override string Summary(Scenario scen)
		{
			return "ScenPart_OnPawnDeathExplode".Translate(new object[]
			{
				this.damage.label,
				this.radius.ToString()
			});
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x00118A3C File Offset: 0x00116E3C
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

		// Token: 0x060020CA RID: 8394 RVA: 0x00118ADC File Offset: 0x00116EDC
		public override void Notify_PawnDied(Corpse corpse)
		{
			if (corpse.Spawned)
			{
				GenExplosion.DoExplosion(corpse.Position, corpse.Map, this.radius, this.damage, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			}
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00118B30 File Offset: 0x00116F30
		private IEnumerable<DamageDef> PossibleDamageDefs()
		{
			yield return DamageDefOf.Bomb;
			yield return DamageDefOf.Flame;
			yield break;
		}

		// Token: 0x040012C4 RID: 4804
		private float radius = 5.9f;

		// Token: 0x040012C5 RID: 4805
		private DamageDef damage;

		// Token: 0x040012C6 RID: 4806
		private string radiusBuf;
	}
}
