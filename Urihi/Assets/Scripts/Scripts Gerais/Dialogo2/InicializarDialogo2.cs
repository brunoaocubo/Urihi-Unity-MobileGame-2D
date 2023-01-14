using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicializarDialogo2 : MonoBehaviour
{
    [SerializeField] private GerenciadorDialogo2 _gerenciador;
    [SerializeField] private Dialogo _dialogo;
    void Start()
    {
        Inicializa();
    }

    public void Inicializa()
    {
        if (_gerenciador == null)
            return;
        _gerenciador.Inicializa(_dialogo);
    }

    
}
