using UnityEngine;

public class GoalsScrollView : MonoBehaviour
{
    [SerializeField] private GameObject goalItemPrefab;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }
}