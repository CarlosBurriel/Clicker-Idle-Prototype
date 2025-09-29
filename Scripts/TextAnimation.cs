using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextAnimation : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    Mesh mesh;
    Vector3[] vertices;

    void Start()
    {
        textComponent= GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        textComponent.ForceMeshUpdate();
        mesh = textComponent.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 offset = Wobble(Time.time + i);
            vertices[i] = vertices[i] + offset;
        }
        mesh.vertices = vertices;
        textComponent.canvasRenderer.SetMesh(mesh);
    }
    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 3f), Mathf.Cos(time * 2f));
    }
}
