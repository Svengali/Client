

global using gui = Terminal.Gui;
global using Veldrid;


using System;
using System.Diagnostics;
using cl;
using math;

log.create( "logs/server.log", log.Endpoints.Console );

gui.Application.Init();
gui.Application.HeightAsBuffer = true;


var top = gui.Application.Top;

int margin = 0;
var win = new gui.Window("Legba")
{
	X = margin,
	Y = margin,

	Width = gui.Dim.Fill() - margin,
	Height = gui.Dim.Fill() - margin - 1,
};

var logList = new LogDataSource( new List<string>() );

var listView = new gui.ListView(logList)
{
	X = 1,
	Y = 2,
	Height = gui.Dim.Fill (),
	Width = gui.Dim.Fill (1),
	//ColorScheme = Colors.TopLevel,
	AllowsMarking = false,
	AllowsMultipleSelection = false
};

listView.RowRender += ListView_RowRender;

//listView.MouseEvent(null);

void ListView_RowRender( gui.ListViewRowEventArgs obj )
{
	if( obj.Row == listView.SelectedItem )
	{
		return;
	}
	if( listView.AllowsMarking && listView.Source.IsMarked( obj.Row ) )
	{
		obj.RowAttribute = new gui.Attribute( gui.Color.BrightRed, gui.Color.BrightYellow );
		return;
	}
	if( obj.Row % 2 == 0 )
	{
		obj.RowAttribute = new gui.Attribute( gui.Color.Gray, gui.Color.DarkGray );
	}
	else
	{
		obj.RowAttribute = new gui.Attribute( gui.Color.DarkGray, gui.Color.Gray );
	}
}


win.Add( listView );

//int loc = 0;

log.addDelegate( ( evt ) => {
	var truncatedCat = evt.Cat.Substring( 0, Math.Min( 8, evt.Cat.Length ) );

	char sym = log.getSymbol( evt.LogType );

	string finalLine = string.Format( "{0,-8}{1}| {2}", truncatedCat, sym, evt.Msg );

	logList.Scenarios.Add( finalLine );

	//scrollView.Add( new gui.Label( finalLine ) );

	listView.TopItem = Math.Max( 0, logList.Scenarios.Count - listView.Frame.Height );

	//scrollView.ContentOffset = gui.Point.Empty;

	//scrollView.ContentOffset = gui.Point.Empty;

	//listView.ScrollUp(1);

	//scrollView.FocusLast();
	win.SetNeedsDisplay();
	listView.SetNeedsDisplay();
	//scrollView.SetNeedsDisplay();
	//scrollView.SetNeedsDisplay();

	//top.Redraw(top.Bounds);
} );


log.high( "********************************************************************************" );
log.high( "********************************************************************************" );
log.high( "********************************************************************************" );

log.high( $"Starting up..." );


log.high( $"Starting in {Directory.GetCurrentDirectory()}" );


log.high( $"Starting text gui" );
//ConsoleDriver.Diagnostics = ConsoleDriver.DiagnosticFlags.FramePadding | ConsoleDriver.DiagnosticFlags.FrameRuler;


var menu = new gui.MenuBar (new gui.MenuBarItem [] {
				new gui.MenuBarItem ("_File", new gui.MenuItem [] {
					new gui.MenuItem ("_New", "Creates new file", () => { } ),
					new gui.MenuItem ("_Open", "", null),
					new gui.MenuItem ("_Close", "", () => { } ),
					new gui.MenuItem ("_Quit", "", () => { top.Running = false; })
				}),
			});



var statusBar = new gui.StatusBar (new gui.StatusItem [] {
				new gui.StatusItem(gui.Key.Null, "FPS: 0", () => gui.MessageBox.Query (50, 7, "Help", "Helping", "Ok")),
				/*
				new gui.StatusItem(gui.Key.F1, "~F1~ Help", () => gui.MessageBox.Query (50, 7, "Help", "Helping", "Ok")),
				new gui.StatusItem(gui.Key.F2, "~F2~ Load", () => gui.MessageBox.Query (50, 7, "Load", "Loading", "Ok")),
				new gui.StatusItem(gui.Key.F3, "~F3~ Save", () => gui.MessageBox.Query (50, 7, "Save", "Saving", "Ok")),
				new gui.StatusItem(gui.Key.CtrlMask | gui.Key.Q, "~^Q~ Quit", () => { top.Running = false; }),
				new gui.StatusItem(gui.Key.Null, gui.Application.Driver.GetType().Name, () => { log.info("test"); } )
				*/
			});

top.Add( win, menu, statusBar );

int count = 0;


log.high( $"Starting graphics" );
cl.Graphics graphics = new();

var runToken = gui.Application.Begin( top );
var firstIteration = true;
gui.Application.RunMainLoopIteration( ref runToken, false, ref firstIteration );

while( graphics.Window.Exists )
{
	long previousFrameTicks = 0;
	Stopwatch sw = new Stopwatch();
	sw.Start();

	gui.Application.RunMainLoopIteration( ref runToken, false, ref firstIteration );

	previousFrameTicks = graphics.RunPass( previousFrameTicks, sw );

	statusBar.Items[0].Title = $"FPS: {previousFrameTicks}";

	statusBar.SetNeedsDisplay();
	win.SetNeedsDisplay();

	//Console.WriteLine( $"{count++}:{previousFrameTicks}" );

	//log.info( $"{count++}:{previousFrameTicks}" );

	//top.Redraw(statusBar.Bounds);

	//Thread.Sleep(100);
}

gui.Application.End( runToken );


graphics.DestroyAll();


//gui.Application.

gui.Application.Run();

