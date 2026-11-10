using System;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocity;

    [Header("Reaparción")]
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private InputActionReference accionMover;

    [Header("Salto")]
    [SerializeField] private InputActionReference accionSaltar;

    public float fuerzaImpulso = 5f;
    [SerializeField] private LayerMask capaSuelo;

    private Rigidbody2D fisica;
    private SpriteRenderer renderizador;
    private Animator animador;
    private Vector2 entradaX;
    private bool saltoPendiente;
    private bool estaTocandoSuelo;
    private float longitudRaycast = 1.6f;
    private float umbralCarrera = 0.1f;

    [Header("Puntuación")]
    [SerializeField] private int puntuacion;
    [Header("Salud")]
    [SerializeField] private int vidas = 3;
    public int Vidas => vidas;
    private Color colorOriginal;
    private bool vulnerable = true;
    [SerializeField, Min(0.01f)] private float segundosInvulnerable = 1f;
    [Header("Tiempo")]
    private float tiempoInicio;
    private float tiempoReal;
    [SerializeField] private int tiempoEmpleado;
    [SerializeField,Min(1)] private int tiempoLimiteNivel = 100; 

    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();

        renderizador = GetComponent<SpriteRenderer>();
        animador = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (renderizador != null)
            colorOriginal = renderizador.color;
        else
            Debug.LogError("El jugador no tiene asignador el objeto Sprite Render");

        tiempoInicio = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        entradaX = accionMover.action.ReadValue<Vector2>();

        ComprobarFinJuegoTiempo();

        SaltoJugador();

        AnimarJugador();



    }

    private void ComprobarFinJuegoTiempo()
    {
        tiempoReal = Time.time - tiempoInicio;
        tiempoEmpleado = Mathf.FloorToInt(tiempoReal);
        if (tiempoEmpleado >= tiempoLimiteNivel)
        {
            FinJuego();
            return;
        }
    }

    private void SaltoJugador()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);

        estaTocandoSuelo = hit.collider != null;

        Color colorRayo = estaTocandoSuelo ? Color.green : Color.red;

        Debug.DrawRay(transform.position, Vector2.down * longitudRaycast, colorRayo);

        if (accionSaltar.action.WasPressedThisFrame() && estaTocandoSuelo)
        {
            saltoPendiente = true;
        }

    }

    private void AnimarJugador()
    {
        if (animador == null)
            return;
        if (!estaTocandoSuelo)
        {
            animador.Play("JugadorSalto");
        }
        else if (Mathf.Abs(fisica.linearVelocityX) > umbralCarrera)
        {
            animador.Play("JugadorCorriendo");
        }
        else
            animador.Play("JugadorParado");
    }

    private void OnEnable()
    {
        if(accionMover == null || accionMover.action == null)
        {
            Debug.Log("Asigna mapa de acción Player/Mover en Acción mover");
            enabled = false;
            return;
        }

        accionMover.action.Enable();

        if (accionSaltar == null || accionSaltar.action == null)
        {
            Debug.Log("Asigna mapa de acción Player/Saltar en Acción saltar");
            enabled = false;
            return;
        }

        accionSaltar.action.Enable();
    }

    private void OnDisable()
    {
        if (accionMover != null && accionMover.action != null)
        {
            accionMover.action.Disable();
        }

        if (accionSaltar != null && accionSaltar.action != null)
        {
            accionSaltar.action.Disable();
        }

    }

    private void FixedUpdate()
    {
        MoverJugador();
    }

    private void MoverJugador()
    {
        fisica.linearVelocity = new Vector2(entradaX.x * velocity, fisica.linearVelocity.y);
        if (saltoPendiente)
        {
            fisica.AddForce(Vector2.up * fuerzaImpulso, ForceMode2D.Impulse);
        }

        saltoPendiente = false;

        OrientarSprite();
    }

    private void OrientarSprite()
    {
        if (renderizador == null)
            return;

        if (fisica.linearVelocity.x > 0)
        {
            renderizador.flipX = false;
        }
        else if (fisica.linearVelocity.x < 0)
        {
            renderizador.flipX = true;

        }
    }

    public void ReaparecerJugador()
    {
        if(spawnPoint == null)
        {
            Debug.LogError("Asigna un Spawn Point al objeto jugador", this);
            enabled = false;
            return;
        }
        fisica.position = (Vector2)spawnPoint.position;
        fisica.linearVelocity = Vector2.zero;
    }

    public void IncrementarPuntos(int cantidad)
    {
        if (cantidad <= 0) return;

        puntuacion += cantidad;
    }
    public void QuitarVida()
    {
        if (!vulnerable) return;
        vulnerable = false;
        vidas--;

        if (renderizador != null)
        {
            renderizador.color = Color.blue;
        }

        if(vidas <= 0)
        {
            FinJuego();
            return;
        }
        Invoke(nameof(HacerVulnerable), segundosInvulnerable);
    }

    private void HacerVulnerable()
    {
        vulnerable = true;
        if (renderizador != null)
            renderizador.color = colorOriginal;
    }

    private void FinJuego()
    {
        Debug.Log("Juego finalizado.!");
        gameObject.SetActive(false);
    }
}
