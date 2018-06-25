using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E73 RID: 3699
	public sealed class LetterStack : IExposable
	{
		// Token: 0x040039BB RID: 14779
		private List<Letter> letters = new List<Letter>();

		// Token: 0x040039BC RID: 14780
		private int mouseoverLetterIndex = -1;

		// Token: 0x040039BD RID: 14781
		private float lastTopYInt;

		// Token: 0x040039BE RID: 14782
		private const float LettersBottomY = 350f;

		// Token: 0x040039BF RID: 14783
		public const float LetterSpacing = 12f;

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06005713 RID: 22291 RVA: 0x002CD4DC File Offset: 0x002CB8DC
		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06005714 RID: 22292 RVA: 0x002CD4F8 File Offset: 0x002CB8F8
		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		// Token: 0x06005715 RID: 22293 RVA: 0x002CD514 File Offset: 0x002CB914
		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTargets, relatedFaction);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06005716 RID: 22294 RVA: 0x002CD538 File Offset: 0x002CB938
		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06005717 RID: 22295 RVA: 0x002CD558 File Offset: 0x002CB958
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

		// Token: 0x06005718 RID: 22296 RVA: 0x002CD5FA File Offset: 0x002CB9FA
		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		// Token: 0x06005719 RID: 22297 RVA: 0x002CD610 File Offset: 0x002CBA10
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

		// Token: 0x0600571A RID: 22298 RVA: 0x002CD6BC File Offset: 0x002CBABC
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

		// Token: 0x0600571B RID: 22299 RVA: 0x002CD734 File Offset: 0x002CBB34
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

		// Token: 0x0600571C RID: 22300 RVA: 0x002CD7D1 File Offset: 0x002CBBD1
		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x002CD7E8 File Offset: 0x002CBBE8
		public void Notify_MapRemoved(Map map)
		{
			for (int i = 0; i < this.letters.Count; i++)
			{
				this.letters[i].Notify_MapRemoved(map);
			}
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x002CD828 File Offset: 0x002CBC28
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
