using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class DrugPolicyDatabase : IExposable
	{
		private List<DrugPolicy> policies = new List<DrugPolicy>();

		[CompilerGenerated]
		private static Func<DrugPolicy, int> <>f__am$cache0;

		public DrugPolicyDatabase()
		{
			this.GenerateStartingDrugPolicies();
		}

		public List<DrugPolicy> AllPolicies
		{
			get
			{
				return this.policies;
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<DrugPolicy>(ref this.policies, "policies", LookMode.Deep, new object[0]);
		}

		public DrugPolicy DefaultDrugPolicy()
		{
			if (this.policies.Count == 0)
			{
				this.MakeNewDrugPolicy();
			}
			return this.policies[0];
		}

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

		[CompilerGenerated]
		private static int <MakeNewDrugPolicy>m__0(DrugPolicy o)
		{
			return o.uniqueId;
		}
	}
}
