using SFML.Graphics;

namespace Pacman;

public class Map
{
    private const int TileSize = 16;

    private readonly List<List<bool>> _map;
    private readonly List<List<Tile>> _tiles;

    private readonly List<(string connection, (int, int) coord)> _tileMapConnections;

    public Map(string mapPath, string tilesetPath)
    {
        var lines = File.ReadAllLines(tilesetPath);
        var texture = new Texture(lines[0].Replace("\r", ""));
        
        var rawMap = File.ReadAllText(mapPath);
        _tileMapConnections = new List<(string connection, (int, int) coord)>();

        _tiles = new List<List<Tile>>();
        _map = new List<List<bool>>();

        for (var r = 0; r < rawMap.Split("\r\n").Length; r++)
        {
            _map.Add(new List<bool>());
            _tiles.Add(new List<Tile>());
            for (var c = 0; c < rawMap.Split("\r\n")[r].Split(" ").Length; c++)
            {
                _map[r].Add(rawMap.Split("\r\n")[r].Split(" ")[c] == "1");
                _tiles[r].Add(new Tile
                {
                    Texture = texture,
                    TileSize = TileSize,
                    TileCoord = (1, 1),
                    RelativePosition = (c * TileSize, r * TileSize)
                });
            }
        }
        
        LoadTileMapConnections(lines[1..]);
        CalculateMap();
    }

    private void CalculateMap()
    {
        for (var r = 0; r < _map.Count; r++)
        {
            for (var c = 0; c < _map[r].Count; c++)
            {
                var connection = GetConnection(r, c);
                
                if (connection[4] == '0')
                    continue;
                
                FixConnection(ref connection);

                _tiles[r][c].TileCoord = FindConnection(connection);
            }
        }
    }

    #region ManageConnections
    
    private void LoadTileMapConnections(IReadOnlyList<string> lines)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var parts = lines[i].Split(" ");
            for (var j = 0; j < parts.Length; j++)
            {
                _tileMapConnections.Add((parts[j], (j, i)));
            }
        }

        SortTileMapConnections();
    }

    private void SortTileMapConnections()
    {
        var swapped = true;
        while (swapped)
        {
            swapped = false;
            for (var i = 1; i < _tileMapConnections.Count; i++)
            {
                if (string.Compare(_tileMapConnections[i - 1].connection, _tileMapConnections[i].connection, StringComparison.Ordinal) <= 0)
                    continue;
                (_tileMapConnections[i - 1], _tileMapConnections[i]) = (_tileMapConnections[i], _tileMapConnections[i - 1]);
                swapped = true;
            }
        }
    }

    private (int, int) FindConnection(string connection)
    {
        /*var min = 0;
        var max = connection.Length;

        while (true)
        {
            if (min >= max)
                return (-1, -1);
            
            var midpoint = (min + max - 1) / 2;

            switch (string.Compare(_tileMapConnections[midpoint].connection, connection, StringComparison.Ordinal))
            {
                case 0:
                    return _tileMapConnections[midpoint].coord;
                case > 0:
                    min = midpoint;
                    break;
                default:
                    max = midpoint;
                    break;
            }
        }*/

        foreach (var tileMapConnection in _tileMapConnections.Where(tileMapConnection => tileMapConnection.connection == connection))
        {
            return tileMapConnection.coord;
        }

        return (-1, -1);
    }
    
    private string GetConnection(int r, int c)
    {
        string GetAt(int row, int col)
        {
            if (row < 0 || row > _map.Count - 1 || col < 0 || col > _map[row].Count - 1)
                return "0";
            return _map[row][col] ? "1" : "0";
        }

        var connection = $"{GetAt(r - 1, c - 1)}{GetAt(r - 1, c)}{GetAt(r - 1, c + 1)}{GetAt(r, c - 1)}{GetAt(r, c)}{GetAt(r, c + 1)}{GetAt(r + 1, c - 1)}{GetAt(r + 1, c)}{GetAt(r + 1, c + 1)}";
        
        return connection;
    }
    
    private static void FixConnection(ref string connection)
    {
        var corrected = connection.ToCharArray();
        if (connection[1] == '1' && connection[4] == '1' && connection[7] == '1')
        {
            if (connection[3] == '1' || connection[5] == '1')
                return;
        }
        else if (connection[3] == '1' && connection[4] == '1' && connection[5] == '1')
        {
            if (connection[1] == '1' || connection[7] == '1')
                return;
        }
        else
            return;

        corrected[0] = '0';
        corrected[2] = '0';
        corrected[6] = '0';
        corrected[8] = '0';

        connection = corrected.Aggregate("", (current, chr) => current + chr);
    }

    #endregion

    public void Render(ref RenderWindow window)
    {
        foreach (var tile in _tiles.SelectMany(row => row))
        {
            window.Draw(tile);
        }
    }
}
