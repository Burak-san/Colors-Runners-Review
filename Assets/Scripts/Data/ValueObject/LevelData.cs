using System;
using Abstract;
using UnityEngine;

namespace Data.ValueObject
{
    [Serializable]
    public class LevelData : SaveableEntitiy
    {

        public static string LevelKey = "Level";
        public GameObject Level;
        public int LevelId;

        public int IdleLevelId;


        public LevelData(int idleLevelId, int levelId)
        {
            IdleLevelId = idleLevelId;
            LevelId = levelId;
        }

        public override string GetKey()
        {
            return LevelKey;
        }
    }
}