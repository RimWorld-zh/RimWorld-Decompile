using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B81 RID: 2945
	public class ReverbSetup
	{
		// Token: 0x06004018 RID: 16408 RVA: 0x0021BFB8 File Offset: 0x0021A3B8
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

		// Token: 0x06004019 RID: 16409 RVA: 0x0021C0A4 File Offset: 0x0021A4A4
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

		// Token: 0x0600401A RID: 16410 RVA: 0x0021C15C File Offset: 0x0021A55C
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

		// Token: 0x04002AF8 RID: 11000
		public float dryLevel = 0f;

		// Token: 0x04002AF9 RID: 11001
		public float room = 0f;

		// Token: 0x04002AFA RID: 11002
		public float roomHF = 0f;

		// Token: 0x04002AFB RID: 11003
		public float roomLF = 0f;

		// Token: 0x04002AFC RID: 11004
		public float decayTime = 1f;

		// Token: 0x04002AFD RID: 11005
		public float decayHFRatio = 0.5f;

		// Token: 0x04002AFE RID: 11006
		public float reflectionsLevel = -10000f;

		// Token: 0x04002AFF RID: 11007
		public float reflectionsDelay = 0f;

		// Token: 0x04002B00 RID: 11008
		public float reverbLevel = 0f;

		// Token: 0x04002B01 RID: 11009
		public float reverbDelay = 0.04f;

		// Token: 0x04002B02 RID: 11010
		public float hfReference = 5000f;

		// Token: 0x04002B03 RID: 11011
		public float lfReference = 250f;

		// Token: 0x04002B04 RID: 11012
		public float diffusion = 100f;

		// Token: 0x04002B05 RID: 11013
		public float density = 100f;
	}
}
