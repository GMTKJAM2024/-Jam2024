using UnityEngine;

public class LegIKControl : MonoBehaviour
{
    protected Animator animator;

    public bool ikActive = true;
    public Transform rightFootObj;
    public Transform leftFootObj;
    public LayerMask groundLayer;
    public float footOffset = 0.1f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (animator && ikActive)
        {
            AdjustFootIK(ref rightFootObj, AvatarIKGoal.RightFoot);
            AdjustFootIK(ref leftFootObj, AvatarIKGoal.LeftFoot);
        }
    }
    private void AdjustFoot(ref Transform footTarget, AvatarIKGoal foot)
    {
        RaycastHit hit;
        Vector3 footPosition = footTarget.position;

        if (Physics.Raycast(footPosition + Vector3.up, Vector3.down, out hit, 1f, groundLayer))
        {
            footTarget.position = hit.point + Vector3.up * footOffset;
            footTarget.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;

            animator.SetIKPositionWeight(foot, 1);
            animator.SetIKRotationWeight(foot, 1);
            animator.SetIKPosition(foot, footTarget.position);
            animator.SetIKRotation(foot, footTarget.rotation);
        }
        else
        {
            animator.SetIKPositionWeight(foot, 0);
            animator.SetIKRotationWeight(foot, 0);
        }
    }
    private void AdjustFootIK(ref Transform footTarget, AvatarIKGoal foot)
    {
        RaycastHit hit;
        Vector3 footPosition = footTarget.position;

        // Raycast từ vị trí hiện tại của chân xuống để kiểm tra mặt đất
        if (Physics.Raycast(footPosition + Vector3.up, Vector3.down, out hit, 2f, groundLayer))
        {
            footTarget.position = hit.point + Vector3.up * footOffset;
            footTarget.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;

            animator.SetIKPositionWeight(foot, 1);
            animator.SetIKRotationWeight(foot, 1);
            animator.SetIKPosition(foot, footTarget.position);
            animator.SetIKRotation(foot, footTarget.rotation);
        }
        else
        {
            animator.SetIKPositionWeight(foot, 0);
            animator.SetIKRotationWeight(foot, 0);
        }
    }
}
