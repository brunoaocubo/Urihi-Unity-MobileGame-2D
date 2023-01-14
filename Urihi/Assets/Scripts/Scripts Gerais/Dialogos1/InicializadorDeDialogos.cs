using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicializadorDeDialogos : MonoBehaviour
{
    [SerializeField] private GerenciadordeDialogos _gerenciador;
    [SerializeField] private Dialogo _dialogo;
    public GameObject Yakecan;

    public void Inicializa()
    {
        if(_gerenciador == null)
            return;
            _gerenciador.Inicializa(_dialogo);
        Yakecan.SetActive(false);
    }
}
