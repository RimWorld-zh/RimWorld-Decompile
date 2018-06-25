using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000516 RID: 1302
	public class Pawn_NativeVerbs : IExposable
	{
		// Token: 0x04000DEA RID: 3562
		private Pawn pawn;

		// Token: 0x04000DEB RID: 3563
		private Verb_BeatFire beatFireVerb;

		// Token: 0x04000DEC RID: 3564
		private Verb_Ignite igniteVerb;

		// Token: 0x06001792 RID: 6034 RVA: 0x000CE7CF File Offset: 0x000CCBCF
		public Pawn_NativeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06001793 RID: 6035 RVA: 0x000CE7E0 File Offset: 0x000CCBE0
		public Verb_BeatFire BeatFireVerb
		{
			get
			{
				if (this.beatFireVerb == null)
				{
					this.CreateVerbs();
				}
				return this.beatFireVerb;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06001794 RID: 6036 RVA: 0x000CE80C File Offset: 0x000CCC0C
		public Verb_Ignite IgniteVerb
		{
			get
			{
				if (this.igniteVerb == null)
				{
					this.CreateVerbs();
				}
				return this.igniteVerb;
			}
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x000CE838 File Offset: 0x000CCC38
		public void NativeVerbsTick()
		{
			if (this.BeatFireVerb != null)
			{
				this.BeatFireVerb.VerbTick();
			}
			if (this.IgniteVerb != null)
			{
				this.IgniteVerb.VerbTick();
			}
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x000CE868 File Offset: 0x000CCC68
		public bool TryStartIgnite(Thing target)
		{
			bool result;
			if (this.IgniteVerb == null)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					this.pawn,
					" tried to ignite ",
					target,
					" but has no ignite verb."
				}), 76453432, false);
				result = false;
			}
			else
			{
				result = (!this.pawn.stances.FullBodyBusy && this.IgniteVerb.TryStartCastOn(target, false, true));
			}
			return result;
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x000CE8F4 File Offset: 0x000CCCF4
		public bool TryBeatFire(Fire targetFire)
		{
			bool result;
			if (this.BeatFireVerb == null)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					this.pawn,
					" tried to beat fire ",
					targetFire,
					" but has no beat fire verb."
				}), 935137531, false);
				result = false;
			}
			else
			{
				result = (!this.pawn.stances.FullBodyBusy && this.BeatFireVerb.TryStartCastOn(targetFire, false, true));
			}
			return result;
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x000CE97E File Offset: 0x000CCD7E
		public void ExposeData()
		{
			Scribe_Deep.Look<Verb_BeatFire>(ref this.beatFireVerb, "beatFireVerb", new object[0]);
			Scribe_Deep.Look<Verb_Ignite>(ref this.igniteVerb, "igniteVerb", new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateVerbsLinksAndProps();
			}
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x000CE9C0 File Offset: 0x000CCDC0
		private void CreateVerbs()
		{
			if (this.pawn.RaceProps.intelligence >= Intelligence.ToolUser)
			{
				UniqueIDsManager uniqueIDsManager = Find.UniqueIDsManager;
				this.beatFireVerb = new Verb_BeatFire();
				if (!this.pawn.RaceProps.IsMechanoid)
				{
					this.igniteVerb = new Verb_Ignite();
				}
				this.UpdateVerbsLinksAndProps();
			}
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x000CEA20 File Offset: 0x000CCE20
		private void UpdateVerbsLinksAndProps()
		{
			if (this.beatFireVerb != null)
			{
				this.beatFireVerb.caster = this.pawn;
				this.beatFireVerb.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire);
				this.beatFireVerb.loadID = VerbUtility.GenerateBeatFireLoadId(this.pawn);
			}
			if (this.igniteVerb != null)
			{
				this.igniteVerb.caster = this.pawn;
				this.igniteVerb.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite);
				this.igniteVerb.loadID = VerbUtility.GenerateIgniteLoadId(this.pawn);
			}
		}
	}
}
