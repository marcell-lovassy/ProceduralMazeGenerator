using UnityEngine;

namespace Game.MazeBuilding
{
    public class MapLocation
    {
        public int x;
        public int z;

        public MapLocation(int x, int z)
        {
            this.x = x;
            this.z = z;
        }
    }

    public class MazeGeneratorBase : MonoBehaviour
    {
        //[SerializeField]
        //private GameObject mazeCube1x1;

        [Header("MAZE SETTINGS")]
        [SerializeField]
        protected int mazeWidth = 10;
        [SerializeField]
        protected int mazeDepth = 10;
        [SerializeField]
        [Tooltip("Size of a building block in Unity Units (meters)")]
        protected int scale = 6;

        //1 is wall 0 is corridor
        protected byte[,] map;


        void Start()
        {
            InitlializeMap();
            GenerateMaze();
            DrawMap();
        }

        private void InitlializeMap()
        {
            map = new byte[mazeWidth, mazeDepth];
            for (int x = 0; x < mazeWidth; x++)
            {
                for (int z = 0; z < mazeDepth; z++)
                {
                    map[x, z] = 1;
                }
            }
        }

        public virtual void GenerateMaze()
        {
            for (int x = 0; x < mazeWidth; x++)
            {
                for (int z = 0; z < mazeDepth; z++)
                {
                    map[x, z] = GetWallOrCorridorAtRandom();
                }
            }
        }

        private void DrawMap()
        {
            for (int x = 0; x < mazeWidth; x++)
            {
                for (int z = 0; z < mazeDepth; z++)
                {
                    if (map[x, z] == 1)
                    {
                        var pos = new Vector3(x * scale, 0, z * scale);
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.transform.localScale = Vector3.one * scale;
                        wall.transform.SetParent(transform, false);
                        wall.transform.position = pos;
                    }
                }
            }
        }

        private byte GetWallOrCorridorAtRandom()
        {
            return (byte)UnityEngine.Random.Range(0, 2);
        }

        public int CountAllEmptyNeighbours(int x, int z)
        {
            return CountStreightEmptyNeighbours(x, z) + CountDiagonalEmptyNeighbours(x, z);
        }

        public int CountStreightEmptyNeighbours(int x, int z)
        {
            int count = 0;

            if(x <= 0 || x >= mazeWidth - 1 || z <= 0 || z >= mazeDepth - 1)
            {
                return 5;
            }

            var neighbourCoordinates = new MapLocation[] 
            {
                new MapLocation(x + 1, z),
                new MapLocation(x - 1, z),
                new MapLocation(x, z + 1),
                new MapLocation(x, z - 1),
            };

            foreach (var neighbour in neighbourCoordinates)
            {
                if (map[neighbour.x, neighbour.z] == 0)
                {
                    count++;
                }
            }

            return count;
        }

        public int CountDiagonalEmptyNeighbours(int x, int z)
        {
            int count = 0;

            if (x <= 0 || x >= mazeWidth - 1 || z <= 0 || z >= mazeDepth - 1)
            {
                return 5;
            }

            var neighbourCoordinates = new MapLocation[]
            {
                new MapLocation(x + 1, z + 1),
                new MapLocation(x - 1, z + 1),
                new MapLocation(x - 1, z - 1),
                new MapLocation(x + 1, z - 1),
            };

            foreach (var neighbour in neighbourCoordinates)
            {
                if (map[neighbour.x, neighbour.z] == 0)
                {
                    count++;
                }
            }

            return count;
        }
    }
}