using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Verse
{
	public class ThreadLocalDeepProfiler
	{
		private Stack<ThreadLocalDeepProfiler.Watcher> watchers = new Stack<ThreadLocalDeepProfiler.Watcher>();

		private static readonly string[] Prefixes = new string[50];

		private const int MaxDepth = 50;

		static ThreadLocalDeepProfiler()
		{
			for (int i = 0; i < 50; i++)
			{
				ThreadLocalDeepProfiler.Prefixes[i] = "";
				for (int j = 0; j < i; j++)
				{
					string[] prefixes;
					int num;
					(prefixes = ThreadLocalDeepProfiler.Prefixes)[num = i] = prefixes[num] + " -";
				}
			}
		}

		public ThreadLocalDeepProfiler()
		{
		}

		public void Start(string label = null)
		{
			if (Prefs.LogVerbose)
			{
				this.watchers.Push(new ThreadLocalDeepProfiler.Watcher(label));
			}
		}

		public void End()
		{
			if (Prefs.LogVerbose)
			{
				if (this.watchers.Count == 0)
				{
					Log.Error("Ended deep profiling while not profiling.", false);
				}
				else
				{
					ThreadLocalDeepProfiler.Watcher watcher = this.watchers.Pop();
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

		private void Output(ThreadLocalDeepProfiler.Watcher root)
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
			Log.Message(stringBuilder.ToString(), false);
		}

		private void AppendStringRecursive(StringBuilder sb, ThreadLocalDeepProfiler.Watcher w, int depth)
		{
			sb.AppendLine(string.Concat(new object[]
			{
				ThreadLocalDeepProfiler.Prefixes[depth],
				" ",
				w.Watch.ElapsedMilliseconds,
				"ms ",
				w.Label
			}));
			if (w.Children != null)
			{
				for (int i = 0; i < w.Children.Count; i++)
				{
					this.AppendStringRecursive(sb, w.Children[i], depth + 1);
				}
			}
		}

		private class Watcher
		{
			private string label;

			private Stopwatch watch;

			private List<ThreadLocalDeepProfiler.Watcher> children;

			public Watcher(string label)
			{
				this.label = label;
				this.watch = Stopwatch.StartNew();
				this.children = null;
			}

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

			public List<ThreadLocalDeepProfiler.Watcher> Children
			{
				get
				{
					return this.children;
				}
			}

			public void AddChildResult(ThreadLocalDeepProfiler.Watcher w)
			{
				if (this.children == null)
				{
					this.children = new List<ThreadLocalDeepProfiler.Watcher>();
				}
				this.children.Add(w);
			}
		}
	}
}
