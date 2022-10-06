

global using gui = Terminal.Gui;
global using Veldrid;


using System;
using System.Diagnostics;

log.create( "logs/server.log", log.Endpoints.Console );

log.high( "********************************************************************************" );
log.high( "********************************************************************************" );
log.high( "********************************************************************************" );

log.high( $"Starting up..." );


log.high( $"Starting in {Directory.GetCurrentDirectory()}" );


log.high( $"Starting text gui" );
gui.Application.Init();
gui.Application.HeightAsBuffer = true;
//ConsoleDriver.Diagnostics = ConsoleDriver.DiagnosticFlags.FramePadding | ConsoleDriver.DiagnosticFlags.FrameRuler;

var top = gui.Application.Top;

int margin = 3;
var win = new gui.Window("Legba")
{
	X = 1,
	Y = 1,

	Width = gui.Dim.Fill() - margin,
	Height = gui.Dim.Fill() - margin,
};

top.Add( win );


log.high( $"Starting graphics" );
cl.Graphics graphics = new();

var runToken = gui.Application.Begin( top );
var firstIteration = true;
gui.Application.RunMainLoopIteration( ref runToken, true, ref firstIteration );

while( graphics.Window.Exists )
{
	gui.Application.RunMainLoopIteration( ref runToken, false, ref firstIteration );


	long previousFrameTicks = 0;
	Stopwatch sw = new Stopwatch();
	sw.Start();

	previousFrameTicks = graphics.RunPass( previousFrameTicks, sw );

}

gui.Application.End( runToken );


graphics.DestroyAll();


//gui.Application.

gui.Application.Run();

