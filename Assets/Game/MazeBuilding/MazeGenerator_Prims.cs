using Game.MazeBuilding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MazeBuilding
{
    public class MazeGenerator_Prims : MazeGeneratorBase
    {
        [SerializeField]
        private PrimsWorker[] primsWorker;

        public override void GenerateMaze()
        {
            foreach (var worker in primsWorker)
            {
                DoPrims(worker);
            }
        }

        private void DoPrims(PrimsWorker worker)
        {
            //set the starting position
            int posX = worker.randomStartX ? UnityEngine.Random.Range(1, mazeWidth - 1) : Mathf.Clamp(worker.startPositionX, 1, mazeWidth - 1);
            int posZ = worker.randomStartZ ? UnityEngine.Random.Range(1, mazeDepth - 1) : Mathf.Clamp(worker.startPositionZ, 1, mazeDepth - 1);
            
            map[posX, posZ] = 0;

            List<MapLocation> neighbourWalls = new List<MapLocation>() 
            { 
                new MapLocation(posX + 1, posZ),
                new MapLocation(posX - 1, posZ),
                new MapLocation(posX, posZ + 1),
                new MapLocation(posX, posZ - 1),
            };

            int loopSafegourd = 5000;

            while (neighbourWalls.Count > 0 && loopSafegourd > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, neighbourWalls.Count);
                posX = neighbourWalls[randomIndex].x;
                posZ = neighbourWalls[randomIndex].z;

                neighbourWalls.RemoveAt(randomIndex);
                if(CountStreightEmptyNeighbours(posX, posZ) == 1)
                {
                    map[posX, posZ] = 0;

                    neighbourWalls.Add(new MapLocation(posX + 1, posZ));
                    neighbourWalls.Add(new MapLocation(posX - 1, posZ));
                    neighbourWalls.Add(new MapLocation(posX, posZ + 1));
                    neighbourWalls.Add(new MapLocation(posX, posZ - 1));
                }

                loopSafegourd--;
            }
        }
    }

    [Serializable]
    public class PrimsWorker
    {
        [Header("X SETTINGS")]
        public bool randomStartX;
        public bool allowBackX = true;
        public int startPositionX;

        [Space(1)]
        [Header("Z SETTINGS")]
        public bool randomStartZ;
        public bool allowBackZ = true;
        public int startPositionZ;
    }
}