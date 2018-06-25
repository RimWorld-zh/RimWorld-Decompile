using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200065C RID: 1628
	public class TaleData_Pawn : TaleData
	{
		// Token: 0x04001348 RID: 4936
		public Pawn pawn;

		// Token: 0x04001349 RID: 4937
		public PawnKindDef kind;

		// Token: 0x0400134A RID: 4938
		public Faction faction;

		// Token: 0x0400134B RID: 4939
		public Gender gender;

		// Token: 0x0400134C RID: 4940
		public Name name;

		// Token: 0x0400134D RID: 4941
		public string title;

		// Token: 0x0400134E RID: 4942
		public ThingDef primaryEquipment;

		// Token: 0x0400134F RID: 4943
		public ThingDef notableApparel;

		// Token: 0x060021FF RID: 8703 RVA: 0x001206A4 File Offset: 0x0011EAA4
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_Defs.Look<PawnKindDef>(ref this.kind, "kind");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Deep.Look<Name>(ref this.name, "name", new object[0]);
			Scribe_Values.Look<Gender>(ref this.gender, "title", Gender.None, false);
			Scribe_Defs.Look<ThingDef>(ref this.primaryEquipment, "peq");
			Scribe_Defs.Look<ThingDef>(ref this.notableApparel, "app");
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x00120740 File Offset: 0x0011EB40
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			return GrammarUtility.RulesForPawn(prefix, this.name, this.title, this.kind, this.gender, this.faction, null);
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x0012077C File Offset: 0x0011EB7C
		public static TaleData_Pawn GenerateFrom(Pawn pawn)
		{
			TaleData_Pawn taleData_Pawn = new TaleData_Pawn();
			taleData_Pawn.pawn = pawn;
			taleData_Pawn.kind = pawn.kindDef;
			taleData_Pawn.faction = pawn.Faction;
			taleData_Pawn.gender = ((!pawn.RaceProps.hasGenders) ? Gender.None : pawn.gender);
			if (pawn.story != null)
			{
				taleData_Pawn.title = pawn.story.title;
			}
			if (pawn.RaceProps.Humanlike)
			{
				taleData_Pawn.name = pawn.Name;
				if (pawn.equipment.Primary != null)
				{
					taleData_Pawn.primaryEquipment = pawn.equipment.Primary.def;
				}
				Apparel apparel;
				if (pawn.apparel.WornApparel.TryRandomElement(out apparel))
				{
					taleData_Pawn.notableApparel = apparel.def;
				}
			}
			return taleData_Pawn;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x0012085C File Offset: 0x0011EC5C
		public static TaleData_Pawn GenerateRandom()
		{
			PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
			Faction faction = FactionUtility.DefaultFactionFrom(random.defaultFactionType);
			Pawn pawn = PawnGenerator.GeneratePawn(random, faction);
			return TaleData_Pawn.GenerateFrom(pawn);
		}
	}
}
