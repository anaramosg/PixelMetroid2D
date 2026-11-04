using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ControlEnemigo : MonoBehaviour
{
    [Header ("Movimiento")]
    public float velocidad = 2f;
    public Vector2 posicionFinal;
    private Vector2 posicionInicial;

    private Boolean movimientoHaciaArriba = true;
    [SerializeField] private float tolerancia = 0.1f;

    private Rigidbody2D fisica;

    [SerializeField]
    private SpriteRenderer renderizador;
    private Vector2 posicionObjetivo;



    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicionInicial = fisica.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //Vector2 posicionObjetivo;

        if (velocidad <= 0)
            return;

        if (movimientoHaciaArriba)
        {
            posicionObjetivo = posicionFinal;
        }
        else
        {
            posicionObjetivo = posicionInicial;
        }

       // Vector2 posicionObjetivo = MovimientoJugador ? posicionFinal : posicionInicial;

        if(Vector2.Distance(fisica.position, posicionObjetivo) <= tolerancia)
        {
            //if (movimientoHaciaArriba)
            //{
            //    movimientoHaciaArriba = false;
            //    posicionObjetivo = posicionInicial;
            //}
            //else
            //{
            //    movimientoHaciaArriba = true;
            //    posicionObjetivo = posicionFinal;
            //}

            movimientoHaciaArriba = !movimientoHaciaArriba;
            posicionObjetivo = movimientoHaciaArriba ? posicionFinal : posicionInicial;
        }

        fisica.MovePosition(Vector2.MoveTowards(fisica.position, posicionObjetivo, velocidad * Time.deltaTime));

        OrientarSprite();
    }

    private void OnCollisionEnter2D(Collision2D colision)
    {
        if (!colision.gameObject.CompareTag("Player")) return;
        if(colision.gameObject.TryGetComponent(out MovimientoJugador jugador))
        {
            jugador.ReaparecerJugador();
        }
        else
        {
            Debug.LogError("El objeto Player necesita ControlJugador en la raíz.");
        }
    }

    private void OrientarSprite()
    {
        float direccion = posicionObjetivo.x - fisica.position.x;

        if (renderizador == null)
            return;

        if (direccion > 0.1f)
        {
            renderizador.flipX = true;
        }
        else if (direccion < -0.1f)
        {
            renderizador.flipX = false;

        }
    }
}
