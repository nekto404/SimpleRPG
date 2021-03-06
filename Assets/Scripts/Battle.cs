﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct TurnData
{
    public Character Character;
    public float TimeByTurn;

    public TurnData(Character character,float time)
    {
        Character = character;
        TimeByTurn = time;
    }
}

public class Battle : MonoBehaviour {

    public Group GroupOne;
    public Group GroupTwo;
    public BattleUI BattleUI;
    public static Battle Instance;
    private List<TurnData> queue;
    private bool battle;


    #region MonoBehaviourLogic
    public void Start()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        if (!BattleUI)
            return;
        queue = QueueCreator(BattleUI.Instance.GetQueueCount());
        BattleUI.Instance.UpdateQueue(queue.GetRange(0, BattleUI.Instance.GetQueueCount()));
        battle = false;
        StartCoroutine(BattleLogic());
    }
    #endregion

    #region BattleLogic
    private IEnumerator BattleLogic()
    {
        yield return StartBattle();
        while (!battle)
        {
            yield return StartTurn();
            yield return TurnLogic();
            yield return EndOfTurn();
        }
        yield return null;
    }

    private IEnumerator StartBattle()
    {
        yield return null;
    }

    private IEnumerator TurnLogic()
    {
        yield return null;
    }

    private IEnumerator StartTurn()
    {
        yield return null;
    }

    private IEnumerator EndOfTurn()
    {
        queue.OrderBy(t => t.TimeByTurn);
        yield return null;
    }
    #endregion

    #region UtilityLogic
    public void AtackEfect(Character character, Character target, bool WithGroupOne)
    {
        if (character.AtackType == AtackType.allies)
        {
            var TargetGroup = WithGroupOne ? GroupOne : GroupTwo;
            foreach (var targetChar in TargetGroup.Characters)
            {
                targetChar.Character.GetDamage(character.Atack, character.DamageType);
            }
        }
        else if (character.AtackType == AtackType.massEnemy)
        {
            var TargetGroup = WithGroupOne ? GroupTwo : GroupOne;
            foreach (var targetChar in TargetGroup.Characters)
            {
                targetChar.Character.GetDamage(character.Atack, character.DamageType);
            }
        }
        else
        {
            target.GetDamage(character.Atack, character.DamageType);
        }
    }

    public List<Character> AvalableTarget(Character character, bool WithGroupOne)
    {
        List<Character> targets = new List<Character>();
        if (character.AtackType == AtackType.allies || character.AtackType == AtackType.ally)
        {
            var TargetGroup = WithGroupOne ? GroupOne : GroupTwo;
            foreach (var targetChar in TargetGroup.Characters)
            {
                targets.Add(targetChar.Character); 
            }
        }
        else if (character.AtackType == AtackType.massEnemy || character.AtackType == AtackType.dist)
        {
            var TargetGroup = WithGroupOne ? GroupTwo : GroupOne;
            foreach (var targetChar in TargetGroup.Characters)
            {
                targets.Add(targetChar.Character);
            }
        }
        else 
        {
            var TargetGroup = WithGroupOne ? GroupTwo : GroupOne;
            foreach (var targetChar in TargetGroup.Characters)
            {
                if (targetChar.PosY==1)
                    targets.Add(targetChar.Character);
            }
            if (targets.Count == 0)
            {
                foreach (var targetChar in TargetGroup.Characters)
                {
                    targets.Add(targetChar.Character);
                }
            }
        }
        return targets;
    }

    public List<TurnData> QueueCreator(int count=5)
    {
        var queue = new List<TurnData>();
        foreach (var character in GroupOne.Characters)
        {
            var timePerTurn = 1f / character.Character.Speed;
            for (var i = 0; i < count; i++)
            {
                queue.Add(new TurnData(character.Character, timePerTurn * i));
            }
        }
        foreach (var character in GroupTwo.Characters)
        {
            var timePerTurn = 1f / character.Character.Speed;
            for (var i = 0; i < count; i++)
            {
                queue.Add(new TurnData(character.Character, timePerTurn * i));
            }
        }
        return queue.OrderBy(t => t.TimeByTurn).ToList();
    }
    #endregion
}
