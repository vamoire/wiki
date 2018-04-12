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

    /// <summary>
    /// 获得角色信息
    /// </summary>
    /// <param name="monsterID">角色ID</param>
    /// <returns>角色信息</returns>
    public MonsterEntity GetMonster(int monsterID) {
        MonsterEntity monster = null;
        foreach (MonsterEntity item in MonsterList)
        {
            if (item.id == monsterID) {
                monster = item;
                break;
            }
        }
        return monster;
    }

    public string Version;
    public List<MonsterEntity> MonsterList;
}