
using Pacman;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

var window = new RenderWindow(new VideoMode(1200, 1100), "Pacman");
window.Closed += (_, _) => window.Close();

var map = new Map("Assets/map.txt", "Assets/TilesetAlternate.txt");

const int padding = 100;

var view = new View();
view.Size = new Vector2f(map.Size.X + padding, map.Size.Y + padding);
view.Center = new Vector2f(0, 0);
view.Move(new Vector2f(map.Size.X / 2.0F - padding / 4.0F, map.Size.Y / 2.0F + padding / 4.0F));

window.Size = new Vector2u((uint) view.Size.X * 2, (uint) view.Size.Y * 2);
window.SetView(view);


while (window.IsOpen)
{
    window.DispatchEvents();
    window.Clear(new Color(150, 150, 150));
    
    map.Render(ref window);
    
    window.Display();
}
