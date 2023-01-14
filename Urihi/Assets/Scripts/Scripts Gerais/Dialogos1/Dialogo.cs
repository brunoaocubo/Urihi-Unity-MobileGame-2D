using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Dialogo
{
    [SerializeField] TextoDialogo[] _frases; 
    [SerializeField] string _nomeNpc;

    public string GetNomeNpc()
    {
        return _nomeNpc;
    }
    public TextoDialogo[] GetFrases()
    {
        return _frases;
    }
}
