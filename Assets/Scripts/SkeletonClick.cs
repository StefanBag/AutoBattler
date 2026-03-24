using UnityEngine;

public class SkeletonClick : MonoBehaviour
{
    [SerializeField] private AudioSource skeletonClickSource;

    private void OnMouseDown()
    {
        skeletonClickSource.PlayOneShot(skeletonClickSource.clip);
    }
}