using UnityEngine;

public class UpgradeReactor : MonoBehaviour
{
    [SerializeField] private Animator _anim;

    public void React()
    {
        _anim.SetTrigger("Hide");
    }
}