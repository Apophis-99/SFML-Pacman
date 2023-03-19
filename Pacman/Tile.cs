using SFML.Graphics;
using SFML.System;

namespace Pacman;

public class Tile : Drawable
{
    private readonly Sprite _sprite;

    public Texture Texture
    {
        set => _sprite.Texture = value;
    }

    public int TileSize = 16;

    public (int X, int Y) TileCoord
    {
        get => (_sprite.TextureRect.Left / TileSize, _sprite.TextureRect.Top / TileSize);
        set => _sprite.TextureRect = new IntRect(TileSize * value.X, TileSize * value.Y, TileSize, TileSize);
    }

    public (int X, int Y) RelativePosition = (0, 0);

    public Tile()
    {
        _sprite = new Sprite();
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        _sprite.Position = new Vector2f(RelativePosition.X, RelativePosition.Y);
        target.Draw(_sprite);
    }
}
