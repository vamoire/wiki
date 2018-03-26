using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterData : Config {

    static MonsterData __monsterData = null;
    public static MonsterData share {
        get {
            if (__monsterData == null) {
                __monsterData = Read<MonsterData> ("Monster.json");
            }
            return __monsterData;
        }
    }

    public string Version;
    public List<MonsterEntity> MonsterList;
}