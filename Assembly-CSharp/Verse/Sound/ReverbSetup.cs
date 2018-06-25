using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B80 RID: 2944
	public class ReverbSetup
	{
		// Token: 0x04002B04 RID: 11012
		public float dryLevel = 0f;

		// Token: 0x04002B05 RID: 11013
		public float room = 0f;

		// Token: 0x04002B06 RID: 11014
		public float roomHF = 0f;

		// Token: 0x04002B07 RID: 11015
		public float roomLF = 0f;

		// Token: 0x04002B08 RID: 11016
		public float decayTime = 1f;

		// Token: 0x04002B09 RID: 11017
		public float decayHFRatio = 0.5f;

		// Token: 0x04002B0A RID: 11018
		public float reflectionsLevel = -10000f;

		// Token: 0x04002B0B RID: 11019
		public float reflectionsDelay = 0f;

		// Token: 0x04002B0C RID: 11020
		public float reverbLevel = 0f;

		// Token: 0x04002B0D RID: 11021
		public float reverbDelay = 0.04f;

		// Token: 0x04002B0E RID: 11022
		public float hfReference = 5000f;

		// Token: 0x04002B0F RID: 11023
		public float lfReference = 250f;

		// Token: 0x04002B10 RID: 11024
		public float diffusion = 100f;

		// Token: 0x04002B11 RID: 11025
		public float density = 100f;

		// Token: 0x0600401D RID: 16413 RVA: 0x0021CA10 File Offset: 0x0021AE10
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			if (widgetRow.ButtonText("Setup from preset...", "Set up the reverb filter from a preset.", true, false))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				IEnumerator enumerator = Enum.GetValues(typeof(AudioReverbPreset)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						AudioReverbPreset audioReverbPreset = (AudioReverbPreset)obj;
						if (audioReverbPreset != AudioReverbPreset.User)
						{
							AudioReverbPreset localPreset = audioReverbPreset;
							list.Add(new FloatMenuOption(audioReverbPreset.ToString(), delegate()
							{
								this.SetupAs(localPreset);
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x0021CAFC File Offset: 0x0021AEFC
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

		// Token: 0x0600401F RID: 16415 RVA: 0x0021CBB4 File Offset: 0x0021AFB4
		public static ReverbSetup Lerp(ReverbSetup A, ReverbSetup B, float t)
		{
			return new ReverbSetup
			{
				dryLevel = Mathf.Lerp(A.dryLevel, B.dryLevel, t),
				room = Mathf.Lerp(A.room, B.room, t),
				roomHF = Mathf.Lerp(A.roomHF, B.roomHF, t),
				roomLF = Mathf.Lerp(A.roomLF, B.roomLF, t),
				decayTime = Mathf.Lerp(A.decayTime, B.decayTime, t),
				decayHFRatio = Mathf.Lerp(A.decayHFRatio, B.decayHFRatio, t),
				reflectionsLevel = Mathf.Lerp(A.reflectionsLevel, B.reflectionsLevel, t),
				reflectionsDelay = Mathf.Lerp(A.reflectionsDelay, B.reflectionsDelay, t),
				reverbLevel = Mathf.Lerp(A.reverbLevel, B.reverbLevel, t),
				reverbDelay = Mathf.Lerp(A.reverbDelay, B.reverbDelay, t),
				hfReference = Mathf.Lerp(A.hfReference, B.hfReference, t),
				lfReference = Mathf.Lerp(A.lfReference, B.lfReference, t),
				diffusion = Mathf.Lerp(A.diffusion, B.diffusion, t),
				density = Mathf.Lerp(A.density, B.density, t)
			};
		}
	}
}
