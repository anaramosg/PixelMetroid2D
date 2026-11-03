using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Rigidbody2D))]

public class PlataformaMovil : MonoBehaviour
{
    [SerializeField] private Transform puntaA;
    [SerializeField] private Transform puntaB;
    [SerializeField, UnityEngine.Min(0f)] private float velocidad = 2f;

    private Rigidbody2D fisica;
    private bool haciaB = true;
    private float tolerancia = 0.01f;

    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();
    }

    public Vector2 Velocidad { get; private set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(puntaA == null || puntaB == null)
        {
            Debug.LogError("Asigna el puntoA y el puntoB a la plataforma móvil", this);

            return;
        }

        //Punto de origen de la plataforma
        fisica.position = (Vector2)puntaA.position;    
    }
    private void FixedUpdate()
    {
        Vector2 destino = haciaB ? (Vector2)puntaB.position : (Vector2)puntaA.position;
        if(Vector2.Distance(fisica.position, destino) <= tolerancia)
        {
            haciaB = !haciaB;
            destino = haciaB ? (Vector2)puntaB.position : (Vector2)puntaA.position;
        }

        //MoveToward movimiento desplazado
        Vector2 siguiente = Vector2.MoveTowards(fisica.position, destino, velocidad*Time.fixedDeltaTime);

        //El movimiento con salto
        fisica.MovePosition(siguiente);
    }

    private void OnDrawGizmos()
    {
        if(puntaA ==  null || puntaB == null)
        {
            return;

            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(puntaA.position, puntaB.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
