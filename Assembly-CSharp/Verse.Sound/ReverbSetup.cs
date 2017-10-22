using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	public class ReverbSetup
	{
		public float dryLevel;

		public float room;

		public float roomHF;

		public float roomLF;

		public float decayTime = 1f;

		public float decayHFRatio = 0.5f;

		public float reflectionsLevel = -10000f;

		public float reflectionsDelay;

		public float reverbLevel;

		public float reverbDelay = 0.04f;

		public float hfReference = 5000f;

		public float lfReference = 250f;

		public float diffusion = 100f;

		public float density = 100f;

		public void DoEditWidgets(WidgetRow widgetRow)
		{
			if (widgetRow.ButtonText("Setup from preset...", "Set up the reverb filter from a preset.", true, false))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (int value in Enum.GetValues(typeof(AudioReverbPreset)))
				{
					if (value != 27)
					{
						AudioReverbPreset localPreset = (AudioReverbPreset)value;
						list.Add(new FloatMenuOption(((Enum)(object)(AudioReverbPreset)value).ToString(), (Action)delegate
						{
							this.SetupAs(localPreset);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		public void ApplyTo(AudioReverbFilter filter)
		{
			filter.dryLevel = this.dryLevel;
			filter.room = this.room;
			filter.roomHF = this.roomHF;
			filter.roomLF = this.roomLF;
			filter.decayTime = this.decayTime;
			filter.decayHFRatio = this.decayHFRatio;
			filter.reflectionsLevel = this.reflectionsLevel;
			filter.reflectionsDelay = this.reflectionsDelay;
			filter.reverbLevel = this.reverbLevel;
			filter.reverbDelay = this.reverbDelay;
			filter.hfReference = this.hfReference;
			filter.lfReference = this.lfReference;
			filter.diffusion = this.diffusion;
			filter.density = this.density;
		}

		public static ReverbSetup Lerp(ReverbSetup A, ReverbSetup B, float t)
		{
			ReverbSetup reverbSetup = new ReverbSetup();
			reverbSetup.dryLevel = Mathf.Lerp(A.dryLevel, B.dryLevel, t);
			reverbSetup.room = Mathf.Lerp(A.room, B.room, t);
			reverbSetup.roomHF = Mathf.Lerp(A.roomHF, B.roomHF, t);
			reverbSetup.roomLF = Mathf.Lerp(A.roomLF, B.roomLF, t);
			reverbSetup.decayTime = Mathf.Lerp(A.decayTime, B.decayTime, t);
			reverbSetup.decayHFRatio = Mathf.Lerp(A.decayHFRatio, B.decayHFRatio, t);
			reverbSetup.reflectionsLevel = Mathf.Lerp(A.reflectionsLevel, B.reflectionsLevel, t);
			reverbSetup.reflectionsDelay = Mathf.Lerp(A.reflectionsDelay, B.reflectionsDelay, t);
			reverbSetup.reverbLevel = Mathf.Lerp(A.reverbLevel, B.reverbLevel, t);
			reverbSetup.reverbDelay = Mathf.Lerp(A.reverbDelay, B.reverbDelay, t);
			reverbSetup.hfReference = Mathf.Lerp(A.hfReference, B.hfReference, t);
			reverbSetup.lfReference = Mathf.Lerp(A.lfReference, B.lfReference, t);
			reverbSetup.diffusion = Mathf.Lerp(A.diffusion, B.diffusion, t);
			reverbSetup.density = Mathf.Lerp(A.density, B.density, t);
			return reverbSetup;
		}
	}
}
