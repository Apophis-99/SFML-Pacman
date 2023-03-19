
using Pacman;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

var window = new RenderWindow(new VideoMode(1200, 1100), "Pacman");
window.Closed += (_, _) => window.Close();
var view = new View();
view.Size = new Vector2f(600, 550);
view.Center = new Vector2f(view.Size.X / 2.0F, view.Size.Y / 2.0F);
window.SetView(view);

var map = new Map("Assets/map.txt", "Assets/TilesetAlternate.txt");

while (window.IsOpen)
{
    window.DispatchEvents();
    window.Clear(new Color(150, 150, 150));
    
    map.Render(ref window);
    
    window.Display();
}
