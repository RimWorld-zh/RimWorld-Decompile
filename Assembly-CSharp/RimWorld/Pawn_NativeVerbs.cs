using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000514 RID: 1300
	public class Pawn_NativeVerbs : IExposable
	{
		// Token: 0x04000DEA RID: 3562
		private Pawn pawn;

		// Token: 0x04000DEB RID: 3563
		private Verb_BeatFire beatFireVerb;

		// Token: 0x04000DEC RID: 3564
		private Verb_Ignite igniteVerb;

		// Token: 0x0600178E RID: 6030 RVA: 0x000CE67F File Offset: 0x000CCA7F
		public Pawn_NativeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x0600178F RID: 6031 RVA: 0x000CE690 File Offset: 0x000CCA90
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
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x000CE6BC File Offset: 0x000CCABC
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

		// Token: 0x06001791 RID: 6033 RVA: 0x000CE6E8 File Offset: 0x000CCAE8
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

		// Token: 0x06001792 RID: 6034 RVA: 0x000CE718 File Offset: 0x000CCB18
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

		// Token: 0x06001793 RID: 6035 RVA: 0x000CE7A4 File Offset: 0x000CCBA4
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

		// Token: 0x06001794 RID: 6036 RVA: 0x000CE82E File Offset: 0x000CCC2E
		public void ExposeData()
		{
			Scribe_Deep.Look<Verb_BeatFire>(ref this.beatFireVerb, "beatFireVerb", new object[0]);
			Scribe_Deep.Look<Verb_Ignite>(ref this.igniteVerb, "igniteVerb", new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateVerbsLinksAndProps();
			}
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x000CE870 File Offset: 0x000CCC70
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

		// Token: 0x06001796 RID: 6038 RVA: 0x000CE8D0 File Offset: 0x000CCCD0
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
