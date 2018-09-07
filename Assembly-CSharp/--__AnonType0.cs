using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[CompilerGenerated]
internal sealed class <>__AnonType0<<Parent>__T, <Tool>__T>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly <Parent>__T <Parent>;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly <Tool>__T <Tool>;

	[DebuggerHidden]
	public <>__AnonType0(<Parent>__T Parent, <Tool>__T Tool)
	{
		this.<Parent> = Parent;
		this.<Tool> = Tool;
	}

	public <Parent>__T Parent
	{
		get
		{
			return this.<Parent>;
		}
	}

	public <Tool>__T Tool
	{
		get
		{
			return this.<Tool>;
		}
	}

	[DebuggerHidden]
	public override bool Equals(object obj)
	{
		var <>__AnonType = obj as <>__AnonType0<<Parent>__T, <Tool>__T>;
		return <>__AnonType != null && EqualityComparer<<Parent>__T>.Default.Equals(this.<Parent>, <>__AnonType.<Parent>) && EqualityComparer<<Tool>__T>.Default.Equals(this.<Tool>, <>__AnonType.<Tool>);
	}

	[DebuggerHidden]
	public override int GetHashCode()
	{
		int num = ((-2128831035 ^ EqualityComparer<<Parent>__T>.Default.GetHashCode(this.<Parent>)) * 16777619 ^ EqualityComparer<<Tool>__T>.Default.GetHashCode(this.<Tool>)) * 16777619;
		num += num << 13;
		num ^= num >> 7;
		num += num << 3;
		num ^= num >> 17;
		return num + (num << 5);
	}

	[DebuggerHidden]
	public override string ToString()
	{
		string[] array = new string[6];
		array[0] = "{";
		array[1] = " Parent = ";
		int num = 2;
		string text;
		if (this.<Parent> != null)
		{
			<Parent>__T <Parent>__T = this.<Parent>;
			text = <Parent>__T.ToString();
		}
		else
		{
			text = string.Empty;
		}
		array[num] = text;
		array[3] = ", Tool = ";
		int num2 = 4;
		string text2;
		if (this.<Tool> != null)
		{
			<Tool>__T <Tool>__T = this.<Tool>;
			text2 = <Tool>__T.ToString();
		}
		else
		{
			text2 = string.Empty;
		}
		array[num2] = text2;
		array[5] = " }";
		return string.Concat(array);
	}
}
