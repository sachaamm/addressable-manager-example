using UnityEngine;

public class SceneReferences : MonoBehaviour
{
    public Renderer cubeRenderer;
    public AudioSource audioSource;
    public static SceneReferences Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
}