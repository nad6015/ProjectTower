using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;
using Assets.CombatSystem;
using Assets.Combat;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    private NavMeshAgent agent;
    private BehaviorGraphAgent graphAgent;
    private Fighter self;
    private Animator animator;
    

    void Start()
    {
        self = GetComponent<NpcFighter>();
        agent = GetComponent<NavMeshAgent>();
        graphAgent = GetComponent<BehaviorGraphAgent>();


        graphAgent.BlackboardReference.SetVariableValue("Speed", speed);
        graphAgent.BlackboardReference.SetVariableValue("Health", (float)self.GetStat(FighterStats.HEALTH));

        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("MotionSpeed", 1);
        animator.SetFloat("Speed", speed);

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    GameObject target = collision.gameObject;
    //    if (target.GetComponent<Fighter>() != null)
    //    {
    //        Debug.Log("eNEMY IS ATTACKING");
    //        target.GetComponent<Animator>().SetTrigger("Injured");
    //        self.Attack(target.GetComponent<Fighter>());
    //    }
    //    GetComponent<Rigidbody>().isKinematic = true;
    //}

    public void Hit()
    {
        //GetComponent<Rigidbody>().isKinematic = false;
        self.Attack();
    }
}
