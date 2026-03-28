using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTextManager : MonoBehaviour
{
    public List<(int start, int end)> shakeRanges = new List<(int, int)>();
    private TextMeshProUGUI tmpText;
    private TMP_Text text;
    private bool hasTextChanged;
    [SerializeField] private float shakeStrength = 1f;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (shakeRanges.Count == 0) return;
    }

    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    private void OnTextChanged(Object obj)
    {
        if (obj == text) hasTextChanged = true;
    }

    void LateUpdate()
    {
        if (shakeRanges.Count == 0) return;

        text.ForceMeshUpdate();
        var textInfo = text.textInfo;

        foreach (var range in shakeRanges)
        {
            for (int i = range.start; i <= range.end; i++)
            {
                if (i >= textInfo.characterCount) continue;

                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;
                Vector3[] sourceVertices = textInfo.meshInfo[materialIndex].vertices;

                Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * shakeStrength;

                sourceVertices[vertexIndex + 0] += offset;
                sourceVertices[vertexIndex + 1] += offset;
                sourceVertices[vertexIndex + 2] += offset;
                sourceVertices[vertexIndex + 3] += offset;
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
