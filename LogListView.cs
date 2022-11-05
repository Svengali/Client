using NStack;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl;

// This is basically the same implementation used by the UICatalog main window
internal class LogDataSource : gui.IListDataSource
{
	int _nameColumnWidth = 30;
	private List<string> scenarios;
	BitArray marks;
	int count, len;

	public List<string> Scenarios
	{
		get => scenarios;
		set
		{
			if( value != null )
			{
				count = value.Count;
				marks = new BitArray( count );
				scenarios = value;
				len = GetMaxLengthItem();
			}
		}
	}
	public bool IsMarked( int item )
	{
		if( item >= 0 && item < count )
			return marks[item];
		return false;
	}

	public int Count => Scenarios != null ? Scenarios.Count : 0;

	public int Length => len;

	public LogDataSource( List<string> itemList ) => Scenarios = itemList;

	public void Render( gui.ListView container, gui.ConsoleDriver driver, bool selected, int item, int col, int line, int width, int start = 0 )
	{
		container.Move( col, line );
		// Equivalent to an interpolated string like $"{Scenarios[item].Name, -widtestname}"; if such a thing were possible
		var s = String.Format( "{0}", Scenarios [item] );
		RenderUstr( driver, $"{s}", col, line, width, start );
	}

	public void SetMark( int item, bool value )
	{
		if( item >= 0 && item < count )
			marks[item] = value;
	}

	int GetMaxLengthItem()
	{
		if( scenarios?.Count == 0 )
		{
			return 0;
		}

		int maxLength = 0;
		for( int i = 0; i < scenarios.Count; i++ )
		{
			var s = String.Format (String.Format ("{{0,{0}}}", -_nameColumnWidth), Scenarios [i]);
			var sc = $"{s}  {Scenarios [i]}";
			var l = sc.Length;
			if( l > maxLength )
			{
				maxLength = l;
			}
		}

		return maxLength;
	}

	// A slightly adapted method from: https://github.com/gui-cs/Terminal.Gui/blob/fc1faba7452ccbdf49028ac49f0c9f0f42bbae91/Terminal.Gui/Views/ListView.cs#L433-L461
	private void RenderUstr( gui.ConsoleDriver driver, ustring ustr, int col, int line, int width, int start = 0 )
	{
		int used = 0;
		int index = start;
		while( index < ustr.Length )
		{
			(var rune, var size) = Utf8.DecodeRune( ustr, index, index - ustr.Length );
			var count = 0; System.Rune.ColumnWidth (rune);
			if( used + count >= width ) break;
			driver.AddRune( rune );
			used += count;
			index += size;
		}

		while( used < width )
		{
			driver.AddRune( ' ' );
			used++;
		}
	}

	public IList ToList()
	{
		return Scenarios;
	}
}
