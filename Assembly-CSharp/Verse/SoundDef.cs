using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B7C RID: 2940
	public class SoundDef : Def
	{
		// Token: 0x04002AEF RID: 10991
		[Description("If checked, this sound is a sustainer.\n\nSustainers are used for sounds with a defined beginning and end (as opposed to OneShots, which just fire at a given instant).\n\nThis value must match what the game expects from the SubSoundDef with this name.")]
		[DefaultValue(false)]
		public bool sustain = false;

		// Token: 0x04002AF0 RID: 10992
		[Description("When the sound is allowed to play: only when the map view is active, only when the world view is active, or always (map + world + main menu).")]
		[DefaultValue(SoundContext.Any)]
		public SoundContext context = SoundContext.Any;

		// Token: 0x04002AF1 RID: 10993
		[Description("Event names for this sound. \n\nThe code will look up sounds to play them according to their name. If the code finds the event name it wants in this list, it will trigger this sound.\n\nThe Def name is also used as an event name.")]
		public List<string> eventNames = new List<string>();

		// Token: 0x04002AF2 RID: 10994
		[Description("For one-shots, this is the number of individual sounds from this Def than can be playing at a time.\n\n For sustainers, this is the number of sustainers that can be running with this sound (each of which can have sub-sounds). Sustainers can fade in and out as you move the camera or objects move, to keep the nearest ones audible.\n\nThis setting may not work for on-camera sounds.")]
		[DefaultValue(4)]
		public int maxVoices = 4;

		// Token: 0x04002AF3 RID: 10995
		[Description("The number of instances of this sound that can play at almost exactly the same moment. Handles cases like six gunners all firing their identical guns at the same time because a target came into view of all of them at the same time. Ordinarily this would make a painfully loud sound, but you can reduce it with this.")]
		[DefaultValue(3)]
		public int maxSimultaneous = 3;

		// Token: 0x04002AF4 RID: 10996
		[Description("If the system has to not play some instances of this sound because of maxVoices, this determines which ones are ignored.\n\nYou should use PrioritizeNewest for things like gunshots, so older still-playing samples are overridden by newer, more important ones.\n\nSustained sounds should usually prioritize nearest, so if a new fire starts burning nearby it can override a more distant one.")]
		[DefaultValue(VoicePriorityMode.PrioritizeNewest)]
		public VoicePriorityMode priorityMode = VoicePriorityMode.PrioritizeNewest;

		// Token: 0x04002AF5 RID: 10997
		[Description("The special sound slot this sound takes. If a sound with this slot is playing, new sounds in this slot will not play.\n\nOnly works for on-camera sounds.")]
		[DefaultValue("")]
		public string slot = "";

		// Token: 0x04002AF6 RID: 10998
		[LoadAlias("sustainerStartSound")]
		[Description("The name of the SoundDef that will be played when this sustainer starts.")]
		[DefaultValue("")]
		public SoundDef sustainStartSound = null;

		// Token: 0x04002AF7 RID: 10999
		[LoadAlias("sustainerStopSound")]
		[Description("The name of the SoundDef that will be played when this sustainer ends.")]
		[DefaultValue("")]
		public SoundDef sustainStopSound = null;

		// Token: 0x04002AF8 RID: 11000
		[Description("After a sustainer is ended, the sound will fade out over this many real-time seconds.")]
		[DefaultValue(0f)]
		public float sustainFadeoutTime = 0f;

		// Token: 0x04002AF9 RID: 11001
		[Description("All the sounds that will play when this set is triggered.")]
		public List<SubSoundDef> subSounds = new List<SubSoundDef>();

		// Token: 0x04002AFA RID: 11002
		[Unsaved]
		public bool isUndefined = false;

		// Token: 0x04002AFB RID: 11003
		[Unsaved]
		public Sustainer testSustainer = null;

		// Token: 0x04002AFC RID: 11004
		private static Dictionary<string, SoundDef> undefinedSoundDefs = new Dictionary<string, SoundDef>();

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06004010 RID: 16400 RVA: 0x0021BD64 File Offset: 0x0021A164
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

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06004011 RID: 16401 RVA: 0x0021BDB8 File Offset: 0x0021A1B8
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

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06004012 RID: 16402 RVA: 0x0021BE0C File Offset: 0x0021A20C
		public int MaxSimultaneousSamples
		{
			get
			{
				return this.maxSimultaneous * this.subSounds.Count;
			}
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x0021BE34 File Offset: 0x0021A234
		public override void ResolveReferences()
		{
			for (int i = 0; i < this.subSounds.Count; i++)
			{
				this.subSounds[i].parentDef = this;
				this.subSounds[i].ResolveReferences();
			}
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x0021BE84 File Offset: 0x0021A284
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

		// Token: 0x06004015 RID: 16405 RVA: 0x0021BEB0 File Offset: 0x0021A2B0
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

		// Token: 0x06004016 RID: 16406 RVA: 0x0021BFB4 File Offset: 0x0021A3B4
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

		// Token: 0x06004017 RID: 16407 RVA: 0x0021C0B8 File Offset: 0x0021A4B8
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
	}
}
