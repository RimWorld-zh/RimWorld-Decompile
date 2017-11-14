using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public sealed class LetterStack : IExposable
	{
		private List<Letter> letters = new List<Letter>();

		private int mouseoverLetterIndex = -1;

		private float lastTopYInt;

		private const float LettersBottomY = 350f;

		public const float LetterSpacing = 12f;

		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, GlobalTargetInfo lookTarget, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTarget);
			this.ReceiveLetter(let, debugInfo);
		}

		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef);
			this.ReceiveLetter(let, debugInfo);
		}

		public void ReceiveLetter(Letter let, string debugInfo = null)
		{
			let.def.arriveSound.PlayOneShotOnCamera(null);
			if (let.def.pauseIfPauseOnUrgentLetter && Prefs.PauseOnUrgentLetter && !Find.TickManager.Paused)
			{
				Find.TickManager.TogglePaused();
			}
			this.letters.Add(let);
			let.arrivalTime = Time.time;
			let.debugInfo = debugInfo;
			let.Received();
		}

		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		public void LettersOnGUI(float baseY)
		{
			float num = (float)(baseY - 30.0);
			for (int num2 = this.letters.Count - 1; num2 >= 0; num2--)
			{
				this.letters[num2].DrawButtonAt(num);
				num = (float)(num - 42.0);
			}
			this.lastTopYInt = num;
			num = (float)(baseY - 30.0);
			for (int num3 = this.letters.Count - 1; num3 >= 0; num3--)
			{
				this.letters[num3].CheckForMouseOverTextAt(num);
				num = (float)(num - 42.0);
			}
		}

		public void LetterStackTick()
		{
			int num = Find.TickManager.TicksGame + 1;
			int num2 = 0;
			LetterWithTimeout letterWithTimeout;
			while (true)
			{
				if (num2 < this.letters.Count)
				{
					letterWithTimeout = (this.letters[num2] as LetterWithTimeout);
					if (letterWithTimeout != null && letterWithTimeout.TimeoutActive && letterWithTimeout.disappearAtTick == num)
						break;
					num2++;
					continue;
				}
				return;
			}
			letterWithTimeout.OpenLetter();
		}

		public void LetterStackUpdate()
		{
			if (this.mouseoverLetterIndex >= 0 && this.mouseoverLetterIndex < this.letters.Count)
			{
				GlobalTargetInfo lookTarget = this.letters[this.mouseoverLetterIndex].lookTarget;
				if (lookTarget.IsValid && lookTarget.IsMapTarget && lookTarget.Map == Find.VisibleMap)
				{
					GenDraw.DrawArrowPointingAt(((TargetInfo)lookTarget).CenterVector3, false);
				}
			}
			this.mouseoverLetterIndex = -1;
			for (int num = this.letters.Count - 1; num >= 0; num--)
			{
				if (!this.letters[num].StillValid)
				{
					this.RemoveLetter(this.letters[num]);
				}
			}
		}

		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Letter>(ref this.letters, "letters", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.letters.RemoveAll((Letter x) => x == null) != 0)
			{
				Log.Error("Some letters were null.");
			}
		}
	}
}
