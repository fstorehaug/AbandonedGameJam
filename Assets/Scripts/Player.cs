using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player
{
    public Color color;
    public string name;

    public readonly ResourceManager reasourceManager;

    public Player(string name)
    {
        reasourceManager = new ResourceManager();
        color = new Color(Random.Range(0, 264), Random.Range(0, 264), Random.Range(0, 264));
        this.name = name;
    }
}
