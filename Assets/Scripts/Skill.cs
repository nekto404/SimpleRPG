﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTarget
{
    Allies,
    Self,
    Enemies,
    Target
}

public enum Triger
{
    StartFigth,
    StartTurn,
    Atack,
    GetDamage
}

[Serializable]
public class Skill : MonoBehaviour
{
    public string Name;
    public EffectTarget TargetType;
    public int Chanse;
    public Triger EffectTriger;
    public List<Effect> Effects;
}
