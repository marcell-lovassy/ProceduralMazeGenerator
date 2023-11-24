using System;
using UnityEngine;

namespace Game.MazeBuilding
{
    public class MazeGenerator_Crawler : MazeGeneratorBase
    {
        [SerializeField]
        private Crawler[] crawlers;

        public override void GenerateMaze()
        {
            foreach (var crawler in crawlers)
            {
                DoCrawl(crawler);
            }
        }

        private void DoCrawl(Crawler crawler)
        {
            bool generateMazeFinished = false;

            //set the starting position
            int posX = crawler.randomStartX ? UnityEngine.Random.Range(1, mazeWidth - 1) : Mathf.Clamp(crawler.startPositionX, 1, mazeWidth - 1);
            int posZ = crawler.randomStartZ ? UnityEngine.Random.Range(1, mazeDepth - 1) : Mathf.Clamp(crawler.startPositionZ, 1, mazeDepth - 1);

            while (!generateMazeFinished)
            {
                map[posX, posZ] = 0;

                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    var randomDirectionX = UnityEngine.Random.Range(crawler.allowBackX ? -1 : 0, 2);
                    posX += randomDirectionX;
                }
                else
                {
                    var randomDirectionZ = UnityEngine.Random.Range(crawler.allowBackZ ? -1 : 0, 2);
                    posZ += randomDirectionZ;
                }

                generateMazeFinished |= (posX < 1 || posX >= mazeWidth - 1 || posZ < 1 || posZ >= mazeDepth - 1);
            }
        }
    }

    [Serializable]
    public class Crawler
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