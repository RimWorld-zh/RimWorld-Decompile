using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Verse
{
	public class ThreadLocalDeepProfiler
	{
		private class Watcher
		{
			private string label;

			private Stopwatch watch;

			private List<Watcher> children;

			public string Label
			{
				get
				{
					return this.label;
				}
			}

			public Stopwatch Watch
			{
				get
				{
					return this.watch;
				}
			}

			public List<Watcher> Children
			{
				get
				{
					return this.children;
				}
			}

			public Watcher(string label)
			{
				this.label = label;
				this.watch = Stopwatch.StartNew();
				this.children = null;
			}

			public void AddChildResult(Watcher w)
			{
				if (this.children == null)
				{
					this.children = new List<Watcher>();
				}
				this.children.Add(w);
			}
		}

		private Stack<Watcher> watchers = new Stack<Watcher>();

		private static readonly string[] Prefixes;

		private const int MaxDepth = 50;

		static ThreadLocalDeepProfiler()
		{
			ThreadLocalDeepProfiler.Prefixes = new string[50];
			for (int i = 0; i < 50; i++)
			{
				ThreadLocalDeepProfiler.Prefixes[i] = string.Empty;
				for (int j = 0; j < i; j++)
				{
					string[] prefixes;
					int num;
					(prefixes = ThreadLocalDeepProfiler.Prefixes)[num = i] = prefixes[num] + " -";
				}
			}
		}

		public void Start(string label = null)
		{
			if (Prefs.LogVerbose)
			{
				this.watchers.Push(new Watcher(label));
			}
		}

		public void End()
		{
			if (Prefs.LogVerbose)
			{
				if (this.watchers.Count == 0)
				{
					Log.Error("Ended deep profiling while not profiling.");
				}
				else
				{
					Watcher watcher = this.watchers.Pop();
					watcher.Watch.Stop();
					if (this.watchers.Count > 0)
					{
						this.watchers.Peek().AddChildResult(watcher);
					}
					else
					{
						this.Output(watcher);
					}
				}
			}
		}

		private void Output(Watcher root)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (UnityData.IsInMainThread)
			{
				stringBuilder.AppendLine("--- Main thread ---");
			}
			else
			{
				stringBuilder.AppendLine("--- Thread " + Thread.CurrentThread.ManagedThreadId + " ---");
			}
			this.AppendStringRecursive(stringBuilder, root, 0);
			Log.Message(stringBuilder.ToString());
		}

		private void AppendStringRecursive(StringBuilder sb, Watcher w, int depth)
		{
			sb.AppendLine(ThreadLocalDeepProfiler.Prefixes[depth] + " " + w.Watch.ElapsedMilliseconds + "ms " + w.Label);
			if (w.Children != null)
			{
				for (int i = 0; i < w.Children.Count; i++)
				{
					this.AppendStringRecursive(sb, w.Children[i], depth + 1);
				}
			}
		}
	}
}
