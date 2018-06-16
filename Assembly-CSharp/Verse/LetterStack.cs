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
		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x060056F1 RID: 22257 RVA: 0x002CB7A0 File Offset: 0x002C9BA0
		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x060056F2 RID: 22258 RVA: 0x002CB7BC File Offset: 0x002C9BBC
		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		// Token: 0x060056F3 RID: 22259 RVA: 0x002CB7D8 File Offset: 0x002C9BD8
		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTargets, relatedFaction);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x002CB7FC File Offset: 0x002C9BFC
		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x060056F5 RID: 22261 RVA: 0x002CB81C File Offset: 0x002C9C1C
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

		// Token: 0x060056F6 RID: 22262 RVA: 0x002CB8BE File Offset: 0x002C9CBE
		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		// Token: 0x060056F7 RID: 22263 RVA: 0x002CB8D4 File Offset: 0x002C9CD4
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

		// Token: 0x060056F8 RID: 22264 RVA: 0x002CB980 File Offset: 0x002C9D80
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

		// Token: 0x060056F9 RID: 22265 RVA: 0x002CB9F8 File Offset: 0x002C9DF8
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

		// Token: 0x060056FA RID: 22266 RVA: 0x002CBA95 File Offset: 0x002C9E95
		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		// Token: 0x060056FB RID: 22267 RVA: 0x002CBAAC File Offset: 0x002C9EAC
		public void Notify_MapRemoved(Map map)
		{
			for (int i = 0; i < this.letters.Count; i++)
			{
				this.letters[i].Notify_MapRemoved(map);
			}
		}

		// Token: 0x060056FC RID: 22268 RVA: 0x002CBAEC File Offset: 0x002C9EEC
		public void ExposeData()
		{
			Scribe_Collections.Look<Letter>(ref this.letters, "letters", LookMode.Reference, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.letters.RemoveAll((Letter x) => x == null);
			}
		}

		// Token: 0x040039AD RID: 14765
		private List<Letter> letters = new List<Letter>();

		// Token: 0x040039AE RID: 14766
		private int mouseoverLetterIndex = -1;

		// Token: 0x040039AF RID: 14767
		private float lastTopYInt;

		// Token: 0x040039B0 RID: 14768
		private const float LettersBottomY = 350f;

		// Token: 0x040039B1 RID: 14769
		public const float LetterSpacing = 12f;
	}
}
