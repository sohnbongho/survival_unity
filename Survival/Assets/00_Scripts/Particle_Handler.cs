using UnityEngine;

public class Particle_Handler : MonoBehaviour
{
    public static Particle_Handler instance = null;
    ParticleSystem particleSystem;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    

    public void OnParticle(MeshRenderer mesh)
    {
        transform.position = mesh.transform.position;
        UPdateParticleMesh(mesh);
        particleSystem.Play();
    }

    private void UPdateParticleMesh(MeshRenderer meshRenderer)
    {
        var shape = particleSystem.shape;
        shape.meshRenderer = meshRenderer;
    }
}
