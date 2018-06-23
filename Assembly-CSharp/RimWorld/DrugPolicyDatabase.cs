using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002EF RID: 751
	public class DrugPolicyDatabase : IExposable
	{
		// Token: 0x0400082A RID: 2090
		private List<DrugPolicy> policies = new List<DrugPolicy>();

		// Token: 0x06000C6B RID: 3179 RVA: 0x0006E720 File Offset: 0x0006CB20
		public DrugPolicyDatabase()
		{
			this.GenerateStartingDrugPolicies();
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x0006E73C File Offset: 0x0006CB3C
		public List<DrugPolicy> AllPolicies
		{
			get
			{
				return this.policies;
			}
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0006E757 File Offset: 0x0006CB57
		public void ExposeData()
		{
			Scribe_Collections.Look<DrugPolicy>(ref this.policies, "policies", LookMode.Deep, new object[0]);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0006E774 File Offset: 0x0006CB74
		public DrugPolicy DefaultDrugPolicy()
		{
			if (this.policies.Count == 0)
			{
				this.MakeNewDrugPolicy();
			}
			return this.policies[0];
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x0006E7AC File Offset: 0x0006CBAC
		public AcceptanceReport TryDelete(DrugPolicy policy)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
			{
				if (pawn.drugs != null && pawn.drugs.CurrentPolicy == policy)
				{
					return new AcceptanceReport("DrugPolicyInUse".Translate(new object[]
					{
						pawn
					}));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.drugs != null && pawn2.drugs.CurrentPolicy == policy)
				{
					pawn2.drugs.CurrentPolicy = null;
				}
			}
			this.policies.Remove(policy);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0006E8C8 File Offset: 0x0006CCC8
		public DrugPolicy MakeNewDrugPolicy()
		{
			int num;
			if (this.policies.Any<DrugPolicy>())
			{
				num = this.policies.Max((DrugPolicy o) => o.uniqueId) + 1;
			}
			else
			{
				num = 1;
			}
			int uniqueId = num;
			DrugPolicy drugPolicy = new DrugPolicy(uniqueId, "DrugPolicy".Translate() + " " + uniqueId.ToString());
			this.policies.Add(drugPolicy);
			return drugPolicy;
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0006E954 File Offset: 0x0006CD54
		private void GenerateStartingDrugPolicies()
		{
			DrugPolicy drugPolicy = this.MakeNewDrugPolicy();
			drugPolicy.label = "SocialDrugs".Translate();
			drugPolicy[ThingDefOf.Beer].allowedForJoy = true;
			drugPolicy[ThingDefOf.SmokeleafJoint].allowedForJoy = true;
			DrugPolicy drugPolicy2 = this.MakeNewDrugPolicy();
			drugPolicy2.label = "NoDrugs".Translate();
			DrugPolicy drugPolicy3 = this.MakeNewDrugPolicy();
			drugPolicy3.label = "Unrestricted".Translate();
			for (int i = 0; i < drugPolicy3.Count; i++)
			{
				if (drugPolicy3[i].drug.IsPleasureDrug)
				{
					drugPolicy3[i].allowedForJoy = true;
				}
			}
			DrugPolicy drugPolicy4 = this.MakeNewDrugPolicy();
			drugPolicy4.label = "OneDrinkPerDay".Translate();
			drugPolicy4[ThingDefOf.Beer].allowedForJoy = true;
			drugPolicy4[ThingDefOf.Beer].allowScheduled = true;
			drugPolicy4[ThingDefOf.Beer].takeToInventory = 1;
			drugPolicy4[ThingDefOf.Beer].daysFrequency = 1f;
			drugPolicy4[ThingDefOf.SmokeleafJoint].allowedForJoy = true;
		}
	}
}
