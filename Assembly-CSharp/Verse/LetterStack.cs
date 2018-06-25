using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E74 RID: 3700
	public sealed class LetterStack : IExposable
	{
		// Token: 0x040039C3 RID: 14787
		private List<Letter> letters = new List<Letter>();

		// Token: 0x040039C4 RID: 14788
		private int mouseoverLetterIndex = -1;

		// Token: 0x040039C5 RID: 14789
		private float lastTopYInt;

		// Token: 0x040039C6 RID: 14790
		private const float LettersBottomY = 350f;

		// Token: 0x040039C7 RID: 14791
		public const float LetterSpacing = 12f;

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06005713 RID: 22291 RVA: 0x002CD6C8 File Offset: 0x002CBAC8
		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06005714 RID: 22292 RVA: 0x002CD6E4 File Offset: 0x002CBAE4
		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		// Token: 0x06005715 RID: 22293 RVA: 0x002CD700 File Offset: 0x002CBB00
		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTargets, relatedFaction);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06005716 RID: 22294 RVA: 0x002CD724 File Offset: 0x002CBB24
		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06005717 RID: 22295 RVA: 0x002CD744 File Offset: 0x002CBB44
		public void ReceiveLetter(Letter let, string debugInfo = null)
		{
			if (let.CanShowInLetterStack)
			{
				let.def.arriveSound.PlayOneShotOnCamera(null);
				if (let.def.pauseIfPauseOnUrgentLetter && Prefs.PauseOnUrgentLetter && !Find.TickManager.Paused)
				{
					Find.TickManager.TogglePaused();
				}
				let.arrivalTime = Time.time;
				let.arrivalTick = Find.TickManager.TicksGame;
				let.debugInfo = debugInfo;
				this.letters.Add(let);
				Find.Archive.Add(let);
				let.Received();
			}
		}

		// Token: 0x06005718 RID: 22296 RVA: 0x002CD7E6 File Offset: 0x002CBBE6
		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		// Token: 0x06005719 RID: 22297 RVA: 0x002CD7FC File Offset: 0x002CBBFC
		public void LettersOnGUI(float baseY)
		{
			float num = baseY - 30f;
			for (int i = this.letters.Count - 1; i >= 0; i--)
			{
				this.letters[i].DrawButtonAt(num);
				num -= 42f;
			}
			this.lastTopYInt = num;
			if (Event.current.type == EventType.Repaint)
			{
				num = baseY - 30f;
				for (int j = this.letters.Count - 1; j >= 0; j--)
				{
					this.letters[j].CheckForMouseOverTextAt(num);
					num -= 42f;
				}
			}
		}

		// Token: 0x0600571A RID: 22298 RVA: 0x002CD8A8 File Offset: 0x002CBCA8
		public void LetterStackTick()
		{
			int num = Find.TickManager.TicksGame + 1;
			for (int i = 0; i < this.letters.Count; i++)
			{
				LetterWithTimeout letterWithTimeout = this.letters[i] as LetterWithTimeout;
				if (letterWithTimeout != null && letterWithTimeout.TimeoutActive && letterWithTimeout.disappearAtTick == num)
				{
					letterWithTimeout.OpenLetter();
					break;
				}
			}
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x002CD920 File Offset: 0x002CBD20
		public void LetterStackUpdate()
		{
			if (this.mouseoverLetterIndex >= 0 && this.mouseoverLetterIndex < this.letters.Count)
			{
				this.letters[this.mouseoverLetterIndex].lookTargets.TryHighlight(true, true, false);
			}
			this.mouseoverLetterIndex = -1;
			for (int i = this.letters.Count - 1; i >= 0; i--)
			{
				if (!this.letters[i].CanShowInLetterStack)
				{
					this.RemoveLetter(this.letters[i]);
				}
			}
		}

		// Token: 0x0600571C RID: 22300 RVA: 0x002CD9BD File Offset: 0x002CBDBD
		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x002CD9D4 File Offset: 0x002CBDD4
		public void Notify_MapRemoved(Map map)
		{
			for (int i = 0; i < this.letters.Count; i++)
			{
				this.letters[i].Notify_MapRemoved(map);
			}
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x002CDA14 File Offset: 0x002CBE14
		public void ExposeData()
		{
			Scribe_Collections.Look<Letter>(ref this.letters, "letters", LookMode.Reference, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.letters.RemoveAll((Letter x) => x == null);
			}
		}
	}
}
