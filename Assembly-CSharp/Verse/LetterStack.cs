using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E71 RID: 3697
	public sealed class LetterStack : IExposable
	{
		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x0600570F RID: 22287 RVA: 0x002CD3B0 File Offset: 0x002CB7B0
		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06005710 RID: 22288 RVA: 0x002CD3CC File Offset: 0x002CB7CC
		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		// Token: 0x06005711 RID: 22289 RVA: 0x002CD3E8 File Offset: 0x002CB7E8
		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTargets, relatedFaction);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06005712 RID: 22290 RVA: 0x002CD40C File Offset: 0x002CB80C
		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06005713 RID: 22291 RVA: 0x002CD42C File Offset: 0x002CB82C
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

		// Token: 0x06005714 RID: 22292 RVA: 0x002CD4CE File Offset: 0x002CB8CE
		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		// Token: 0x06005715 RID: 22293 RVA: 0x002CD4E4 File Offset: 0x002CB8E4
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

		// Token: 0x06005716 RID: 22294 RVA: 0x002CD590 File Offset: 0x002CB990
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

		// Token: 0x06005717 RID: 22295 RVA: 0x002CD608 File Offset: 0x002CBA08
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

		// Token: 0x06005718 RID: 22296 RVA: 0x002CD6A5 File Offset: 0x002CBAA5
		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		// Token: 0x06005719 RID: 22297 RVA: 0x002CD6BC File Offset: 0x002CBABC
		public void Notify_MapRemoved(Map map)
		{
			for (int i = 0; i < this.letters.Count; i++)
			{
				this.letters[i].Notify_MapRemoved(map);
			}
		}

		// Token: 0x0600571A RID: 22298 RVA: 0x002CD6FC File Offset: 0x002CBAFC
		public void ExposeData()
		{
			Scribe_Collections.Look<Letter>(ref this.letters, "letters", LookMode.Reference, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.letters.RemoveAll((Letter x) => x == null);
			}
		}

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
	}
}
