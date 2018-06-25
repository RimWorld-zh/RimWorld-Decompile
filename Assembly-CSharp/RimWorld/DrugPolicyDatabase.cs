using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F1 RID: 753
	public class DrugPolicyDatabase : IExposable
	{
		// Token: 0x0400082D RID: 2093
		private List<DrugPolicy> policies = new List<DrugPolicy>();

		// Token: 0x06000C6E RID: 3182 RVA: 0x0006E878 File Offset: 0x0006CC78
		public DrugPolicyDatabase()
		{
			this.GenerateStartingDrugPolicies();
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0006E894 File Offset: 0x0006CC94
		public List<DrugPolicy> AllPolicies
		{
			get
			{
				return this.policies;
			}
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0006E8AF File Offset: 0x0006CCAF
		public void ExposeData()
		{
			Scribe_Collections.Look<DrugPolicy>(ref this.policies, "policies", LookMode.Deep, new object[0]);
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0006E8CC File Offset: 0x0006CCCC
		public DrugPolicy DefaultDrugPolicy()
		{
			if (this.policies.Count == 0)
			{
				this.MakeNewDrugPolicy();
			}
			return this.policies[0];
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0006E904 File Offset: 0x0006CD04
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

		// Token: 0x06000C73 RID: 3187 RVA: 0x0006EA20 File Offset: 0x0006CE20
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

		// Token: 0x06000C74 RID: 3188 RVA: 0x0006EAAC File Offset: 0x0006CEAC
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
