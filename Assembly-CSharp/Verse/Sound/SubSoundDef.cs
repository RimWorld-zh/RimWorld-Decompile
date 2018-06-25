using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000BA1 RID: 2977
	public class SubSoundDef : Editable
	{
		// Token: 0x04002B47 RID: 11079
		[Description("A name to help you identify the sound.")]
		[DefaultValue("UnnamedSubSoundDef")]
		public string name = "UnnamedSubSoundDef";

		// Token: 0x04002B48 RID: 11080
		[Description("Whether this sound plays on the camera or in the world.\n\nThis must match what the game expects from the sound Def with this name.")]
		[DefaultValue(false)]
		public bool onCamera = false;

		// Token: 0x04002B49 RID: 11081
		[Description("Whether to mute this subSound while the game is paused (either by the pausing in play or by opening a menu)")]
		[DefaultValue(false)]
		public bool muteWhenPaused = false;

		// Token: 0x04002B4A RID: 11082
		[Description("Whether this subSound's tempo should be affected by the current tick rate.")]
		[DefaultValue(false)]
		public bool tempoAffectedByGameSpeed;

		// Token: 0x04002B4B RID: 11083
		[Description("The sound grains used for this sample. The game will choose one of these randomly when the sound plays. Sustainers choose one for each sample as it begins.")]
		public List<AudioGrain> grains = new List<AudioGrain>();

		// Token: 0x04002B4C RID: 11084
		[EditSliderRange(0f, 100f)]
		[Description("This sound will play at a random volume inside this range.\n\nSustainers will choose a different random volume for each sample.")]
		[DefaultFloatRange(50f, 50f)]
		public FloatRange volumeRange = new FloatRange(50f, 50f);

		// Token: 0x04002B4D RID: 11085
		[EditSliderRange(0.05f, 2f)]
		[Description("This sound will play at a random pitch inside this range.\n\nSustainers will choose a different random pitch for each sample.")]
		[DefaultFloatRange(1f, 1f)]
		public FloatRange pitchRange = FloatRange.One;

		// Token: 0x04002B4E RID: 11086
		[EditSliderRange(0f, 200f)]
		[Description("This sound will play max volume when it is under minDistance from the camera.\n\nIt will fade out linearly until the camera distance reaches its max.")]
		[DefaultFloatRange(25f, 70f)]
		public FloatRange distRange = new FloatRange(25f, 70f);

		// Token: 0x04002B4F RID: 11087
		[Description("When the sound chooses the next grain, you may use this setting to have it avoid repeating the last grain, or avoid repeating any of the grains in the last X played, X being half the total number of grains defined.")]
		[DefaultValue(RepeatSelectMode.NeverLastHalf)]
		public RepeatSelectMode repeatMode = RepeatSelectMode.NeverLastHalf;

		// Token: 0x04002B50 RID: 11088
		[Description("Mappings between game parameters (like fire size or wind speed) and properties of the sound.")]
		[DefaultEmptyList(typeof(SoundParameterMapping))]
		public List<SoundParameterMapping> paramMappings = new List<SoundParameterMapping>();

		// Token: 0x04002B51 RID: 11089
		[Description("The filters to be applied to this sound.")]
		[DefaultEmptyList(typeof(SoundFilter))]
		public List<SoundFilter> filters = new List<SoundFilter>();

		// Token: 0x04002B52 RID: 11090
		[Description("A range of possible times between when this sound is triggered and when it will actually start playing.")]
		[DefaultFloatRange(0f, 0f)]
		public FloatRange startDelayRange = FloatRange.Zero;

		// Token: 0x04002B53 RID: 11091
		[Description("If true, each sample in the sustainer will be looped and ended only after sustainerLoopDurationRange. If not, the sounds will just play once and end after their own length.")]
		[DefaultValue(true)]
		public bool sustainLoop = true;

		// Token: 0x04002B54 RID: 11092
		[EditSliderRange(0f, 10f)]
		[Description("The range of durations that individual looped samples in the sustainer will have. Each sample ends after a time randomly chosen in this range.\n\nOnly used if the sustainer is looped.")]
		[DefaultFloatRange(9999f, 9999f)]
		public FloatRange sustainLoopDurationRange = new FloatRange(9999f, 9999f);

		// Token: 0x04002B55 RID: 11093
		[EditSliderRange(-2f, 2f)]
		[Description("The time between when one sample ends and the next starts.\n\nSet to negative if you wish samples to overlap.")]
		[LoadAlias("sustainInterval")]
		[DefaultFloatRange(0f, 0f)]
		public FloatRange sustainIntervalRange = FloatRange.Zero;

		// Token: 0x04002B56 RID: 11094
		[EditSliderRange(0f, 2f)]
		[Description("The fade-in time of each sample. The sample will start at 0 volume and fade in over this number of seconds.")]
		[DefaultValue(0f)]
		public float sustainAttack = 0f;

		// Token: 0x04002B57 RID: 11095
		[Description("Skip the attack on the first sustainer sample.")]
		[DefaultValue(true)]
		public bool sustainSkipFirstAttack = true;

		// Token: 0x04002B58 RID: 11096
		[EditSliderRange(0f, 2f)]
		[Description("The fade-out time of each sample. At this number of seconds before the sample ends, it will start fading out. Its volume will be zero at the moment it finishes fading out.")]
		[DefaultValue(0f)]
		public float sustainRelease = 0f;

		// Token: 0x04002B59 RID: 11097
		[Unsaved]
		public SoundDef parentDef;

		// Token: 0x04002B5A RID: 11098
		[Unsaved]
		private List<ResolvedGrain> resolvedGrains = new List<ResolvedGrain>();

		// Token: 0x04002B5B RID: 11099
		[Unsaved]
		private ResolvedGrain lastPlayedResolvedGrain = null;

		// Token: 0x04002B5C RID: 11100
		[Unsaved]
		private int numToAvoid = 0;

		// Token: 0x04002B5D RID: 11101
		[Unsaved]
		private int distinctResolvedGrainsCount = 0;

		// Token: 0x04002B5E RID: 11102
		[Unsaved]
		private Queue<ResolvedGrain> recentlyPlayedResolvedGrains = new Queue<ResolvedGrain>();

		// Token: 0x06004068 RID: 16488 RVA: 0x0021D6FC File Offset: 0x0021BAFC
		public virtual void TryPlay(SoundInfo info)
		{
			if (this.resolvedGrains.Count == 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot play ",
					this.parentDef,
					" (subSound ",
					this,
					"_: No resolved grains."
				}), false);
			}
			else if (Find.SoundRoot.oneShotManager.CanAddPlayingOneShot(this.parentDef, info))
			{
				ResolvedGrain resolvedGrain = this.RandomizedResolvedGrain();
				ResolvedGrain_Clip resolvedGrain_Clip = resolvedGrain as ResolvedGrain_Clip;
				if (resolvedGrain_Clip != null)
				{
					if (SampleOneShot.TryMakeAndPlay(this, resolvedGrain_Clip.clip, info) == null)
					{
						return;
					}
					SoundSlotManager.Notify_Played(this.parentDef.slot, resolvedGrain_Clip.clip.length);
				}
				if (this.distinctResolvedGrainsCount > 1)
				{
					if (this.repeatMode == RepeatSelectMode.NeverLastHalf)
					{
						while (this.recentlyPlayedResolvedGrains.Count >= this.numToAvoid)
						{
							this.recentlyPlayedResolvedGrains.Dequeue();
						}
						if (this.recentlyPlayedResolvedGrains.Count < this.numToAvoid)
						{
							this.recentlyPlayedResolvedGrains.Enqueue(resolvedGrain);
						}
					}
					else if (this.repeatMode == RepeatSelectMode.NeverTwice)
					{
						this.lastPlayedResolvedGrain = resolvedGrain;
					}
				}
			}
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0021D840 File Offset: 0x0021BC40
		public ResolvedGrain RandomizedResolvedGrain()
		{
			ResolvedGrain chosenGrain = null;
			for (;;)
			{
				chosenGrain = this.resolvedGrains.RandomElement<ResolvedGrain>();
				if (this.distinctResolvedGrainsCount <= 1)
				{
					break;
				}
				if (this.repeatMode == RepeatSelectMode.NeverLastHalf)
				{
					if (!(from g in this.recentlyPlayedResolvedGrains
					where g.Equals(chosenGrain)
					select g).Any<ResolvedGrain>())
					{
						break;
					}
				}
				else
				{
					if (this.repeatMode != RepeatSelectMode.NeverTwice)
					{
						break;
					}
					if (!chosenGrain.Equals(this.lastPlayedResolvedGrain))
					{
						break;
					}
				}
			}
			return chosenGrain;
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x0021D8F0 File Offset: 0x0021BCF0
		public float RandomizedVolume()
		{
			float randomInRange = this.volumeRange.RandomInRange;
			return randomInRange / 100f;
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x0021D91A File Offset: 0x0021BD1A
		public override void ResolveReferences()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.resolvedGrains.Clear();
				foreach (AudioGrain audioGrain in this.grains)
				{
					foreach (ResolvedGrain item in audioGrain.GetResolvedGrains())
					{
						this.resolvedGrains.Add(item);
					}
				}
				this.distinctResolvedGrainsCount = this.resolvedGrains.Distinct<ResolvedGrain>().Count<ResolvedGrain>();
				this.numToAvoid = Mathf.FloorToInt((float)this.distinctResolvedGrainsCount / 2f);
				if (this.distinctResolvedGrainsCount >= 6)
				{
					this.numToAvoid++;
				}
			});
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x0021D930 File Offset: 0x0021BD30
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.resolvedGrains.Count == 0)
			{
				yield return "No grains resolved.";
			}
			if (this.sustainAttack + this.sustainRelease > this.sustainLoopDurationRange.TrueMin)
			{
				yield return "Attack + release < min loop duration. Sustain samples will cut off.";
			}
			if (this.distRange.min > this.distRange.max)
			{
				yield return "Dist range min/max are reversed.";
			}
			foreach (SoundParameterMapping mapping in this.paramMappings)
			{
				if (mapping.inParam == null || mapping.outParam == null)
				{
					yield return "At least one parameter mapping is missing an in or out parameter.";
					break;
				}
				if (mapping.outParam != null)
				{
					Type neededFilter = mapping.outParam.NeededFilterType;
					if (neededFilter != null)
					{
						if (!(from fil in this.filters
						where fil.GetType() == neededFilter
						select fil).Any<SoundFilter>())
						{
							yield return "A parameter wants to modify the " + neededFilter.ToString() + " filter, but this sound doesn't have it.";
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0021D95C File Offset: 0x0021BD5C
		public override string ToString()
		{
			return this.name;
		}
	}
}
