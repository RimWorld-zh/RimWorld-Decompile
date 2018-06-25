using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	public class SoundDef : Def
	{
		[DefaultValue(false)]
		[Description("If checked, this sound is a sustainer.\n\nSustainers are used for sounds with a defined beginning and end (as opposed to OneShots, which just fire at a given instant).\n\nThis value must match what the game expects from the SubSoundDef with this name.")]
		public bool sustain = false;

		[DefaultValue(SoundContext.Any)]
		[Description("When the sound is allowed to play: only when the map view is active, only when the world view is active, or always (map + world + main menu).")]
		public SoundContext context = SoundContext.Any;

		[Description("Event names for this sound. \n\nThe code will look up sounds to play them according to their name. If the code finds the event name it wants in this list, it will trigger this sound.\n\nThe Def name is also used as an event name.")]
		public List<string> eventNames = new List<string>();

		[DefaultValue(4)]
		[Description("For one-shots, this is the number of individual sounds from this Def than can be playing at a time.\n\n For sustainers, this is the number of sustainers that can be running with this sound (each of which can have sub-sounds). Sustainers can fade in and out as you move the camera or objects move, to keep the nearest ones audible.\n\nThis setting may not work for on-camera sounds.")]
		public int maxVoices = 4;

		[DefaultValue(3)]
		[Description("The number of instances of this sound that can play at almost exactly the same moment. Handles cases like six gunners all firing their identical guns at the same time because a target came into view of all of them at the same time. Ordinarily this would make a painfully loud sound, but you can reduce it with this.")]
		public int maxSimultaneous = 3;

		[DefaultValue(VoicePriorityMode.PrioritizeNewest)]
		[Description("If the system has to not play some instances of this sound because of maxVoices, this determines which ones are ignored.\n\nYou should use PrioritizeNewest for things like gunshots, so older still-playing samples are overridden by newer, more important ones.\n\nSustained sounds should usually prioritize nearest, so if a new fire starts burning nearby it can override a more distant one.")]
		public VoicePriorityMode priorityMode = VoicePriorityMode.PrioritizeNewest;

		[DefaultValue("")]
		[Description("The special sound slot this sound takes. If a sound with this slot is playing, new sounds in this slot will not play.\n\nOnly works for on-camera sounds.")]
		public string slot = "";

		[DefaultValue("")]
		[Description("The name of the SoundDef that will be played when this sustainer starts.")]
		[LoadAlias("sustainerStartSound")]
		public SoundDef sustainStartSound = null;

		[DefaultValue("")]
		[Description("The name of the SoundDef that will be played when this sustainer ends.")]
		[LoadAlias("sustainerStopSound")]
		public SoundDef sustainStopSound = null;

		[DefaultValue(0f)]
		[Description("After a sustainer is ended, the sound will fade out over this many real-time seconds.")]
		public float sustainFadeoutTime = 0f;

		[Description("All the sounds that will play when this set is triggered.")]
		public List<SubSoundDef> subSounds = new List<SubSoundDef>();

		[Unsaved]
		public bool isUndefined = false;

		[Unsaved]
		public Sustainer testSustainer = null;

		private static Dictionary<string, SoundDef> undefinedSoundDefs = new Dictionary<string, SoundDef>();

		public SoundDef()
		{
		}

		private bool HasSubSoundsOnCamera
		{
			get
			{
				for (int i = 0; i < this.subSounds.Count; i++)
				{
					if (this.subSounds[i].onCamera)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool HasSubSoundsInWorld
		{
			get
			{
				for (int i = 0; i < this.subSounds.Count; i++)
				{
					if (!this.subSounds[i].onCamera)
					{
						return true;
					}
				}
				return false;
			}
		}

		public int MaxSimultaneousSamples
		{
			get
			{
				return this.maxSimultaneous * this.subSounds.Count;
			}
		}

		public override void ResolveReferences()
		{
			for (int i = 0; i < this.subSounds.Count; i++)
			{
				this.subSounds[i].parentDef = this;
				this.subSounds[i].ResolveReferences();
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			if (this.slot != "" && !this.HasSubSoundsOnCamera)
			{
				yield return "Sound slots only work for on-camera sounds.";
			}
			if (this.HasSubSoundsInWorld && this.context != SoundContext.MapOnly)
			{
				yield return "Sounds with non-on-camera subsounds should use MapOnly context.";
			}
			if (this.priorityMode == VoicePriorityMode.PrioritizeNewest && this.sustain)
			{
				yield return "PrioritizeNewest is not supported with sustainers.";
			}
			if (this.maxVoices < 1)
			{
				yield return "Max voices is less than 1.";
			}
			if (!this.sustain && (this.sustainStartSound != null || this.sustainStopSound != null))
			{
				yield return "Sustainer start and end sounds only work with sounds defined as sustainers.";
			}
			if (!this.sustain)
			{
				for (int i = 0; i < this.subSounds.Count; i++)
				{
					if (this.subSounds[i].startDelayRange.TrueMax > 0.001f)
					{
						yield return "startDelayRange is only supported on sustainers.";
					}
				}
			}
			List<SoundDef> defs = DefDatabase<SoundDef>.AllDefsListForReading;
			for (int j = 0; j < defs.Count; j++)
			{
				if (!defs[j].eventNames.NullOrEmpty<string>())
				{
					for (int k = 0; k < defs[j].eventNames.Count; k++)
					{
						if (defs[j].eventNames[k] == this.defName)
						{
							yield return this.defName + " is also defined in the eventNames list for " + defs[j];
						}
					}
				}
			}
			yield break;
		}

		public void DoEditWidgets(WidgetRow widgetRow)
		{
			if (this.testSustainer == null)
			{
				if (widgetRow.ButtonIcon(TexButton.Play, null, null))
				{
					this.ResolveReferences();
					SoundInfo info;
					if (this.HasSubSoundsInWorld)
					{
						IntVec3 mapPosition = Find.CameraDriver.MapPosition;
						info = SoundInfo.InMap(new TargetInfo(mapPosition, Find.CurrentMap, false), MaintenanceType.PerFrame);
						for (int i = 0; i < 5; i++)
						{
							MoteMaker.ThrowDustPuff(mapPosition, Find.CurrentMap, 1.5f);
						}
					}
					else
					{
						info = SoundInfo.OnCamera(MaintenanceType.PerFrame);
					}
					info.testPlay = true;
					if (this.sustain)
					{
						this.testSustainer = this.TrySpawnSustainer(info);
					}
					else
					{
						this.PlayOneShot(info);
					}
				}
			}
			else
			{
				this.testSustainer.Maintain();
				if (widgetRow.ButtonIcon(TexButton.Stop, null, null))
				{
					this.testSustainer.End();
					this.testSustainer = null;
				}
			}
		}

		public static SoundDef Named(string defName)
		{
			SoundDef namedSilentFail = DefDatabase<SoundDef>.GetNamedSilentFail(defName);
			SoundDef result;
			if (namedSilentFail != null)
			{
				result = namedSilentFail;
			}
			else
			{
				if (!Prefs.DevMode)
				{
					if (SoundDef.undefinedSoundDefs.ContainsKey(defName))
					{
						return SoundDef.UndefinedDefNamed(defName);
					}
				}
				List<SoundDef> allDefsListForReading = DefDatabase<SoundDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].eventNames.Count > 0)
					{
						for (int j = 0; j < allDefsListForReading[i].eventNames.Count; j++)
						{
							if (allDefsListForReading[i].eventNames[j] == defName)
							{
								return allDefsListForReading[i];
							}
						}
					}
				}
				if (DefDatabase<SoundDef>.DefCount == 0)
				{
					Log.Warning("Tried to get SoundDef named " + defName + ", but sound defs aren't loaded yet (is it a static variable initialized before play data?).", false);
				}
				result = SoundDef.UndefinedDefNamed(defName);
			}
			return result;
		}

		private static SoundDef UndefinedDefNamed(string defName)
		{
			SoundDef soundDef;
			if (!SoundDef.undefinedSoundDefs.TryGetValue(defName, out soundDef))
			{
				soundDef = new SoundDef();
				soundDef.isUndefined = true;
				soundDef.defName = defName;
				SoundDef.undefinedSoundDefs.Add(defName, soundDef);
			}
			return soundDef;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SoundDef()
		{
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal int <i>__1;

			internal List<SoundDef> <defs>__0;

			internal int <i>__2;

			internal int <j>__3;

			internal SoundDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.slot != "" && !base.HasSubSoundsOnCamera)
					{
						this.$current = "Sound slots only work for on-camera sounds.";
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_C3;
				case 3u:
					goto IL_102;
				case 4u:
					goto IL_132;
				case 5u:
					goto IL_181;
				case 6u:
					goto IL_1EA;
				case 7u:
					IL_2D1:
					k++;
					goto IL_2E0;
				default:
					return false;
				}
				if (base.HasSubSoundsInWorld && this.context != SoundContext.MapOnly)
				{
					this.$current = "Sounds with non-on-camera subsounds should use MapOnly context.";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_C3:
				if (this.priorityMode == VoicePriorityMode.PrioritizeNewest && this.sustain)
				{
					this.$current = "PrioritizeNewest is not supported with sustainers.";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_102:
				if (this.maxVoices < 1)
				{
					this.$current = "Max voices is less than 1.";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_132:
				if (!this.sustain && (this.sustainStartSound != null || this.sustainStopSound != null))
				{
					this.$current = "Sustainer start and end sounds only work with sounds defined as sustainers.";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_181:
				if (!this.sustain)
				{
					i = 0;
					goto IL_1F9;
				}
				goto IL_215;
				IL_1EA:
				i++;
				IL_1F9:
				if (i < this.subSounds.Count)
				{
					if (this.subSounds[i].startDelayRange.TrueMax > 0.001f)
					{
						this.$current = "startDelayRange is only supported on sustainers.";
						if (!this.$disposing)
						{
							this.$PC = 6;
						}
						return true;
					}
					goto IL_1EA;
				}
				IL_215:
				defs = DefDatabase<SoundDef>.AllDefsListForReading;
				j = 0;
				goto IL_316;
				IL_2E0:
				if (k < defs[j].eventNames.Count)
				{
					if (defs[j].eventNames[k] == this.defName)
					{
						this.$current = this.defName + " is also defined in the eventNames list for " + defs[j];
						if (!this.$disposing)
						{
							this.$PC = 7;
						}
						return true;
					}
					goto IL_2D1;
				}
				IL_307:
				j++;
				IL_316:
				if (j >= defs.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (!defs[j].eventNames.NullOrEmpty<string>())
					{
						k = 0;
						goto IL_2E0;
					}
					goto IL_307;
				}
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				SoundDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new SoundDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
