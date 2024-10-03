
public class BehaviourTree{
    private BehaviorNode root;
    public BehaviourTree(BehaviorNode root){
        this.root = root;
    }

    public void Update(){
        if(root != null){
            root.Evaluate();
        }
    }
}
